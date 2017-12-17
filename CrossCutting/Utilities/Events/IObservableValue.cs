namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Observable value. It does emit events when value changes.
	/// </summary>
	/// <typeparam name="T">Type of value stored.</typeparam>
	public interface IObservableValue<T>: IReadOnlyObservableValue<T>
	{
		/// <summary>Gets or sets the value.</summary>
		/// <value>The value.</value>
		new T Value { get; set; }
	}
}
