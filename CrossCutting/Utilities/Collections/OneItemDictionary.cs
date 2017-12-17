using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// A dictionary with maximum one item in it.
	/// </summary>
	/// <typeparam name="K">The type of key.</typeparam>
	/// <typeparam name="V">The type of value.</typeparam>
	public class OneItemDictionary<K, V>: IDictionary<K, V>
	{
		#region fields

		private bool m_Empty = true;
		private K m_Key;
		private V m_Value;

		#endregion

		#region properties

		/// <summary>
		/// Gets a value indicating whether this <see cref="OneItemDictionary&lt;K, V&gt;"/> is empty.
		/// </summary>
		/// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
		public bool Empty
		{
			get { return m_Empty; }
		}

		/// <summary>
		/// Gets or sets the item key. Dictionary may be empty, new item will be created then.
		/// It item is already in dictionary the key of the item will be replaced.
		/// </summary>
		/// <value>The item key.</value>
		public K ItemKey
		{
			get
			{
				EnsureNotEmpty();
				return m_Key;
			}
			set
			{
				m_Key = value;
				m_Empty = false;
			}
		}

		/// <summary>
		/// Gets or sets the item value. It replaces the value of an item, so dictionary must not be empty.
		/// </summary>
		/// <value>The item value.</value>
		public V ItemValue
		{
			get
			{
				EnsureNotEmpty();
				return m_Value;
			}
			set
			{
				EnsureNotEmpty();
				m_Value = value;
			}
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="OneItemDictionary&lt;K, V&gt;"/> class.
		/// </summary>
		public OneItemDictionary()
		{
			m_Empty = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OneItemDictionary&lt;K, V&gt;"/> class
		/// with predefined entry.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public OneItemDictionary(K key, V value)
		{
			SetItem(key, value);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OneItemDictionary&lt;K, V&gt;"/> class
		/// with predefined entry.
		/// </summary>
		/// <param name="item">The item.</param>
		public OneItemDictionary(KeyValuePair<K, V> item)
		{
			SetItem(item);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OneItemDictionary&lt;K, V&gt;"/> class.
		/// Initializes items from other collection. It may initialize with first item and skip other items, 
		/// or throw exception when more than one item is provided.
		/// </summary>
		/// <param name="items">The items.</param>
		/// <param name="throwIfMany">if set to <c>true</c> throws if many items provided.</param>
		public OneItemDictionary(IEnumerable<KeyValuePair<K, V>> items, bool throwIfMany)
		{
			m_Empty = true;
			bool first = true;

			foreach (KeyValuePair<K, V> item in items)
			{
				if (first)
				{
					SetItem(item);
				}
				else
				{
					if (throwIfMany)
						throw new ArgumentException("Given item collection has more than one item");
				}
				if (!throwIfMany) break; // no need for more, first item read
			}
		}

		#endregion

		#region utilities

		/// <summary>
		/// Cleans the item.
		/// </summary>
		private void CleanItem()
		{
			m_Key = default(K);
			m_Value = default(V);
			m_Empty = true;
		}

		/// <summary>
		/// Sets the item.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		private void SetItem(K key, V value)
		{
			m_Key = key;
			m_Value = value;
			m_Empty = false;
		}

		/// <summary>
		/// Sets the item.
		/// </summary>
		/// <param name="item">The item.</param>
		private void SetItem(KeyValuePair<K, V> item)
		{
			m_Key = item.Key;
			m_Value = item.Value;
			m_Empty = false;
		}

		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <returns></returns>
		private KeyValuePair<K, V> GetItem()
		{
			EnsureNotEmpty();
			return new KeyValuePair<K, V>(m_Key, m_Value);
		}

		/// <summary>
		/// Throws an exception when dictinary is empty.
		/// </summary>
		private void EnsureNotEmpty()
		{
			if (m_Empty)
				throw new NotSupportedException("This operation is not supported for empty dictionary");
		}

		/// <summary>
		/// Determines whether given key is equal to stored one.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns><c>true</c> if key is equal; otherwise, <c>false</c>.</returns>
		private bool IsSameKey(K key)
		{
			return object.Equals(m_Key, key);
		}

		/// <summary>
		/// Determines whether given value is equal to stored one.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	<c>true</c> if given value is equal to stored one; otherwise, <c>false</c>.
		/// </returns>
		private bool IsSameValue(V value)
		{
			return object.Equals(m_Value, value);
		}

		#endregion

		#region IDictionary<K,V> Members

		/// <summary>
		/// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <param name="key">The object to use as the key of the element to add.</param>
		/// <param name="value">The object to use as the value of the element to add.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="key"/> is null.</exception>
		/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
		public void Add(K key, V value)
		{
			if (m_Empty)
			{
				SetItem(key, value);
			}
			else
			{
				if (IsSameKey(key))
					throw new ArgumentException("Given key aleady exists in OneItemDictionary", "key");
				throw new ArgumentException("OneItemDictionary an hold only one item and is already full");
			}
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</param>
		/// <returns>
		/// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
		public bool ContainsKey(K key)
		{
			return !m_Empty && IsSameKey(key);
		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.</returns>
		public ICollection<K> Keys
		{
			get
			{
				if (m_Empty) return EmptyCollection<K>.Default;
				return Transforms.ReadOnly<K>(OneItemCollection<K>.For(m_Key));
			}
		}

		/// <summary>
		/// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>
		/// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="key"/> is null.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
		public bool Remove(K key)
		{
			if (!ContainsKey(key)) return false;

			Clear();
			return true;
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key whose value to get.</param>
		/// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
		/// <returns>
		/// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
		public bool TryGetValue(K key, out V value)
		{
			value = default(V);
			if (!ContainsKey(key)) return false;

			value = m_Value;
			return true;
		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.</returns>
		public ICollection<V> Values
		{
			get
			{
				if (m_Empty) return EmptyCollection<V>.Default;
				return Transforms.ReadOnly<V>(OneItemCollection<V>.For(m_Value));
			}
		}

		/// <summary>
		/// Gets or sets the <typeparamref name="V"/> with the specified key.
		/// </summary>
		public V this[K key]
		{
			get
			{
				if (!ContainsKey(key))
					throw new ArgumentException("Given key has not been found in dictionary");
				return m_Value;

			}
			set
			{
				if (m_Empty)
				{
					SetItem(key, value);
				}
				else
				{
					if (!IsSameKey(key))
						throw new ArgumentException("OneItemDictionary is full, cannot add more items");
					m_Value = value;
				}
			}
		}

		#endregion

		#region ICollection<KeyValuePair<K,V>> Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public void Add(KeyValuePair<K, V> item)
		{
			SetItem(item.Key, item.Value);
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			CleanItem();
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
			return ContainsKey(item.Key) && IsSameValue(item.Value);
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
		/// 	<paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
		public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
		{
			if (m_Empty) return;
			array[arrayIndex] = new KeyValuePair<K, V>(m_Key, m_Value);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count
		{
			get { return m_Empty ? 0 : 1; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public bool Remove(KeyValuePair<K, V> item)
		{
			if (!Contains(item)) return false;

			Clear();
			return true;
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
			if (m_Empty)
			{
				return EmptyEnumerator<KeyValuePair<K, V>>.Default;
			}
			return new OneItemEnumerator<KeyValuePair<K, V>>(new KeyValuePair<K, V>(m_Key, m_Value));
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
