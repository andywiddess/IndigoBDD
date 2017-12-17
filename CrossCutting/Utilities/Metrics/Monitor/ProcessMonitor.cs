using System;

using System.Diagnostics;

namespace Indigo.CrossCutting.Utilities.Metrics.Monitor
{
	public class ProcessMonitor :
		MonitorBase
	{
		public ProcessMonitor(Type ownerType, string name)
			: base(ownerType, name)
		{
		}

		public int ProcessorCount
		{
			get { return Environment.ProcessorCount; }
		}

		public long MemoryUsed
		{
			get { return System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 >> 20; }
		}

		public int ThreadCount
		{
			get { return System.Diagnostics.Process.GetCurrentProcess().Threads.Count; }
		}

		public TimeSpan ProcessorTimeUsed
		{
			get { return System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime; }
		}
	}
}