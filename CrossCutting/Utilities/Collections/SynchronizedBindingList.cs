using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Indigo.CrossCutting.Utilities.Collections
{
	#region class SynchronizedBindingList

	/// <summary>IBindingList which fires events synchronized to target.</summary>
	public class SynchronizedBindingList: IBindingList, ISynchronizeInvoke
	{
		#region fields

		/// <summary>Local sync object.</summary>
		private readonly object m_SyncRoot = new object();

		/// <summary>Synchronization target.</summary>
		private readonly ISynchronizeInvoke m_Target;

		/// <summary>Type of synchronization.</summary>
		private readonly bool m_Async;

		/// <summary>Original IBindingList.</summary>
		private readonly IBindingList m_Source;

		/// <summary>Should it use original sender as sender.</summary>
		private readonly bool m_UseOriginalSender;

		/// <summary>Indicates if this is listening on source events.</summary>
		private bool m_AttachedToSource;

		/// <summary>Event handler.</summary>
		private ListChangedEventHandler m_Handlers;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SynchronizedBindingList"/> class.
		/// </summary>
		/// <param name="source">The source IBindingList (source of events).</param>
		/// <param name="target">The target control (target of events).</param>
		/// <param name="async">if set to <c>true</c> events will be called asynchronously to <c>this</c> thread
		/// (fire and forget), <c>false</c> if you want to wait for return (nicer but, prone to deadlock).</param>
		/// <param name="useOriginalSender">if set to <c>true</c> passes original sender. It can be quite dangerous because 
		/// if you want to unregister from sender inside handler, it wont work (as you are not actually registered to original 
		/// sender).</param>
		public SynchronizedBindingList(IBindingList source, ISynchronizeInvoke target, bool async = false, bool useOriginalSender = false)
		{
			if (source == null)
				throw new ArgumentNullException("source", "source is null.");

			m_Target = target;
			m_Source = source;
			m_Async = async;
			m_UseOriginalSender = useOriginalSender;
		}

		#endregion

		#region event handler

		/// <summary>Internal event handler.</summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.ListChangedEventArgs"/> instance containing the event data.</param>
		private void SynchronizedListChanged(object sender, ListChangedEventArgs e)
		{
			var handlers = m_Handlers;
			if (handlers != null)
			{
				if (!m_UseOriginalSender) sender = this;

				if (m_Target == null || !m_Target.InvokeRequired)
				{
					handlers(sender, e);
				}
				else
				{
					var args = new object[] { sender, e };
					if (m_Async)
					{
						m_Target.Invoke(handlers, args);
					}
					else
					{
						m_Target.BeginInvoke(handlers, args);
					}
				}
			}
		}

		#endregion

		#region ISynchronizeInvoke Members

		IAsyncResult ISynchronizeInvoke.BeginInvoke(Delegate method, object[] args)
		{
			return m_Target.BeginInvoke(method, args);
		}

		object ISynchronizeInvoke.EndInvoke(IAsyncResult result)
		{
			return m_Target.EndInvoke(result);
		}

		object ISynchronizeInvoke.Invoke(Delegate method, object[] args)
		{
			return m_Target.Invoke(method, args);
		}

		bool ISynchronizeInvoke.InvokeRequired
		{
			get { return m_Target.InvokeRequired; }
		}

		#endregion

		#region IBindingList Members

		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			m_Source.AddIndex(property);
		}

		object IBindingList.AddNew()
		{
			return m_Source.AddNew();
		}

		bool IBindingList.AllowEdit
		{
			get { return m_Source.AllowEdit; }
		}

		bool IBindingList.AllowNew
		{
			get { return m_Source.AllowNew; }
		}

		bool IBindingList.AllowRemove
		{
			get { return m_Source.AllowRemove; }
		}

		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			m_Source.ApplySort(property, direction);
		}

		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			return m_Source.Find(property, key);
		}

		bool IBindingList.IsSorted
		{
			get { return m_Source.IsSorted; }
		}

		event ListChangedEventHandler IBindingList.ListChanged
		{
			add
			{
				lock (m_SyncRoot)
				{
					m_Handlers += value;
					if (!m_AttachedToSource)
					{
						m_Source.ListChanged += SynchronizedListChanged;
						m_AttachedToSource = true;
					}
				}
			}
			remove
			{
				lock (m_SyncRoot)
				{
					m_Handlers -= value;
					if (m_Handlers == null && m_AttachedToSource)
					{
						m_Source.ListChanged -= SynchronizedListChanged;
						m_AttachedToSource = false;
					}
				}
			}
		}

		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
			m_Source.RemoveIndex(property);
		}

		void IBindingList.RemoveSort()
		{
			m_Source.RemoveSort();
		}

		ListSortDirection IBindingList.SortDirection
		{
			get { return m_Source.SortDirection; }
		}

		PropertyDescriptor IBindingList.SortProperty
		{
			get { return m_Source.SortProperty; }
		}

		bool IBindingList.SupportsChangeNotification
		{
			get { return m_Source.SupportsChangeNotification; }
		}

		bool IBindingList.SupportsSearching
		{
			get { return m_Source.SupportsSearching; }
		}

		bool IBindingList.SupportsSorting
		{
			get { return m_Source.SupportsSorting; }
		}

		#endregion

		#region IList Members

		int System.Collections.IList.Add(object value)
		{
			return m_Source.Add(value);
		}

		void System.Collections.IList.Clear()
		{
			m_Source.Clear();
		}

		bool System.Collections.IList.Contains(object value)
		{
			return m_Source.Contains(value);
		}

		int System.Collections.IList.IndexOf(object value)
		{
			return m_Source.IndexOf(value);
		}

		void System.Collections.IList.Insert(int index, object value)
		{
			m_Source.Insert(index, value);
		}

		bool System.Collections.IList.IsFixedSize
		{
			get { return m_Source.IsFixedSize; }
		}

		bool System.Collections.IList.IsReadOnly
		{
			get { return m_Source.IsReadOnly; }
		}

		void System.Collections.IList.Remove(object value)
		{
			m_Source.Remove(value);
		}

		void System.Collections.IList.RemoveAt(int index)
		{
			m_Source.RemoveAt(index);
		}

		object System.Collections.IList.this[int index]
		{
			get { return m_Source[index]; }
			set { m_Source[index] = value; }
		}

		#endregion

		#region ICollection Members

		void System.Collections.ICollection.CopyTo(Array array, int index)
		{
			m_Source.CopyTo(array, index);
		}

		int System.Collections.ICollection.Count
		{
			get { return m_Source.Count; }
		}

		bool System.Collections.ICollection.IsSynchronized
		{
			get { return m_Source.IsSynchronized; }
		}

		object System.Collections.ICollection.SyncRoot
		{
			get { return m_Source.SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return m_Source.GetEnumerator();
		}

		#endregion
	}

	#endregion

	#region class SynchronizedBindingList<T>

	/// <summary>IBindingList which fires events synchronized to target. It implicitely requires but also implements 
	/// <see cref="IList{T}"/>. This apparently helps some UI components which try to guess what elements will be stored
	/// in collection even if it is empty.</summary>
	/// <typeparam name="T"></typeparam>
	public class SynchronizedBindingList<T>: SynchronizedBindingList, IList<T>
	{
		#region fields

		private readonly IList<T> m_Typed;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SynchronizedBindingList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="source">The source IBindingList (source of events). Please note, this parameter does not have
		/// be <see cref="IList{T}"/>, but it will be quicker if it is.</param>
		/// <param name="target">The target control (target of events).</param>
		/// <param name="async">if set to <c>true</c> events will be called asynchronously to <c>this</c> thread
		/// (fire and forget), <c>false</c> if you want to wait for return (nicer but, prone to deadlock).</param>
		/// <param name="useOriginalSender">if set to <c>true</c> passes original sender. It can be quite dangerous because
		/// if you want to unregister from sender inside handler, it wont work (as you are not actually registered to original
		/// sender).</param>
		public SynchronizedBindingList(
			IBindingList source, ISynchronizeInvoke target, bool async = false, bool useOriginalSender = false)
			: base(source, target, async, useOriginalSender)
		{
			if (source == null)
				throw new ArgumentNullException("source", "source is null.");
			var typed = source as IList<T>;
			if (typed == null) typed = source.TypedAs<T>();
			m_Typed = typed;
		}

		#endregion

		#region IList<T> Members

		/// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.</summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		/// <returns>The index of item if found in the list; otherwise, -1.</returns>
		public int IndexOf(T item)
		{
			return m_Typed.IndexOf(item);
		}

		/// <summary>Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.</summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		public void Insert(int index, T item)
		{
			m_Typed.Insert(index, item);
		}

		/// <summary>Removes the <see cref="T:System.Collections.IList"/> item at the specified index.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
		public void RemoveAt(int index)
		{
			m_Typed.RemoveAt(index);
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
		/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IList"/> is read-only. </exception>
		public T this[int index]
		{
			get { return m_Typed[index]; }
			set { m_Typed[index] = value; }
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(T item)
		{
			m_Typed.Add(item);
		}

		/// <summary>Removes all items from the <see cref="T:System.Collections.IList"/>.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only. </exception>
		public void Clear()
		{
			m_Typed.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.</summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.</returns>
		public bool Contains(T item)
		{
			return m_Typed.Contains(item);
		}

		/// <summary>Copies all elements to given array.</summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			m_Typed.CopyTo(array, arrayIndex);
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.</returns>
		public int Count
		{
			get { return m_Typed.Count; }
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList"/> is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.IList"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return m_Typed.IsReadOnly; }
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public bool Remove(T item)
		{
			return m_Typed.Remove(item);
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return m_Typed.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return m_Typed.GetEnumerator();
		}

		#endregion
	}

	#endregion
}
