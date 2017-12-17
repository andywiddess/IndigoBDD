using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Dynamically translates one collection into another.
	/// </summary>
	/// <typeparam name="S">Source collection item type.</typeparam>
	/// <typeparam name="T">Target collection item type.</typeparam>
	public class TranslatedCollection<S, T> : ICollection<T>
	{
		#region fields

		/// <summary>
		/// Original collection.
		/// </summary>
		private ICollection<S> m_Collection;

		/// <summary>
		/// Object translator.
		/// </summary>
		private IObjectTranslator<S, T> m_Translator;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslatedCollection&lt;S, T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public TranslatedCollection(ICollection<S> collection)
			: this(collection, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslatedCollection&lt;S, T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="translator">The translator.</param>
		public TranslatedCollection(ICollection<S> collection, IObjectTranslator<S, T> translator)
		{
			if (collection == null)
				throw new ArgumentNullException("collection", "collection is null.");
			if (translator == null)
				translator = CastObjectTranslator<S, T>.Default;

			m_Collection = collection;
			m_Translator = translator;
		}

		#endregion

		#region translation

		/// <summary>
		/// Converts target to source.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>Converted item.</returns>
		protected S TargetToSource(T item)
		{
			return m_Translator.TargetToSource(item);
		}

		/// <summary>
		/// Converts source to target.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>Converted item.</returns>
		protected T SourceToTarget(S item)
		{
			return m_Translator.SourceToTarget(item);
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public void Add(T item)
		{
			m_Collection.Add(TargetToSource(item));
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			m_Collection.Clear();
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
			return m_Collection.Contains(TargetToSource(item));
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
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
			int length = Math.Min(array.Length - arrayIndex, Count);
			S[] arrayOfS = new S[length];
			m_Collection.CopyTo(arrayOfS, 0);

			for (int i = 0; i < length; i++)
				array[i + arrayIndex] = SourceToTarget(arrayOfS[i]);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count
		{
			get { return m_Collection.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return m_Collection.IsReadOnly; }
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
			return m_Collection.Remove(TargetToSource(item));
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
			return new TranslatedEnumerator<S, T>(m_Collection.GetEnumerator(), m_Translator);
		}

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
