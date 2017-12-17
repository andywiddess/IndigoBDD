namespace Indigo.CrossCutting.Utilities.Process.Tasks
{
	/// <summary>
	/// Defines unified interface for task being executed by <see cref="AsyncTaskRunner"/>.
	/// </summary>
	public interface IAsyncTask
	{
		/// <summary>
		/// Executes the task within specified runner.
		/// </summary>
		/// <param name="runner">The runner.</param>
		void Execute(AsyncRunner runner);
	}
}
