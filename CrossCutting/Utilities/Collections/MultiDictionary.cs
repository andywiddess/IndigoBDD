using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// MultiDictionary allowing to store multiple values for same key.
	/// It's not finished, 
	/// </summary>
	/// <typeparam name="K">Type of key.</typeparam>
	/// <typeparam name="V">Type of value.</typeparam>
	public class MultiDictionary<K, V>
	{
		#region fields

		/// <summary>Indicates of dictionary allows duplicated values.</summary>
		private readonly bool m_AllowDuplicates = true;

		/// <summary>Factory creating collections when needed.</summary>
		private readonly Func<ICollection<V>> m_CollectionFactory;

		/// <summary>Internal storage.</summary>
		private readonly Dictionary<K, ICollection<V>> m_Storage = new Dictionary<K, ICollection<V>>();

		#endregion

		#region properties

		/// <summary>
		/// Gets the <see cref="System.Collections.Generic.ICollection&lt;V&gt;"/> with the specified key.
		/// If the key isn't present in the collection it will return an empty ICollection of type V.
		/// </summary>
		public ICollection<V> this[K key]
		{
			get
			{
				if (key == null)
					return EmptyCollection<V>.Default;
				else
					return m_Storage.TryGetValue(key, EmptyCollection<V>.Default); 
			}
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="MultiDictionary&lt;K, V&gt;"/> class.</summary>
		/// <param name="allowDuplicates">if set to <c>true</c> [allow duplicates].</param>
		public MultiDictionary(bool allowDuplicates)
		{
			m_AllowDuplicates = allowDuplicates;

			if (allowDuplicates)
			{
				m_CollectionFactory = () => new LinkedList<V>();
			}
			else
			{
				m_CollectionFactory = () => new HashSet<V>();
			}
		}

		#endregion

		#region private implementation

		/// <summary>Returns the row for the specified key.</summary>
		/// <param name="key">The key.</param>
		/// <param name="create">if set to <c>true</c> row is created when it does not exist.</param>
		/// <returns>A row for given key of <c>null</c> if it does not exist and <paramref name="create"/> is <c>false</c>.</returns>
		private ICollection<V> Row(K key, bool create = false)
		{
			if (create)
			{
				return m_Storage.TryGetValueOrCreate(key, (k) => m_CollectionFactory(), true);
			}
			else
			{
				return m_Storage.TryGetValue(key);
			}
		}

		#endregion

		#region public interface

		/// <summary>Determines whether dictionary contains key.</summary>
		/// <param name="key">The key.</param>
		/// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
		public bool ContainsKey(K key)
		{
			return m_Storage.ContainsKey(key);
		}

		/// <summary>Removes the value for dictionary.</summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if values has been removed; <c>false</c> otherwise.</returns>
		public bool Remove(K key, V value)
		{
			bool result = false;

			var row = Row(key, false);
			if (row != null)
			{
				result = row.Remove(value);
				if (row.Count == 0) m_Storage.Remove(key);
			}

			return result;
		}

		/// <summary>Adds the value to dictionary.</summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public void Add(K key, V value)
		{
			var row = Row(key, true);
			row.Add(value);
		}

		/// <summary>Removes the specified key, thus removing all values attached to it.</summary>
		/// <param name="key">The key.</param>
		/// <returns><c>true</c> if key has been removed; <c>false</c> otherwise;</returns>
		public bool Remove(K key)
		{
			return m_Storage.Remove(key);
		}

		#endregion
	}
}
