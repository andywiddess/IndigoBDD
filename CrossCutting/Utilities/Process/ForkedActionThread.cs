using System;
using System.ComponentModel;

namespace Indigo.CrossCutting.Utilities.Process
{
	/// <summary>
	/// Result of ForkThread executed for action.
	/// </summary>
	public class ForkedActionThread
        : AbstractForkedThread
	{
		#region fields

		/// <summary>Action to be executed.</summary>
		private readonly Action<ForkedActionThread> m_ActionCallback;

		/// <summary>Callback to call when finished.</summary>
		private readonly Action<ForkedActionThread> m_FinishedCallback;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="ForkedActionThread"/> class.</summary>
		/// <param name="action">The action to execute.</param>
		/// <param name="onFinished">Callback called when finished.</param>
		/// <param name="synchronizationTarget">The synchronization target.</param>
		public ForkedActionThread(
			Action<ForkedActionThread> action,
			Action<ForkedActionThread> onFinished = null, ISynchronizeInvoke synchronizationTarget = null)
			: base(synchronizationTarget)
		{
			m_ActionCallback = action;
			m_FinishedCallback = onFinished;
		}

		/// <summary>Initializes a new instance of the <see cref="ForkedActionThread"/> class.</summary>
		/// <param name="action">The action to execute.</param>
		/// <param name="onFinished">Callback called when finished.</param>
		/// <param name="onSuspend">The on suspend.</param>
		/// <param name="onResume">The on resume.</param>
		/// <param name="synchronizationTarget">The synchronization target.</param>
		public ForkedActionThread(
			Action<ForkedActionThread> action,
			Action<ForkedActionThread> onFinished, Action onSuspend, Action onResume, ISynchronizeInvoke synchronizationTarget = null)
			: base(onSuspend, onResume, synchronizationTarget)
		{
			m_ActionCallback = action;
			m_FinishedCallback = onFinished;
		}

		#endregion

		#region internal event handlers

        /// <summary>
        /// Called to run the actual action.
        /// </summary>
		protected override void OnRun()
		{
			if (m_ActionCallback != null) m_ActionCallback(this);
		}

        /// <summary>
        /// Specific implementation for OnFinished handler. Implementation depends on derived class.
        /// </summary>
		protected override void OnFinishedCallback()
		{
			if (m_FinishedCallback != null) m_FinishedCallback(this);
		}

		#endregion
	}
}