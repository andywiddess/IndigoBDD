using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>Specialised interface to link ProgressTrackers. Do not use directly.</summary>
	public interface IProgressTrackerUplink
	{
		/// <summary>Passes position change up.</summary>
		/// <param name="position">The position.</param>
		void UplinkPosition(double position);

		/// <summary>Passes status change up.</summary>
		/// <param name="status">The status.</param>
		void UplinkStatus(object status);
	}
}
