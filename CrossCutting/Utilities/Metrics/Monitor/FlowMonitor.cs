using System;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Metrics.Monitor
{
	public class FlowMonitor :
		MonitorBase
	{
		private long _bytesRead;
		private long _bytesWritten;
		private long _readCount;
		private long _writeCount;

		public FlowMonitor(Type ownerType, string name)
			: base(ownerType, name)
		{
		}

		public long WriteCount
		{
			get { return _writeCount; }
		}

		public long BytesWritten
		{
			get { return _bytesWritten; }
		}

		public long ReadCount
		{
			get { return _readCount; }
		}

		public long BytesRead
		{
			get { return _bytesRead; }
		}

		public void IncrementWrite(long length)
		{
			Interlocked.Increment(ref _writeCount);
			Interlocked.Add(ref _bytesWritten, length);
		}

		public void IncrementRead(long length)
		{
			Interlocked.Increment(ref _readCount);
			Interlocked.Add(ref _bytesRead, length);
		}
	}
}