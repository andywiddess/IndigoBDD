using System;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Metrics.Monitor
{
	public class ExecutionMonitor :
		MonitorBase
	{
		private long _completed;
		private long _failed;
		private long _started;

		public ExecutionMonitor(Type ownerType, string name)
			: base(ownerType, name)
		{
		}

		public long Completed
		{
			get { return _completed; }
		}

		public long Started
		{
			get { return _started; }
		}

		public long Failed
		{
			get { return _failed; }
		}

		public long Executing
		{
			get { return _started - _failed - _completed; }
		}

		public void IncrementStarted()
		{
			Interlocked.Increment(ref _started);
		}

		public void IncrementCompleted()
		{
			Interlocked.Increment(ref _completed);
		}

		public void IncrementFailed()
		{
			Interlocked.Increment(ref _failed);
		}
	}
}