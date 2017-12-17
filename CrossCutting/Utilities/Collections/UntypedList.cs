using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Translates <see cref="IList&lt;T&gt;"/> to <see cref="IList"/> on the fly.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class UntypedList<T>:IList
	{
		#region fields

		/// <summary>
		/// Original list.
		/// </summary>
		private IList<T> m_List;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="UntypedList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="list">The list.</param>
		public UntypedList(IList<T> list)
		{
			m_List = list;
		}

		#endregion

		#region IList Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"/> to add to the <see cref="T:System.Collections.IList"/>.</param>
		/// <returns>
		/// The position into which the new element was inserted.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
		public int Add(object value)
		{
			int count = m_List.Count;
			T typedValue = (T)value;
			m_List.Add(typedValue);

			// sanity check, disappears in 'release'
			Debug.Assert(object.ReferenceEquals(m_List[count], typedValue) || m_List[count].Equals(typedValue));

			return count; // NOTE, assumed that item has been added on the end
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.IList"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only. </exception>
		public void Clear()
		{
			m_List.Clear();
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
		/// <returns>
		/// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
		/// </returns>
		public bool Contains(object value)
		{
			return m_List.Contains((T)value);
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
		/// <returns>
		/// The index of <paramref name="value"/> if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf(object value)
		{
			return m_List.IndexOf((T)value);
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to insert into the <see cref="T:System.Collections.IList"/>.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
		/// <exception cref="T:System.NullReferenceException">
		/// 	<paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
		public void Insert(int index, object value)
		{
			m_List.Insert(index, (T)value);
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.</returns>
		public bool IsFixedSize
		{
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.IList"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return m_List.IsReadOnly; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"/> to remove from the <see cref="T:System.Collections.IList"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
		public void Remove(object value)
		{
			m_List.Remove((T)value);
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.IList"/> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
		public void RemoveAt(int index)
		{
			m_List.RemoveAt(index);
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Object"/> at the specified index.
		/// </summary>
		/// <value></value>
		public object this[int index]
		{
			get { return m_List[index]; }
			set { m_List[index] = (T)value; }
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="array"/> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="array"/> is multidimensional.-or- <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
		/// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
		public void CopyTo(Array array, int index)
		{
			int count = m_List.Count;
			ArrayList list = new ArrayList(count);
			for (int i = 0; i < count; i++) list.Add(m_List[i]);
			list.CopyTo(array, index);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</returns>
		public int Count
		{
			get { return m_List.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
		/// </summary>
		/// <value></value>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
		public bool IsSynchronized
		{
			get { return false; }
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		/// <value></value>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
		public object SyncRoot
		{
			get { return m_List; }
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator GetEnumerator()
		{
			return m_List.GetEnumerator();
		}

		#endregion
	}
}
