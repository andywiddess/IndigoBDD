using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Interface exposing properties and methods need by other components related to WeakEvents.
	/// Nothing interesting for general audience.
	/// </summary>
	/// <typeparam name="E">Type of EventArgs.</typeparam>
	internal interface IWeakEventHandler<E> where E: EventArgs
	{
		/// <summary>Gets the handler.</summary>
		EventHandler<E> Handler { get; }

		/// <summary>Tests if this IWeakEventHandler actually calls given concrete handler.</summary>
		/// <param name="value">The concrete handler.</param>
		/// <returns><c>true</c> if this IWeakEventHandler actually calls given concrete handler, 
		/// <c>false</c> otherwise</returns>
		bool Matches(EventHandler<E> value);
		void Unregister();
	}
}
