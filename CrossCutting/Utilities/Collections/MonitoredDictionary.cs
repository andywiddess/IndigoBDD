using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Indigo.CrossCutting.Utilities.Events;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Creates monitored dictionary on top of any dictionary.
	/// </summary>
	/// <typeparam name="K">Type of Key.</typeparam>
	/// <typeparam name="V">Type of Value.</typeparam>
	public class MonitoredDictionary<K, V>: IDictionary<K, V>, ISuspendableEvents, ISyncRoot
	{
		#region fields

		/// <summary>
		/// Original list.
		/// </summary>
		private readonly IDictionary<K, V> m_Dictionary;

		/// <summary>
		/// Indicates if events has been suspended.
		/// </summary>
		private int m_Suspended;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="MonitoredDictionary&lt;K, V&gt;"/> class.</summary>
		/// <param name="dictionary">The dictionary.</param>
		public MonitoredDictionary(IDictionary<K, V> dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException("dictionary", "dictionary is null.");
			m_Dictionary = dictionary;
		}

		/// <summary>Initializes a new instance of the <see cref="MonitoredDictionary&lt;K, V&gt;"/> class.</summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="handler">The handler.</param>
		public MonitoredDictionary(IDictionary<K, V> dictionary, MonitoredDictionaryEvent<K, V> handler)
			: this(dictionary)
		{
			Notification += handler;
		}

		#endregion

		#region event

		/// <summary>
		/// Occurs when dictionary is going to be/has been changed.
		/// </summary>
		public event MonitoredDictionaryEvent<K, V> Notification;

		private bool Raise(
			MonitoredDictionaryEventType type, 
			K key = default(K), 
			V newValue = default(V), V oldValue = default(V))
		{
			if (Notification != null)
			{
				bool send =
					(m_Suspended <= 0) ||
					(type == MonitoredDictionaryEventType.Suspended) ||
					(type == MonitoredDictionaryEventType.Resumed);

				if (send)
				{
					var args = new MonitoredDictionaryEventArgs<K, V>(type, key, newValue, oldValue);
					Notification(this, args);
					return !args.Cancel;
				}
			}
			return true;
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
			if (Raise(MonitoredDictionaryEventType.Adding, key, value))
			{
				m_Dictionary.Add(key, value);
				Raise(MonitoredDictionaryEventType.Added, key, value);
			}
		}

		private void Set(K key, V value)
		{
			V oldValue;
			if (m_Dictionary.TryGetValue(key, out oldValue))
			{
				if (Raise(MonitoredDictionaryEventType.SettingAt, key, value, oldValue))
				{
					m_Dictionary[key] = value;
					Raise(MonitoredDictionaryEventType.SetAt, key, value, oldValue);
				}
			}
			else
			{
				if (Raise(MonitoredDictionaryEventType.Adding, key, value))
				{
					m_Dictionary.Add(key, value);
					Raise(MonitoredDictionaryEventType.Added, key, value);
				}
			}
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
			var value = m_Dictionary.TryGetValue(key);

			if (Raise(MonitoredDictionaryEventType.Removing, key, value, value))
			{
				bool result = m_Dictionary.Remove(key);
				Raise(result ? MonitoredDictionaryEventType.Removed : MonitoredDictionaryEventType.Missed, key, value, value);
				return result;
			}
			return false;
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
			return m_Dictionary.TryGetValue(key, out value);
		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.</returns>
		public ICollection<V> Values
		{
			get { return m_Dictionary.Values; }
		}

		/// <summary>
		/// Gets or sets the <see typeparamref="V"/> with the specified key.
		/// </summary>
		/// <value></value>
		public V this[K key]
		{
			get
			{
				return m_Dictionary[key];
			}
			set
			{
				Set(key, value);
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
			if (Raise(MonitoredDictionaryEventType.Adding, item.Key, item.Value))
			{
				m_Dictionary.Add(item);
				Raise(MonitoredDictionaryEventType.Added, item.Key, item.Value);
			}
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			if (Raise(MonitoredDictionaryEventType.Clearing))
			{
				Raise(MonitoredDictionaryEventType.ClearApproved);
				m_Dictionary.Clear();
				Raise(MonitoredDictionaryEventType.Cleared);
			}
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
			return m_Dictionary.Contains(item);
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
			m_Dictionary.CopyTo(array, arrayIndex);
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
		public bool Remove(KeyValuePair<K, V> item)
		{
			if (Raise(MonitoredDictionaryEventType.Removing, item.Key))
			{
				bool result = m_Dictionary.Remove(item);
				Raise(result ? MonitoredDictionaryEventType.Removed : MonitoredDictionaryEventType.Missed, item.Key);
				return result;
			}
			return false;
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
			return m_Dictionary.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Dictionary.GetEnumerator();
		}

		#endregion

		#region ISuspendableEvents Members

		/// <summary>
		/// Suspends events.
		/// </summary>
		public void SuspendEvents()
		{
			m_Suspended++;
			if (m_Suspended > 0)
			{
				Raise(MonitoredDictionaryEventType.Suspended);
			}
		}

		/// <summary>
		/// Resumes events.
		/// </summary>
		public void ResumeEvents()
		{
			m_Suspended--;
			if (m_Suspended == 0)
			{
				Raise(MonitoredDictionaryEventType.Resumed);
			}
		}

		/// <summary>
		/// Create event lock to use with <c>using</c> keyword.
		/// </summary>
		/// <returns></returns>
		public IDisposable EventLock()
		{
			return new EventLock(this);
		}

		#endregion

		#region ISyncRoot Members

		/// <summary>Exposes synchronisation root.</summary>
		public object SyncRoot
		{
			get { return m_Dictionary.SafeSyncRoot(); }
		}

		#endregion
	}

	#region enum MonitoredDictionaryEventType

	/// <summary>
	/// Event types triggered by <see cref="MonitoredDictionary&lt;K,V&gt;"/>
	/// </summary>
	public enum MonitoredDictionaryEventType
	{
		/// <summary>No action.</summary>
		None,

		/// <summary>Events has been suspeded.</summary>
		Suspended,

		/// <summary>Events has been resumed.</summary>
		Resumed,

		/// <summary>Item is about to be added.</summary>
		Adding,

		/// <summary>Item has been added.</summary>
		Added,

		/// <summary>Item is about to be removed.</summary>
		Removing,

		/// <summary>Item removed.</summary>
		Removed,

		/// <summary>Item has not been removed, because it was in dictionary.</summary>
		Missed,

		/// <summary>Setting value for a given key.</summary>
		SettingAt,

		/// <summary>Values has been set for given key.</summary>
		SetAt,

		/// <summary>Dictionary is about to be cleared.</summary>
		Clearing,

		/// <summary>Dictionary will be clearead (no chance to cancel anymore).</summary>
		ClearApproved,

		/// <summary>
		/// Dictionary has been cleared.
		/// </summary>
		Cleared,
	}

	#endregion

	#region delegate MonitoredDictionaryEvent

	/// <summary>
	/// Event triggered by <see cref="MonitoredDictionary&lt;K,V&gt;"/>.
	/// </summary>
	public delegate void MonitoredDictionaryEvent<K, V>(object sender, MonitoredDictionaryEventArgs<K, V> args);

	#endregion

	#region class MonitoredDictionaryEventArgs<K, V>

	/// <summary>
	/// Arguments for events triggered by <see cref="MonitoredDictionary&lt;K,V&gt;"/>.
	/// </summary>
	/// <typeparam name="K">Key type.</typeparam>
	/// <typeparam name="V">Value type.</typeparam>
	public class MonitoredDictionaryEventArgs<K, V>: CancelEventArgs
	{
		#region properties

		/// <summary>Gets or sets the type of event.</summary>
		/// <value>The event type.</value>
		public MonitoredDictionaryEventType EventType { get; protected internal set; }

		/// <summary>Gets or sets the key.</summary>
		/// <value>The key.</value>
		public K Key { get; protected internal set; }

		/// <summary>Gets or sets the new value.</summary>
		/// <value>The new value.</value>
		public V NewValue { get; protected internal set; }

		/// <summary>Gets or sets the old value.</summary>
		/// <value>The old value.</value>
		public V OldValue { get; protected internal set; }

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="MonitoredDictionaryEventArgs&lt;K, V&gt;"/> class.</summary>
		/// <param name="type">The type.</param>
		/// <param name="key">The key.</param>
		/// <param name="newValue">The new value.</param>
		/// <param name="oldValue">The old value.</param>
		public MonitoredDictionaryEventArgs(MonitoredDictionaryEventType type, K key, V newValue, V oldValue)
		{
			EventType = type;
			Key = key;
			NewValue = newValue;
			OldValue = oldValue;
		}

		#endregion
	}

	#endregion
}
