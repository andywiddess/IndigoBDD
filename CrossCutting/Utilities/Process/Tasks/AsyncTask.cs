using System;

namespace Indigo.CrossCutting.Utilities.Process.Tasks
{
	/// <summary>
	/// Basic implementation of <see cref="IAsyncTask"/>. As <see cref="AsyncTaskRunner"/> can
	/// run any <see cref="IAsyncTask"/> I'd strongly advise to derive your tasks from this class, 
	/// not the interface.
	/// </summary>
	public abstract class AsyncTask: IAsyncTask
	{
		#region fields

		private AsyncRunner m_Runner;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the runner which is used to run this task.
		/// If task is not running this property is set to <c>null</c>.
		/// </summary>
		/// <value>The runner.</value>
		public AsyncRunner Runner
		{
			get { return m_Runner; }
			set
			{
				if (m_Runner != null)
					throw new InvalidOperationException("Cannot assign a runner to a task which is already running");

				m_Runner = value;
			}
		}

		#endregion

		#region IAsyncTask Members

		/// <summary>
		/// Executes the task within specified runner.
		/// </summary>
		/// <param name="runner">The runner.</param>
		public void Execute(AsyncRunner runner)
		{
			m_Runner = runner;
			try
			{
				Execute();
			}
			finally
			{
				m_Runner = null;
			}
		}

		#endregion

		#region execute

		/// <summary>
		/// Executing of a task. Put your code here.
		/// </summary>
		protected abstract void Execute();

		/// <summary>
		/// Runs this task, without creating separate thread (synchronously).
		/// Used for quick tasks, which may not need any feedback.
		/// Please note that even if tasks in general are using last-chance-exception-handling
		/// this call will not. Last chance exception handling is for running tasks is separated thread,
		/// as this one is actually ran synchronously it is not needed and potential exception will
		/// be thrown here.
		/// </summary>
		public void Run()
		{
			new AsyncTaskRunner().Run(this, false);
		}

		#endregion
	}
}
