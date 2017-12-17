using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Logging;

namespace Indigo.CrossCutting.Utilities.Events
{
	#region class BroadcastCenter

	/// <summary>
	/// Class responsible for broadcasting events to all listeners.
	/// </summary>
	public class BroadcastCenter
	{
		#region static fields

		/// <summary>Default BroadcastCenter.</summary>
		private static BroadcastCenter m_Default;

		#endregion

		#region fields

		/// <summary>Synchronization root.</summary>
		private readonly object m_SyncRoot = new object();

		/// <summary>Time of last purge.</summary>
		private DateTime m_LastPurge = DateTime.Now;

		/// <summary>AutoPurge interval.</summary>
		private TimeSpan m_AutoPurge = new TimeSpan(0, 0, 1);

		/// <summary>Collection of listener proxy.</summary>
		private readonly HashSet<ListenerProxy> m_Listeners = new HashSet<ListenerProxy>();

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the auto purge interval.
		/// </summary>
		/// <value>The auto purge interval.</value>
		public TimeSpan AutoPurge
		{
			get { return m_AutoPurge; }
			set { m_AutoPurge = value; }
		}

		/// <summary>Access to the default broadcast center. This is your primary interface to the broadcast center.</summary>
		public static BroadcastCenter Default
		{
			get
			{
				if (m_Default == null) m_Default = new BroadcastCenter();
				return m_Default;
			}
		}

		#endregion

		#region manage

		/// <summary>Attaches the listener. By default attaches as weak reference, without synchronization.</summary>
		/// <param name="listener">The listener.</param>
		public void Attach(IBroadcastListener listener)
		{
			Attach(listener, false, false);
		}

		/// <summary>Attaches the listener. By default attaches without synchronization.</summary>
		/// <param name="listener">The listener.</param>
		/// <param name="freeze">if set to <c>true</c> listener starts frozen.</param>
		public void Attach(IBroadcastListener listener, bool freeze)
		{
			Attach(listener, freeze, false);
		}

		/// <summary>Attaches the listener.</summary>
		/// <param name="listener">The listener.</param>
		/// <param name="freeze">if set to <c>true</c> listener starts frozen.</param>
		/// <param name="sync">if set to <c>true</c> all calls to listener will be synchronized.</param>
		public void Attach(IBroadcastListener listener, bool freeze, bool sync)
		{
			lock (m_SyncRoot)
			{
				Purge(false);

				if ((sync) && !(listener is ISynchronizeInvoke))
				{
					throw new ArgumentException("Synchronization requested for object not iplementing ISynchronizeInvoke");
				}

				var proxy = FindProxyForListener(listener);

				if (proxy == null)
				{
					m_Listeners.Add(new ListenerProxy(listener, freeze, sync));
				}
				else
				{
					// duplicate? 
					// NOTE:MAK is it an error or not? currently it is not, it does not do any harm
					Patterns.NoOp();
				}
			}
		}

		/// <summary>Detaches the listener.</summary>
		/// <param name="listener">The listener.</param>
		/// <returns><c>true</c> if it was attached at all; <c>false</c> otherwise</returns>
		public bool Detach(IBroadcastListener listener)
		{
			lock (m_SyncRoot)
			{
				Purge(false);

				var proxy = FindProxyForListener(listener);

				if (proxy != null)
				{
					m_Listeners.Remove(proxy);
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>Determines whether the specified listener is attached.</summary>
		/// <param name="listener">The listener.</param>
		/// <returns><c>true</c> if the specified listener is attached; otherwise, <c>false</c>.</returns>
		public bool IsAttached(IBroadcastListener listener)
		{
			lock (m_SyncRoot)
			{
				return listener != null && FindProxyForListener(listener) != null;
			}
		}

		#endregion

		#region notify

		/// <summary>
		/// Notifies all related listeners.
		/// </summary>
		/// <param name="origin">The origin.</param>
		/// <param name="broadcast">The <see cref="BroadcastArgs"/> instance containing the broadcast data.</param>
		/// <param name="ignoreError">if set to <c>true</c> exceptions thrown by listeners are ignored; it is <c>false</c> 
		/// by default, but there is a philosophical question: "should notfier be responsible for what listeners do with the 
		/// message".</param>
		/// <returns>CompositeException containing all exceptions thrown by listeners.</returns>
		public AggregateException Notify(object origin, BroadcastArgs broadcast, bool ignoreError = false)
		{
			if (broadcast == null) return null;

			ListenerProxy[] proxies;

			lock (m_SyncRoot)
			{
				// Ensure the lock is as short a possible - purge the weak references and then duplicate the listeners
				Purge(false);

				// it creates a copy because part of handling broadcast can be detaching 
				proxies = m_Listeners.ToArray();
			}

			List<Exception> exceptions = null;

			// Note, the proxies list is no longer locked and this is a duplicated reference...
			// Side effect: if one of handlers is detaching listeners they will be called anyway one last time
			foreach (var proxy in proxies)
			{
				try
				{
					// NOTE:MAK should exception prevent other proxies to be called? currently it is not
					proxy.Notify(origin, broadcast);
				}
				catch (Exception e)
				{
					if (exceptions == null) exceptions = new List<Exception>();
					exceptions.Add(e);
				}
			}

			if (exceptions != null && exceptions.Count > 0)
			{
				Log.Warn("Exception has been thrown while handling '{0}' broadcast", broadcast.GetType().Name);
				foreach (var e in exceptions) Log.Error(e);
				var aggregateException = new AggregateException(exceptions);
				if (!ignoreError) throw aggregateException;
				return aggregateException;
			}

			return null;
		}

		#endregion

		#region purge

		/// <summary>Determines whether <see cref="Purge"/> is required.</summary>
		/// <param name="force">if set to <c>true</c> <see cref="Purge"/> will be forced.</param>
		/// <returns><c>true</c> if <see cref="Purge"/> is required; otherwise, <c>false</c>.</returns>
		private bool IsPurgeRequired(bool force)
		{
			return force || DateTime.Now.Subtract(m_LastPurge) >= m_AutoPurge;
		}

		/// <summary>
		/// Purges list of listeners. Because list can contain weak references, some of them will become invalid over time.
		/// There is <see cref="AutoPurge"/> interval which prevents from doing purge too often. If you need to force
		/// purging call with <paramref name="force"/> set to <c>true</c>.
		/// </summary>
		/// <param name="force">if set to <c>true</c> purge is forced.</param>
		public void Purge(bool force)
		{
			lock (m_SyncRoot)
			{
				// it's not ideal to check when already locked
				// but it's important to have object locked
				// when checking datetime, can be improved for sure
				// IMP:MAK avoid locking as long as possible
				if (IsPurgeRequired(force))
				{
					// ToArray() part is important! it makes a copy because we are removing from
					// the same collection we are iterating over
					m_Listeners.ExceptWith(m_Listeners.Where(p => (p.Listener.Target == null)).ToArray());
					m_LastPurge = DateTime.Now;
				}
			}
		}

		#endregion

		#region utilities

		/// <summary>
		/// Finds the proxy for listener.
		/// </summary>
		/// <param name="listener">The listener.</param>
		/// <returns>Proxy for listener.</returns>
		private ListenerProxy FindProxyForListener(IBroadcastListener listener)
		{
			if (listener == null) return null;
			return m_Listeners.FirstOrDefault(p => object.ReferenceEquals(p.Listener.Target, listener));
		}

		#endregion
	}

	#endregion
}
