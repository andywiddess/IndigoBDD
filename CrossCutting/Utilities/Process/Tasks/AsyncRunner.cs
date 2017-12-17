using System;
using System.ComponentModel;
using System.Threading;
using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Exceptions;

namespace Indigo.CrossCutting.Utilities.Process.Tasks
{
	#region enum AsyncState

	/// <summary>
	/// Describes state of task.
	/// </summary>
	[Flags]
	public enum AsyncState
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

	#endregion

	#region class AsyncRunnerBase

	/// <summary>
	/// Base class for <see cref="AsyncRunner"/>.
	/// </summary>
	public class AsyncRunnerBase
	{
		#region fields

		private Thread m_Thread;
		private readonly SyncInvoker m_SyncIvoker;

		private readonly ManualResetEvent m_ResumeSignal = new ManualResetEvent(true);
		private readonly ManualResetEvent m_TerminateSignal = new ManualResetEvent(false);
		private readonly ManualResetEvent m_StoppedSignal = new ManualResetEvent(true);
		private readonly ManualResetEvent m_OutOfThread = new ManualResetEvent(true);
		private string m_StatusText = String.Empty;
		private double m_Progress = double.NaN;

		#endregion

		#region properties

		/// <summary>
		/// Gets the actual thread in which task is executed.
		/// It is exposed, but if you are going to use it you are asking for trouble.
		/// </summary>
		/// <value>The thread.</value>
		public Thread Thread
		{
			get { return m_Thread; }
		}

		/// <summary>
		/// Determines if current thread belongs to the task. 
		/// Tells you if current code is executed inside task or outside it.
		/// </summary>
		/// <value><c>true</c> if this instance is current thread; otherwise, <c>false</c>.</value>
		public bool IsCurrentThread
		{
			get { return object.ReferenceEquals(m_Thread, Thread.CurrentThread); }
		}

		/// <summary>
		/// Gets the synchronous invoker. In most cases it is synchronized to calling UI thread.
		/// </summary>
		/// <value>The sync invoker.</value>
		public SyncInvoker SyncInvoker
		{
			get { return m_SyncIvoker; }
		}

		/// <summary>
		/// Gets or sets the task state.
		/// </summary>
		/// <value>The state.</value>
		public AsyncState State { get; protected set; }

		/// <summary>
		/// Gets a value indicating whether this task is still running.
		/// </summary>
		/// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
		public bool IsActive
		{
			get { return TestStateAll(AsyncState.Started) && !TestStateAny(AsyncState.Finished); }
		}

		/// <summary>
		/// Gets or sets the argument passed to handler.
		/// </summary>
		/// <value>The argument.</value>
		public object Argument { get; set; }

		/// <summary>
		/// Gets or sets the context. Context is not used by runner itself, but can be used
		/// to pass information between event handlers. It is completely unders user's control.
		/// </summary>
		/// <value>The context.</value>
		public virtual object Context { get; set; }

		/// <summary>
		/// Gets or sets the status text. Triggers <see cref="OnStatusTextChanged"/> event.
		/// NOTE, event is triggered in task's thread.
		/// </summary>
		/// <value>The status text.</value>
		public string StatusText
		{
			set
			{
				if (value != m_StatusText)
				{
					m_StatusText = value;
					OnStatusTextChanged();
				}
			}
			get { return m_StatusText; }
		}

		/// <summary>
		/// Gets or sets the progress. Triggers <see cref="OnProgressChanged"/> event.
		/// NOTE, event is triggered in task's thread.
		/// </summary>
		/// <value>The progress.</value>
		public double Progress
		{
			set
			{
				if (value != m_Progress)
				{
					m_Progress = value;
					OnProgressChanged();
				}
			}
			get { return m_Progress; }
		}

		/// <summary>
		/// Sets the progress field. Equivalent to <c>Progress = current / maximum</c> 
		/// with some checks (<c>NaN</c> and <c>Infinity</c>).
		/// </summary>
		/// <param name="current">The current progress.</param>
		/// <param name="maximum">The maximum progress.</param>
		public void SetProgress(double current, double maximum)
		{
			double progress = current / maximum;

			if (progress < 0.0) progress = 0.0;
			else if (progress > 1.0) progress = 1.0;
			else if (double.IsNaN(progress)) progress = double.NaN;

			Progress = progress;
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncTask"/> class. 
		/// Note: this creates a task without synchronization target. 
		/// You probably don't want to do this. Check <see cref="AsyncRunner(ISynchronizeInvoke)"/>.
		/// </summary>
		public AsyncRunnerBase()
			: this((SyncInvoker)null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncTask"/> class.
		/// </summary>
		/// <param name="sync">The sync target.</param>
		public AsyncRunnerBase(SyncInvoker sync)
		{
			m_SyncIvoker = sync;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncTask"/> class.
		/// </summary>
		/// <param name="sync">The sync target.</param>
		public AsyncRunnerBase(ISynchronizeInvoke sync)
		{
			m_SyncIvoker = (sync == null ? null : new SyncInvoker(sync));
		}

		#endregion

		#region sync/async calls

		/// <summary>
		/// Calls method with sync to sync target (usually UI).
		/// Note: Waits for return, so it may be a reason for deadlock.
		/// </summary>
		/// <param name="method">The method.</param>
		public void Sync(Action method)
		{
			if (m_SyncIvoker != null)
			{
				m_SyncIvoker.Invoke(method);
			}
			else
			{
				method();
			}
		}

		/// <summary>
		/// Calls method with sync to sync target (usually UI).
		/// Note: Waits for return, so it may be a reason for deadlock.
		/// </summary>
		/// <typeparam name="T">Result type.</typeparam>
		/// <param name="factory">The factory method.</param>
		/// <returns>Value returned by factory.</returns>
		public T Sync<T>(Func<T> factory)
		{
			if (m_SyncIvoker != null)
			{
				return m_SyncIvoker.Invoke<T>(factory);
			}
			else
			{
				return factory();
			}
		}

		/// <summary>
		/// Calls method with sync to sync target (usually UI) but it does not wait for return.
		/// Note: As it does not wait for method to return it is safer it terms of deadlocks.
		/// </summary>
		/// <param name="method">The method.</param>
		public void SyncNoWait(Action method)
		{
			if (m_SyncIvoker != null)
			{
				m_SyncIvoker.InvokeAsync(method);
			}
			else
			{
				method();
			}
		}

		/// <summary>
		/// Calls method asynchronously. It is called in completely spearated thread, 
		/// it's not sync'ed with UI thread nor task thread.
		/// </summary>
		/// <param name="method">The method.</param>
		public static void Async(Action method)
		{
			ThreadStart threadMethod = new ThreadStart(method);
			new Thread(threadMethod).Start(); // new Thread(method).Start(); ???
		}

		#endregion

		#region virtual handlers

		/// <summary>
		/// Called before running task. 
		/// </summary>
		protected virtual void OnBeforeRun()
		{
		}

		/// <summary>
		/// Called after task is finished. 
		/// </summary>
		protected virtual void OnAfterRun()
		{
		}

		/// <summary>
		/// Execution of the task. 
		/// </summary>
		protected virtual void OnRun()
		{
		}

		/// <summary>
		/// Called when task is suspend. 
		/// </summary>
		protected virtual void OnSuspend()
		{
		}

		/// <summary>
		/// Called when task is resumed. 
		/// </summary>
		protected virtual void OnResume()
		{
		}

		/// <summary>
		/// Called when task succeeded. 
		/// </summary>
		protected virtual void OnSuccess()
		{
		}

		/// <summary>
		/// Called when task failed. 
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns><c>true</c> if failure has been handled (ie: message displayed)</returns>
		protected virtual bool OnFailure(Exception exception)
		{
			return false;
		}

		/// <summary>
		/// Called when task has been terminated. 
		/// </summary>
		/// <param name="dirty">if set to <c>true</c> task has been force to terminate by
		/// using <see cref="System.Threading.Thread.Abort()"/>, <c>false</c> is terminated gracefully.</param>
		/// <returns><c>true</c> if termination has been handled (ie: message displayed)</returns>
		protected virtual bool OnTerminate(bool dirty)
		{
			return false;
		}

		/// <summary>
		/// Called when exception is thrown. 
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns><c>true</c> if exception has been handled (ie: message displayed), and should not be rethrown.</returns>
		protected virtual bool OnException(Exception exception)
		{
			return false;
		}

		/// <summary>
		/// Called when exception is thrown and has not been handled. It's default implementation should 
		/// call general exception handler (not task specific).
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns><c>true</c> if exception has been handled (ie: message displayed), and should not be rethrown.</returns>
		protected virtual bool OnLastChanceException(Exception exception)
		{
			return false;
		}

		/// <summary>
		/// Called when progress changed.
		/// </summary>
		protected virtual void OnProgressChanged()
		{
		}

		/// <summary>
		/// Called when status text changed.
		/// </summary>
		protected virtual void OnStatusTextChanged()
		{
		}

		#endregion

		#region utilities

		/// <summary>
		/// Tests if all flags are set.
		/// </summary>
		/// <param name="testAgaist">The value.</param>
		/// <returns><c>true</c> if all flags are set, <c>false</c> otherwise</returns>
		private bool TestStateAll(AsyncState testAgaist)
		{
			return (State & testAgaist) == testAgaist;
		}

		/// <summary>
		/// Tests the any of the flags is set.
		/// </summary>
		/// <param name="testAgaist">The value.</param>
		/// <returns><c>true</c> if any of given flags is set; <c>false</c> otherwise</returns>
		private bool TestStateAny(AsyncState testAgaist)
		{
			return (State & testAgaist) != AsyncState.None;
		}

		private static ManualResetEvent WaitForAny(bool testOnly, params ManualResetEvent[] signals)
		{
			int index = WaitHandle.WaitAny(signals, testOnly ? 0 : Timeout.Infinite, !testOnly);
			if (index == WaitHandle.WaitTimeout) return null;
			return signals[index];
		}

		/// <summary>
		/// Checks for terminate/resume signals.
		/// Call it from inside task body only.
		/// </summary>
		public void Checkpoint()
		{
			if (IsCurrentThread)
			{
				bool suspendSent = false;
				WaitHandle signaled = null;

				// this loop is a little bit tricky
				// the problem is: we need to trigger OnSuspend event when thread is 
				// suspended and we shouldn't trigger it if it is not needed
				// so we wait twice:
				// - first time with timeout 0 to test (suspendSent = false)
				// - if thread should be suspended we trigger event (and set suspendSent flag) 
				// - then we wait again with infinite timeout (suspendSent = true)

				while (signaled == null)
				{
					// if OnSuspend() has not been sent then don't wait, send suspend straight away if needed
					// if OnSuspend() has been already send we can wait for signal forever
					signaled = WaitForAny(!suspendSent, m_TerminateSignal, m_ResumeSignal);
					if (signaled == null && !suspendSent)
					{
						State &= ~AsyncState.Resumed; // reset resumed flag
						OnSuspend();
						suspendSent = true;
					}
				}

				// trigger on resume only if it was suspended
				if (signaled == m_ResumeSignal && suspendSent)
				{
					State |= AsyncState.Resumed;
					OnResume();
				}

				// throw an exception if task should be terminated
				if (signaled == m_TerminateSignal)
				{
					throw new TerminateException();
				}
			}
		}

		#endregion

		#region run

		/// <summary>
		/// Runs the task.
		/// </summary>
		public void Run()
		{
			Run(true);
		}

		/// <summary>
		/// Runs the task.
		/// </summary>
		/// <param name="async">if set to <c>true</c> task is ran asynchronously (default). NOTE, it may cause the
		/// deadlock if you run task in current thread (<c>false</c>); I deciced to allow this because unresponsible 
		/// coder may cause deadlock anyway. There is no code in this class which may cause dead-lock but this class is triggering
		/// events and your event handlers may/should use <see cref="Sync"/> if it is ran asynchronously and SHOULD NOT use <see cref="Sync"/>
		/// if ran synchronously. It's your call.
		/// </param>
		public void Run(bool async)
		{
			if (async)
			{
				using (ManualResetEvent initializedSignal = new ManualResetEvent(false))
				{
					m_Thread = new Thread(ThreadLoop);
					m_Thread.Start(initializedSignal);
					initializedSignal.WaitOne(); // wait for all fields to be initialized
				} 
			}
			else
			{
				// NOTE, it may lead to deadlock if you run task in current thread; 
				// I decided to allow this, because it can be useful;
				// Not responsible coder may cause deadlock anyway
				ThreadLoop(null);
			}
		}

		/// <summary>
		/// Main thread loop, apart from the fact it's not a loop.
		/// </summary>
		/// <param name="initializedSignal">The initialized signal.</param>
		private void ThreadLoop(object initializedSignal)
		{
			// NOTE, the flag indicating if task has been run synchronously or asynchronously is actually
			// derived from having or not initializedSignal
			// if initializedSignal is null, it means that it was ran synchronously
			bool async = (initializedSignal != null);

			try
			{
				m_OutOfThread.Reset();
				State |= AsyncState.Started | AsyncState.Resumed;

				try
				{
					m_StoppedSignal.Reset();
					m_TerminateSignal.Reset();
					m_ResumeSignal.Set();

					if (m_Thread == null) m_Thread = Thread.CurrentThread;
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

				OnBeforeRun();

				try
				{
					try
					{
						Checkpoint();
						OnRun();
					}
					finally
					{
						m_StoppedSignal.Set();
					}
					OnSuccess();
				}
				catch (TerminateException e)
				{
					State |= AsyncState.Terminated;
					if (!OnTerminate(false) && !OnException(e))
						throw;
				}
				catch (ThreadAbortException e)
				{
					State |= AsyncState.Terminated;
					if (!OnTerminate(true) && !OnException(e))
					{
						// throw; // ThreadAbortException is autamatically reraised
					}
					else
					{
						Thread.ResetAbort();
					}
				}
				catch (Exception e)
				{
					State |= AsyncState.Failed;
					if (!OnFailure(e) && !OnException(e))
						throw;
				}
				finally
				{
					State |= AsyncState.Finished;
					OnAfterRun();
				}
			}
			catch (Exception e)
			{
				if (!async)
				{
					// exception has not been handled, but it was ran synchronously, so someone is waiting for result
					throw;
				}
				else
				{
					// this last-chance exception handler
					// exceptions in threads are very ugly, so try handle them at all cost
					bool handled = false;

					try
					{
						handled = OnException(e);
					}
					catch
					{
						// do nothing, we are interested in original exception not the one from exception handler
						Patterns.NoOp();
					}

					if (/* still */ !handled)
					{
						try
						{
							handled = OnLastChanceException(e);
						}
						catch
						{
							// do nothing, we are interested in original exception not the one from exception handler
							Patterns.NoOp();
						}
					}

					if (/* still */ !handled)
					{
						// noone was interested in exception
						// it is better to throw it rather than swallowing
						// there are many options to avoid it though (handling OnException for example)
						throw;
					}
				}
			}
			finally
			{
				m_Thread = null;
				m_OutOfThread.Set();
			}
		}

		#endregion

		#region control

		/// <summary>
		/// Sends 'suspend' signal to task. 
		/// Note: Task doesn't have to call <see cref="Checkpoint"/>, it it may never be suspended.
		/// </summary>
		public void Suspend()
		{
			m_ResumeSignal.Reset();
		}

		/// <summary>
		/// Sends 'resume' signal to task.
		/// </summary>
		public void Resume()
		{
			m_ResumeSignal.Set();
		}

		/// <summary>
		/// Sends 'terminate' signal to task.
		/// Note: Task doesn't have to call <see cref="Checkpoint"/>, it it may never be terminated.
		/// </summary>
		public void Terminate()
		{
			if (IsCurrentThread)
			{
				throw new TerminateException();
			}
			else
			{
				m_TerminateSignal.Set();
			}
		}

		/// <summary>
		/// If timeout is greater than 0, it tries to terminate task gracefully.
		/// If task has not been terminated, it forcefully aborts it 
		/// (it's reliable - well, unless task is using <c>ResetAbort()</c> - but dirty).
		/// </summary>
		/// <param name="timeout">The timeout (milliseconds).</param>
		public void Abort(int timeout)
		{
			if (IsCurrentThread)
			{
				throw new TerminateException();
			}
			else
			{
				if (timeout > 0)
				{
					Terminate();
				}

				if (!WaitAbort(timeout))
				{
					m_Thread.Abort();
				}
			}
		}

		#endregion

		#region wait

		/// <summary>
		/// Waits for termination.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <returns><c>true</c> if task has been terminated.</returns>
		public bool WaitTerminate(int? timeout)
		{
			int t = timeout.GetValueOrDefault(Timeout.Infinite);
			return m_StoppedSignal.WaitOne(t, t == Timeout.Infinite ? true : false);
		}

		/// <summary>
		/// Waits for abort.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <returns><c>true</c> if task has been aborted.</returns>
		public bool WaitAbort(int? timeout)
		{
			int t = timeout.GetValueOrDefault(Timeout.Infinite);
			return m_OutOfThread.WaitOne(t, t == Timeout.Infinite ? true : false);
		}

		#endregion
	}

	#endregion

	#region class AsyncRunner

	/// <summary>
	/// Generic action runner (kinda <see cref="BackgroundWorker"/>).
	/// </summary>
	public class AsyncRunner: AsyncRunnerBase
	{
		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncTask"/> class. 
		/// Note: this creates a task without synchronization target. 
		/// You probably don't want to do this. Check <see cref="AsyncRunner(ISynchronizeInvoke)"/>.
		/// </summary>
		public AsyncRunner()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncTask"/> class.
		/// </summary>
		/// <param name="sync">The sync target.</param>
		public AsyncRunner(SyncInvoker sync)
			: base(sync)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncTask"/> class.
		/// </summary>
		/// <param name="sync">The sync target.</param>
		public AsyncRunner(ISynchronizeInvoke sync)
			: base(sync)
		{
		}

		#endregion

		#region virtual handlers

		/// <summary>
		/// Called before running task. <see cref="TaskStarted"/> for details.
		/// </summary>
		protected override void OnBeforeRun()
		{
			if (TaskStarted != null)
			{
				TaskStarted(this);
			}
		}

		/// <summary>
		/// Called after task is finished. <see cref="TaskFinished"/> for details.
		/// </summary>
		protected override void OnAfterRun()
		{
			if (TaskFinished != null)
			{
				TaskFinished(this);
			}
		}

		/// <summary>
		/// Execution of the task. <see cref="TaskRunning"/> for details.
		/// </summary>
		protected override void OnRun()
		{
			if (TaskRunning != null)
			{
				TaskRunning(this);
			}
		}

		/// <summary>
		/// Called when task is suspend. <see cref="TaskSuspended"/> for details.
		/// </summary>
		protected override void OnSuspend()
		{
			if (TaskSuspended != null)
			{
				TaskSuspended(this);
			}
		}

		/// <summary>
		/// Called when task is resumed. <see cref="TaskResumed"/> for details.
		/// </summary>
		protected override void OnResume()
		{
			if (TaskResumed != null)
			{
				TaskResumed(this);
			}
		}

		/// <summary>
		/// Called when task succeeded. <see cref="TaskSucceeded"/> for details.
		/// </summary>
		protected override void OnSuccess()
		{
			if (TaskSucceeded != null)
			{
				TaskSucceeded(this);
			}
		}

		/// <summary>
		/// Called when task failed. <see cref="TaskFailed"/> for details.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns><c>true</c> if failure has been handled (ie: message displayed)</returns>
		protected override bool OnFailure(Exception exception)
		{
			if (TaskFailed != null)
			{
				return TaskFailed(this, exception);
			}
			return false;
		}

		/// <summary>
		/// Called when task has been terminated. <see cref="TaskTerminated"/> for details.
		/// </summary>
		/// <param name="dirty">if set to <c>true</c> task has been force to terminate by
		/// using <see cref="System.Threading.Thread.Abort()"/>, <c>false</c> is terminated gracefully.</param>
		/// <returns><c>true</c> if termination has been handled (ie: message displayed)</returns>
		protected override bool OnTerminate(bool dirty)
		{
			if (TaskTerminated != null)
			{
				return TaskTerminated(this, dirty);
			}
			return false;
		}

		/// <summary>
		/// Called when exception is thrown. <see cref="TaskException"/> for details.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns><c>true</c> if exception has been handled (ie: message displayed)</returns>
		protected override bool OnException(Exception exception)
		{
			if (TaskException != null)
			{
				return TaskException(this, exception);
			}
			return false;
		}

		/// <summary>
		/// Called when exception is thrown and has not been handled. It's default implementation should
		/// call general exception handler (not task specific).
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns><c>true</c> if exception has been handled (ie: message displayed), and should not be rethrown.</returns>
		protected override bool OnLastChanceException(Exception exception)
		{
			if (TaskLastChanceException != null)
			{
				return TaskLastChanceException(this, exception);
			}
			return false;
		}

		/// <summary>
		/// Called when progress changed. <see cref="TaskProgressChanged"/> for details.
		/// </summary>
		protected override void OnProgressChanged()
		{
			if (TaskProgressChanged != null)
			{
				TaskProgressChanged(this, Progress);
			}
		}

		/// <summary>
		/// Called when status text changed. <see cref="TaskStatusTextChanged"/> for details.
		/// </summary>
		protected override void OnStatusTextChanged()
		{
			if (TaskStatusTextChanged != null)
			{
				TaskStatusTextChanged(this, StatusText);
			}
		}

		#endregion

		#region events

		/// <summary>
		/// Occurs when task progress has changed.
		/// Note: All the events are called in Task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// </summary>
		public event AsyncNotifyEvent<double> TaskProgressChanged;

		/// <summary>
		/// Occurs when task status text has changed.
		/// Note: All the events are called in Task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// </summary>
		public event AsyncNotifyEvent<string> TaskStatusTextChanged;

		/// <summary>
		/// Occurs when task is started, before actual execution but already in thread.
		/// Note: All the events are called in Task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// </summary>
		public event AsyncNotifyEvent TaskStarted;

		/// <summary>
		/// Occurs when task is finished, whether is has succeeded (<see cref="AsyncRunner.TaskSucceeded"/>),
		/// failed (<see cref="AsyncRunner.TaskFailed"/>) or has been terminated (<see cref="AsyncRunner.TaskTerminated"/>).
		/// Note: it is called after a call to (<see cref="AsyncRunner.TaskSucceeded"/>),
		/// (<see cref="AsyncRunner.TaskFailed"/>) or (<see cref="AsyncRunner.TaskTerminated"/>).
		/// Note: All the events are called in Task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// </summary>
		public event AsyncNotifyEvent TaskFinished;

		/// <summary>
		/// Occurs when task running. Actually, it is the essence of AsyncRunner. Put you 
		/// implementation here.
		/// Note: All the events are called in Task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// </summary>
		public event AsyncNotifyEvent TaskRunning;

		/// <summary>
		/// Occurs when task is suspended.
		/// Note: All the events are called in Task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// </summary>
		public event AsyncNotifyEvent TaskSuspended;

		/// <summary>
		/// Occurs when task is resumed.
		/// Note: All the events are called in Task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// </summary>
		public event AsyncNotifyEvent TaskResumed;

		/// <summary>
		/// Occurs when task succeeded. It's called only if task has neither failed (<see cref="AsyncRunner.TaskFailed"/>) 
		/// or has been terminated (<see cref="AsyncRunner.TaskTerminated"/>).
		/// Note: All the events are called in Task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// </summary>
		public event AsyncNotifyEvent TaskSucceeded;

		/// <summary>
		/// Occurs when task failed. It's not called when task has been terminated (<see cref="AsyncRunner.TaskTerminated"/>).
		/// The returned value indicates if 'failure' has been handled. It it has <see cref="AsyncRunner.TaskException"/> 
		/// won't be called.
		/// Note: All the events are called in task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// </summary>
		public event AsyncQueryEvent<Exception> TaskFailed;

		/// <summary>
		/// Occurs when task has been terminated. It is called when task has been (successfully) terminated
		/// by <see cref="AsyncRunnerBase.Terminate()"/> or <see cref="AsyncRunnerBase.Abort(int)"/>
		/// The returned value indicates if 'termination' has been handled. It it has <see cref="AsyncRunner.TaskException"/> 
		/// won't be called.
		/// Note: All the events are called in task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// </summary>
		public event AsyncQueryEvent<bool> TaskTerminated;

		/// <summary>
		/// Occurs when task throws an exception. It is called after <see cref="AsyncRunner.TaskFailed"/>
		/// and <see cref="AsyncRunner.TaskTerminated"/> if they have not been 'handled'.
		/// Note: All the events are called in Task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// </summary>
		public event AsyncQueryEvent<Exception> TaskException;

		/// <summary>
		/// Occurs when task throws an exception and has not been handled by <see cref="TaskFailed"/> not <see cref="TaskException"/>.
		/// Note: All the events are called in Task's thread. If you want to sync them to 
		/// caller's (sort of) thread use <see cref="AsyncRunnerBase.Sync"/> or <see cref="AsyncRunnerBase.SyncNoWait"/>.
		/// To call them in separated thread use <see cref="AsyncRunnerBase.Async"/>.
		/// Note, it's not an <c>event</c> it's a <c>delegate</c>, no multiple handlers allowed.
		/// </summary>
		public static AsyncQueryEvent<Exception> TaskLastChanceException { get; set; }

		#endregion
	}

	#endregion

	#region AsyncRunner delegates

	/// <summary>
	/// Async task notification delegate (when task want to tell you something).
	/// </summary>
	public delegate void AsyncNotifyEvent(AsyncRunner runner);

	/// <summary>
	/// Async task notification delegate (when task want to tell you something) with argument.
	/// </summary>
	public delegate void AsyncNotifyEvent<T>(AsyncRunner runner, T args);

	/// <summary>
	/// Async task query delegate (when task wants to ask you something).
	/// </summary>
	public delegate bool AsyncQueryEvent(AsyncRunner runner);

	/// <summary>
	/// Async task query delegate (when task wants to ask you something) with argument.
	/// </summary>
	public delegate bool AsyncQueryEvent<T>(AsyncRunner runner, T args);

	/// <summary>
	/// Async task query delegate (when task wants to ask you something) with argument.
	/// </summary>
	public delegate R AsyncQueryEvent<T, R>(AsyncRunner runner, T args);

	#endregion
}
