using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Indigo.CrossCutting.Utilities.Extensions;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// IEnumerator enumerating item in separate thread and string them in the queue.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class AsyncEnumerator<T>: IEnumerator<T>
	{
		#region fields

		/// <summary>
		/// Internal enumerator.
		/// </summary>
		private readonly IEnumerator<T> m_Internal;

        /// <summary>
		/// Internal queue.
		/// </summary>
		private readonly BlockingQueue<T> m_Queue;

		/// <summary>
		/// Indicates if current item is set.
		/// </summary>
		private bool m_HasCurrentItem;

		/// <summary>
		/// Current item.
		/// </summary>
		private T m_CurrentItem;

		/// <summary>Enumerating task.</summary>
		private Task m_LoopTask;

		/// <summary>Cancelation token.</summary>
		private CancellationTokenSource m_LoopCancel;

		/// <summary>Indicates if enumerator has been disposed.</summary>
		private bool m_Disposed;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncEnumerator&lt;T&gt;"/> class.
		/// Starts asynchronous enumeration immediately.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <param name="maximumSize">The maximum size.</param>
		public AsyncEnumerator(IEnumerator<T> other, int maximumSize)
		{
			m_Internal = other;
			m_Queue = new BlockingQueue<T>(maximumSize);
			Start();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncEnumerator&lt;T&gt;"/> class.
		/// Starts asynchronous enumeration immediately.
		/// </summary>
		/// <param name="other">The other.</param>
		public AsyncEnumerator(IEnumerator<T> other)
			: this(other, int.MaxValue)
		{
		}

		#endregion

		#region thread

		/// <summary>
		/// Starts this thread.
		/// </summary>
		private void Start()
		{
			Stop();
			m_LoopCancel = new CancellationTokenSource();
			m_LoopTask = Task.Factory.StartNew(Loop, m_LoopCancel.Token);
		}

		/// <summary>
		/// Stops this thread.
		/// </summary>
		private void Stop()
		{
			m_LoopCancel.SafeExec(t => t.Cancel());
			m_LoopTask.SafeExec(t => t.Wait());
		}

		/// <summary>
		/// Thread loop.
		/// </summary>
		private void Loop()
		{
			var token = m_LoopCancel.Token;

			token.ThrowIfCancellationRequested();

			try
			{
				while (true)
				{
					token.ThrowIfCancellationRequested();
					if (!m_Internal.MoveNext()) break;
					m_Queue.Enqueue(m_Internal.Current);
				}
			}
			finally
			{
				m_Queue.Seal();
			}
		}

		#endregion

		#region next, current, reset

		/// <summary>
		/// Gets the next item. Waits for item to appear in a queue, or 
		/// returns false if there are no more items (means: queue has no items
		/// and is sealed, so no more items are going to be added).
		/// </summary>
		/// <returns></returns>
		private bool GetNextItem()
		{
			try
			{
				m_CurrentItem = m_Queue.Dequeue();
				m_HasCurrentItem = true;
			}
			catch (InvalidOperationException)
			{
				m_HasCurrentItem = false;
			}
			return m_HasCurrentItem;
		}

		/// <summary>
		/// Gets the current item or throws <see cref="InvalidOperationException"/> is item has
		/// not been set.
		/// </summary>
		/// <returns></returns>
		private T GetCurrentItem()
		{
			if (!m_HasCurrentItem)
				throw new InvalidOperationException("No current item found");
			return m_CurrentItem;
		}

		/// <summary>
		/// Resets the enumerator. Stops current thread, resets the queue and restarts internal
		/// enumerator assuming it can be Reset. If internal enumerator cannot be reset and throws exception
		/// it will screw whole enumeration, you should not continue.
		/// </summary>
		private void ResetEnumerator()
		{
			Stop();
			m_Internal.Reset();
			Start();
		}

		#endregion

		#region IEnumerator<T> Members

		/// <summary>
		/// Gets the element in the collection at the current position of the enumerator.
		/// </summary>
		/// <value></value>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		public T Current
		{
			get { return GetCurrentItem(); }
		}

		#endregion

		#region IDisposable Members

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
			if (m_Disposed)
				return;

			if (disposing) DisposeManaged();
				DisposeUnmanaged();
			m_Disposed = true;
		}

		/// <summary>
		/// Disposes managed resources.
		/// </summary>
		protected virtual void DisposeManaged()
		{
			Stop();
			m_Queue.Clear();
			m_Internal.Dispose();
		}

		/// <summary>
		/// Disposes unmanaged resources.
		/// </summary>
		protected virtual void DisposeUnmanaged()
		{
		}

		#endregion

		#region IEnumerator Members

		/// <summary>
		/// Gets the element in the collection at the current position of the enumerator.
		/// </summary>
		/// <value></value>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		object IEnumerator.Current
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Advances the enumerator to the next element of the collection.
		/// </summary>
		/// <returns>
		/// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
		/// </returns>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		public bool MoveNext()
		{
			return GetNextItem();
		}

		/// <summary>
		/// Sets the enumerator to its initial position, which is before the first element in the collection.
		/// Stops current thread, resets the queue and restarts internal
		/// enumerator assuming it can by reset. If internal enumerator cannot be reset and throw exception
		/// it will screw whole enumeration, you should not continue.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		public void Reset()
		{
			ResetEnumerator();
		}

		#endregion
	}
}
