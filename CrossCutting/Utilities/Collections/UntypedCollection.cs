using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Translates untyped ICollection&lt;T&gt; to ICollection.
	/// </summary>
	/// <typeparam name="T">Type of item</typeparam>
	public class UntypedCollection<T>: ICollection
	{
		#region fields

		/// <summary>
		/// Origial collection.
		/// </summary>
		private readonly ICollection<T> m_Collection;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="UntypedCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public UntypedCollection(ICollection<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection", "collection is null.");
			m_Collection = collection;
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
			int count = m_Collection.Count;
			var list = new ArrayList(count);
			foreach (var item in m_Collection.Cast<T>()) list.Add(item);
			list.CopyTo(array, index);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</returns>
		public int Count
		{
			get { return m_Collection.Count; }
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
			get { return m_Collection; }
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
			return m_Collection.GetEnumerator();
		}

		#endregion
	}
}
