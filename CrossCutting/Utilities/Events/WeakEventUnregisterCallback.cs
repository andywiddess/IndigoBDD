using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>Self-unregister callback.</summary>
	/// <typeparam name="E">Type of EventArgs.</typeparam>
	/// <param name="eventHandler">The event handler.</param>
	public delegate void WeakEventUnregisterCallback<E>(EventHandler<E> eventHandler) where E: EventArgs;
}
