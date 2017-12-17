using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Dummy interface for identification purposes only. You should not use it.
	/// Use <see cref="IBroadcastListener&lt;T&gt;"/> instead.
	/// </summary>
	public interface IBroadcastListener
	{
	}

	/// <summary>
	/// Event handler interface.
	/// </summary>
	/// <typeparam name="T">Any <see cref="EventArgs"/> subclass.</typeparam>
	public interface IBroadcastListener<T>: IBroadcastListener
		where T: BroadcastArgs
	{
		/// <summary>
		/// Called when broadcast received.
		/// </summary>
		/// <param name="origin">The origin.</param>
		/// <param name="broadcast">The received broadcast.</param>
		void OnBroadcastReceived(object origin, T broadcast);
	}
}
