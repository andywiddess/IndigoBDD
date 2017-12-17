using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Emulates IEnumerator on one item.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class OneItemEnumerator<T>: IEnumerator<T>
	{
		#region fields

		/// <summary>
		/// Indicates if enumerator has been already used.
		/// </summary>
		private bool m_Used;

		/// <summary>
		/// Item.
		/// </summary>
		private readonly T m_Current;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="OneItemEnumerator&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="item">The item.</param>
		public OneItemEnumerator(T item)
		{
			m_Used = false;
			m_Current = item;
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
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
			get { return m_Current; }
		}

		/// <summary>
		/// Gets the element in the collection at the current position of the enumerator.
		/// </summary>
		/// <value></value>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		object IEnumerator.Current
		{
			get { return m_Current; }
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
			if (m_Used)
			{
				return false;
			}
			else
			{
				m_Used = true;
				return true;
			}
		}

		/// <summary>
		/// Sets the enumerator to its initial position, which is before the first element in the collection.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		public void Reset()
		{
			m_Used = false;
		}

		#endregion
	}
}
