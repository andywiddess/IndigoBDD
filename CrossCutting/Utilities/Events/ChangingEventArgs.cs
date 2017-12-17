using System.ComponentModel;

namespace Indigo.CrossCutting.Utilities.Events
{
	#region class ChaningEventArgs<T>

	/// <summary>
	/// EventArgs for notification before values is changed. Allows to cancel.
	/// </summary>
	/// <typeparam name="T">Any type.</typeparam>
	public class ChangingEventArgs<T> : CancelEventArgs
	{
		#region properties

		/// <summary>
		/// Gets the old value (before change).
		/// </summary>
		/// <value>The old value.</value>
		public T OldValue { get; protected internal set; }

		/// <summary>
		/// Gets the new value (after change).
		/// </summary>
		/// <value>The new value.</value>
		public T NewValue { get; protected internal set; }

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="ChangingEventArgs&lt;T&gt;"/> class.</summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		public ChangingEventArgs(T oldValue, T newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		#endregion
	}

	#endregion
}
