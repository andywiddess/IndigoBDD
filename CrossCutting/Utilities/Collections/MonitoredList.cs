using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Indigo.CrossCutting.Utilities.Events;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Creates monitored list on top of any list.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class MonitoredList<T>: IList<T>, ISuspendableEvents, IBindingList
	{
		#region fields

		/// <summary>
		/// Original list.
		/// </summary>
		private readonly IList<T> m_List;

		/// <summary>
		/// Indicates if events has been suspended.
		/// </summary>
		private int m_Suspended;

		/// <summary>Indicates if type T is actually a ValueType, it is needed in IList implementation and as it never 
		/// changes, it can be precalculated</summary>
		private static readonly bool m_IsValueType = typeof(T).IsValueType;

		#endregion

		#region properties

		/// <summary>Gets or sets a value indicating whether adding new item should fire event with index.
		/// The problem is default <see cref="IList{T}"/> does not require <see cref="M:IList{T}.Add()"/> to
		/// return an index, but events do want it. So after adding an element we need to find it. In most cases
		/// it is the last element, so finding it is easy, but "in most cases" is not "always" so it can be expensive
		/// sometime, especially when element type has complex implementation of <see cref="M:System.Object.Equals()"/>.
		/// So, prefered value for <see cref="AlwaysReturnIndex"/> is <c>false</c>.
		/// </summary>
		/// <value><c>true</c> if adding new item should fire event with index; otherwise, <c>false</c>.</value>
		public bool AlwaysReturnIndex { get; set; }

		/// <summary>
		/// Gets or the inner list.
		/// Please understand the consequences of using this inner list... If you don't know, don't use! ;-)
		/// </summary>
		/// <value>
		/// The inner list.
		/// </value>
		public IList<T> InnerList
		{
			get
			{
				return m_List;
			}
		}

		#endregion

		#region events

		/// <summary>Occurs when notification is needed.</summary>
		public event MonitoredListEvent<T> Notification;

		/// <summary>Handler for IBindingList.</summary>
		private ListChangedEventHandler BindingListHandler;

		/// <summary>Raises the specified event type.</summary>
		/// <param name="eventType">Type of the event.</param>
		/// <returns><c>true</c> event has not been cancelled.</returns>
		public bool Raise(MonitoredListEventType eventType)
		{
			return Raise(eventType, default(T), -1);
		}

		/// <summary>Raises the specified event type.</summary>
		/// <param name="eventType">Type of the event.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> event has not been cancelled.</returns>
		public bool Raise(MonitoredListEventType eventType, T item)
		{
			return Raise(eventType, item, -1);
		}

		/// <summary>Raises the specified event type.</summary>
		/// <param name="eventType">Type of the event.</param>
		/// <param name="item">The item.</param>
		/// <param name="index">The index.</param>
		/// <returns><c>true</c> event has not been cancelled.</returns>
		public bool Raise(MonitoredListEventType eventType, T item, int index)
		{
			var result = true;
			var send = 
				(m_Suspended <= 0) ||
				(eventType == MonitoredListEventType.Suspended) ||
				(eventType == MonitoredListEventType.Resumed);

			if (send && Notification != null)
			{
				var args = new MonitoredListEventArgs<T>(eventType, item, index);
				Notification(this, args);
				result = !args.Cancel;
			}

			// if action has not been canceled
			if (send && result)
			{
				RaiseBindingListEvent(eventType, index);
			}

			return true;
		}

		private void RaiseBindingListEvent(MonitoredListEventType eventType, int index)
		{
			if (BindingListHandler == null) return;

			ListChangedEventArgs args = null;
			switch (eventType)
			{
				case MonitoredListEventType.Added:
					args = new ListChangedEventArgs(ListChangedType.ItemAdded, index);
					break;
				case MonitoredListEventType.Cleared:
					args = new ListChangedEventArgs(ListChangedType.Reset, -1);
					break;
				case MonitoredListEventType.Inserted:
					args = new ListChangedEventArgs(ListChangedType.ItemAdded, index);
					break;
				case MonitoredListEventType.Removed:
					args = new ListChangedEventArgs(ListChangedType.ItemDeleted, index);
					break;
				case MonitoredListEventType.RemovedAt:
					args = new ListChangedEventArgs(ListChangedType.ItemDeleted, index);
					break;
				case MonitoredListEventType.SetAt:
					args = new ListChangedEventArgs(ListChangedType.ItemChanged, index);
					break;
				case MonitoredListEventType.Resumed:
					args = new ListChangedEventArgs(ListChangedType.Reset, -1);
					break;
				default:
					/* BindingList does not handle other events */
					break;
			}

			if (args != null)
			{
				BindingListHandler(this, args);
			}
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="MonitoredList&lt;T&gt;"/> class.</summary>
		public MonitoredList()
			: this(new List<T>())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="list">The list.</param>
		public MonitoredList(IList<T> list)
		{
			if (list == null)
				throw new ArgumentNullException("list", "list is null.");
			m_List = list;
		}

		#endregion

		#region utilities

		/// <summary>
		/// Helper method. Finds value in collection.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="index">The index.</param>
		/// <returns>Index of value found or <c>-1</c></returns>
		private int FindValue(T value, int index)
		{
			if (typeof(T).IsValueType)
			{
				if (index >= 0 && EqualByValue(value, index)) return index;
				for (int i = 0; i < m_List.Count; i++)
				{
					// do not check at 'index' again, no need
					if (i != index && EqualByValue(value, i)) return i;
				}
				return -1;
			}
			else
			{
				// try to find by reference first (is much faster)
				if (index >= 0 && EqualByReference(value, index)) return index;
				for (int i = 0; i < m_List.Count; i++)
				{
					if (EqualByReference(value, i)) return i;
				}

				// if no success try to find by value
				if (index >= 0 && EqualByValue(value, index)) return index;
				for (int i = 0; i < m_List.Count; i++)
				{
					if (EqualByValue(value, i)) return i;
				}
				return -1;
			}
		}

		/// <summary>
		/// Checks if item at given index is equal by reference.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="index">The index.</param>
		/// <returns><c>true</c> is equal, <c>false</c> otherwise</returns>
		private bool EqualByReference(T value, int index)
		{
			return object.ReferenceEquals(m_List[index], value);
		}

		/// <summary>
		/// Checks if item at given index is equal by value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="index">The index.</param>
		/// <returns><c>true</c> if equal, <c>false</c> otherwise</returns>
		private bool EqualByValue(T value, int index)
		{
			return object.Equals(m_List[index], value);
		}

		/// <summary>
		/// Adds many items.
		/// </summary>
		/// <param name="collection">The collection of items to add.</param>
		public void AddMany(IEnumerable<T> collection)
		{
			foreach (var item in collection) Add(item);
		}

		/// <summary>Notifies the item has changed.</summary>
		/// <param name="item">The item..</param>
		public void NotifyItemChanged(T item)
		{
			if (m_Suspended <= 0)
			{
				var index = m_List.IndexOf(item);
				if (index >= 0)
				{
					Raise(MonitoredListEventType.SetAt, item, index);
				}
			}
		}

		/// <summary>Notifies the item at given index has changed.</summary>
		/// <param name="index">The index.</param>
		public void NotifyItemChangedAt(int index)
		{
			if (m_Suspended <= 0 && index >= 0)
			{
				Raise(MonitoredListEventType.SetAt, m_List[index], index);
			}
		}

		/// <summary>Notifies that the list has changed. Enforces all listeners to refresh.</summary>
		public void NotifyListChanged()
		{
			if (m_Suspended <= 0)
			{
				Raise(MonitoredListEventType.Suspended);
				Raise(MonitoredListEventType.Resumed);
			}
		}

		#endregion

		#region IList<T> Members

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <returns>
		/// The index of <paramref name="item"/> if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf(T item)
		{
			return m_List.IndexOf(item);
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
		public void Insert(int index, T item)
		{
			if (Raise(MonitoredListEventType.Inserting, item, index))
			{
				m_List.Insert(index, item);
				Raise(MonitoredListEventType.Inserted, item);
			}
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
		public void RemoveAt(int index)
		{
			var item = SafeGetItemAt(index);
			if (Raise(MonitoredListEventType.RemovingAt, item, index))
			{
				m_List.RemoveAt(index);
				Raise(MonitoredListEventType.RemovedAt, item, index);
			}
		}

		/// <summary>
		/// Sets the item at.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		private void SetItemAt(int index, T value)
		{
			var item = SafeGetItemAt(index);
			if (Raise(MonitoredListEventType.SettingAt, item, index))
			{
				m_List[index] = value;
				Raise(MonitoredListEventType.SetAt, item, index); // TODO:MAK it passes old item!!! it should be two proerties
			}
		}

		/// <summary>
		/// Safes the get item at.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private T SafeGetItemAt(int index)
		{
			if (index >= 0 && index < m_List.Count)
			{
				return m_List[index];
			}
			return default(T);
		}

		/// <summary>
		/// Gets or sets the <typeparamref name="T"/> at the specified index.
		/// </summary>
		/// <value></value>
		public T this[int index]
		{
			get { return m_List[index]; }
			set { SetItemAt(index, value); }
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>Decides if the index should be returned.</summary>
		/// <returns><c>true</c> if it should; <c>false</c> otherwise</returns>
		private bool ShouldReturnIndex()
		{
			return (AlwaysReturnIndex || (BindingListHandler != null)) && (m_Suspended <= 0);
		}

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> 
		/// is read-only.</exception>
		public void Add(T item)
		{
			if (Raise(MonitoredListEventType.Adding, item))
			{
				m_List.Add(item);
				bool isIndexRequired = ShouldReturnIndex();
				int index = isIndexRequired ? FindValue(item, m_List.Count - 1) : -1;
				Raise(MonitoredListEventType.Added, item, index);
			}
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>. Returns index in a list where item has been added.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns></returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public int AddAndFind(T item)
		{
			if (Raise(MonitoredListEventType.Adding, item))
			{
				m_List.Add(item);
				int index = FindValue(item, m_List.Count - 1);
				Raise(MonitoredListEventType.Added, item, index);
				return index;
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// Finds the item and removes it. 
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>Index ehre item was.</returns>
		public int FindAndRemove(T item)
		{
			int index = FindValue(item, -1);
			if (index < 0) return index;

			item = m_List[index];
			if (Raise(MonitoredListEventType.RemovingAt, item, index))
			{
				m_List.RemoveAt(index);
				Raise(MonitoredListEventType.RemovedAt, item, index);
				return index;
			}

			return -1;
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			if (Raise(MonitoredListEventType.Clearing))
			{
				Raise(MonitoredListEventType.ClearApproved);
				m_List.Clear();
				Raise(MonitoredListEventType.Cleared);
			}
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
		/// </returns>
		public bool Contains(T item)
		{
			return m_List.Contains(item);
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
		/// 	<paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			m_List.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count
		{
			get { return m_List.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return m_List.IsReadOnly; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public bool Remove(T item)
		{
			if (ShouldReturnIndex())
			{
				return FindAndRemove(item) >= 0;
			}
			else
			{
				bool result = false;
				if (Raise(MonitoredListEventType.Removing, item))
				{
					result = m_List.Remove(item);
					Raise(result ? MonitoredListEventType.Removed : MonitoredListEventType.Missed, item);
				}
				return result;
			}
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return m_List.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_List.GetEnumerator();
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
				Raise(MonitoredListEventType.Suspended);
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
				Raise(MonitoredListEventType.Resumed);
			}
		}

		#endregion

		#region IBindingList Members

		/// <summary>
		/// Adds the <see cref="T:System.ComponentModel.PropertyDescriptor"/> to the indexes used for searching.
		/// </summary>
		/// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to add to the indexes used for searching.</param>
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Adds a new item to the list.
		/// </summary>
		/// <returns>
		/// The item added to the list.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.ComponentModel.IBindingList.AllowNew"/> is false. </exception>
		object IBindingList.AddNew()
		{
			// IMP:MAK IBindingList.AddNew could be implemented
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets whether you can update items in the list.
		/// </summary>
		/// <returns>true if you can update the items in the list; otherwise, false.</returns>
		bool IBindingList.AllowEdit
		{
			get { return true; }
		}

		/// <summary>
		/// Gets whether you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew"/>.
		/// </summary>
		/// <returns>true if you can add items to the list using <see cref="M:System.ComponentModel.IBindingList.AddNew"/>; otherwise, false.</returns>
		bool IBindingList.AllowNew
		{
			get { return true; }
		}

		/// <summary>
		/// Gets whether you can remove items from the list, using <see cref="M:System.Collections.IList.Remove(System.Object)"/> or <see cref="M:System.Collections.IList.RemoveAt(System.Int32)"/>.
		/// </summary>
		/// <returns>true if you can remove items from the list; otherwise, false.</returns>
		bool IBindingList.AllowRemove
		{
			get { return true; }
		}

		/// <summary>
		/// Sorts the list based on a <see cref="T:System.ComponentModel.PropertyDescriptor"/> and a <see cref="T:System.ComponentModel.ListSortDirection"/>.
		/// </summary>
		/// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to sort by.</param>
		/// <param name="direction">One of the <see cref="T:System.ComponentModel.ListSortDirection"/> values.</param>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns the index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor"/>.
		/// </summary>
		/// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to search on.</param>
		/// <param name="key">The value of the <paramref name="property"/> parameter to search for.</param>
		/// <returns>
		/// The index of the row that has the given <see cref="T:System.ComponentModel.PropertyDescriptor"/>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		/// <see cref="P:System.ComponentModel.IBindingList.SupportsSearching"/> is false. </exception>
		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets whether the items in the list are sorted.
		/// </summary>
		/// <returns>true if <see cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"/> has been called and <see cref="M:System.ComponentModel.IBindingList.RemoveSort"/> has not been called; otherwise, false.</returns>
		/// <exception cref="T:System.NotSupportedException">
		/// <see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
		bool IBindingList.IsSorted
		{
			get { return false; }
		}

		/// <summary>
		/// Occurs when the list changes or an item in the list changes.
		/// </summary>
		event ListChangedEventHandler IBindingList.ListChanged
		{
			add { BindingListHandler += value; }
			remove { BindingListHandler -= value; }
		}

		/// <summary>
		/// Removes the <see cref="T:System.ComponentModel.PropertyDescriptor"/> from the indexes used for searching.
		/// </summary>
		/// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor"/> to remove from the indexes used for searching.</param>
		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Removes any sort applied using <see cref="M:System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor,System.ComponentModel.ListSortDirection)"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
		void IBindingList.RemoveSort()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets the direction of the sort.
		/// </summary>
		/// <returns>One of the <see cref="T:System.ComponentModel.ListSortDirection"/> values.</returns>
		///   
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
		ListSortDirection IBindingList.SortDirection
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Gets the <see cref="T:System.ComponentModel.PropertyDescriptor"/> that is being used for sorting.
		/// </summary>
		/// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor"/> that is being used for sorting.</returns>
		///   
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.ComponentModel.IBindingList.SupportsSorting"/> is false. </exception>
		PropertyDescriptor IBindingList.SortProperty
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Gets whether a <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> event is raised when the list changes or an item in the list changes.
		/// </summary>
		/// <returns>true if a <see cref="E:System.ComponentModel.IBindingList.ListChanged"/> event is raised when the list changes or when an item changes; otherwise, false.</returns>
		bool IBindingList.SupportsChangeNotification
		{
			get { return true; }
		}

		/// <summary>
		/// Gets whether the list supports searching using the <see cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)"/> method.
		/// </summary>
		/// <returns>true if the list supports searching using the <see cref="M:System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor,System.Object)"/> method; otherwise, false.</returns>
		bool IBindingList.SupportsSearching
		{
			get { return false; }
		}

		/// <summary>
		/// Gets whether the list supports sorting.
		/// </summary>
		/// <returns>true if the list supports sorting; otherwise, false.</returns>
		bool IBindingList.SupportsSorting
		{
			get { return false; }
		}

		#endregion

		#region IList Members

		/// <summary>Determines whether this instance can contain the specified value.
		/// It is not a part of IList interface but is strongly related to it.</summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if this instance can contain the specified value; otherwise, <c>false</c>.</returns>
		private static bool CanContain(object value)
		{
			return (value is T || (value == null && !m_IsValueType));
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The object to add to the <see cref="T:System.Collections.IList"/>.</param>
		/// <returns>
		/// The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection,
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
		int IList.Add(object value)
		{
			return AddAndFind((T)value);
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
		void IList.Clear()
		{
			Clear();
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
		/// </summary>
		/// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>.</param>
		/// <returns>
		/// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
		/// </returns>
		bool IList.Contains(object value)
		{
			return CanContain(value) ? Contains((T)value) : false;
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The object to locate in the <see cref="T:System.Collections.IList"/>.</param>
		/// <returns>
		/// The index of <paramref name="value"/> if found in the list; otherwise, -1.
		/// </returns>
		int IList.IndexOf(object value)
		{
			return CanContain(value) ? IndexOf((T)value) : -1;
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted.</param>
		/// <param name="value">The object to insert into the <see cref="T:System.Collections.IList"/>.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
		/// <exception cref="T:System.NullReferenceException">
		///   <paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
		void IList.Insert(int index, object value)
		{
			Insert(index, (T)value);
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
		/// </summary>
		/// <returns>true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.</returns>
		bool IList.IsFixedSize
		{
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
		/// </summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
		bool IList.IsReadOnly
		{
			get { return m_List.IsReadOnly; }
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The object to remove from the <see cref="T:System.Collections.IList"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
		void IList.Remove(object value)
		{
			if (value is T || value == null)
			{
				Remove((T)value);
			}
			/* else
			{
				// nothing to delete
			} */
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		void IList.RemoveAt(int index)
		{
			RemoveAt(index);
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
		/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
		object IList.this[int index]
		{
			get { return this[index]; }
			set { this[index] = (T)value; }
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array"/> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index"/> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
		/// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
		void ICollection.CopyTo(Array array, int index)
		{
			for (int i = 0; i < m_List.Count; i++)
				array.SetValue(m_List[i], index + i);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		int ICollection.Count
		{
			get { return m_List.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
		/// </summary>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
		object ICollection.SyncRoot
		{
			get { return m_List; }
		}

		#endregion
	}

	#region class MonitoredListEvent

	/// <summary>
	/// Events types of monitored list.
	/// </summary>
	public enum MonitoredListEventType
	{
		/// <summary>
		/// No event.
		/// </summary>
		None,

		/// <summary>
		/// Occurs before inserting.
		/// </summary>
		Inserting,

		/// <summary>
		/// Occurs after inserting.
		/// </summary>
		Inserted,

		/// <summary>
		/// Occurs before adding.
		/// </summary>
		Adding,

		/// <summary>
		/// Occurs after adding.
		/// </summary>
		Added,

		/// <summary>
		/// Occurs before clearing.
		/// </summary>
		Clearing,

		/// <summary>
		/// Occurs after clearing.
		/// </summary>
		Cleared,

		/// <summary>
		/// Occurs before clearing is done but after it has approved by all listeners (vetoing it won't help!).
		/// </summary>
		ClearApproved,

		/// <summary>
		/// Occurs before removing.
		/// </summary>
		Removing,

		/// <summary>
		/// Occurs after removing.
		/// </summary>
		Removed,

		/// <summary>
		/// Occurs when removing, but item wasn't in collection.
		/// </summary>
		Missed,

		/// <summary>
		/// Occurs before removing at index.
		/// </summary>
		RemovingAt,

		/// <summary>
		/// Occurs after removing at index.
		/// </summary>
		RemovedAt,

		/// <summary>
		/// Occurs before setting at index.
		/// </summary>
		SettingAt,

		/// <summary>
		/// Occurs after setting at index.
		/// </summary>
		SetAt,

		/// <summary>
		/// Occurs when events are suspended.
		/// </summary>
		Suspended,

		/// <summary>
		/// Occurs when events are resumed.
		/// </summary>
		Resumed,
	}

	/// <summary>
	/// Monitores list event handler delegate.
	/// </summary>
	public delegate void MonitoredListEvent<T>(object sender, MonitoredListEventArgs<T> args);

	/// <summary>
	/// Monitored list event hanlder args.
	/// </summary>
	/// <typeparam name="T">Type of list item.</typeparam>
	public class MonitoredListEventArgs<T>: CancelEventArgs
	{
		#region properties

		/// <summary>
		/// Gets or sets the type of the event.
		/// </summary>
		/// <value>The type of the event.</value>
		public MonitoredListEventType EventType { get; protected internal set; }

		/// <summary>
		/// Gets or sets the index.
		/// </summary>
		/// <value>The index.</value>
		public int Index { get; protected internal set; }

		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
		public T Item { get; protected internal set; }

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredListEventArgs&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="eventType">The event type.</param>
		/// <param name="item">The item.</param>
		/// <param name="index">The index.</param>
		public MonitoredListEventArgs(MonitoredListEventType eventType, T item, int index)
		{
			EventType = eventType;
			Item = item;
			Index = index;
		}

		#endregion
	}

	/// <summary>
	/// Monitored list event handler.
	/// </summary>
	/// <typeparam name="T">Type of list item.</typeparam>
	public interface IListEventHandler<T>
	{
		/// <summary>
		/// Handles the event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="Indigo.CrossCutting.Utilities.Collections.MonitoredListEventArgs&lt;T&gt;"/> 
		/// instance containing the event data.</param>
		void ListEvent(object sender, MonitoredListEventArgs<T> args);
	}

	#endregion
}
