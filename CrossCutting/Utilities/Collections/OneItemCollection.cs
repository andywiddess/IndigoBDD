using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{

	/// <summary>
	/// Emulates ICollection with one item.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class OneItemCollection<T>: ICollection<T>
	{
		#region fields

		/// <summary>
		/// Number of items in collection (0 or 1).
		/// </summary>
		private int m_Count;

		/// <summary>
		/// Item.
		/// </summary>
		private T m_Item;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the only item. Sets item count to 1.
		/// </summary>
		/// <value>The item.</value>
		public T Item
		{
			get { return m_Item; }
			set
			{
				m_Item = value;
				m_Count = 1;
			}
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="OneItemCollection&lt;T&gt;"/> class with 0 items.
		/// </summary>
		public OneItemCollection()
		{
			m_Item = default(T);
			m_Count = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OneItemCollection&lt;T&gt;"/> class with specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public OneItemCollection(T item)
		{
			m_Item = item;
			m_Count = 1;
		}

		/// <summary>
		/// Creates one item collection.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>ICollection with given item.</returns>
		public static OneItemCollection<T> For(T item)
		{
			return new OneItemCollection<T>(item);
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Adds an item to the collection. Throws an exception if collection already contains one item.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public void Add(T item)
		{
			if (m_Count == 0)
			{
				m_Item = item;
				m_Count = 1;
			}
			else
			{
				throw new NotSupportedException("Cannot add more items to OneItemCollection");
			}
		}

		/// <summary>
		/// Removes all items from the collection.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			m_Count = 0;
			m_Item = default(T);
		}

		/// <summary>
		/// Determines whether the collection contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the collection.</param>
		/// <returns>
		/// true if <paramref name="item"/> is found in the collection; otherwise, false.
		/// </returns>
		public bool Contains(T item)
		{
			return (m_Count == 1) && (m_Item.Equals(item));
		}

		/// <summary>
		/// Copies the elements of the collection to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="array"/> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="arrayIndex"/> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (m_Count != 0)
			{
				array[arrayIndex] = m_Item;
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count
		{
			get { return m_Count; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public bool Remove(T item)
		{
			if ((m_Count != 0) && (m_Item.Equals(item)))
			{
				Clear();
				return true;
			}
			return false;
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
			if (m_Count == 0)
			{
				return EmptyEnumerator<T>.Default;
			}
			else
			{
				return new OneItemEnumerator<T>(m_Item);
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
