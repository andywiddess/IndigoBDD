using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Queue which accepts IEnumerable and uses it only when needed.
	/// </summary>
	/// <typeparam name="T">Type of element.</typeparam>
	public class RepeatQueue<T>: IEnumerable<T>
	{
		#region internal fields

		private IEnumerator<T> m_Stream;
		private Queue<T> m_Queue = new Queue<T>();

		private T m_Buffer;
		private bool m_BufferAvailable;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="RepeatQueue&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="items">The items.</param>
		public RepeatQueue(IEnumerable<T> items)
		{
			m_Stream = items.GetEnumerator();
			EnsureBuffer();
		}

		#endregion

		#region interface

		/// <summary>
		/// Enqueues the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Enqueue(T item)
		{
			m_Queue.Enqueue(item);
		}

		/// <summary>
		/// Dequeues item from queue.
		/// </summary>
		/// <returns></returns>
		public T Dequeue()
		{
			if (EnsureBuffer())
			{
				return DequeueBuffer();
			}
			else
			{
				return m_Queue.Dequeue();
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="RepeatQueue&lt;T&gt;"/> is empty.
		/// </summary>
		/// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
		public bool Empty
		{
			get
			{
				if (m_Queue.Count > 0) return false;
				return !EnsureBuffer();
			}
		}

		/// <summary>
		/// Gets the item count in queue. Count might be unknown (<c>null</c>)
		/// if source enumerator hasn't reached it's end yet. Use <see cref="KnownCount"/>
		/// to check how many items are in queue for sure.
		/// </summary>
		/// <value>The count.</value>
		public int? Count
		{
			get { return EnsureBuffer() ? (int?)null : (int?)m_Queue.Count; }
		}

		/// <summary>
		/// Gets the count of known items. The actual number of items can be graeter.
		/// </summary>
		/// <value>The known count.</value>
		public int KnownCount
		{
			get { return (EnsureBuffer() ? 1 : 0) + m_Queue.Count; }
		}

		#endregion

		#region utility

		private bool EnsureBuffer()
		{
			if (!m_BufferAvailable)
			{
				if (m_Stream.MoveNext())
				{
					m_Buffer = m_Stream.Current;
					m_BufferAvailable = true;
				}
			}
			return m_BufferAvailable;
		}

		private T DequeueBuffer()
		{
			if (m_BufferAvailable)
			{
				T result = m_Buffer;
				m_Buffer = default(T);
				m_BufferAvailable = false;
				return result;
			}
			throw new InvalidOperationException("Cannot dequeue item from empty buffer");
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection. 
		/// IMPORTANT: Enumerating empties the collection!
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			while (!Empty)
			{
				yield return Dequeue();
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection. 
		/// IMPORTANT: Enumerating empties the collection!
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
