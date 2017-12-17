using System;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Threading
{
	public class AsyncResult :
		IAsyncResult
	{
		private readonly AsyncCallback _callback;
		private readonly object _state;
		private volatile bool _completed;
		private ManualResetEvent _completedEvent = new ManualResetEvent(false);

		public AsyncResult()
		{
			_state = null;
		}

		public AsyncResult(AsyncCallback callback, object state)
		{
			_callback = callback;
			_state = state;
		}

		public Exception Exception { get; private set; }

		public bool IsCompleted
		{
			get { return _completed; }
		}

		public WaitHandle AsyncWaitHandle
		{
			get { return _completedEvent; }
		}

		public object AsyncState
		{
			get { return _state; }
		}

		public bool CompletedSynchronously
		{
			get { return false; }
		}

		public void SetAsCompleted()
		{
			_completed = true;
			_completedEvent.Set();

			if (_callback != null)
				_callback(this);
		}

		public void SetAsCompleted(Exception exception)
		{
			Exception = exception;

			SetAsCompleted();
		}

		public void EndInvoke()
		{
			if (!IsCompleted)
				_completedEvent.WaitOne();

			if (_completedEvent != null)
			{
				_completedEvent.Close();
				_completedEvent = null;
			}

			if (Exception != null)
				throw Exception;
		}
	}
}