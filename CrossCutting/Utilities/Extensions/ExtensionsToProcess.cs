namespace Indigo.CrossCutting.Utilities.Extensions
{
	/// <summary>
	/// Class to extend System.Diagnostics.Process class
	/// </summary>
	public static class ProcessExtender
	{
		/// <summary>
		/// Extension method to determine if the current process is already running,
		/// Useful to prevent multiple instances of an application from launching.
		/// </summary>
		/// <param name="process">The process</param>
		/// <returns>true, if the application can be launched, otherwise returns false</returns>
		public static bool CanOpenApplication(this System.Diagnostics.Process process)
		{
			return !(RunningInstancesCount(process) > 1);			
		}

		/// <summary>
		/// Extension method for getting the number of currently running instances of a process
		/// </summary>
		/// <param name="process">The process</param>
		/// <returns>number of currently running instances of a process</returns>
		public static int RunningInstancesCount(this System.Diagnostics.Process process)
		{
			return System.Diagnostics.Process.GetProcessesByName(process.ProcessName).Length;
		}
	}
}
