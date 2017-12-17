using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// A dictionary with fixed reference. It can be used when list reference is exposed, and we want to it to be valid
	/// even if original list has been rebuilt.
	/// </summary>
	/// <typeparam name="K">Key type.</typeparam>
	/// <typeparam name="V">Value type.</typeparam>
	public class FixedRefDictionary<K, V>: FixedRefBase<IDictionary<K, V>, FixedRefDictionary<K, V>>, IDictionary<K, V>
	{
		#region constructor

		/// <summary>Initializes a new instance of the <see cref="FixedRefDictionary&lt;K, V&gt;"/> class.</summary>
		public FixedRefDictionary()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefDictionary&lt;K, V&gt;"/> class.</summary>
		/// <param name="collection">The collection.</param>
		public FixedRefDictionary(IDictionary<K, V> collection)
			: base(collection)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefDictionary&lt;K, V&gt;"/> class.</summary>
		/// <param name="getter">The getter.</param>
		/// <param name="setter">The setter.</param>
		public FixedRefDictionary(Func<IDictionary<K, V>> getter, Action<IDictionary<K, V>> setter)
			: base(getter, setter)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefDictionary&lt;K, V&gt;"/> class.</summary>
		/// <param name="collection">The collection.</param>
		/// <param name="getter">The getter for concrete collection.</param>
		/// <param name="setter">The setter for concrete collection.</param>
		public FixedRefDictionary(IDictionary<K, V> collection, Func<IDictionary<K, V>> getter, Action<IDictionary<K, V>> setter)
			: base(collection, getter, setter)
		{
		}

		#endregion

		#region IDictionary<K,V> Members

		/// <summary>Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</summary>
		/// <param name="key">The object to use as the key of the element to add.</param>
		/// <param name="value">The object to use as the value of the element to add.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</exception>
		/// <exception cref="T:System.ArgumentNullException">key is null.</exception>
		public void Add(K key, V value)
		{
			Data.Add(key, value);
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the specified key.</summary>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</param>
		/// <returns>true if the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the key; otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentNullException">key is null.</exception>
		public bool ContainsKey(K key)
		{
			return Data.ContainsKey(key);
		}

		/// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
		public ICollection<K> Keys
		{
			get { return Data.Keys; }
		}

		/// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>true if the element is successfully removed; otherwise, false.  This method also returns false if key was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentNullException">key is null.</exception>
		public bool Remove(K key)
		{
			return Data.Remove(key);
		}

		/// <summary>Tries the get value.</summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if success.</returns>
		public bool TryGetValue(K key, out V value)
		{
			return Data.TryGetValue(key, out value);
		}

		/// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
		public ICollection<V> Values
		{
			get { return Data.Values; }
		}

		/// <summary>Gets or sets the element with the specified key.</summary>
		/// <returns>The element with the specified key.</returns>
		/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentNullException">key is null.</exception>
		/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found.</exception>
		public V this[K key]
		{
			get { return Data[key]; }
			set { Data[key] = value; }
		}

		#endregion

		#region ICollection<KeyValuePair<K,V>> Members

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(KeyValuePair<K, V> item)
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
		public bool Contains(KeyValuePair<K, V> item)
		{
			return Data.Contains(item);
		}

		/// <summary>Copies to.</summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
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
		public bool Remove(KeyValuePair<K, V> item)
		{
			return Data.Remove(item);
		}

		#endregion

		#region IEnumerable<KeyValuePair<K,V>> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.</returns>
		public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
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
