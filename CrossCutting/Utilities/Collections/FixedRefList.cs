using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// A list with fixed reference. It can be used when list reference is exposed, and we want to it to be valid 
	/// even if original list has been rebuilt.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class FixedRefList<T>: FixedRefBase<IList<T>, FixedRefList<T>>, IList<T>
	{
		#region constructor

		/// <summary>Initializes a new instance of the <see cref="FixedRefList&lt;T&gt;"/> class.</summary>
		public FixedRefList()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefList&lt;T&gt;"/> class.</summary>
		/// <param name="collection">The collection.</param>
		public FixedRefList(IList<T> collection)
			: base(collection)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefList&lt;T&gt;"/> class.</summary>
		/// <param name="getter">The getter for concrete collection.</param>
		/// <param name="setter">The setter for concrete collection.</param>
		public FixedRefList(Func<IList<T>> getter, Action<IList<T>> setter)
			: base(getter, setter)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefList&lt;T&gt;"/> class.</summary>
		/// <param name="collection">The collection.</param>
		/// <param name="getter">The getter for concrete collection.</param>
		/// <param name="setter">The setter for concrete collection.</param>
		public FixedRefList(IList<T> collection, Func<IList<T>> getter, Action<IList<T>> setter)
			: base(collection, getter, setter)
		{
		}
		 
		#endregion

		#region IList<T> Members

		/// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.</summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		/// <returns>The index of item if found in the list; otherwise, -1.</returns>
		public int IndexOf(T item)
		{
			return Data.IndexOf(item);
		}

		/// <summary>Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.</summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		public void Insert(int index, T item)
		{
			Data.Insert(index, item);
		}

		/// <summary>Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		public void RemoveAt(int index)
		{
			Data.RemoveAt(index);
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		public T this[int index]
		{
			get { return Data[index]; }
			set { Data[index] = value; }
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(T item)
		{
			Data.Add(item);
		}

		/// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
		public void Clear()
		{
			Data.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.</summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.</returns>
		public bool Contains(T item)
		{
			return Data.Contains(item);
		}

		/// <summary>Copies to.</summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			Data.CopyTo(array, arrayIndex);
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		public int Count
		{
			get { return Data.Count; }
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return Data.IsReadOnly; }
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public bool Remove(T item)
		{
			return Data.Remove(item);
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return Data.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Data.GetEnumerator();
		}

		#endregion
	}
}
