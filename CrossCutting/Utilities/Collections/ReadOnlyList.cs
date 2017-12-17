using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Read-only proxy collection.
	/// </summary>
	/// <typeparam name="T">Any type.</typeparam>
	public class ReadOnlyList<T>: IList<T>
	{

		#region fields

		/// <summary>
		/// Internal list.
		/// </summary>
		private readonly IList<T> m_Internal;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="other">The other.</param>
		public ReadOnlyList(IList<T> other)
		{
			if (other == null)
				throw new ArgumentNullException("other", "other is null.");
			m_Internal = other;
		}

		#endregion

		#region utilities

		/// <summary>Returns read to throw <see cref="NotSupportedException"/> exception.</summary>
		/// <param name="operationName">Name of the operation.</param>
		/// <returns><see cref="NotSupportedException"/>.</returns>
		private static NotSupportedException NotSupported(string operationName)
		{
			return new NotSupportedException(
				string.Format("Operation '{0}' is not supported", operationName));
		}

		#endregion

		#region IList<T> Members

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <returns>
		/// The index of <paramref name="item"/> if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf(T item)
		{
			return m_Internal.IndexOf(item);
		}

		/// <summary>
		/// Gets the <typeparamref name="T"/> at the specified index.
		/// </summary>
		/// <value></value>
		public T this[int index]
		{
			get { return m_Internal[index]; }
		}

		T IList<T>.this[int index]
		{
			get { return m_Internal[index]; }
			set { throw NotSupported("this[].Set"); }
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
		/// Actually, it immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
		void IList<T>.Insert(int index, T item)
		{
			throw NotSupported("Insert");
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
		/// Actually, it immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
		void IList<T>.RemoveAt(int index)
		{
			throw NotSupported("RemoveAt");
		}

		#endregion

		#region ICollection<T> Members


		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
		/// </returns>
		public bool Contains(T item)
		{
			return m_Internal.Contains(item);
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an 
		/// <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied 
		/// from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have 
		/// zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-
		/// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of 
		/// elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space 
		/// from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type 
		/// <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
		/// </exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			m_Internal.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count
		{
			get { return m_Internal.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// And it is.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return true; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Actually, it immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		bool ICollection<T>.Remove(T item)
		{
			throw NotSupported("Remove");
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Actually, it immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		void ICollection<T>.Add(T item)
		{
			throw NotSupported("Add");
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Actually, it immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		void ICollection<T>.Clear()
		{
			throw NotSupported("Clear");
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return m_Internal.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Internal.GetEnumerator();
		}

		#endregion

	}
}
