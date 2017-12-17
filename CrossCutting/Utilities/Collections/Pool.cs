using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using Indigo.CrossCutting.Utilities.Threading;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Simple Pool pattern. Provides pool of objects of the same type.
	/// It is used when object creation takes long time, otherwise don't use it.
	/// </summary>
	/// <typeparam name="T">Type objects.</typeparam>
	public class Pool<T>
	{
		#region fields

		/// <summary>The queue.</summary>
		private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

		/// <summary>Produce callback.</summary>
		private readonly Func<T> _produce;

		/// <summary>Recycle callback.</summary>
		private readonly Func<T, bool> _recycle;

		/// <summary>Maximum pool size.</summary>
		private readonly int _maximumSize;

		/// <summary>Current pool size.</summary>
		private int _currentSize;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="Pool&lt;T&gt;"/> class.</summary>
		/// <param name="maximumSize">The maximum size of the pool.</param>
		/// <param name="produce">The produce callback (required).</param>
		/// <param name="recycle">The recycle callback (optional).</param>
		public Pool(
			int maximumSize,
			Func<T> produce,
			Func<T, bool> recycle = null)
		{
			if (produce == null)
				throw new ArgumentNullException("produce", "produce is null.");

			_maximumSize = Math.Max(1, maximumSize);
			_currentSize = 0;
			_produce = produce;
			_recycle = recycle;
		}

		#endregion

		#region public interface

		/// <summary>Executes the action using one object from pool.</summary>
		/// <param name="action">The action.</param>
		public void Use(Action<T> action)
		{
			var item = Acquire();
			try
			{
				action(item);
			}
			finally
			{
				Recycle(item);
			}
		}

		/// <summary>Executes the action using object from pool.</summary>
		/// <typeparam name="R">Type of function result.</typeparam>
		/// <param name="action">The action.</param>
		/// <returns>Value returned by function.</returns>
		public R Use<R>(Func<T, R> action)
		{
			var item = Acquire();
			try
			{
				return action(item);
			}
			finally
			{
				Recycle(item);
			}
		}

		#endregion

		/// <summary>Acquires new object. It is taken from the pool, or if pool is empty 
		/// new one is created.</summary>
		/// <returns>Object.</returns>
		private T Acquire()
		{
			T item;

			if (!_queue.TryDequeue(out item))
			{
				item = _produce();
			}
			else
			{
				Interlocked.Decrement(ref _currentSize);
			}

			return item;
		}

		/// <summary>Recycles the specified item.</summary>
		/// <param name="item">The item.</param>
		private void Recycle(T item)
		{
			if (ReferenceEquals(null, item)) return;

			Task.Factory.StartNew(() =>
			{
				if (_recycle == null || _recycle(item))
				{
					if (Interlocked.Increment(ref _currentSize) <= _maximumSize)
					{
						try
						{
							_queue.Enqueue(item);
						}
						catch
						{
							// decrement it was not enqueued after all
							Interlocked.Decrement(ref _currentSize);
						}
					}
					else
					{
						// decrement because it was incremeneted above queue maximum size
						Interlocked.Decrement(ref _currentSize);
					}
				}
			}).IgnoreException();

		}
	}
}
