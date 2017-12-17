using System;
using System.ComponentModel;
using System.Threading;
using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Exceptions;
using Indigo.CrossCutting.Utilities.Threading;

namespace Indigo.CrossCutting.Utilities.Process
{
	/// <summary>
	/// Base class for forked threads.
	/// </summary>
	public abstract class AbstractForkedThread
        : IDisposable
	{
		#region fields

		/// <summary>Internal lock.</summary>
		private readonly object m_Lock = new object();

		/// <summary>Lock to prevent hard abort.</summary>
		private readonly object m_AbortLock = new object();

		/// <summary>Number of 'hard' suspends.</summary>
		private int m_HardSuspendCount;

		/// <summary>Callback synchronization target.</summary>
		private readonly ISynchronizeInvoke m_SynchronizationTarget;

		/// <summary>Callback called when execution is suspended.</summary>
		private readonly Action m_SuspendedCallback;

		/// <summary>Callback called when execution is resumed.</summary>
		private readonly Action m_ResumedCallback;

		/// <summary>Determines if callbacks should be suppressed.</summary>
		private bool m_SupressCallback; // = false;

		/// <summary>Physical thread running the action.</summary>
		private Thread m_Thread;

		/// <summary>Resume signal (input).</summary>
		private ManualResetEvent m_ResumeSignal = new ManualResetEvent(true);

		/// <summary>Terminate signal (input).</summary>
		private ManualResetEvent m_TerminateSignal = new ManualResetEvent(false);

		/// <summary>Finished signal (output).</summary>
		private ManualResetEvent m_FinishedSignal = new ManualResetEvent(false);

		#endregion

		#region properties

		/// <summary>Gets the control lock.</summary>
		protected internal object ControlLock { get { return m_Lock; } }

		/// <summary>Gets or sets the exception thrown by action.</summary>
		/// <value>The exception.</value>
		public Exception Exception { get; protected set; }

		/// <summary>Gets the wait handle. It will be set when action is finished.</summary>
		public WaitHandle WaitHandle { get { return m_FinishedSignal; } }

		/// <summary>Gets or sets the state flags. Note, FLAGS.</summary>
		/// <value>The state.</value>
		public ForkedThreadState StateFlags { get; protected internal set; }

		/// <summary>Gets a value indicating whether this instance is suspended.</summary>
		/// <value><c>true</c> if this instance is suspended; otherwise, <c>false</c>.</value>
		public bool IsSuspended { get { return (StateFlags & ForkedThreadState.Resumed) == 0; } }

		/// <summary>Gets a value indicating whether this instance is resumed (not suspended).</summary>
		/// <value><c>true</c> if this instance is resumed; otherwise, <c>false</c>.</value>
		public bool IsResumed { get { return (StateFlags & ForkedThreadState.Resumed) != 0; } }

		/// <summary>Gets a value indicating whether this instance is started (there is rare case that thread can be aborted 
		/// before execution started).</summary>
		/// <value><c>true</c> if this instance is started; otherwise, <c>false</c>.</value>
		public bool IsStarted { get { return (StateFlags & ForkedThreadState.Started) != 0; } }

		/// <summary>Gets a value indicating whether this instance is finished (succeeded, failed or aborted).</summary>
		/// <value><c>true</c> if this instance is finished; otherwise, <c>false</c>.</value>
		public bool IsFinished { get { return (StateFlags & ForkedThreadState.Finished) != 0; } }

		/// <summary>Gets a value indicating whether this instance is failed (exception has been thrown, any exception, 
		/// including aort exception).</summary>
		/// <value><c>true</c> if this instance is failed; otherwise, <c>false</c>.</value>
		public bool IsFailed { get { return (StateFlags & ForkedThreadState.Failed) != 0; } }

		/// <summary>Gets a value indicating whether this instance is terminated (aborted).</summary>
		/// <value><c>true</c> if this instance is terminated; otherwise, <c>false</c>.</value>
		public bool IsTerminated { get { return (StateFlags & ForkedThreadState.Terminated) != 0; } }

		/// <summary>Gets a value indicating whether this instance is abortable.</summary>
		/// <value><c>true</c> if this instance is abortable; otherwise, <c>false</c>.</value>
		public bool IsAbortable { get { return !IsFinished; } }

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="AbstractForkedThread"/> class.</summary>
		/// <param name="synchronizationTarget">The synchronization target.</param>
		public AbstractForkedThread(
			ISynchronizeInvoke synchronizationTarget = null)
		{
			m_SynchronizationTarget = synchronizationTarget;
		}

		/// <summary>Initializes a new instance of the <see cref="AbstractForkedThread"/> class.</summary>
		/// <param name="onSuspend">The OnSuspend handler.</param>
		/// <param name="onResume">The OnResume handler.</param>
		/// <param name="synchronizationTarget">The synchronization target.</param>
		public AbstractForkedThread(
			Action onSuspend, Action onResume, ISynchronizeInvoke synchronizationTarget = null)
		{
			m_SuspendedCallback = onSuspend;
			m_ResumedCallback = onResume;
			m_SynchronizationTarget = synchronizationTarget;
		}

		#endregion

		#region internal event handlers

		/// <summary>Notifies listeners.</summary>
		/// <param name="action">The action.</param>
		protected virtual void OnNotify(Action action)
		{
			if (action != null && !m_SupressCallback)
			{
				if (m_SynchronizationTarget == null)
				{
					// fork, never block this thread
					Patterns.Fork(action);
				}
				else if (!m_SynchronizationTarget.InvokeRequired)
				{
					// fork, never block this thread
					Patterns.Fork(() => m_SynchronizationTarget.Synchronize(action, true));
				}
				else
				{
					m_SynchronizationTarget.Synchronize(action, true);
				}
			}
		}

		/// <summary>Called to run the actual action.</summary>
		protected abstract void OnRun();

		/// <summary>Called when task is suspend. </summary>
		protected virtual void OnSuspend()
		{
			lock (ControlLock)
			{
				if (IsSuspended) return;
				StateFlags &= ~ForkedThreadState.Resumed; // reset resumed flag
				OnNotify(m_SuspendedCallback);
			}
		}

		/// <summary>Called when task is resumed. </summary>
		protected virtual void OnResume()
		{
			lock (ControlLock)
			{
				if (IsResumed) return;
				StateFlags |= ForkedThreadState.Resumed; // set resumed flag
				OnNotify(m_ResumedCallback);
			}
		}

		/// <summary>Called when task in terminated.</summary>
		/// <param name="e">The exception.</param>
		protected virtual void OnTerminated(Exception e)
		{
			lock (ControlLock)
			{
				Exception = e;
				StateFlags |= ForkedThreadState.Terminated | ForkedThreadState.Failed;
			}
		}

		/// <summary>Called when task failed.</summary>
		/// <param name="e">The exception thrown by task.</param>
		protected virtual void OnFailed(Exception e)
		{
			lock (ControlLock)
			{
				Exception = e;
				StateFlags |= ForkedThreadState.Failed;
			}
		}

		/// <summary>Called when task finishes. </summary>
		protected virtual void OnFinished()
		{
			lock (ControlLock)
			{
				StateFlags |= ForkedThreadState.Finished;
				OnNotify(OnFinishedCallback);
			}
		}

		/// <summary>Specific implementation for OnFinished handler. Implementation depends on derived class.</summary>
		protected abstract void OnFinishedCallback();

		#endregion

		#region thread body

		/// <summary>Main thread loop, apart from the fact it's not a loop.</summary>
		/// <param name="initializedSignal">The initialized signal.</param>
		private void ThreadBody(object initializedSignal)
		{
			try
			{
				StateFlags |= ForkedThreadState.Started | ForkedThreadState.Resumed;

				try
				{
					m_FinishedSignal.Reset();
					m_TerminateSignal.Reset();
					m_ResumeSignal.Set();
				}
				finally
				{
					if (initializedSignal != null)
					{
						// tell caller that all signals has been initialized
						((ManualResetEvent)initializedSignal).Set();
						// dereference to allow garbage collection right now (not when method exits)
						initializedSignal = null;
					}
				}

				try
				{
					OnRun();
				}
				catch (CanceledException e)
				{
					OnTerminated(e);
				}
				catch (ThreadAbortException e)
				{
					OnTerminated(e);
					Thread.ResetAbort();
				}
				catch (Exception e)
				{
					OnFailed(e);
				}
				finally
				{
					OnFinished();
				}
			}
			finally
			{
				m_Thread = null;
				m_FinishedSignal.Set(); // last instruction in this thread
			}
		}

		#endregion

		#region public interface

		/// <summary>Starts the execution.</summary>
		public void Start()
		{
			lock (ControlLock)
			{
				using (var initializedSignal = new ManualResetEvent(false))
				{
					m_Thread = new Thread(ThreadBody);
					m_Thread.Start(initializedSignal);
					initializedSignal.WaitOne(); // wait for all fields to be initialized
				}
			}
		}

		/// <summary>Suspends the execution.</summary>
		/// <param name="graceful">if set to <c>true</c> tries to suspend gracefully, by setting the signal. 
		/// Executed task needs to monitor this signal by using <see cref="Pulse"/> method.</param>
		public void Suspend(bool graceful = true)
		{
			lock (ControlLock)
			{
				if (IsSuspended) return;

				m_ResumeSignal.Reset();

				if (!graceful)
				{
					if (IsCurrentThread)
						throw new InvalidOperationException("Cannot forcefully suspend current thread");
					HardSuspend();
				}
			}
		}

		/// <summary>Resumes the execution.</summary>
		public void Resume()
		{
			lock (ControlLock)
			{
				if (IsResumed) return;

				m_ResumeSignal.Set();
				if (!IsCurrentThread && m_HardSuspendCount > 0)
				{
					HardResume();
				}
			}
		}

		/// <summary>Aborts the execution.</summary>
		/// <param name="graceful">if set to <c>true</c> tries to abort gracefully, by setting the signal. 
		/// Executed task needs to monitor this signal by using <see cref="Pulse"/> method.</param>
		/// <param name="supressCallback">if set to <c>true</c> callback will be suppressed.</param>
		public void Abort(bool graceful = true, bool supressCallback = false)
		{
			lock (ControlLock)
			{
				if (!IsAbortable) return;
				m_SupressCallback = supressCallback;

				if (IsCurrentThread)
				{
					throw new CanceledException();
				}
				else
				{
					m_TerminateSignal.Set();

					if (!graceful)
					{
						lock (m_AbortLock)
						{
							m_Thread.Abort();
						}
					}
				}
			}
		}

		/// <summary>Checks for terminate/resume signals. Call it from inside task body only.</summary>
		public void Pulse()
		{
			if (IsCurrentThread)
			{
				WaitHandle signaled = null;

				// this loop is a little bit tricky
				// the problem is: we need to trigger OnSuspend event when thread is 
				// suspended and we shouldn't trigger it if it is not needed
				// so we wait twice:
				// - first time with timeout 0 to test (suspendSent = false)
				// - if thread should be suspended we trigger event (and set suspendSent flag) 
				// - then we wait again with infinite timeout (suspendSent = true)

				bool suspendSent = false;

				while (signaled == null)
				{
					// if OnSuspend() has not been sent then don't wait, send suspend straight away if needed
					// if OnSuspend() has been already send we can wait for signal forever
					suspendSent = IsSuspended;
					signaled = WaitForAny(!suspendSent, m_TerminateSignal, m_ResumeSignal);
					if (signaled == null && !suspendSent) OnSuspend();
				}

				// trigger on resume only if it was suspended
				if (signaled == m_ResumeSignal && suspendSent) OnResume();

				// throw an exception if task should be terminated
				if (signaled == m_TerminateSignal) throw new CanceledException();
			}
		}

		/// <summary>Waits for task to finish. If execution ended with exception it will throw an exception.</summary>
		/// <param name="milliseconds">The timeout in milliseconds.</param>
		/// <returns><c>true</c> if task finished in time, <c>false</c> if it is still running.</returns>
		private bool TryWait(int milliseconds)
		{
			WaitCheck();
			var finished = m_FinishedSignal.WaitOne(milliseconds);
			if (finished && Exception != null) RethrowException();
			return finished;
		}

		/// <summary>Waits for task to finish. If execution ended with exception it will throw an exception.</summary>
		/// <param name="timespan">The timeout.</param>
		/// <returns><c>true</c> if task finished in time, <c>false</c> if it is still running.</returns>
		private bool TryWait(TimeSpan timespan)
		{
			WaitCheck();
			var finished = m_FinishedSignal.WaitOne(timespan);
			if (finished && Exception != null) RethrowException();
			return finished;
		}

		/// <summary>Waits for task to finish.</summary>
		public void Wait()
		{
			WaitCheck();
			m_FinishedSignal.WaitOne();
			if (Exception != null) RethrowException();
		}

		/// <summary>Waits for task to finish for a given time in milliseconds. 
		/// If task does not finish it will throw <see cref="TimeoutException"/>. 
		/// If task ended with exception it will rethrow this exception.</summary>
		/// <param name="milliseconds">The timeout.</param>
		public void Wait(int milliseconds)
		{
			WaitCheck();
			if (!TryWait(milliseconds)) throw new TimeoutException();
		}

		/// <summary>Waits for task to finish for a given time.
		/// If task does not finish it will throw <see cref="TimeoutException"/>. 
		/// If task ended with exception it will rethrow this exception.</summary>
		/// <param name="timespan">The timeout.</param>
		public void Wait(TimeSpan timespan)
		{
			WaitCheck();
			if (!TryWait(timespan)) throw new TimeoutException();
		}

		/// <summary>
		/// It is highly advised to use graceful suspend/resume/abort. Although some actions cannot be gracefully controlled.
		/// For example 3rd party code does not check for our signals. This method will monitor signals and control
		/// (suspend/resume/abort) given action. It won't be graceful anymore, but at least we can mix
		/// graceful-ready actions with not-graceful-ready actions.
		/// Note, current thread waits for this action to finish, so be careful to do not cause deadlock.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="pulseAround">if set to <c>true</c> pulses before and after task.</param>
		public void DisgracefulWrap(Action action, bool pulseAround = true)
		{
			if (!IsCurrentThread)
				throw new InvalidOperationException("This method can be only called from inside forked thread");

			if (pulseAround) Pulse();

			var startSignal = new ManualResetEvent(false);
			var stopSignal = new ManualResetEvent(false);

			try
			{
				// the action starts...
				var main = Patterns.ForkThread(() => DisgracefulActionProxy(ref startSignal, stopSignal, action));
				// ...and auxiliary monitoring thread, which will kill action thread if signals are set
				Patterns.Fork(() => DisgracefulGuardProxy(startSignal, ref stopSignal, main));
				// wait for action thread to finish (one way or another)
				main.Wait();
				return;
			}
			finally
			{
				if (pulseAround) Pulse();

				// it's not doing it here (NoOp) it is just for get rid of warning
				Patterns.NoOp(() => startSignal.Dispose());
				Patterns.NoOp(() => stopSignal.Dispose());
			}
		}

		/// <summary>
		/// It is highly advised to use graceful suspend/resume/abort. Although some actions cannot be gracefully controlled.
		/// For example 3rd party code does not check for our signals. This method will monitor signals and control
		/// (suspend/resume/abort) given action. It won't be graceful anymore, but at least we can mix
		/// graceful-ready actions with not-graceful-ready actions.
		/// Note, current thread waits for this action to finish, so be careful to do not cause deadlock.
		/// </summary>
		/// <typeparam name="T">Type of result.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="pulseAround">if set to <c>true</c> the task pulses before and after action.</param>
		/// <returns>
		/// Result of action.
		/// </returns>
		public T DisgracefulWrap<T>(Func<T> action, bool pulseAround = true)
		{
			if (!IsCurrentThread)
				throw new InvalidOperationException("This method can be only called from inside forked thread");

			if (pulseAround) Pulse();

			var startSignal = new ManualResetEvent(false);
			var stopSignal = new ManualResetEvent(false);

			try
			{
				var main = Patterns.ForkThread(() => DisgracefulActionProxy(ref startSignal, stopSignal, action));
				Patterns.Fork(() => DisgracefulGuardProxy(startSignal, ref stopSignal, main));
				return main.Wait();
			}
			finally
			{
				if (pulseAround) Pulse();

				// it's not doing it here (NoOp) it is just for get rid of warning
				Patterns.NoOp(() => startSignal.Dispose());
				Patterns.NoOp(() => stopSignal.Dispose());
			}
		}

		/// <summary>
		/// Makes sure action cannot be aborted. Use it what aborting action can cause harm.
		/// Because potential abort will wait until this action is finished use it only for simple action.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="pulseAround">if set to <c>true</c> task pulses before and after action.</param>
		public void NonabortableWrap(Action action, bool pulseAround = true)
		{
			if (pulseAround) Pulse();

			lock (m_AbortLock)
			{
				action();
				if (pulseAround) Pulse();
			}
		}

		/// <summary>
		/// Makes sure action cannot be aborted. Use it what aborting action can cause harm.
		/// Because potential abort will wait until this action is finished use it only for simple action.
		/// </summary>
		/// <typeparam name="T">Result type.</typeparam>
		/// <param name="factiory">The factory.</param>
		/// <param name="pulseAround">if set to <c>true</c> the task pulses before and after action.</param>
		/// <returns>
		/// Result of factory.
		/// </returns>
		public T NonabortableWrap<T>(Func<T> factiory, bool pulseAround = true)
		{
			if (pulseAround) Pulse();

			lock (m_AbortLock)
			{
				var result = factiory();
				if (pulseAround) Pulse();
				return result;
			}
		}

		#endregion

		#region private implementation

		/// <summary>Determines if current thread belongs to task. 
		/// Tells you if current code is executed inside task or outside it.</summary>
		/// <value><c>true</c> if this instance is current thread; otherwise, <c>false</c>.</value>
		private bool IsCurrentThread
		{
			get { return object.ReferenceEquals(m_Thread, Thread.CurrentThread); }
		}

		/// <summary>Waits for any signal.</summary>
		/// <param name="testOnly">if set to <c>true</c> test signals only, do not wait.</param>
		/// <param name="signals">The signals.</param>
		/// <returns>Triggered signal or <c>null</c>.</returns>
		private static ManualResetEvent WaitForAny(bool testOnly, params ManualResetEvent[] signals)
		{
			int index = WaitHandle.WaitAny(signals, testOnly ? 0 : Timeout.Infinite, !testOnly);
			if (index == WaitHandle.WaitTimeout) return null;
			return signals[index];
		}

		/// <summary>Check used in Wait method to ensure that thread does not wait for itself.</summary>
		private void WaitCheck()
		{
			if (IsCurrentThread)
				throw new InvalidOperationException(
					"Invalid usage of Wait(). Waiting for thread in the same thread would cause a deadlock");
		}

		/// <summary>Rethrows the exception. Makes sure that ThreadAbortException is not thrown in any other thread.</summary>
		private void RethrowException()
		{
			if (Exception == null) return;

			if (IsCurrentThread)
			{
				throw ForkedException.Make(Exception);
			}
			else
			{
				if (Exception is ThreadAbortException)
				{
					throw new CanceledException("Thread execution has been aborted", Exception);
				}
				else
				{
					throw ForkedException.Make(Exception);
				}
			}
		}


#pragma warning disable 0618

		/// <summary>Performs a hard suspend. Note, it has been deprecated.</summary>
		private void HardSuspend()
		{
			m_Thread.Suspend();
			m_HardSuspendCount++;
			OnSuspend();
		}

		/// <summary>Performs a hard resume. Note, it has been deprecated.</summary>
		private void HardResume()
		{
			m_Thread.Resume();
			m_HardSuspendCount--;
			OnResume();
		}

#pragma warning restore 0618

		/// <summary>Disgraceful guard for action. Monitors signals and reacts accordingly (ie: by killing guarded action).</summary>
		/// <param name="startSignal">The start signal.</param>
		/// <param name="stopSignal">The stop signal.</param>
		/// <param name="main">The action being guarded.</param>
		private void DisgracefulGuardProxy(
			ManualResetEvent startSignal, ref ManualResetEvent stopSignal,
			AbstractForkedThread main)
		{
			var abortSignal = m_TerminateSignal;
			var resumeSignal = m_ResumeSignal;
			var signals = new[] { abortSignal, stopSignal, resumeSignal };
			bool suspended = false;

			try
			{
				startSignal.Set();

				// this loop is spinning all the time while guarded action is running
				while (true)
				{
					// if thread is suspended wait, if thread is not suspended (resume signal is set) just check (don't wait)
					var signaled = WaitHandle.WaitAny(signals, suspended ? Timeout.Infinite : 0);

					if (signaled == 2 /* resumeSignal */)
					{
						// resumed or just working
						if (suspended)
						{
							main.Resume();
							suspended = false;
							OnResume();
						}
						else
						{
							// slow the spinning a little
							Thread.Sleep(1);
						}
					}
					else if (signaled == WaitHandle.WaitTimeout /* none, means suspend */)
					{
						// no signal set, so suspend is needed
						main.Suspend(false);
						suspended = true;
						OnSuspend();
					}
					else if (signaled == 0 /* abortSignal */)
					{
						// abort guarded action
						if (main.IsSuspended) main.Resume(); // have to be done, technicality
						main.Abort(false, true); // kill'im
						break;
					}
					else if (signaled == 1 /* stopSignal */)
					{
						// action has finished
						break;
					}
				}
			}
			finally
			{
				stopSignal.WaitOne();
				stopSignal.Dispose();
				stopSignal = null;
			}
		}

		/// <summary>Runs the action. Used with disgraceful wrapper, <see cref="DisgracefulWrap"/>.</summary>
		/// <param name="startSignal">The start signal.</param>
		/// <param name="stopSignal">The stop signal.</param>
		/// <param name="action">The action.</param>
		private static void DisgracefulActionProxy(
			ref ManualResetEvent startSignal, ManualResetEvent stopSignal,
			Action action)
		{
			try
			{
				startSignal.WaitOne();
				startSignal.Dispose();
				startSignal = null;

				action();
			}
			finally
			{
				stopSignal.Set();
			}
		}

		/// <summary>Runs the action. Used with disgraceful wrapper, <see cref="DisgracefulWrap"/>.</summary>
		/// <typeparam name="T">Result type.</typeparam>
		/// <param name="startSignal">The start signal.</param>
		/// <param name="stopSignal">The stop signal.</param>
		/// <param name="action">The action.</param>
		/// <returns></returns>
		private static T DisgracefulActionProxy<T>(
			ref ManualResetEvent startSignal, ManualResetEvent stopSignal,
			Func<T> action)
		{
			try
			{
				startSignal.WaitOne();
				startSignal.Dispose();
				startSignal = null;

				return action();
			}
			finally
			{
				stopSignal.Set();
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>Indicates if object has been already disposed.</summary>
		private bool m_Disposed;

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="AbstractForkedThread"/> is reclaimed by garbage collection.
		/// </summary>
		~AbstractForkedThread()
		{
			Dispose(false);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		private void Dispose(bool disposing)
		{
			if (!m_Disposed)
			{
				if (disposing)
					DisposeManaged();
				DisposeUnmanaged();
			}
			m_Disposed = true;
		}

		/// <summary>Disposes the managed resources.</summary>
		protected virtual void DisposeManaged()
		{
			Patterns.DisposeAndNull(ref m_ResumeSignal);
			Patterns.DisposeAndNull(ref m_FinishedSignal);
			Patterns.DisposeAndNull(ref m_TerminateSignal);
			Patterns.DisposeAndNull(ref m_Thread);
		}

		/// <summary>Disposes the unmanaged resources.</summary>
		protected virtual void DisposeUnmanaged()
		{
			Patterns.NoOp();
		}

		#endregion
	}
}
