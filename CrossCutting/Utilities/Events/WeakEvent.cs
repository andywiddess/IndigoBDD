using System;
using System.Linq;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Helper class to create weak event handlers. It is public (because it have to be) but have to be used following 
	/// strictly defined pattern. 
	/// Based on: http://diditwith.net/2007/03/23/SolvingTheProblemWithEventsWeakEventHandlers.aspx
	/// See the example.
	/// <example><![CDATA[
	/// public event EventHandler<EventArgs> OnEvent;
	/// public event EventHandler<EventArgs> OnEventWeak
	/// {
	///     add { OnEvent += WeakEvent.Make(value, (h) => OnEvent -= h); }
	///     remove { OnEvent -= WeakEvent.Unmake(value, OnEvent.GetInvocationList()); }
	/// }
	/// ]]></example>
	/// </summary>
	public static class WeakEvent
	{
		/// <summary>Makes weak event handler for specified concreate handler. Unregistering action have to be specified.</summary>
		/// <typeparam name="E">Type of EventArgs.</typeparam>
		/// <param name="value">The value.</param>
		/// <param name="unregisterCallback">The unregister callback.</param>
		/// <returns>Weak event handler.</returns>
		public static EventHandler<E> Make<E>(EventHandler<E> value, WeakEventUnregisterCallback<E> unregisterCallback)
			where E: EventArgs
		{
			if (value == null)
				throw new ArgumentNullException("value");
			if (value.Method.IsStatic || value.Target == null)
				throw new ArgumentException("Only instance methods are supported.", "value");

			var wehType = typeof(WeakEventHandler<,>).MakeGenericType(value.Method.DeclaringType, typeof(E));
			var wehConstructor = wehType.GetConstructor(new Type[] { typeof(EventHandler<E>), typeof(WeakEventUnregisterCallback<E>) });
			var weh = (IWeakEventHandler<E>)wehConstructor.Invoke(new object[] { value, unregisterCallback });

			return weh.Handler;
		}

		/// <summary>Finds handler-in-the-middle handler for specified concreate handler. 
		/// Note, simply unregistering concreate handler wouldn't work because concreate handler is not physically registered. 
		/// There is a object-in-the-middle which has to be found first.</summary>
		/// <typeparam name="E">Type of EventArgs.</typeparam>
		/// <param name="value">The concreate handler.</param>
		/// <param name="invocationList">The invocation list.</param>
		public static EventHandler<E> Unmake<E>(EventHandler<E> value, Delegate[] invocationList) where E: EventArgs
		{
			var weakHandler = invocationList
				.Select(d => d.Target as IWeakEventHandler<E>)
				.FirstOrDefault(t => t != null && t.Matches(value));
			return 
				weakHandler != null 
				? weakHandler.Handler 
				: null;
		}

		/// <summary>Finds handler-in-the-middle handler for specified concreate handler. 
		/// Note, simply unregistering concreate handler wouldn't work because concreate handler is not physically registered. 
		/// There is a object-in-the-middle which has to be found first.
		/// Note, it does the same thing as <see cref="Unmake{E}(EventHandler{E},Delegate[])"/> it is just more user fieldly, 
		/// but slower.</summary>
		/// <typeparam name="E">Type of EventArgs.</typeparam>
		/// <param name="value">The concreate handler.</param>
		/// <param name="eventMulticast">The event multicast.</param>
		public static EventHandler<E> Unmake<E>(EventHandler<E> value, EventHandler<E> eventMulticast) where E: EventArgs
		{
			return Unmake(value, eventMulticast.GetInvocationList());
		}
	}
}
