using System;
using Indigo.CrossCutting.Utilities.Extensions;

namespace Indigo.CrossCutting.Utilities.Metrics.Monitor
{
	public class SuccessRateMonitor :
		MonitorBase
	{
		private readonly long _itemThreshold;
		private readonly ExecutionMonitor _monitor;
		private readonly long _timeThreshold;
		private long _previousCompleted;
		private long _previousFailed;
		private long _previousTicks;

		public SuccessRateMonitor(Type monitorType, string name, ExecutionMonitor monitor)
			: this(monitorType, name, monitor, 100, 5.Seconds())
		{
		}

		public SuccessRateMonitor(Type monitorType, string name, ExecutionMonitor monitor, long itemThreshold, TimeSpan timeThreshold)
			: base(monitorType, name)
		{
			_monitor = monitor;

			InitializePreviousValues();

			_itemThreshold = itemThreshold;
			_timeThreshold = timeThreshold.Ticks;
		}

		public long SuccessRate
		{
			get { return GetSuccessRate(); }
		}

		private long GetSuccessRate()
		{
			long completed = _monitor.Completed;
			long failed = _monitor.Failed;

			long rate = GetCurrentSuccessRate(completed, failed);
			if (rate == -1)
				return rate;

			long newItems = (completed - _previousCompleted) + (failed - _previousFailed);
			long now = SystemUtil.Now.Ticks;
			long elapsedTime = now - _previousTicks;

			if (newItems >= _itemThreshold || elapsedTime >= _timeThreshold)
			{
				_previousCompleted = completed;
				_previousFailed = failed;
				_previousTicks = now;
			}

			return rate;
		}

		private long GetCurrentSuccessRate(long completed, long failed)
		{
			try
			{
				long justCompleted = completed - _previousCompleted;
				long justFailed = failed - _previousFailed;

				if (completed < 0 || failed < 0 || justCompleted < 0 || justFailed < 0)
					return -1;

				if (justCompleted + justFailed == 0)
					return 100;

				return CalculateSuccessRate(justCompleted, justFailed);
			}
			catch
			{
				return -1;
			}
		}

		private void InitializePreviousValues()
		{
			_previousCompleted = _monitor.Completed;
			_previousFailed = _monitor.Failed;

			_previousTicks = SystemUtil.Now.Ticks;
		}

		private static long CalculateSuccessRate(long completed, long failed)
		{
			return (long) Math.Round(100 - (100*(double) failed)/(completed + failed));
		}
	}
}