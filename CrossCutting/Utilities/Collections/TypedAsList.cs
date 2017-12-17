using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Encapsulation for <see cref="IList"/> making items typed.</summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class TypedAsList<T>: IList<T>
	{
		#region fields

		private readonly IList m_List;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="T:TypeAsList{T}"/> class.</summary>
		/// <param name="list">The list.</param>
		public TypedAsList(IList list)
		{
			if (list == null)
				throw new ArgumentNullException("list", "list is null.");
			m_List = list;
		}

		#endregion

		#region IList<T> Members

		/// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.</summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		/// <returns>The index of item if found in the list; otherwise, -1.</returns>
		public int IndexOf(T item)
		{
			return m_List.IndexOf(item);
		}

		/// <summary>Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.</summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		public void Insert(int index, T item)
		{
			m_List.Insert(index, item);
		}

		/// <summary>Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		public void RemoveAt(int index)
		{
			m_List.RemoveAt(index);
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		public T this[int index]
		{
			get { return (T)m_List[index]; }
			set { m_List[index] = value; }
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(T item)
		{
			m_List.Add(item);
		}

		/// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
		public void Clear()
		{
			m_List.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.</summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.</returns>
		public bool Contains(T item)
		{
			return m_List.Contains(item);
		}

		/// <summary>Copies all elements to given array.</summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			m_List.CopyTo(array, arrayIndex);
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		public int Count
		{
			get { return m_List.Count; }
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return m_List.IsReadOnly; }
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public bool Remove(T item)
		{
			var index = IndexOf(item);
			if (index < 0)
			{
				return false;
			}

			m_List.RemoveAt(index);
			return true;
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return new TypedAsEnumerator<T>(m_List.GetEnumerator());
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_List.GetEnumerator();
		}

		#endregion
	}
}