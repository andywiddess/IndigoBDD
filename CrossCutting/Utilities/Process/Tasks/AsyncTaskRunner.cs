using System.ComponentModel;

namespace Indigo.CrossCutting.Utilities.Process.Tasks
{
	/// <summary>
	/// <see cref="AsyncRunner"/> dedicated version to run <see cref="AsyncTask"/>
	/// </summary>
	public class AsyncTaskRunner
        : AsyncRunner
	{
		#region fields

		private AsyncTask m_Task;

		#endregion

		#region properties

		/// <summary>
		/// Task being ran. Note: It is set to <c>null</c> if task is not running.
		/// </summary>
		/// <value>The task.</value>
		public AsyncTask Task
		{
			get { return m_Task; }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncTaskRunner"/> class. 
		/// Note: this creates a task without synchronization target. 
		/// You probably don't want to do this. Check <see cref="AsyncTaskRunner(ISynchronizeInvoke)"/>.
		/// </summary>
		public AsyncTaskRunner()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncRunner"/> class.
		/// </summary>
		/// <param name="sync">The sync target.</param>
		public AsyncTaskRunner(SyncInvoker sync)
			: base(sync)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncRunner"/> class.
		/// </summary>
		/// <param name="sync">The sync target.</param>
		public AsyncTaskRunner(ISynchronizeInvoke sync)
			: base(sync)
		{
		}

		#endregion

		#region overrides

		/// <summary>
		/// Execution of the task. <see cref="AsyncRunner.TaskRunning"/> for details.
		/// </summary>
		protected override void OnRun()
		{
			base.OnRun(); // calls the original handler anyway
			if (m_Task != null) m_Task.Execute(this);
		}

		/// <summary>
		/// Called after task is finished. <see cref="AsyncRunner.TaskFinished"/> for details.
		/// </summary>
		protected override void OnAfterRun()
		{
			try
			{
				base.OnAfterRun();
			}
			finally
			{
				m_Task = null;
			}
		}

		#endregion

		#region run

		/// <summary>
		/// Runs the specified task.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <param name="async">if set to <c>true</c> task is ran asynchronously (default).</param>
		public void Run(AsyncTask task, bool async)
		{
			m_Task = task;
			base.Run(async);
		}

		/// <summary>
		/// Runs the specified task.
		/// </summary>
		/// <param name="task">The task.</param>
		public void Run(AsyncTask task)
		{
			Run(task, true);
		}

		#endregion
	}
}
