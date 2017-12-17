using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Read-only proxy set.</summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class ReadOnlySet<T>: ISet<T>
	{
		#region fields

		/// <summary>Physical storage.</summary>
		private readonly ISet<T> m_Internal;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="ReadOnlySet&lt;T&gt;"/> class.</summary>
		/// <param name="other">The other.</param>
		public ReadOnlySet(ISet<T> other)
		{
			m_Internal = other;
		}

		#endregion

		#region utilities

		/// <summary>Create ready to throw <see cref="NotSupportedException"/> exception.</summary>
		/// <param name="operationName">Name of the operation.</param>
		/// <returns><see cref="NotSupportedException"/></returns>
		private static NotSupportedException NotSupported(string operationName)
		{
			return new NotSupportedException(string.Format("Operation '{0}' is not supported", operationName));
		}

		#endregion

		#region ISet<T> Members

		/// <summary>Adds the specified item.</summary>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item has been added</returns>
		bool ISet<T>.Add(T item)
		{
			throw NotSupported("Add");
		}

		/// <summary>Removes <paramref name="other"/> items from set.</summary>
		/// <param name="other">The other.</param>
		void ISet<T>.ExceptWith(IEnumerable<T> other)
		{
			throw NotSupported("ExceptWith");
		}

		/// <summary>Intersects with <paramref name="other"/> items.</summary>
		/// <param name="other">The other.</param>
		void ISet<T>.IntersectWith(IEnumerable<T> other)
		{
			throw NotSupported("IntersectWith");
		}

		/// <summary>Determines whether set is a proper subset of <paramref name="other"/>.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if set is a proper subset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			return m_Internal.IsProperSubsetOf(other);
		}

		/// <summary>Determines whether set is a proper superset of <paramref name="other"/>.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if set is a proper superset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			return m_Internal.IsProperSupersetOf(other);
		}

		/// <summary>Determines whether set is a subset of <paramref name="other"/>.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if set is a subset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			return m_Internal.IsSubsetOf(other);
		}

		/// <summary>Determines whether set is a proper superset of <paramref name="other"/>.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if set is a proper superset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			return m_Internal.IsSupersetOf(other);
		}

		/// <summary>Determines whether set overlaps with <paramref name="other"/>.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if set overlaps with <paramref name="other"/>; <c>false</c> otherwise</returns>
		public bool Overlaps(IEnumerable<T> other)
		{
			return m_Internal.Overlaps(other);
		}

		/// <summary>Determines whether sets are equal.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if sets are equal; <c>false</c> otherwise;</returns>
		public bool SetEquals(IEnumerable<T> other)
		{
			return m_Internal.SetEquals(other);
		}

		/// <summary>Modifies the current set so that it contains only elements that are present either in the current 
		/// set or in the specified collection, but not both. </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
		{
			throw NotSupported("SymmetricExceptWith");
		}

		/// <summary>Modifies the current set so that it contains all elements that are present in both the current set 
		/// and in the specified collection.</summary>
		/// <param name="other">The collection to compare to the current set.</param>
		void ISet<T>.UnionWith(IEnumerable<T> other)
		{
			throw NotSupported("UnionWith");
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> 
		/// is read-only.</exception>
		void ICollection<T>.Add(T item)
		{
			throw NotSupported("Add");
		}

		/// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> 
		/// is read-only.</exception>
		void ICollection<T>.Clear()
		{
			throw NotSupported("Clear");
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific 
		/// value.</summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; 
		/// otherwise, false.</returns>
		public bool Contains(T item)
		{
			return m_Internal.Contains(item);
		}

		/// <summary>Copies items to array.</summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			m_Internal.CopyTo(array, arrayIndex);
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count
		{
			get { return m_Internal.Count; }
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return true; }
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		bool ICollection<T>.Remove(T item)
		{
			throw NotSupported("Remove");
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return m_Internal.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Internal.GetEnumerator();
		}

		#endregion
	}
}
