using System;

namespace Indigo.CrossCutting.Utilities.Process
{
	/// <summary>
	/// Describes state of task.
	/// </summary>
	[Flags]
	public enum ForkedThreadState
	{
#pragma warning disable 1591

		None = 0x0000,
		Started = 0x0001,
		Resumed = 0x0002, // NOTE, ~Resumed means Suspended
		Finished = 0x0004,
		Terminated = 0x008,
		Failed = 0x0010,

#pragma warning restore 1591
	}
}
