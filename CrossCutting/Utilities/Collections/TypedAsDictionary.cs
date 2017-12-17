using System;
using System.Collections.Generic;
using System.Collections;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Encapsulation for <see cref="IDictionary"/> making items typed.
	/// </summary>
	/// <typeparam name="K">Type of key.</typeparam>
	/// <typeparam name="V">Type of value.</typeparam>
	public class TypedAsDictionary<K, V>: IDictionary<K, V>
	{
		#region fields

		/// <summary>Encapsulated dictionary.</summary>
		private readonly IDictionary m_Map;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="TypedAsDictionary&lt;K, V&gt;"/> class.</summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <exception cref="ArgumentNullException">Thrown if given dictionary is <c>null</c>.</exception>
		public TypedAsDictionary(IDictionary dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException("dictionary", "dictionary is null.");
			m_Map = dictionary;
		}

		#endregion

		#region class InternalEnumerator

		/// <summary>
		/// Internal enumerator, converting <see cref="DictionaryEntry"/> into <see cref="KeyValuePair{K,V}"/> on the fly.
		/// </summary>
		protected class InternalEnumerator: IEnumerator<KeyValuePair<K, V>>
		{
			#region fields

			/// <summary>Internal dictionary enumerator.</summary>
			private readonly IDictionaryEnumerator m_Enumerator;

			#endregion

			#region constructor

			/// <summary>Initializes a new instance of the <see cref="TypedAsDictionary&lt;K, V&gt;.InternalEnumerator"/> class.</summary>
			/// <param name="enumerator">The enumerator.</param>
			public InternalEnumerator(IDictionaryEnumerator enumerator)
			{
				m_Enumerator = enumerator;
			}

			#endregion

			#region IEnumerator<KeyValuePair<K,V>> Members

			/// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
			/// <returns>The element in the collection at the current position of the enumerator.</returns>
			public KeyValuePair<K, V> Current
			{
				get { return CreateCurrent(); }
			}

			/// <summary>Creates the current item.</summary>
			/// <returns>Current item converted to <see cref="KeyValuePair{K,V}"/></returns>
			private KeyValuePair<K, V> CreateCurrent()
			{
				return new KeyValuePair<K, V>((K)m_Enumerator.Key, (V)m_Enumerator.Value);
			}

			#endregion

			#region IDisposable Members

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
			}

			#endregion

			#region IEnumerator Members

			/// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
			/// <returns>The element in the collection at the current position of the enumerator.</returns>
			object IEnumerator.Current
			{
				get { return Current; }
			}

			/// <summary>Advances the enumerator to the next element of the collection.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public bool MoveNext()
			{
				return m_Enumerator.MoveNext();
			}

			/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public void Reset()
			{
				m_Enumerator.Reset();
			}

			#endregion
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
			m_Map.Add(key, value);
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the specified key.</summary>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</param>
		/// <returns>true if the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the key; otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentNullException">key is null.</exception>
		public bool ContainsKey(K key)
		{
			return m_Map.Contains(key);
		}

		/// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
		public ICollection<K> Keys
		{
			get { return new TypedAsCollection<K>(m_Map.Keys); }
		}

		/// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>true if the element is successfully removed; otherwise, false.  This method also returns false if key was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentNullException">key is null.</exception>
		public bool Remove(K key)
		{
			if (!m_Map.Contains(key))
				return false;

			m_Map.Remove(key);
			return true;
		}

		/// <summary>Tries the get value.</summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if key has been found; <c>false</c> otherwise;</returns>
		public bool TryGetValue(K key, out V value)
		{
			if (!m_Map.Contains(key))
			{
				value = default(V);
				return false;
			}

			value = (V)m_Map[key];
			return true;
		}

		/// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
		public ICollection<V> Values
		{
			get { return new TypedAsCollection<V>(m_Map.Values); }
		}

		/// <summary>Gets or sets the element with the specified key.</summary>
		/// <returns>The element with the specified key.</returns>
		/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentNullException">key is null.</exception>
		/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found.</exception>
		public V this[K key]
		{
			get { return (V)m_Map[key]; }
			set { m_Map[key] = value; }
		}

		#endregion

		#region ICollection<KeyValuePair<K,V>> Members

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(KeyValuePair<K, V> item)
		{
			m_Map.Add(item.Key, item.Value);
		}

		/// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
		public void Clear()
		{
			m_Map.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.</summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.</returns>
		public bool Contains(KeyValuePair<K, V> item)
		{
			V value;
			if (TryGetValue(item.Key, out value))
			{
				return object.Equals(value, item.Value);
			}
			return false;
		}

		/// <summary>Copies to elements to array.</summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
		{
			foreach (var kv in this)
			{
				array[arrayIndex] = kv;
				arrayIndex++;
			}
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		public int Count
		{
			get { return m_Map.Count; }
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return m_Map.IsReadOnly; }
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public bool Remove(KeyValuePair<K, V> item)
		{
			if (!Contains(item))
				return false;

			m_Map.Remove(item.Key);
			return true;
		}

		#endregion

		#region IEnumerable<KeyValuePair<K,V>> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.</returns>
		public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
		{
			return new InternalEnumerator(m_Map.GetEnumerator());
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
