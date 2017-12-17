using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Cache for objects with background cleaning thread. Please note, that there is strong reference to key, so it 
	/// is not safe to use objects as keys. Value if referenced by weak reference and entry is deleted as soon as value 
	/// is collected by garbage collector.</summary>
	/// <typeparam name="K">Key type.</typeparam>
	/// <typeparam name="V">Value type.</typeparam>
	public class Cache<K, V>: IDisposable
		where V: class
	{
		#region consts

		/// <summary>Sanity thread timeout.</summary>
		private const int SanityThreadTimeout = 5000;

		#endregion

		#region fields

		/// <summary>Synchronization root.</summary>
		private readonly object m_SyncRoot = new object();

		/// <summary>Cache.</summary>
		private readonly Dictionary<K, CacheEntry<K, V>> m_Entries = new Dictionary<K, CacheEntry<K, V>>();

		/// <summary>Cycle pointer.</summary>
		private CacheEntry<K, V> m_Cycle;

		/// <summary>Resume signal.</summary>
		private readonly ManualResetEvent m_ResumeSignal = new ManualResetEvent(false);

		/// <summary>Abort signal.</summary>
		private readonly ManualResetEvent m_AbortSignal = new ManualResetEvent(false);

		/// <summary>Cleanup thread.</summary>
		private Thread m_SanityThread;

		/// <summary>Count how many times cache has been accessed.</summary>
		private int m_AccessCount;

		/// <summary>Counts how many times value has been found in cache.</summary>
		private int m_HitCount;

		#endregion

		#region properties

		/// <summary>Gets the cache size.</summary>
		public int Size
		{
			get { lock (m_SyncRoot) { return m_Entries.Count; } }
		}

		/// <summary>Gets the hit ratio.</summary>
		public double HitRatio
		{
			get { lock (m_SyncRoot) { return m_AccessCount == 0 ? 0 : (double)m_HitCount / (double)m_AccessCount; } }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Cache&lt;K, V&gt;"/> is reclaimed by garbage collection.
		/// </summary>
		~Cache()
		{
			Dispose(false);
		}

		#endregion

		#region Get

		/// <summary>
		/// Gets object cached under specified key. If key does not exist new object is created using factory.
		/// Factory can be <c>null</c>, meaning you don't want to create new object.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="factory">The factory.</param>
		/// <returns>Existing or newly created object.</returns>
		public V Get(K key, Func<K, V> factory = null)
		{
			lock (m_SyncRoot)
			{
				m_AccessCount++;

				CacheEntry<K, V> entry;
				bool exists = m_Entries.TryGetValue(key, out entry);

				if (exists && entry.IsAlive)
				{
					var cachedValue = entry.Value;
					if (cachedValue != null)
					{
						// second step of validation
						// it is very unlikely that object gets disposed between .IsAlive and .Value but it can be
						m_HitCount++;
						return cachedValue;
					}
				}

				if (factory == null)
				{
					return null;
				}

				var newValue = factory(key);

				Add(key, newValue, entry);
				return newValue;
			}
		}

		#endregion

		#region CheckIntegrity

		/// <summary>Checks integrity of cache. Used for debug only.</summary>
		/// <returns><c>true</c> if no errors found; <c>false</c> otherwise</returns>
		[Conditional("DEBUG")]
		public void CheckIntegrity()
		{
			var result = true;
			lock (m_SyncRoot)
			{
				var entry = m_Cycle;
				var sentinel = entry;

				if (entry == null)
				{
					if (m_Entries.Count > 0) result = false;
				}
				else
				{
					int count = 0;
					do
					{
						if (!m_Entries.ContainsKey(entry.Key) || m_Entries[entry.Key] != entry) result = false;
						entry = entry.Next;
						count++;
					} while (entry != sentinel);

					if (count != m_Entries.Count) result = false;
				}
			}

			if (!result)
				throw new InvalidOperationException("Cache integrity check failed");
		}

		#endregion

		#region utilities

		/// <summary>
		/// Adds the specified key/value pair to cache. Reuses entry if available.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="entry">The entry for given key (if found).</param>
		private void Add(K key, V value, CacheEntry<K, V> entry)
		{
			lock (m_SyncRoot)
			{
				if (entry == null)
				{
					entry = new CacheEntry<K, V>(key, value);
					if (m_Cycle == null)
					{
						m_Cycle = entry;
						m_Cycle.MakeCyclic();
					}
					else
					{
						m_Cycle.InsertBefore(entry);
						// but m_Cycle stay at the same item...
					}
					m_Entries.Add(entry.Key, entry);
				}
				else
				{
					// no need to relink in cyclic list
					// no need to reinsert into dictionary
					entry.Value = value;
				}

				UpdateSanityLoop();
			}
		}

		#endregion

		#region sanity thread

		/// <summary>Clears empty entries from cache.</summary>
		private void SanityLoop()
		{
			WaitHandle[] signals = { m_AbortSignal, m_ResumeSignal };

			while (true)
			{
				var signal = WaitHandle.WaitAny(signals, SanityThreadTimeout);

				lock (m_SyncRoot)
				{
					bool abort =
						// abort signaled
						(signal == 0) ||
						// retest if resume hasn't been set right after timeout
						(signal == WaitHandle.WaitTimeout && !m_ResumeSignal.WaitOne(0));

					if (abort)
					{
						m_SanityThread = null;
						break;
					}
					else
					{
						if (!m_Cycle.IsAlive)
						{
							m_Entries.Remove(m_Cycle.Key);
							m_Cycle = m_Cycle.RemoveItself();
							if (m_Entries.Count == 0)
							{
								m_ResumeSignal.Reset();
							}
						}
						else
						{
							m_Cycle = m_Cycle.Next;
						}
					}
				}

				Thread.Sleep(0);
			}
		}

		/// <summary>Starts or suspends sanity loop.</summary>
		private void UpdateSanityLoop()
		{
			lock (m_SyncRoot)
			{
				if (m_Entries.Count > 0)
				{
					// start or resume
					m_ResumeSignal.Set();

					if (m_SanityThread == null)
					{
						m_SanityThread = new Thread(SanityLoop) { IsBackground = true, Priority = ThreadPriority.Lowest };
						m_SanityThread.Start();
					}
				}
				else
				{
					// suspend
					m_ResumeSignal.Reset();
				}
			}
		}

		/// <summary>Stops the sanity loop. Kills the thread.</summary>
		private void StopSanityLoop()
		{
			var thread = m_SanityThread;

			if (thread != null)
			{
				// kill it!
				m_AbortSignal.Set();
				thread.Join();
			}
		}

		#endregion

		#region IDisposable Members

		private bool m_Disposed;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		private void Dispose(bool disposing)
		{
			if (!m_Disposed)
			{
				if (disposing)
					DisposeManaged();
				DisposeUnmanaged();
				m_Disposed = true;
			}
		}

		/// <summary>
		/// Disposes managed resources.
		/// </summary>
		protected virtual void DisposeManaged()
		{
			StopSanityLoop();

			lock (m_SyncRoot)
			{
				m_Entries.Clear();
				m_Cycle = null;
				m_AbortSignal.Dispose();
				m_ResumeSignal.Dispose();
			}
		}

		/// <summary>
		/// Disposes unmanaged resources.
		/// </summary>
		protected virtual void DisposeUnmanaged()
		{
			StopSanityLoop();
		}

		#endregion
	}
}