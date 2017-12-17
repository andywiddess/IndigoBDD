using System;
using System.ComponentModel;
using System.Threading;
using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Process
{
	/// <summary>Result of ForkThread on <see cref="Func{T}"/>.</summary>
	/// <typeparam name="T">Type of result.</typeparam>
	public class ForkedFuncThread<T>
        : AbstractForkedThread
	{
		#region fields

		/// <summary>Function to execute.</summary>
		private readonly Func<ForkedFuncThread<T>, T> m_Action;

		/// <summary>Callback to call when finished.</summary>
		private readonly Action<ForkedFuncThread<T>> m_FinishedCallback;

		/// <summary>Result of function.</summary>
		private T m_Result;

		/// <summary>Signal telling that result is ready.</summary>
		private ManualResetEvent m_ResultSignal = new ManualResetEvent(false);

		#endregion

		#region properties

		/// <summary>Gets the result. If result is not ready throws <see cref="InvalidOperationException"/>. 
		/// Use Wait() if not sure.</summary>
		public T Result
		{
			get
			{
				if (m_ResultSignal.WaitOne(0))
				{
					return m_Result;
				}
				else
				{
					throw new InvalidOperationException("Trying to result of Forked function before result is ready");
				}
			}
		}

		/// <summary>Gets the result or default. If result is not ready or exception has been throw and it was never set 
		/// returns whatever the internal storage contains (usually default value for result type, but do not depend on 
		/// this).</summary>
		public T ResultOrDefault
		{
			get { return m_Result; }
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="ForkedFuncThread&lt;T&gt;"/> class.</summary>
		/// <param name="factory">The function to call.</param>
		/// <param name="onFinished">The OnFinished handler.</param>
		/// <param name="synchronizationTarget">The synchronization target.</param>
		public ForkedFuncThread(
			Func<ForkedFuncThread<T>, T> factory,
			Action<ForkedFuncThread<T>> onFinished = null, ISynchronizeInvoke synchronizationTarget = null)
			: base(synchronizationTarget)
		{
			m_Action = factory;
			m_FinishedCallback = onFinished;
		}

		/// <summary>Initializes a new instance of the <see cref="ForkedFuncThread&lt;T&gt;"/> class.</summary>
		/// <param name="factory">The function to call.</param>
		/// <param name="onFinished">The OnFinished handler.</param>
		/// <param name="onSuspended">The OnSuspended handler.</param>
		/// <param name="onResumed">The OnResumed handler.</param>
		/// <param name="synchronizationTarget">The synchronization target.</param>
		public ForkedFuncThread(
			Func<ForkedFuncThread<T>, T> factory,
			Action<ForkedFuncThread<T>> onFinished, Action onSuspended, Action onResumed, ISynchronizeInvoke synchronizationTarget = null)
			: base(onSuspended, onResumed, synchronizationTarget)
		{
			m_Action = factory;
			m_FinishedCallback = onFinished;
		}

		#endregion

		#region internal event handlers

		/// <summary>Called to run the actual action.</summary>
		protected override void OnRun()
		{
			if (m_Action != null) m_Result = m_Action(this);
			m_ResultSignal.Set();
		}

		/// <summary>Specific implementation for OnFinished handler. Implementation depends on derived class.</summary>
		protected override void OnFinishedCallback()
		{
			if (m_FinishedCallback != null) m_FinishedCallback(this);
		}

		#endregion

		#region overrides

		/// <summary>Disposes the managed resources.</summary>
		protected override void DisposeManaged()
		{
			Patterns.DisposeAndNull(ref m_ResultSignal);
			base.DisposeManaged();
		}

		#endregion

		#region public interface

		/// <summary>Waits for the task to finish. Returns values returned by task.</summary>
		/// <returns>Result of the task.</returns>
		public new T Wait()
		{
			base.Wait();
			return m_Result;
		}

		/// <summary>Waits for the task to finish. Returns values returned by task.</summary>
		/// <returns>Result of the task.</returns>
		public new T Wait(int milliseconds)
		{
			base.Wait(milliseconds);
			return m_Result;
		}

		/// <summary>Waits for the task to finish. Returns values returned by task.</summary>
		/// <returns>Result of the task.</returns>
		public new T Wait(TimeSpan timespan)
		{
			base.Wait(timespan);
			return m_Result;
		}

		#endregion
	}
}
