using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Indigo.CrossCutting.Utilities.Events;
using Indigo.CrossCutting.Utilities.Extensions;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Monitored set. Raises events when set is modified.</summary>
	/// <typeparam name="T"></typeparam>
	[Obsolete("This class wasn't tested")]
	public class MonitoredSet<T>: ISet<T>, ISuspendableEvents
	{
		#region fields

		/// <summary>Physical storage.</summary>
		private readonly ISet<T> m_Internal;

		/// <summary>Suspended count.</summary>
		private int m_Suspended;

		#endregion

		#region events

		/// <summary>
		/// Occurs when notification is needed.
		/// </summary>
		public event EventHandler<MonitoredCollectionEventArgs<T>> Notification;

		/// <summary>
		/// Raises the specified event type.
		/// </summary>
		/// <param name="eventType">The event type.</param>
		/// <param name="allowCancel">if set to <c>true</c> canceling is allowed.</param>
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
		/// <returns><c>true</c> if event has been canceled. Note, when suspended it will never be canceled.</returns>
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

		/// <summary>Initializes a new instance of the <see cref="MonitoredSet&lt;T&gt;"/> class.</summary>
		/// <param name="other">The other.</param>
		public MonitoredSet(ISet<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			m_Internal = other;
		}

		#endregion

		#region utilities

		/// <summary>Returns ready to throw <see cref="NotSupportedException"/>.</summary>
		/// <param name="operationName">Name of the operation.</param>
		/// <returns><see cref="NotSupportedException"/></returns>
		private static NotSupportedException NotSupported(string operationName)
		{
			return new NotSupportedException(string.Format("Operation '{0}' is not supported", operationName));
		}

		#endregion

		#region ISet<T> Members

		/// <summary>Adds an element to the current set and returns a value to indicate if the element was successfully 
		/// added.</summary>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item has been added.</returns>
		public bool Add(T item)
		{
			bool result = false;

			if (Raise(MonitoredCollectionEventType.Adding, item, true))
			{
				result = m_Internal.Add(item);
				if (result)
				{
					Raise(MonitoredCollectionEventType.Added, item, false);
				}
			}

			return result;
		}

		/// <summary>Removes all elements in the specified collection from the current set.</summary>
		/// <param name="other">The other collection.</param>
		public void ExceptWith(IEnumerable<T> other)
		{
			if (IsReadOnly)
			{
				throw NotSupported("ExceptWith");
			}

			if (m_Suspended <= 0)
			{
				// 'overlapping' item will be deleted
				var overlapping = new HashSet<T>(other.Where(item => m_Internal.Contains(item)));

				foreach (var item in overlapping)
				{
					// this will trigger all events needed
					Remove(item);
				}
			}
			else
			{
				m_Internal.ExceptWith(other);
			}
		}

		/// <summary>Modifies the current set so that it contains only elements that are also in a specified 
		/// collection.</summary>
		/// <param name="other">The other collection.</param>
		public void IntersectWith(IEnumerable<T> other)
		{
			if (IsReadOnly)
			{
				throw NotSupported("IntersectWith");
			}

			if (m_Suspended <= 0)
			{
				// 'overlapping' needs to be set for quick 'Contains'
				var overlapping = new HashSet<T>(other.Where(item => m_Internal.Contains(item)));

				// 'outstanding' can be list for quick iteration
				var outstanding = new List<T>(m_Internal.Where(item => !overlapping.Contains(item)));

				foreach (var item in outstanding)
				{
					// this will trigger all events needed
					Remove(item);
				}
			}
			else
			{
				// in suspended mode we can use 'shortcut'
				m_Internal.IntersectWith(other);
			}
		}

		/// <summary>Determines whether the current set is a property (strict) subset of a specified collection.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns><c>true</c> if the current set is a property (strict) subset of a specified collection; 
		/// otherwise, <c>false</c>.</returns>
		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			return m_Internal.IsProperSubsetOf(other);
		}

		/// <summary>Determines whether the current set is a correct superset of a specified collection.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns><c>true</c> if the current set is a correct superset of a specified collection; 
		/// otherwise, <c>false</c>.</returns>
		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			return m_Internal.IsProperSupersetOf(other);
		}

		/// <summary>Determines whether a set is a subset of a specified collection.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns><c>true</c> if set is a subset of a specified collection; otherwise, <c>false</c>.</returns>
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			return m_Internal.IsSubsetOf(other);
		}

		/// <summary>Determines whether the current set is a superset of a specified collection.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns><c>true</c> if the current set is a superset of a specified collection; otherwise, <c>false</c>.</returns>
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			return m_Internal.IsSupersetOf(other);
		}

		/// <summary>Determines whether the current set overlaps with the specified collection.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns>the current set overlaps with the specified collection</returns>
		public bool Overlaps(IEnumerable<T> other)
		{
			return m_Internal.Overlaps(other);
		}

		/// <summary>Determines whether the current set and the specified collection contain the same elements.</summary>
		/// <param name="other">The other collection.</param>
		/// <returns><c>true</c> if the current set and the specified collection contain the same elements; 
		/// <c>false</c> otherwise</returns>
		public bool SetEquals(IEnumerable<T> other)
		{
			return m_Internal.SetEquals(other);
		}

		/// <summary>Modifies the current set so that it contains only elements that are present either in the current set or 
		/// in the specified collection, but not both. </summary>
		/// <param name="other">The other collection.</param>
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			if (m_Suspended <= 0)
			{
				// partition items into two sets
				var partition = other.Partition((item) => m_Internal.Contains(item), () => new HashSet<T>());
				var overlapping = partition[true];
				var missing = partition[false];

				foreach (var item in overlapping)
				{
					Remove(item);
				}

				foreach (var item in missing)
				{
					Add(item);
				}
			}
			else
			{
				m_Internal.SymmetricExceptWith(other);
			}
		}

		/// <summary>Modifies the current set so that it contains all elements that are present in both the current set and 
		/// in the specified collection.</summary>
		/// <param name="other">The other collection.</param>
		public void UnionWith(IEnumerable<T> other)
		{
			if (m_Suspended <= 0)
			{
				var missing = new HashSet<T>(other.Where(item => !m_Internal.Contains(item)));

				foreach (var item in missing)
				{
					Add(item);
				}
			}
			else
			{
				m_Internal.UnionWith(other);
			}
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> 
		/// is read-only.</exception>
		void ICollection<T>.Add(T item)
		{
			Add(item);
		}

		/// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> 
		/// is read-only. </exception>
		public void Clear()
		{
			if (Raise(MonitoredCollectionEventType.Clearing, true))
			{
				Raise(MonitoredCollectionEventType.ClearApproved, false);
				m_Internal.Clear();
				Raise(MonitoredCollectionEventType.Cleared, false);

			}
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific 
		/// value.</summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; 
		/// otherwise, false.</returns>
		public bool Contains(T item)
		{
			return m_Internal.Contains(item);
		}

		/// <summary>Copies items to array.</summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			m_Internal.CopyTo(array, arrayIndex);
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		public int Count
		{
			get { return m_Internal.Count; }
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is 
		/// read-only.</summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; 
		/// otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return m_Internal.IsReadOnly; }
		}

		/// <summary>Removes the first occurrence of a specific object from the 
		/// <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>true if item was successfully removed from the 
		/// <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if 
		/// item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> 
		/// is read-only.</exception>
		public bool Remove(T item)
		{
			bool result = false;

			if (Raise(MonitoredCollectionEventType.Removing, item, true))
			{
				result = m_Internal.Remove(item);
				if (result)
				{
					Raise(MonitoredCollectionEventType.Removed, item, false);
				}
			}

			return result;
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the 
		/// collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return m_Internal.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the 
		/// collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Internal.GetEnumerator();
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
		/// <returns><see cref="EventLock"/></returns>
		public IDisposable EventLock()
		{
			return new EventLock(this);
		}

		#endregion
	}
}
