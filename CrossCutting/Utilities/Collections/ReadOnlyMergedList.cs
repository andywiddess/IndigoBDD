using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Provides a readonly proxy list of merged lists
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ReadOnlyMergedList<T>: IList<T>
	{
		#region Member Variables
		/// <summary>
		/// The internal lists of lists
		/// </summary>
		private List<IList<T>> m_lists;
		#endregion

		#region Constructors and Finalisers

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyMergedList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="listsToMerge">The lists to merge.</param>
		public ReadOnlyMergedList(IEnumerable<IList<T>> listsToMerge)
		{
			if (listsToMerge == null)
				throw new ArgumentNullException("listsToMerge", "listsToMerge is null.");

			m_lists = new List<IList<T>>();
			foreach (IList<T> list in listsToMerge)
				m_lists.Add(list);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyMergedList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="listsToMerge">The lists to merge.</param>
		public ReadOnlyMergedList(params IList<T>[] listsToMerge)
		{
			m_lists = new List<IList<T>>();

			foreach (IList<T> list in listsToMerge)
				m_lists.Add(list);
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
			int index = 0;

			foreach (IList<T> list in m_lists)
			{
				if (list.Contains(item))
					return index + list.IndexOf(item);
				else
					index += list.Count;
			}

			return -1;
		}

		/// <summary>
		/// Gets the item at.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private T GetItemAt(int index)
		{
			int currMax = 0;
			foreach (IList<T> list in m_lists)
			{
				if (list.Count > 0)
				{
					if (index <= currMax)
						return list[index - currMax];
					else
						currMax += list.Count;
				}
			}

			throw new IndexOutOfRangeException("Item index out of range");
		}

		/// <summary>
		/// Gets or sets the object of type <typeparamref name="T"/> at the specified index.
		/// </summary>
		/// <value>Value</value>
		public T this[int index]
		{
			get { return GetItemAt(index); }
		}

		/// <summary>
		/// Gets or sets the <typeparamref name="T"/> at the specified index.
		/// </summary>
		/// <value></value>
		T IList<T>.this[int index]
		{
			get { return GetItemAt(index); }
			set
			{
				throw new NotSupportedException("This operation is not supported by read-only list");
			}
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
		/// </exception>
		void IList<T>.Insert(int index, T item)
		{
			throw new NotSupportedException("This operation is not supported by read-only list");
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
		/// </exception>
		void IList<T>.RemoveAt(int index)
		{
			throw new NotSupportedException("This operation is not supported by read-only list");
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </exception>
		void ICollection<T>.Add(T item)
		{
			throw new NotSupportedException("This operation is not supported by read-only list");
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </exception>
		void ICollection<T>.Clear()
		{
			throw new NotSupportedException("This operation is not supported by read-only list");
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
		/// </returns>
		public bool Contains(T item)
		{
			foreach (IList<T> list in m_lists)
			{
				if (list.Contains(item))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="arrayIndex"/> is less than 0.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="array"/> is multidimensional.
		/// -or-
		/// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
		/// -or-
		/// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
		/// -or-
		/// Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
		/// </exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			foreach (IList<T> list in m_lists)
			{
				list.CopyTo(array, arrayIndex);
				arrayIndex += list.Count;
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		public int Count
		{
			get
			{
				int count = 0;

				foreach (IList<T> list in m_lists)
					count += list.Count;

				return count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
		/// </returns>
		public bool IsReadOnly
		{
			get { return true; }
		}

		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		bool ICollection<T>.Remove(T item)
		{
			throw new NotSupportedException("This operation is not supported by read-only list");
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
			foreach (IList<T> list in m_lists)
			{
				foreach (T item in list)
					yield return item;
			}
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
