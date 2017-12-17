using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Indigo.CrossCutting.Utilities.Collections;

namespace Indigo.CrossCutting.Utilities.Events
{
	#region class ListenerProxy

	/// <summary>
	/// Proxy for listeners. Reference to listener is stored as weak reference.
	/// </summary>
	internal class ListenerProxy
	{
		#region fields

		/// <summary>The 'real' listener.</summary>
		private readonly Reference<IBroadcastListener> m_Listener;

		/// <summary>Indicates if listener should be called synchronously.</summary>
		private readonly bool m_Sync;

		/// <summary>BroadcastArgs types to action map.</summary>
		private readonly Dictionary<Type, Action<IBroadcastListener, object, BroadcastArgs>> m_Actions;

		#endregion

		#region properties

		/// <summary>
		/// Gets the listener.
		/// </summary>
		/// <value>The listener.</value>
		public Reference<IBroadcastListener> Listener
		{
			get { return m_Listener; }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ListenerProxy"/> class.
		/// </summary>
		/// <param name="listener">The listener.</param>
		/// <param name="freeze">if set to <c>true</c> listener starts frozen.</param>
		/// <param name="sync">if set to <c>true</c> all calls to listener will be synchronized.</param>
		public ListenerProxy(IBroadcastListener listener, bool freeze, bool sync)
		{
			m_Actions = BuildActionMap(listener);
			m_Listener = new Reference<IBroadcastListener>(listener, freeze);
			m_Sync = sync;
		}

		#endregion

		#region BuildActionMap

		/// <summary>
		/// Builds the action map.
		/// </summary>
		/// <param name="listener">The listener.</param>
		/// <returns>BroadcastArgs types to action map.</returns>
		private Dictionary<Type, Action<IBroadcastListener, object, BroadcastArgs>> BuildActionMap(IBroadcastListener listener)
		{
			List<Type> types = GetHandledBroadcastArgsTypes(listener);
			Dictionary<Type, Action<IBroadcastListener, object, BroadcastArgs>> result =
				new Dictionary<Type, Action<IBroadcastListener, object, BroadcastArgs>>();

			foreach (Type type in types)
			{
				result[type] = BuildAction(type, m_Sync);
			}

			return result;
		}

		/// <summary>
		/// Executes the action.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="target">The target.</param>
		/// <param name="origin">The origin.</param>
		/// <param name="broadcast">The broadcast.</param>
		/// <param name="sync">if set to <c>true</c> [sync].</param>
		private static void ExecuteAction(MethodInfo method, IBroadcastListener target, object origin, BroadcastArgs broadcast, bool sync)
		{
			object[] arguments = { origin, broadcast };
			Action action = () => method.Invoke(target, arguments);

			if (sync)
			{
				ISynchronizeInvoke syncTarget = target as ISynchronizeInvoke;

				if (syncTarget == null || !syncTarget.InvokeRequired)
				{
					action();
				}
				else
				{
					syncTarget.Invoke(action, null);
				}
			}
			else
			{
				action();
			}
		}

		/// <summary>Builds the action delegate.</summary>
		/// <param name="type">The type.</param>
		/// <param name="sync">if set to <c>true</c> execution will be sync'ed.</param>
		/// <returns>Action.</returns>
		private static Action<IBroadcastListener, object, BroadcastArgs> BuildAction(Type type, bool sync)
		{
			// NOTE:MAK classic (?) example of partial methods

			Type iface = typeof(IBroadcastListener<>).MakeGenericType(new[] { type });
			MethodInfo method = iface.GetMethod(@"OnBroadcastReceived");

			Action<IBroadcastListener, object, BroadcastArgs> result =
				(target, origin, broadcast) => ExecuteAction(method, target, origin, broadcast, sync);

			return result;
		}

		#endregion

		#region notify

		/// <summary>
		/// Notifies the listener.
		/// </summary>
		/// <param name="origin">The origin.</param>
		/// <param name="broadcast">The <see cref="BroadcastArgs"/> instance containing the broadcast data.</param>
		public void Notify(object origin, BroadcastArgs broadcast)
		{
			IBroadcastListener target = Listener.Target;
			if (target == null)
			{
				// reference expired
				return;
			}

			Type broadcastType = broadcast.GetType();
			Action<IBroadcastListener, object, BroadcastArgs> action = null;

			// get handler for exact type...
			if (!m_Actions.TryGetValue(broadcastType, out action))
			{
				// if not found - find closest one

				try
				{
					KeyValuePair<Type, int> match = 
						m_Actions.Keys.WithMin(t => GetTypeDistance(t, broadcastType));

					if (match.Value < int.MaxValue) action = m_Actions[match.Key];
				}
				catch (ArgumentException)
				{
					// argument exception is thrown when collection is empty
					action = null;
				}

				m_Actions[broadcastType] = action; // we don't want to evaluate it next time
			}

			if (action != null)
			{
				action(target, origin, broadcast);
			}
		}

		#endregion

		#region utility

		/// <summary>
		/// Gets the distance between types.
		/// Note: it does not work with interfaces, only concrete classes. 
		/// To make it work with interfaces would require to implement BFS (hierarchy can branch), 
		/// without interfaces it is just linear scan on uni-directional list.
		/// </summary>
		/// <param name="superclass">The superclass.</param>
		/// <param name="subclass">The subclass.</param>
		/// <returns>
		/// Distance. 
		/// If <paramref name="subclass"/> is <paramref name="superclass"/> returns 0. 
		/// If <paramref name="subclass"/> does not derive from <paramref name="superclass"/> return <c>int.MaxValue</c>
		/// </returns>
		private static int GetTypeDistance(Type superclass, Type subclass)
		{
			if (superclass.IsInterface)
				throw new ArgumentException("superclass is an interface");
			if (subclass.IsInterface)
				throw new ArgumentException("subclass is an interface");

			if (subclass == superclass) return 0;

			// NOTE:MAK maybe it uses same loop as below, it such case it is not needed
			if (!subclass.IsSubclassOf(superclass)) return int.MaxValue;

			int result = 0;
			Type current = subclass;
			while (current != null)
			{
				if (current == superclass) return result;
				if (current == typeof(object)) break;
				current = current.BaseType;
				result++;
			}

			return int.MaxValue;
		}

		/// <summary>
		/// Gets the handled broadcast args types.
		/// </summary>
		/// <param name="listener">The listener.</param>
		/// <returns>List of handled broadcast types.</returns>
		private static List<Type> GetHandledBroadcastArgsTypes(IBroadcastListener listener)
		{
			List<Type> result = new List<Type>();

			foreach (Type i in listener.GetType().GetInterfaces())
			{
				// not a generic type
				if (!i.IsGenericType)
					continue;

				// not derived from IBroadcastListener<>
				if (i.GetGenericTypeDefinition() != typeof(IBroadcastListener<>))
					continue;

				Type[] arguments = i.GetGenericArguments();
				result.Add(arguments[0]); // first (and only) template argument
			}

			if (result.Count == 0)
			{
				throw new ArgumentException(
					@"Listener implements IBroadcastListener but does not implement any specific IBroadcastListener<T>");
			}

			return result;
		}


		#endregion
	}

	#endregion
}
