using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Translates <see cref="IDictionary&lt;K,V&gt;"/> to IDictionary on the fly.
	/// </summary>
	/// <typeparam name="K">Type of key.</typeparam>
	/// <typeparam name="V">Type of value.</typeparam>
	public class UntypedDictionary<K, V> : IDictionary
	{
		#region fields

		/// <summary>Internal dictionary.</summary>
		private readonly IDictionary<K, V> m_Internal;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="UntypedDictionary&lt;K, V&gt;"/> class.</summary>
		/// <param name="dictionary">The dictionary.</param>
		public UntypedDictionary(IDictionary<K, V> dictionary)
		{
			m_Internal = dictionary;
		}

		#endregion

		#region class InternalEnumerator

		/// <summary>Internal <see cref="IDictionaryEnumerator"/> helper.</summary>
		protected class InternalEnumerator : IDictionaryEnumerator
		{
			#region fields

			/// <summary>Internal enumerator.</summary>
			private readonly IEnumerator<KeyValuePair<K, V>> m_Internal;

			#endregion

			#region constructor

			/// <summary>Initializes a new instance of the <see cref="UntypedDictionary&lt;K, V&gt;.InternalEnumerator"/> class.</summary>
			/// <param name="enumerator">The enumerator.</param>
			public InternalEnumerator(IEnumerator<KeyValuePair<K, V>> enumerator)
			{
				m_Internal = enumerator;
			}

			#endregion

			#region utilities

			/// <summary>Creates the current entry.</summary>
			/// <returns>Current entry as <see cref="DictionaryEntry"/></returns>
			private DictionaryEntry CreateCurrentEntry()
			{
				return new DictionaryEntry(m_Internal.Current.Key, m_Internal.Current.Value);
			}

			#endregion

			#region IDictionaryEnumerator Members

			/// <summary>Gets both the key and the value of the current dictionary entry.</summary>
			/// <returns>A <see cref="T:System.Collections.DictionaryEntry"/> containing both the key and the value of the current dictionary entry.</returns>
			/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.IDictionaryEnumerator"/> is positioned before the first entry of the dictionary or after the last entry. </exception>
			public DictionaryEntry Entry
			{
				get { return CreateCurrentEntry(); }
			}

			/// <summary>Gets the key of the current dictionary entry.</summary>
			/// <returns>The key of the current element of the enumeration.</returns>
			/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.IDictionaryEnumerator"/> is positioned before the first entry of the dictionary or after the last entry. </exception>
			public object Key
			{
				get { return m_Internal.Current.Key; }
			}

			/// <summary>Gets the value of the current dictionary entry.</summary>
			/// <returns>The value of the current element of the enumeration.</returns>
			/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.IDictionaryEnumerator"/> is positioned before the first entry of the dictionary or after the last entry. </exception>
			public object Value
			{
				get { return m_Internal.Current.Value; }
			}

			#endregion

			#region IEnumerator Members

			/// <summary>Gets the current element in the collection.</summary>
			/// <returns>The current element in the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
			public object Current
			{
				get { return CreateCurrentEntry(); }
			}

			/// <summary>Advances the enumerator to the next element of the collection.</summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public bool MoveNext()
			{
				return m_Internal.MoveNext();
			}

			/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
			public void Reset()
			{
				m_Internal.Reset();
			}

			#endregion
		}

		#endregion

		#region IDictionary Members

		/// <summary>Adds an element with the provided key and value to the <see cref="T:System.Collections.IDictionary"/> object.</summary>
		/// <param name="key">The <see cref="T:System.Object"/> to use as the key of the element to add.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to use as the value of the element to add.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null. </exception>
		/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.IDictionary"/> object. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IDictionary"/> is read-only.-or- The <see cref="T:System.Collections.IDictionary"/> has a fixed size. </exception>
		public void Add(object key, object value)
		{
			m_Internal.Add((K) key, (V) value);
		}

		/// <summary>Removes all elements from the <see cref="T:System.Collections.IDictionary"/> object.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IDictionary"/> object is read-only. </exception>
		public void Clear()
		{
			m_Internal.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.IDictionary"/> object contains an element with the specified key.</summary>
		/// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary"/> object.</param>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary"/> contains an element with the key; otherwise, false.</returns>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null. </exception>
		public bool Contains(object key)
		{
			return m_Internal.ContainsKey((K) key);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator"/> object for the <see cref="T:System.Collections.IDictionary"/> object.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator"/> object for the <see cref="T:System.Collections.IDictionary"/> object.</returns>
		public IDictionaryEnumerator GetEnumerator()
		{
			return new InternalEnumerator(m_Internal.GetEnumerator());
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IDictionary"/> object has a fixed size.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary"/> object has a fixed size; otherwise, false.</returns>
		public bool IsFixedSize
		{
			// NOTE:MAK I don't know what it should return
			get { return false; }
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IDictionary"/> object is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary"/> object is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return m_Internal.IsReadOnly; }
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection"/> object containing the keys of the <see cref="T:System.Collections.IDictionary"/> object.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection"/> object containing the keys of the <see cref="T:System.Collections.IDictionary"/> object.</returns>
		public ICollection Keys
		{
			get { return new UntypedCollection<K>(m_Internal.Keys); }
		}

		/// <summary>Gets an <see cref="T:System.Collections.ICollection"/> object containing the values in the <see cref="T:System.Collections.IDictionary"/> object.</summary>
		/// <returns>An <see cref="T:System.Collections.ICollection"/> object containing the values in the <see cref="T:System.Collections.IDictionary"/> object.</returns>
		public ICollection Values
		{
			get { return new UntypedCollection<V>(m_Internal.Values); }
		}

		/// <summary>
		/// Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary"/> object.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IDictionary"/> object is read-only.-or- The <see cref="T:System.Collections.IDictionary"/> has a fixed size. </exception>
		public void Remove(object key)
		{
			m_Internal.Remove((K) key);
		}

		/// <summary>Gets or sets the element with the specified key.</summary>
		/// <returns>The element with the specified key.</returns>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IDictionary"/> object is read-only.-or- The property is set, <paramref name="key"/> does not exist in the collection, and the <see cref="T:System.Collections.IDictionary"/> has a fixed size. </exception>
		public object this[object key]
		{
			get { return m_Internal[(K) key]; }
			set { m_Internal[(K) key] = (V) value; }
		}

		#endregion

		#region ICollection Members

		/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
		/// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
		public void CopyTo(Array array, int index)
		{
			foreach (var item in this)
			{
				array.SetValue(item, index);
				index++;
			}
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</returns>
		public int Count
		{
			get { return m_Internal.Count; }
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
		public bool IsSynchronized
		{
			get { return false; }
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
		public object SyncRoot
		{
			get { return m_Internal; }
		}

		#endregion

		#region IEnumerable Members

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
