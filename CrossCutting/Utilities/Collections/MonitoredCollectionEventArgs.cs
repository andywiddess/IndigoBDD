using System;

namespace Indigo.CrossCutting.Utilities.Collections
{
	#region class MonitoredCollectionEventArgs<T>

	/// <summary>
	/// Collection event data.
	/// </summary>
	/// <typeparam name="T">Type of collection item.</typeparam>
	public class MonitoredCollectionEventArgs<T>: EventArgs
	{
		private bool m_Cancel; // = false;
		private readonly bool m_CancelAllowed; // = false;

		#region properties

		/// <summary>
		/// Gets or sets the type of the event.
		/// </summary>
		/// <value>The type of the event.</value>
		public MonitoredCollectionEventType EventType { get; protected internal set; }

		/// <summary>
		/// Gets or sets the item which is subject of event.
		/// </summary>
		/// <value>The item.</value>
		public T Item { get; protected internal set; }

		/// <summary>Gets a value indicating whether this <see cref="MonitoredCollectionEventArgs&lt;T&gt;"/> is canceled.</summary>
		/// <value><c>true</c> if canceled; otherwise, <c>false</c>.</value>
		public bool Canceled
		{
			get { return m_Cancel; }
		}

		/// <summary>Gets a value indicating whether canceling operation is allowed.</summary>
		/// <value><c>true</c> if cancel is allowed; otherwise, <c>false</c>.</value>
		public bool CancelAllowed
		{
			get { return m_CancelAllowed; }
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="MonitoredCollectionEventArgs&lt;T&gt;"/> class.</summary>
		/// <param name="eventType">The event type.</param>
		/// <param name="item">The item.</param>
		/// <param name="allowCancel">if set to <c>true</c> canceling the operation will be allowed.</param>
		public MonitoredCollectionEventArgs(MonitoredCollectionEventType eventType, T item, bool allowCancel)
		{
			EventType = eventType;
			Item = item;
			m_CancelAllowed = allowCancel;
		}

		#endregion

		#region public interface

		/// <summary>Cancels the event.</summary>
		public void Cancel()
		{
			if (m_Cancel) return;
			if (!m_CancelAllowed) 
				new InvalidOperationException("Cancel operation is not allowed, some actions cannot be rolled back");
			m_Cancel = true;
		}

		#endregion
	}

	#endregion

	#region enum MonitoredCollectionEventType

	/// <summary>
	/// Event types for <see cref="MonitoredCollection&lt;T&gt;"/>
	/// </summary>
	public enum MonitoredCollectionEventType
	{
		/// <summary>
		/// No event.
		/// </summary>
		None,

		/// <summary>
		/// Occurs before item is added.
		/// </summary>
		Adding,

		/// <summary>
		/// Occurs after item is added.
		/// </summary>
		Added,

		/// <summary>
		/// Occurs before collection is cleared.
		/// </summary>
		Clearing,

		/// <summary>
		/// Occurs before clearing is done but after it has approved by all listeners (vetoing it won't help!).
		/// </summary>
		ClearApproved,

		/// <summary>
		/// Occurs after collection is cleared.
		/// </summary>
		Cleared,

		/// <summary>
		/// Occurs before item is removed.
		/// </summary>
		Removing,

		/// <summary>
		/// Occurs after item is removed.
		/// </summary>
		Removed,

		/// <summary>
		/// Occurs when events are suspended. May be triggered many times.
		/// </summary>
		Suspended,

		/// <summary>
		/// Occurs when events are resumed.
		/// </summary>
		Resumed,

		/// <summary>
		/// Occurs when last action has been canceled.
		/// </summary>
		Cancelled,
	}

	#endregion

	#region interface IMonitoredCollectionEventHadler<T>

	/// <summary>
	/// Event handler for monitored collections.
	/// </summary>
	/// <typeparam name="T">Type of collection item.</typeparam>
	public interface IMonitoredCollectionEventHadler<T>
	{
		/// <summary>
		/// Handles collection event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="Indigo.CrossCutting.Utilities.Collections.MonitoredCollectionEventArgs&lt;T&gt;"/> 
		/// instance containing the event data.</param>
		void OnCollectionEvent(object sender, MonitoredCollectionEventArgs<T> args);
	}

	#endregion
}
