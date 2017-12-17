using System;
using System.Collections.Generic;
using Indigo.CrossCutting.Utilities.Events;

namespace Indigo.CrossCutting.Utilities.Collections
{
	#region class MonitoredProxyDictionary

	/// <summary>
	/// Helper class to create MonitoredProxyDictionary.
	/// </summary>
	public static class MonitoredProxyDictionary
	{
		/// <summary>Makes the proxy for specified dictionary.</summary>
		/// <typeparam name="K">Key type.</typeparam>
		/// <typeparam name="V">Value type.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="notification">The notification.</param>
		/// <returns>New proxy.</returns>
		public static MonitoredProxyDictionary<K, V> Make<K, V>(
			IDictionary<K, V> dictionary, MonitoredDictionaryEvent<K, V> notification)
		{
			return new MonitoredProxyDictionary<K, V>(dictionary, notification);
		}

		/// <summary>Makes the proxy for specified dictionary.</summary>
		/// <typeparam name="K">Key type.</typeparam>
		/// <typeparam name="V">Value type.</typeparam>
		/// <param name="factory">The factory.</param>
		/// <param name="notification">The notification.</param>
		/// <returns>New proxy.</returns>
		public static MonitoredProxyDictionary<K, V> Make<K, V>(
			Func<IDictionary<K, V>> factory, MonitoredDictionaryEvent<K, V> notification)
		{
			return new MonitoredProxyDictionary<K, V>(factory, notification);
		}
	}

	#endregion

	#region class MonitoredProxyDictionary<K, V>

	/// <summary>
	/// Dictionary with monitored proxy.
	/// </summary>
	/// <typeparam name="K">Key.</typeparam>
	/// <typeparam name="V">Value.</typeparam>
	public class MonitoredProxyDictionary<K, V>: LazyProxy<IDictionary<K, V>, MonitoredDictionary<K, V>>, ISuspendableEvents
	{
		#region fields

		/// <summary>
		/// Notification callback.
		/// </summary>
		private MonitoredDictionaryEvent<K, V> m_Notification;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the notification callback.
		/// </summary>
		/// <value>The notification callback.</value>
		public MonitoredDictionaryEvent<K, V> Notification
		{
			get { return m_Notification; }
			set { m_Notification = value; }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredProxyDictionary&lt;K, V&gt;"/> class.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="notification">The notification callback.</param>
		public MonitoredProxyDictionary(IDictionary<K, V> dictionary, MonitoredDictionaryEvent<K, V> notification)
			: base(dictionary, null)
		{
			Converter = CreateProxy;
			m_Notification = notification;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredProxyDictionary&lt;K, V&gt;"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="notification">The notification callback.</param>
		public MonitoredProxyDictionary(Func<IDictionary<K, V>> factory, MonitoredDictionaryEvent<K, V> notification)
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
		/// <param name="dictionary">The dictionary.</param>
		/// <returns>Generated proxy.</returns>
		private MonitoredDictionary<K, V> CreateProxy(IDictionary<K, V> dictionary)
		{
			MonitoredDictionary<K, V> result = new MonitoredDictionary<K, V>(dictionary);
			result.Notification += PassNotification;
			return result;
		}

		#endregion

		#region notification handler

		/// <summary>
		/// Passes the notification.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The instance containing the event data.</param>
		void PassNotification(object sender, MonitoredDictionaryEventArgs<K, V> args)
		{
			if (m_Notification != null) m_Notification(sender, args);
		}

		#endregion

		#region ISuspendableEvents Members

		/// <summary>
		/// Suspends events.
		/// </summary>
		void ISuspendableEvents.SuspendEvents()
		{
			this.Proxy.SuspendEvents();
		}

		/// <summary>
		/// Resumes events.
		/// </summary>
		void ISuspendableEvents.ResumeEvents()
		{
			this.Proxy.ResumeEvents();
		}

		#endregion
	}

	#endregion
}
