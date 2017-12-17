using System;
using System.Collections.Generic;
using System.Linq;
using Indigo.CrossCutting.Utilities;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Both-sides-weak dictionary. Allows to store relations between objects
	/// without preventing them from disposal.
	/// </summary>
	/// <typeparam name="K">Key type.</typeparam>
	/// <typeparam name="V">Value type.</typeparam>
	public class WeakDictionary<K, V>: IDictionary<K, V>
		where K: class
		where V: class
	{
		#region fields

		/// <summary>
		/// Actual implementation.
		/// </summary>
		private Dictionary<WeakDictionaryKey<K>, WeakReference> m_Dictionary =
				new Dictionary<WeakDictionaryKey<K>, WeakReference>();

		#endregion

		#region utilities

		/// <summary>
		/// Enumerates the Key/Value pairs.
		/// </summary>
		/// <param name="skipNulls">if set to <c>true</c> skips already disposed objects.</param>
		/// <returns>List of Key/Value pairs.</returns>
		private IEnumerable<KeyValuePair<K, V>> EnumeratePairs(bool skipNulls)
		{
			foreach (KeyValuePair<WeakDictionaryKey<K>, WeakReference> kv in m_Dictionary)
			{
				if ((!skipNulls) || (kv.Key.IsAlive && kv.Value.IsAlive))
				{
					K k = kv.Key.Target;
					V v = (V)kv.Value.Target;
					yield return new KeyValuePair<K, V>(k, v);
				}
			}
		}

		/// <summary>
		/// Flushes this instance. Removes all disposed pairs.
		/// </summary>
		public void Flush()
		{
			Dictionary<WeakDictionaryKey<K>, WeakReference> newDictionary =
					new Dictionary<WeakDictionaryKey<K>, WeakReference>();

			foreach (KeyValuePair<WeakDictionaryKey<K>, WeakReference> kv in m_Dictionary)
			{
				if (kv.Key.IsAlive && kv.Value.IsAlive)
				{
					newDictionary.Add(kv.Key, kv.Value);
				}
			}
			m_Dictionary = newDictionary;
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
			m_Dictionary.Add(new WeakDictionaryKey<K>(key), new WeakReference(value));
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
			return m_Dictionary.ContainsKey(new WeakDictionaryKey<K>(key));
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
				return new VirtualCollection<ICollection<WeakDictionaryKey<K>>, K>(
						m_Dictionary.Keys, new WeakDictionaryKeyCollectionHandler<K>());
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
			return m_Dictionary.Remove(new WeakDictionaryKey<K>(key));
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
			WeakReference reference;
			bool result = m_Dictionary.TryGetValue(new WeakDictionaryKey<K>(key), out reference);
			if ((!result) || (reference == null) || (!reference.IsAlive))
			{
				value = default(V);
				return false;
			}
			else
			{
				value = (V)reference.Target;
				return true;
			}

		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.</returns>
		public ICollection<V> Values
		{
			get
			{
				return new VirtualCollection<ICollection<WeakReference>, V>(
						m_Dictionary.Values, new WeakDictionaryValueCollectionHandler<V>());
			}
		}

		/// <summary>
		/// Gets or sets the <typeparamref name="V"/> with the specified key.
		/// </summary>
		/// <value></value>
		public V this[K key]
		{
			get { return (V)m_Dictionary[new WeakDictionaryKey<K>(key)].Target; }
			set { m_Dictionary[new WeakDictionaryKey<K>(key)] = new WeakReference(value); }
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
			Add(item.Key, item.Value);
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			m_Dictionary.Clear();
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
			V value;
			if (TryGetValue(item.Key, out value))
			{
				if (value == item.Value) return true;
				if (value == null) return false;
				return value.Equals(item.Value);
			}
			else
			{
				return false;
			}
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
			List<KeyValuePair<K, V>> list = new List<KeyValuePair<K, V>>(EnumeratePairs(false));
			list.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count
		{
			get { return m_Dictionary.Count; }
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
			// IMP:MAK check, it removes an entry regardless 'value' (only 'key' is important)
			return Remove(item.Key);
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
			return EnumeratePairs(true).GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}

	#region class WeakDictionaryKey<T>

	internal class WeakDictionaryKey<T>
	{
		private readonly int m_HashCode;
		private readonly WeakReference m_Subject;

		public WeakDictionaryKey(T subject)
		{
			m_Subject = new WeakReference(subject);
			m_HashCode = subject.GetHashCode();
		}

		public T Target
		{
			get { return (T)m_Subject.Target; }
		}

		public bool IsAlive
		{
			get { return m_Subject.IsAlive; }
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj == this) return true;
			var other = obj as WeakDictionaryKey<T>;
			if (other == null) return false;

			T thisObject = (T)m_Subject.Target;
			T otherObject = (T)other.m_Subject.Target;

			if (object.ReferenceEquals(thisObject, otherObject)) return true;
			if (object.ReferenceEquals(thisObject, null)) return false;

			return
					(m_HashCode == other.m_HashCode) &&
					(thisObject.Equals(otherObject));
		}

		public override int GetHashCode()
		{
			return m_HashCode;
		}
	}

	#endregion

	#region class WeakDictionaryKeyCollectionHandler

	/// <summary>
	/// Helper class which transparently allows to use key collection of <see cref="WeakDictionary&lt;K, V&gt;"/>.
	/// </summary>
	/// <typeparam name="K"></typeparam>
	internal class WeakDictionaryKeyCollectionHandler<K>: IVirtualCollectionHandler<ICollection<WeakDictionaryKey<K>>, K>
	{
		#region IVirtualCollectionHandler<ICollection<WeakDictionaryKey<K>>,K> Members

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="item">The item.</param>
		public void Add(ICollection<WeakDictionaryKey<K>> discriminator, K item)
		{
			discriminator.Add(new WeakDictionaryKey<K>(item));
		}

		/// <summary>
		/// Clears collection.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		public void Clear(ICollection<WeakDictionaryKey<K>> discriminator)
		{
			discriminator.Clear();
		}

		/// <summary>
		/// Determines whether collection contains specified item.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="item">The item.</param>
		/// <returns>
		/// 	<c>true</c> if collection contains specified item; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(ICollection<WeakDictionaryKey<K>> discriminator, K item)
		{
			return discriminator.Contains(new WeakDictionaryKey<K>(item));
		}

		/// <summary>
		/// Copies items to array.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Starting index in the array.</param>
		public void CopyTo(ICollection<WeakDictionaryKey<K>> discriminator, K[] array, int arrayIndex)
		{
			discriminator.Select(w => w.Target).CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <returns>Number of items in collection.</returns>
		public int GetCount(ICollection<WeakDictionaryKey<K>> discriminator)
		{
			return discriminator.Count;
		}

		/// <summary>
		/// Determines whether collection is read only.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <returns>
		/// 	<c>true</c> if collection is read only; otherwise, <c>false</c>.
		/// </returns>
		public bool IsReadOnly(ICollection<WeakDictionaryKey<K>> discriminator)
		{
			return discriminator.IsReadOnly;
		}

		/// <summary>
		/// Removes the item from collection.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item has been removed.</returns>
		public bool Remove(ICollection<WeakDictionaryKey<K>> discriminator, K item)
		{
			return discriminator.Remove(new WeakDictionaryKey<K>(item));
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <returns>Enumerator.</returns>
		public IEnumerator<K> GetEnumerator(ICollection<WeakDictionaryKey<K>> discriminator)
		{
			return discriminator.Select(w => w.Target).GetEnumerator();
		}

		#endregion
	}

	#endregion

	#region class WeakDictionaryValueCollectionHandler<V>

	/// <summary>
	/// Helper class alowing to use values collecton of <see cref="WeakDictionary&lt;K, V&gt;"/> transparently.
	/// </summary>
	/// <typeparam name="V"></typeparam>
	internal class WeakDictionaryValueCollectionHandler<V>: IVirtualCollectionHandler<ICollection<WeakReference>, V>
	{
		#region IVirtualCollectionHandler<ICollection<WeakReference>,V> Members

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="item">The item.</param>
		public void Add(ICollection<WeakReference> discriminator, V item)
		{
			discriminator.Add(new WeakReference(item));
		}

		/// <summary>
		/// Clears collection.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		public void Clear(ICollection<WeakReference> discriminator)
		{
			discriminator.Clear();
		}

		/// <summary>
		/// Determines whether collection contains specified item.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="item">The item.</param>
		/// <returns>
		/// 	<c>true</c> if collection contains specified item; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(ICollection<WeakReference> discriminator, V item)
		{
			return discriminator.Contains(new WeakReference(item));
		}

		/// <summary>
		/// Copies items to array.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Starting index in the array.</param>
		public void CopyTo(ICollection<WeakReference> discriminator, V[] array, int arrayIndex)
		{
			discriminator.Select(w => (V)w.Target).CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <returns>Number of items in collection.</returns>
		public int GetCount(ICollection<WeakReference> discriminator)
		{
			return discriminator.Count;
		}

		/// <summary>
		/// Determines whether collection is read only.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <returns>
		/// 	<c>true</c> if collection is read only; otherwise, <c>false</c>.
		/// </returns>
		public bool IsReadOnly(ICollection<WeakReference> discriminator)
		{
			return discriminator.IsReadOnly;
		}

		/// <summary>
		/// Removes the item from collection.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item has been removed.</returns>
		public bool Remove(ICollection<WeakReference> discriminator, V item)
		{
			return discriminator.Remove(new WeakReference(item));
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <returns>Enumerator.</returns>
		public IEnumerator<V> GetEnumerator(ICollection<WeakReference> discriminator)
		{
			return discriminator.Select(w => (V)w.Target).GetEnumerator();
		}

		#endregion
	}

	#endregion
}
