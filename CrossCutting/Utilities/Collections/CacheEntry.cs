using System;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Cache entry.</summary>
	/// <typeparam name="K">Type of key.</typeparam>
	/// <typeparam name="V">Type of value.</typeparam>
	internal class CacheEntry<K, V>
	{
		#region fields

		/// <summary>Key.</summary>
		private readonly K m_Key;

		/// <summary>WeakReference to cached value.</summary>
		private readonly WeakReference m_Reference;

		#endregion

		#region properties

		/// <summary>Gets the key.</summary>
		public K Key
		{
			get { return m_Key; }
		}

		/// <summary>Gets the reference.</summary>
		public WeakReference Reference
		{
			get { return m_Reference; }
		}

		/// <summary>Gets a value indicating whether cached value is alive.</summary>
		/// <value><c>true</c> if cached value is alive; otherwise, <c>false</c>.</value>
		public bool IsAlive
		{
			get { return m_Reference.IsAlive; }
		}

		/// <summary>Gets or sets the cached value.</summary>
		/// <value>The cached value.</value>
		public V Value
		{
			get { return (V)Reference.Target; }
			set { Reference.Target = value; }
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="CacheEntry&lt;K, V&gt;"/> class.</summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public CacheEntry(K key, V value)
		{
			m_Key = key;
			m_Reference = new WeakReference(value);
		}

		#endregion

		#region Cyclic list aspect

		/// <summary>Gets or sets the previous item in a list.</summary>
		/// <value>Previous item in a list.</value>
		public CacheEntry<K, V> Prev { get; set; }

		/// <summary>Gets or sets the next item in a list.</summary>
		/// <value>Next item in a list.</value>
		public CacheEntry<K, V> Next { get; set; }

		/// <summary>Enforces item to be cyclic list.</summary>
		internal void MakeCyclic()
		{
			Prev = Next = this;
		}

		/// <summary>Inserts new item before 'this'.</summary>
		/// <param name="item">The item to insert.</param>
		/// <returns>Inserted item.</returns>
		internal CacheEntry<K, V> InsertBefore(CacheEntry<K, V> item)
		{
			var list = this; // this is redundant but makes it a little bit easier to read

			item.Prev = list.Prev;
			item.Next = list;

			if (item.Prev != null)
			{
				item.Prev.Next = item;
			}
			if (item.Next != null)
			{
				item.Next.Prev = item;
			}

			return item;
		}

		/// <summary>Removes this instance from a list.</summary>
		/// <returns><c>null</c> if it was the list item; next or previous if there are some items left</returns>
		internal CacheEntry<K, V> RemoveItself()
		{
			var list = this;

			var prev = list.Prev;
			var next = list.Next;

			list.Prev = null;
			list.Next = null;

			if (prev == next)
			{
				var item = prev; // = next

				if ((item == null) || (item == list))
				{
					return null;
				}
				else
				{
					// create cyclic list
					item.Prev = item;
					item.Next = item;
					return item;
				}
			}
			else
			{
				if (prev != null)
				{
					prev.Next = next;
				}
				if (next != null)
				{
					next.Prev = prev;
				}

				return next ?? prev;
			}
		}

		#endregion
	}
}
