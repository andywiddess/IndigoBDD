using System;

namespace Indigo.CrossCutting.Utilities.Metrics
{
	/// <summary>
	/// Standard interface supported by all monitors
	/// </summary>
	public interface IMonitor
	{
		/// <summary>
		/// The type that owns this monitor
		/// </summary>
		Type OwnerType { get; }

		/// <summary>
		/// The name of the monitor
		/// </summary>
		string Name { get; }
	}
}