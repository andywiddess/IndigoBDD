using System;
using System.Collections;
using System.Collections.Generic;
using Indigo.CrossCutting.Utilities.Events;
using Indigo.CrossCutting.Utilities.Extensions;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Monitored collection. Raises event in case collection is modified.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class MonitoredCollection<T>: ICollection<T>, ISuspendableEvents, ISyncRoot
	{
		#region fields

		/// <summary>
		/// Original collection.
		/// </summary>
		private readonly ICollection<T> m_Collection;

		/// <summary>
		/// Suspended counter.
		/// </summary>
		private int m_Suspended;

		#endregion

		#region events

		/// <summary>Occurs when notification is needed.</summary>
		public event EventHandler<MonitoredCollectionEventArgs<T>> Notification;

		/// <summary>Raises the specified event type.</summary>
		/// <param name="eventType">The event type.</param>
		/// <param name="allowCancel">if set to <c>true</c> canceling operation will be allowed.</param>
		/// <returns><c>true</c> if event has been canceled. Note, when suspended it will never be canceled.</returns>
		public bool Raise(MonitoredCollectionEventType eventType, bool allowCancel)
		{
			return Raise(eventType, default(T), allowCancel);
		}

		/// <summary>
		/// Raises the specified event type.
		/// </summary>
		/// <param name="eventType">Type of the event.</param>
		/// <param name="item">The item.</param>
		/// <param name="allowCancel">if set to <c>true</c> canceling is allowed.</param>
		/// <returns><c>true</c> if event has been allowed. Note, when suspended it will never be canceled.</returns>
		public bool Raise(MonitoredCollectionEventType eventType, T item, bool allowCancel)
		{
			// when suspended send only 'suspended' and 'resumed' events
			bool send =
					(m_Suspended <= 0) ||
					(eventType == MonitoredCollectionEventType.Suspended) ||
					(eventType == MonitoredCollectionEventType.Resumed);

			if (send && Notification != null)
			{
				var args = new MonitoredCollectionEventArgs<T>(eventType, item, allowCancel);
				Notification.SafeRaise(this, args);
				var canceled = args.Canceled;

				if (canceled)
				{
					args = new MonitoredCollectionEventArgs<T>(MonitoredCollectionEventType.Cancelled, item, false);
					Notification.SafeRaise(this, args);
					return false;
				}
			}

			return true;
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredCollection&lt;T&gt;"/> class.
		/// Creates monitored collection on top of other collection.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public MonitoredCollection(ICollection<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection", "collection is null.");
			m_Collection = collection;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredCollection&lt;T&gt;"/> class.
		/// Creates monitored collection on top of other collection.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="handler">The event handler.</param>
		public MonitoredCollection(ICollection<T> collection, EventHandler<MonitoredCollectionEventArgs<T>> handler)
			: this(collection)
		{
			Notification += handler;
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public void Add(T item)
		{
			if (Raise(MonitoredCollectionEventType.Adding, item, true))
			{
				m_Collection.Add(item);
				Raise(MonitoredCollectionEventType.Added, item, false);
			}
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			if (Raise(MonitoredCollectionEventType.Clearing, true))
			{
				Raise(MonitoredCollectionEventType.ClearApproved, false);
				m_Collection.Clear();
				Raise(MonitoredCollectionEventType.Cleared, false);
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
			return m_Collection.Contains(item);
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
			m_Collection.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count
		{
			get { return m_Collection.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return m_Collection.IsReadOnly; }
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
			bool result = false;
			if (Raise(MonitoredCollectionEventType.Removing, item, true))
			{
				result = m_Collection.Remove(item);
				if (result)
				{
					Raise(MonitoredCollectionEventType.Removed, item, false);
				}
			}
			return result;
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
			return m_Collection.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Collection.GetEnumerator();
		}

		#endregion

		#region ISuspendableEvents Members

		/// <summary>
		/// Suspends the events.
		/// </summary>
		public void SuspendEvents()
		{
			m_Suspended++;
			if (m_Suspended > 0)
			{
				Raise(MonitoredCollectionEventType.Suspended, false);
			}
		}

		/// <summary>
		/// Resumes the events.
		/// </summary>
		public void ResumeEvents()
		{
			m_Suspended--;
			if (m_Suspended == 0)
			{
				Raise(MonitoredCollectionEventType.Resumed, false);
			}
		}

		/// <summary>
		/// Create event lock. Actually, it just creates <see cref="IDisposable"/>
		/// object which calls <see cref="SuspendEvents"/> when created and
		/// <see cref="ResumeEvents"/> when disposed, so it can be used
		/// with <c>using (obj.EventLock()) { ... }</c>.
		/// </summary>
		/// <returns></returns>
		public IDisposable EventLock()
		{
			return new EventLock(this);
		}

		#endregion

		#region ISyncRoot Members

		/// <summary>Gets the sync root.</summary>
		public object SyncRoot
		{
			get { return m_Collection.SafeSyncRoot(); }
		}

		#endregion
	}
}
