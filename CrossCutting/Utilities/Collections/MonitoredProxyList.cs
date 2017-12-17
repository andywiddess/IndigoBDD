using System;
using System.Collections.Generic;
using Indigo.CrossCutting.Utilities.Events;

namespace Indigo.CrossCutting.Utilities.Collections
{
	#region class MonitoredProxyList<T>

	/// <summary>
	/// List with monitored proxy.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class MonitoredProxyList<T>: LazyProxy<IList<T>, MonitoredList<T>>, ISuspendableEvents
	{
		#region fields

		/// <summary>
		/// Notification callback.
		/// </summary>
		private MonitoredListEvent<T> m_Notification;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the notification callback.
		/// </summary>
		/// <value>The notification callback.</value>
		public MonitoredListEvent<T> Notification
		{
			get { return m_Notification; }
			set { m_Notification = value; }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredProxyList&lt;K&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="notification">The notification callback.</param>
		public MonitoredProxyList(IList<T> collection, MonitoredListEvent<T> notification)
			: base(collection, null)
		{
			Converter = CreateProxy;
			m_Notification = notification;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredProxyList&lt;K&gt;"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="notification">The notification callback.</param>
		public MonitoredProxyList(Func<IList<T>> factory, MonitoredListEvent<T> notification)
			: base(factory, null)
		{
			Converter = CreateProxy;
			m_Notification = notification;
		}

		#endregion

		#region create proxy

		/// <summary>
		/// Creates the proxy.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <returns>Generated proxy.</returns>
		private MonitoredList<T> CreateProxy(IList<T> collection)
		{
			MonitoredList<T> result = new MonitoredList<T>(collection);
			result.Notification += PassNotification;
			return result;
		}

		#endregion

		#region notification handler

		void PassNotification(object sender, MonitoredListEventArgs<T> args)
		{
			if (m_Notification != null) m_Notification(sender, args);
		}

		#endregion

		#region ISuspendableEvents Members

		/// <summary>Suspends events.</summary>
		public void SuspendEvents()
		{
			if (Proxy != null) Proxy.SuspendEvents();
		}

		/// <summary>Resumes events.</summary>
		public void ResumeEvents()
		{
			if (Proxy != null) Proxy.ResumeEvents();
		}

		#endregion
	}

	#endregion
}
