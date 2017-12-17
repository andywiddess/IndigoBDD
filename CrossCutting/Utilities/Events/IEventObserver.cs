using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>Event observer.</summary>
	/// <typeparam name="E">Event type.</typeparam>
	public interface IEventObserver<E> where E: EventArgs
	{
		/// <summary>
		/// Called when event is invoked.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The event args.</param>
		void OnInvoked(object sender, E eventArgs);
	}
}
