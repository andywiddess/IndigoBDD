using System;
using System.Collections.Generic;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Lightweight blocking pipe. It servers the same purpose as BlockingQueue 
	/// while it is a little bit faster for unbounded one-producer-one-consumer queue.
	/// </summary>
	/// <typeparam name="T">Any type.</typeparam>
	public class LightweightPipe<T>
	{
		#region fields

		/// <summary>Internal queue.</summary>
		private readonly Queue<T> _queue = new Queue<T>(1024);

		/// <summary>Internal lock.</summary>
		private readonly object _lock = new object();

		/// <summary>Length of the queue.</summary>
		private int _length;

		/// <summary>Flag indicating that no more items will be added.</summary>
		private bool _closed;

		#endregion

		#region public interface

		/// <summary>Enqueues the specified item.</summary>
		/// <param name="item">The item.</param>
		public void Enqueue(T item)
		{
			lock (_lock)
			{
				if (_closed) 
					throw new InvalidOperationException("Cannot add to closed pipe");
				_queue.Enqueue(item);
				_length++;
				Monitor.PulseAll(_lock);
			}
		}

		/// <summary>Dequeues the item. It there are no items in the queue it waits for
		/// new items to arrive or queue is closed.</summary>
		/// <param name="item">The dequeued item.</param>
		/// <returns><c>true</c> if operation succeeded, <c>false</c> if there are no 
		/// more items and queue is closed.</returns>
		public bool Dequeue(out T item)
		{
			lock (_lock)
			{
				while (true)
				{
					if (_length > 0)
					{
						// there are items in the queue
						item = _queue.Dequeue();
						_length--;
						return true;
					}
					if (_closed)
					{
						// no items, and queue has been closed
						item = default(T);
						return false;
					}
					// no items but not closed either, so wait for Pulse when
					// item has been added or queue has been closed
					Monitor.Wait(_lock);
				}
			}
		}

		/// <summary>Enqueues many items.</summary>
		/// <param name="items">The items.</param>
		public void EnqueueMany(IEnumerable<T> items)
		{
			items.ForEach(Enqueue);
		}

		/// <summary>Dequeues the many.</summary>
		/// <returns>Sequence of items.</returns>
		public IEnumerable<T> DequeueMany()
		{
			T item;
			while (Dequeue(out item)) yield return item;
		}

		/// <summary>Closes the pipe, so no new items can be added and consumer will be 
		/// notified that queue is empty.</summary>
		public void Close()
		{
			lock (_lock)
			{
				_closed = true;
				Monitor.PulseAll(_lock);
			}
		}

		#endregion
	}
}
