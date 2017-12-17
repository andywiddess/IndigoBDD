using System.Collections.Generic;
using System;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// A set with fixed reference. It can be used when list reference is exposed, and we want to it to be valid 
	/// even if original list has been rebuilt.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class FixedRefSet<T>: FixedRefBase<ISet<T>, FixedRefSet<T>>, ISet<T>
	{
		#region constructor

		/// <summary>Initializes a new instance of the <see cref="FixedRefSet&lt;T&gt;"/> class.</summary>
		public FixedRefSet()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefSet&lt;T&gt;"/> class.</summary>
		/// <param name="collection">The collection.</param>
		public FixedRefSet(ISet<T> collection)
			: base(collection)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefSet&lt;T&gt;"/> class.</summary>
		/// <param name="getter">The getter for concrete collection.</param>
		/// <param name="setter">The setter for concrete collection.</param>
		public FixedRefSet(Func<ISet<T>> getter, Action<ISet<T>> setter)
			: base(getter, setter)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefSet&lt;T&gt;"/> class.</summary>
		/// <param name="collection">The collection.</param>
		/// <param name="getter">The getter for concrete collection.</param>
		/// <param name="setter">The setter for concrete collection.</param>
		public FixedRefSet(ISet<T> collection, Func<ISet<T>> getter, Action<ISet<T>> setter)
			: base(collection, getter, setter)
		{
		}

		#endregion

		#region ISet<T> Members

		/// <summary>Adds an element to the current set and returns a value to indicate if the element was successfully 
		/// added.</summary>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item has been added.</returns>
		public bool Add(T item)
		{
			return Data.Add(item);
		}

		/// <summary>Removes all elements in the specified collection from the current set.</summary>
		/// <param name="other">The other collection.</param>
		public void ExceptWith(IEnumerable<T> other)
		{
			Data.ExceptWith(other);
		}

		/// <summary>Modifies the current set so that it contains only elements that are also in a specified 
		/// collection.</summary>
		/// <param name="other">The other collection.</param>
		public void IntersectWith(IEnumerable<T> other)
		{
			Data.IntersectWith(other);
		}

		/// <summary>Determines whether the current set is a property (strict) subset of a specified collection.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns><c>true</c> if the current set is a property (strict) subset of a specified collection; 
		/// otherwise, <c>false</c>.</returns>
		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			return Data.IsProperSubsetOf(other);
		}

		/// <summary>Determines whether the current set is a correct superset of a specified collection.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns><c>true</c> if the current set is a correct superset of a specified collection; 
		/// otherwise, <c>false</c>.</returns>
		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			return Data.IsProperSupersetOf(other);
		}

		/// <summary>Determines whether a set is a subset of a specified collection.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns><c>true</c> if set is a subset of a specified collection; otherwise, <c>false</c>.</returns>
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			return Data.IsSubsetOf(other);
		}

		/// <summary>Determines whether the current set is a superset of a specified collection.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns><c>true</c> if the current set is a superset of a specified collection; otherwise, <c>false</c>.</returns>
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			return Data.IsSupersetOf(other);
		}

		/// <summary>Determines whether the current set overlaps with the specified collection.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns>the current set overlaps with the specified collection</returns>
		public bool Overlaps(IEnumerable<T> other)
		{
			return Data.Overlaps(other);
		}

		/// <summary>Determines whether the current set and the specified collection contain the same elements.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns><c>true</c> if the current set and the specified collection contain the same elements; 
		/// <c>false</c> otherwise</returns>
		public bool SetEquals(IEnumerable<T> other)
		{
			return Data.SetEquals(other);
		}

		/// <summary>Modifies the current set so that it contains only elements that are present either in the current set or 
		/// in the specified collection, but not both. </summary>
		/// <param name="other">The other collection.</param>
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			Data.SymmetricExceptWith(other);
		}

		/// <summary>Modifies the current set so that it contains all elements that are present in both the current set and 
		/// in the specified collection.</summary>
		/// <param name="other">The other collection.</param>
		public void UnionWith(IEnumerable<T> other)
		{
			Data.UnionWith(other);
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		void ICollection<T>.Add(T item)
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
