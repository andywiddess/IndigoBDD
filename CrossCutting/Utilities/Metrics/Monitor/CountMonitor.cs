using System;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Metrics.Monitor
{
	public class CountMonitor :
		MonitorBase
	{
		private long _count;

		public CountMonitor(Type ownerType, string name)
			: base(ownerType, name)
		{
		}

		public long Count
		{
			get { return _count; }
		}

		public void Increment()
		{
			Interlocked.Increment(ref _count);
		}
	}
}