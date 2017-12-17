using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Implements empty set of any type.</summary>
	/// <typeparam name="T">Type of element.</typeparam>
	public class EmptySet<T>: EmptyCollection<T>, ISet<T>
	{
		#region static fields

		/// <summary>Default instance of EmptySet.</summary>
		public static readonly new EmptySet<T> Default = new EmptySet<T>();

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
		public void ExceptWith(IEnumerable<T> other)
		{
			// do nothing, no elements in collection nothing to remove
		}

		/// <summary>
		/// Intersects with <paramref name="other"/> items.
		/// </summary>
		/// <param name="other">The other.</param>
		public void IntersectWith(IEnumerable<T> other)
		{
			// do nothing, no elements in collection nothing to remove
		}

		/// <summary>Determines whether set is a proper subset of <paramref name="other"/>.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if set is a proper subset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			return !other.IsEmpty();
		}

		/// <summary>Determines whether set is a proper superset of <paramref name="other"/>.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if set is a proper superset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			return false;
		}

		/// <summary>Determines whether set is a subset of <paramref name="other"/>.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if set is a subset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			return true;
		}

		/// <summary>Determines whether set is a proper superset of <paramref name="other"/>.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if set is a proper superset of <paramref name="other"/>; otherwise, <c>false</c>.</returns>
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			return other.IsEmpty();
		}

		/// <summary>Determines whether set overlaps with <paramref name="other"/>.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if set overlaps with <paramref name="other"/>; <c>false</c> otherwise</returns>
		public bool Overlaps(IEnumerable<T> other)
		{
			return false;
		}

		/// <summary>Determines whether sets are equal.</summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if sets are equal; <c>false</c> otherwise;</returns>
		public bool SetEquals(IEnumerable<T> other)
		{
			return other.IsEmpty();
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
	}
}
