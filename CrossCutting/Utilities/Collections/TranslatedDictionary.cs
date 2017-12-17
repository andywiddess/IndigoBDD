using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Translates one dictionary into another.
	/// </summary>
	/// <typeparam name="K">Key type.</typeparam>
	/// <typeparam name="SV">Source value type.</typeparam>
	/// <typeparam name="TV">Target value type.</typeparam>
	public class TranslatedDictionary<K, SV, TV>:
			IDictionary<K, TV>,
			IObjectTranslator<KeyValuePair<K, SV>, KeyValuePair<K, TV>>
	{
		#region fields

		/// <summary>
		/// Original dictionary.
		/// </summary>
		private IDictionary<K, SV> m_Dictionary;

		/// <summary>
		/// Translator.
		/// </summary>
		private IObjectTranslator<SV, TV> m_Translator;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslatedDictionary&lt;K, SV, TV&gt;"/> class.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		public TranslatedDictionary(IDictionary<K, SV> dictionary)
			: this(dictionary, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslatedDictionary&lt;K, SV, TV&gt;"/> class.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="translator">The translator.</param>
		public TranslatedDictionary(IDictionary<K, SV> dictionary, IObjectTranslator<SV, TV> translator)
		{
			if (dictionary == null)
				throw new ArgumentNullException("dictionary", "dictionary is null.");
			if (translator == null)
				translator = CastObjectTranslator<SV, TV>.Default;

			m_Dictionary = dictionary;
			m_Translator = translator;
		}

		#endregion

		#region translation

		/// <summary>
		/// Translates target to source.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>Translated value.</returns>
		private SV TargetToSource(TV item)
		{
			return m_Translator.TargetToSource(item);
		}

		/// <summary>
		/// Translates source to target.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>Translated values.</returns>
		private TV SourceToTarget(SV item)
		{
			return m_Translator.SourceToTarget(item);
		}

		/// <summary>
		/// Translates target KV pair to source KV pair.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>Translated KV pair.</returns>
		private KeyValuePair<K, SV> TargetToSourceKV(KeyValuePair<K, TV> item)
		{
			return new KeyValuePair<K, SV>(item.Key, TargetToSource(item.Value));
		}

		/// <summary>
		/// Translates source KV pair to target KV pair.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>Translated KV pair.</returns>
		private KeyValuePair<K, TV> SourceToTargetKV(KeyValuePair<K, SV> item)
		{
			return new KeyValuePair<K, TV>(item.Key, SourceToTarget(item.Value));
		}

		#endregion

		#region IDictionary<K,TV> Members

		/// <summary>
		/// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <param name="key">The object to use as the key of the element to add.</param>
		/// <param name="value">The object to use as the value of the element to add.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="key"/> is null.</exception>
		/// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
		public void Add(K key, TV value)
		{
			m_Dictionary.Add(key, TargetToSource(value));
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
			return m_Dictionary.ContainsKey(key);
		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.</returns>
		public ICollection<K> Keys
		{
			get { return m_Dictionary.Keys; }
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
			return m_Dictionary.Remove(key);
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
		public bool TryGetValue(K key, out TV value)
		{
			SV outValue;
			bool result = m_Dictionary.TryGetValue(key, out outValue);
			value = SourceToTarget(outValue);
			return result;
		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.</returns>
		public ICollection<TV> Values
		{
			get { return new TranslatedCollection<SV, TV>(m_Dictionary.Values, m_Translator); }
		}

		/// <summary>
		/// Gets or sets the <typeparamref name="TV"/> with the specified key.
		/// </summary>
		/// <value></value>
		public TV this[K key]
		{
			get { return SourceToTarget(m_Dictionary[key]); }
			set { m_Dictionary[key] = TargetToSource(value); }
		}

		#endregion

		#region ICollection<KeyValuePair<K,TV>> Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public void Add(KeyValuePair<K, TV> item)
		{
			m_Dictionary.Add(TargetToSourceKV(item));
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
		public bool Contains(KeyValuePair<K, TV> item)
		{
			return m_Dictionary.Contains(TargetToSourceKV(item));
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
		public void CopyTo(KeyValuePair<K, TV>[] array, int arrayIndex)
		{
			int length = Math.Min(array.Length - arrayIndex, Count);
			KeyValuePair<K, SV>[] arrayOfS = new KeyValuePair<K, SV>[length];
			m_Dictionary.CopyTo(arrayOfS, 0);

			for (int i = 0; i < length; i++)
				array[i + arrayIndex] = SourceToTargetKV(arrayOfS[i]);
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
			get { return m_Dictionary.IsReadOnly; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public bool Remove(KeyValuePair<K, TV> item)
		{
			return m_Dictionary.Remove(TargetToSourceKV(item));
		}

		#endregion

		#region IEnumerable<KeyValuePair<K,TV>> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<KeyValuePair<K, TV>> GetEnumerator()
		{
			return new TranslatedEnumerator<KeyValuePair<K, SV>, KeyValuePair<K, TV>>(
					m_Dictionary.GetEnumerator(), this);
		}

		#endregion

		#region IEnumerable Members

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

		#region IObjectTranslator<KeyValuePair<K,SV>,KeyValuePair<K,TV>> Members

		/// <summary>
		/// Converts source to target.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>Converted object.</returns>
		KeyValuePair<K, TV> IObjectTranslator<KeyValuePair<K, SV>, KeyValuePair<K, TV>>.SourceToTarget(KeyValuePair<K, SV> source)
		{
			return SourceToTargetKV(source);
		}

		/// <summary>
		/// Converts targets to source.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns>Converted object.</returns>
		KeyValuePair<K, SV> IObjectTranslator<KeyValuePair<K, SV>, KeyValuePair<K, TV>>.TargetToSource(KeyValuePair<K, TV> target)
		{
			return TargetToSourceKV(target);
		}

		#endregion
	}
}
