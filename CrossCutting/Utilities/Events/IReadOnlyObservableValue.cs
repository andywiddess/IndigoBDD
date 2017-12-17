using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>Observable value. Value is read only.</summary>
	/// <typeparam name="T">Value type.</typeparam>
	public interface IReadOnlyObservableValue<out T>: IObservable<T>
	{
		/// <summary>Gets the value.</summary>
		/// <value>The value.</value>
		T Value { get; }
	}
}
