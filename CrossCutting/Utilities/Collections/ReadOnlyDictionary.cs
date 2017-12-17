using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Read-only encapsulation for dictionary.
	/// </summary>
	/// <typeparam name="K">Key type.</typeparam>
	/// <typeparam name="V">Value type.</typeparam>
	public class ReadOnlyDictionary<K, V>: IDictionary<K, V>
	{
		#region fields

		/// <summary>
		/// Internal dictionary.
		/// </summary>
		private readonly IDictionary<K, V> m_Internal;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyDictionary&lt;K, V&gt;"/> class.
		/// </summary>
		/// <param name="other">The other.</param>
		public ReadOnlyDictionary(IDictionary<K, V> other)
		{
			m_Internal = other;
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

		#region IDictionary<K,V> Members

		/// <summary>
		/// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// Actually, it immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <param name="key">The object to use as the key of the element to add.</param>
		/// <param name="value">The object to use as the value of the element to add.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="key"/> is null.</exception>
		/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
		void IDictionary<K, V>.Add(K key, V value)
		{
			throw NotSupported("Add");
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param>
		/// <returns>
		/// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="key"/> is null.</exception>
		public bool ContainsKey(K key)
		{
			return m_Internal.ContainsKey(key);
		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.</returns>
		public ICollection<K> Keys
		{
			get { return m_Internal.Keys; }
		}

		/// <summary>
		/// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// Actually, it immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>
		/// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="key"/> is null.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
		bool IDictionary<K, V>.Remove(K key)
		{
			throw NotSupported("Remove");
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key whose value to get.</param>
		/// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
		/// <returns>
		/// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="key"/> is null.</exception>
		public bool TryGetValue(K key, out V value)
		{
			return m_Internal.TryGetValue(key, out value);
		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.</returns>
		public ICollection<V> Values
		{
			get { return m_Internal.Values; }
		}

		/// <summary>
		/// Gets the <typeparamref name="V"/> with the specified key.
		/// Invoking <c>set</c> immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <value></value>
		public V this[K key]
		{
			get { return m_Internal[key]; }
		}

		/// <summary>
		/// Gets the <typeparamref name="V"/> with the specified key.
		/// Invoking <c>set</c> immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <value></value>
		V IDictionary<K, V>.this[K key]
		{
			get { return m_Internal[key]; }
			set { throw NotSupported("this[].Set"); }
		}

		#endregion

		#region ICollection<KeyValuePair<K,V>> Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Actually, it immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		void ICollection<KeyValuePair<K, V>>.Add(KeyValuePair<K, V> item)
		{
			throw NotSupported("Add");
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Actually, it immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		void ICollection<KeyValuePair<K, V>>.Clear()
		{
			throw NotSupported("Clear");
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
		/// </returns>
		public bool Contains(KeyValuePair<K, V> item)
		{
			return m_Internal.Contains(item);
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an 
		/// <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied 
		/// from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-
		/// based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-
		/// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of 
		/// elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space 
		/// from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type 
		/// <see cref="KeyValuePair{K,V}"/> cannot be cast automatically to the type of the destination 
		/// <paramref name="array"/>.</exception>
		public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
		{
			m_Internal.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count
		{
			get { return m_Internal.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// And it is.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return true; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Actually, it immediately throws an exception, because it is read-only collection.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		bool ICollection<KeyValuePair<K, V>>.Remove(KeyValuePair<K, V> item)
		{
			throw NotSupported("Remove");
		}

		#endregion

		#region IEnumerable<KeyValuePair<K,V>> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
		{
			return m_Internal.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Internal.GetEnumerator();
		}

		#endregion
	}
}
