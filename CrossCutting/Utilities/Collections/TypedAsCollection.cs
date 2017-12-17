using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Encapsulation for <see cref="ICollection"/> making items typed.
	/// Due to limitations of <see cref="ICollection"/> TypedAsCollection is always read-only.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class TypedAsCollection<T>: ICollection<T>
	{
		#region fields

		private readonly ICollection m_Collection;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TypedAsCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <exception cref="ArgumentNullException">Thrown if given collection is <c>null</c>.</exception>
		public TypedAsCollection(ICollection collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection", "collection is null.");
			m_Collection = collection;
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

		#region ICollection<T> Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/>
		/// is read-only.</exception>
		public void Add(T item)
		{
			throw NotSupported("Add");
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/>
		/// is read-only.</exception>
		public void Clear()
		{
			throw NotSupported("Clear");
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific 
		/// value.</summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, 
		/// false.</returns>
		public bool Contains(T item)
		{
			foreach (var obj in m_Collection)
			{
				if (object.Equals(item, obj))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Copies items to array.</summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			m_Collection.CopyTo(array, arrayIndex);
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count
		{
			get { return m_Collection.Count; }
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return true; }
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public bool Remove(T item)
		{
			throw NotSupported("Remove");
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return new TypedAsEnumerator<T>(m_Collection.GetEnumerator());
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Collection.GetEnumerator();
		}

		#endregion
	}
}