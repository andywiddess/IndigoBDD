using System.Threading;

namespace Indigo.CrossCutting.Utilities.Process
{
	/// <summary>
	/// The event similar to ManulaResetEventSlim but with even less
	/// overhead. Can be used only for very short waits as it spins
	/// actively on wait.
	/// </summary>
	public class QuickEvent
	{
		#region fields

		/// <summary>The state of event.</summary>
		private int _state;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="QuickEvent"/> class.</summary>
		public QuickEvent()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="QuickEvent"/> class.</summary>
		/// <param name="initialState">Initial state.</param>
		public QuickEvent(bool initialState)
		{
			_state = initialState ? 1 : 0;
		}

		#endregion

		#region public interface

		/// <summary>Sets event to the specified state.</summary>
		/// <param name="state">New event state.</param>
		public void Set(bool state = true)
		{
			Interlocked.Exchange(ref _state, state ? 1 : 0);
		}

		/// <summary>Resets event state.</summary>
		public void Reset()
		{
			Set(false);
		}

		/// <summary>Waits for event to reach non-zero state.</summary>
		public void Wait()
		{
			while (Interlocked.CompareExchange(ref _state, 0, 0) == 0) Thread.Yield();
		}

		#endregion
	}
}
