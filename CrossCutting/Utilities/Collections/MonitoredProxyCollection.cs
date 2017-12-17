using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	#region class MonitoredProxyCollection

	/// <summary>
	/// Helper class to create MonitoredProxyCollection.
	/// </summary>
	public static class MonitoredProxyCollection
	{
		/// <summary>
		/// Makes the proxy for specified collection.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="notification">The notification.</param>
		/// <returns>New proxy.</returns>
		public static MonitoredProxyCollection<T> Make<T>(
			ICollection<T> collection, EventHandler<MonitoredCollectionEventArgs<T>> notification)
		{
			return new MonitoredProxyCollection<T>(collection, notification);
		}

		/// <summary>Makes the proxy for specified factory.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="factory">The factory.</param>
		/// <param name="notification">The notification.</param>
		/// <returns>New proxy.</returns>
		public static MonitoredProxyCollection<T> Make<T>(
			Func<ICollection<T>> factory, EventHandler<MonitoredCollectionEventArgs<T>> notification)
		{
			return new MonitoredProxyCollection<T>(factory, notification);
		}
	}

	#endregion

	#region class MonitoredProxyCollection<T>

	/// <summary>
	/// Collection with monitored proxy.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class MonitoredProxyCollection<T>: LazyProxy<ICollection<T>, MonitoredCollection<T>>
	{
		#region fields

		/// <summary>
		/// Notification callback.
		/// </summary>
		private EventHandler<MonitoredCollectionEventArgs<T>> m_Notification;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the notification callback.
		/// </summary>
		/// <value>The notification callback.</value>
		public EventHandler<MonitoredCollectionEventArgs<T>> Notification
		{
			get { return m_Notification; }
			set { m_Notification = value; }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredProxyDictionary&lt;K, V&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="notification">The notification callback.</param>
		public MonitoredProxyCollection(ICollection<T> collection, EventHandler<MonitoredCollectionEventArgs<T>> notification)
			: base(collection, null)
		{
			Converter = CreateProxy;
			m_Notification = notification;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredProxyDictionary&lt;K, V&gt;"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="notification">The notification callback.</param>
		public MonitoredProxyCollection(Func<ICollection<T>> factory, EventHandler<MonitoredCollectionEventArgs<T>> notification)
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
		private MonitoredCollection<T> CreateProxy(ICollection<T> collection)
		{
			MonitoredCollection<T> result = new MonitoredCollection<T>(collection);
			result.Notification += PassNotification;
			return result;
		}

		#endregion

		#region notification handler

		void PassNotification(object sender, MonitoredCollectionEventArgs<T> args)
		{
			if (m_Notification != null) m_Notification(sender, args);
		}

		#endregion
	}

	#endregion
}
