using System;
using System.Collections.Generic;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Same as Queue except Dequeue function blocks until there is an object to return.
	/// </summary>
	public class BlockingQueue<T>
	{
		#region fields

		private readonly object m_Lock = new object();
		private readonly Queue<T> m_Queue;

		private bool m_Open = true;
		private bool m_Sealed; // = false;

		private int m_MaximumSize = int.MaxValue;

		#endregion

		#region properties

		/// <summary>
		/// Gets flag indicating if queue has been closed.
		/// </summary>
		public bool Closed
		{
			get { return !m_Open; }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="BlockingQueue&lt;T&gt;"/> is sealed.
		/// </summary>
		/// <value><c>true</c> if sealed; otherwise, <c>false</c>.</value>
		public bool Sealed
		{
			get { return m_Sealed; }
		}

		/// <summary>
		/// Gets the synchronisation root. Although this class des not need synchronisation
		/// it might be useful to aquire exclusive lock to it sometimes.
		/// </summary>
		/// <value>The sync root.</value>
		public object SyncRoot
		{
			get { return m_Lock; }
		}

		/// <summary>
		/// Gets or sets the maximum size.
		/// </summary>
		/// <value>The maximum size.</value>
		public int MaximumSize
		{
			get { return m_MaximumSize; }
			set { m_MaximumSize = value; }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Create new BlockingQueue.
		/// </summary>
		/// <param name="collection">The System.Collections.ICollection to copy elements from</param>
		public BlockingQueue(IEnumerable<T> collection)
			: this(collection, int.MaxValue)
		{
		}

		/// <summary>
		/// Create new BlockingQueue.
		/// </summary>
		public BlockingQueue()
			: this(int.MaxValue)
		{
		}

		/// <summary>
		/// Create new BlockingQueue.
		/// </summary>
		/// <param name="collection">The System.Collections.ICollection to copy elements from</param>
		/// <param name="maximumSize">The maximum size.</param>
		public BlockingQueue(IEnumerable<T> collection, int maximumSize)
		{
			m_Queue = new Queue<T>(collection);
			m_MaximumSize = Math.Max(maximumSize, 1);
		}

		/// <summary>
		/// Create new BlockingQueue.
		/// </summary>
		/// <param name="capacity">The initial number of elements that the queue can contain</param>
		/// <param name="maximumSize">The maximum size.</param>
		public BlockingQueue(int capacity, int maximumSize)
		{
			m_Queue = new Queue<T>(capacity);
			m_MaximumSize = Math.Max(maximumSize, 1);
		}

		/// <summary>
		/// Create new BlockingQueue.
		/// </summary>
		/// <param name="maximumSize">The maximum size.</param>
		public BlockingQueue(int maximumSize)
		{
			m_Queue = new Queue<T>();
			m_MaximumSize = Math.Max(maximumSize, 1);
		}

		/// <summary>
		/// BlockingQueue Destructor (Close queue, resume any waiting thread).
		/// </summary>
		~BlockingQueue()
		{
			Close();
		}

		#endregion

		#region utilities

		/// <summary>
		/// Remove all objects from the Queue.
		/// </summary>
		public void Clear()
		{
			lock (m_Lock)
			{
				m_Queue.Clear();
				Monitor.PulseAll(m_Lock);
			}
		}

		/// <summary>
		/// Seals this instance. Resumes all waiting threads. They should retest their status.
		/// </summary>
		public void Seal()
		{
			lock (m_Lock)
			{
				m_Sealed = true;
				Monitor.PulseAll(m_Lock);
			}
		}

		/// <summary>
		/// Remove all objects from the Queue, resume all dequeue threads.
		/// </summary>
		public void Close()
		{
			lock (m_Lock)
			{
				m_Open = false;
				m_Queue.Clear();
			}
		}

		/// <summary>
		/// Removes and returns the object at the beginning of the Queue.
		/// </summary>
		/// <returns>Object in queue.</returns>
		public T Dequeue()
		{
			return Dequeue(Timeout.Infinite);
		}

		/// <summary>
		/// Removes and returns the object at the beginning of the Queue.
		/// </summary>
		/// <param name="timeout">time to wait before returning</param>
		/// <returns>Object in queue.</returns>
		public T Dequeue(TimeSpan timeout)
		{
			return Dequeue((int)timeout.TotalMilliseconds);
		}

		/// <summary>
		/// Indicates if process should wait for dequeue.
		/// </summary>
		/// <returns></returns>
		private bool WaitForDequeue()
		{
			return m_Open && !m_Sealed && (m_Queue.Count <= 0);
		}

		/// <summary>
		/// Indicates if process should wait for enqueue.
		/// </summary>
		/// <returns></returns>
		private bool WaitForEnqueue()
		{
			return m_Open && !m_Sealed && (m_Queue.Count > m_MaximumSize);
		}

		/// <summary>Removes and returns the object at the beginning of the Queue.</summary>
		/// <param name="timeout">time to wait before returning (in milliseconds)</param>
		/// <returns>Object in queue.</returns>
		public T Dequeue(int timeout)
		{
			lock (m_Lock)
			{
				while (WaitForDequeue())
				{
					if (!Monitor.Wait(m_Lock, timeout))
						throw new TimeoutException("Timeout on Dequeue");
				}

				if (m_Open)
				{
					// it has to throw exception on the end of the queue, this is the only way
					// to return without returning value
					T result = m_Queue.Dequeue();
					Monitor.PulseAll(m_Lock);
					return result;
				}

				throw new InvalidOperationException("Queue is closed");
			}
		}

		/// <summary>
		/// Adds an object to the end of the Queue.
		/// </summary>
		/// <param name="item">Object to put in queue</param>
		/// <param name="timeout">The timeout.</param>
		public void Enqueue(T item, int timeout)
		{
			lock (m_Lock)
			{
				while (WaitForEnqueue())
				{
					if (!Monitor.Wait(m_Lock, timeout))
						throw new InvalidOperationException("Timeout on Dequeue");
				}

				if (!m_Open || m_Sealed)
				{
					throw new InvalidOperationException("Cannot Enqueue. Queue is closed or sealed.");
				}

				m_Queue.Enqueue(item);
				Monitor.PulseAll(m_Lock);
			}
		}

		/// <summary>
		/// Adds an object to the end of the Queue.
		/// </summary>
		/// <param name="item">Object to put in queue</param>
		/// <param name="timeout">The timeout.</param>
		public void Enqueue(T item, TimeSpan timeout)
		{
			Enqueue(item, (int)timeout.TotalMilliseconds);
		}

		/// <summary>
		/// Enqueues the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Enqueue(T item)
		{
			Enqueue(item, Timeout.Infinite);
		}

		#endregion
	}
}
