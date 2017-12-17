using System;
using System.Threading.Tasks;

using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Exceptions;

namespace Indigo.CrossCutting.Utilities.Process
{
	/// <summary>
	/// Result of spawning a thread executing an action with <see cref="Patterns.Fork"/>
	/// </summary>
	public class ForkedAction
	{
		#region fields

		/// <summary>A task which was actually executing action.</summary>
		private readonly Task m_Task;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="ForkedAction"/> class.</summary>
		/// <param name="task">The task.</param>
		internal ForkedAction(Task task)
		{
			m_Task = task;
		}

		#endregion

		#region public interface

		/// <summary>Waits for task to finish for a given time. 
		/// If task ended with exception the exception will be rethrown.</summary>
		/// <param name="millisecondsTimeout">The milliseconds timeout.</param>
		/// <returns><c>true</c> if task has finished, <c>false</c> otherwise.</returns>
		public bool TryWait(int millisecondsTimeout)
		{
			// wait for task to finish, if it throws exception it means it has been finished
			// (thus finished = true) it just failed.
			var finished = Patterns.IgnoreException(() => m_Task.Wait(millisecondsTimeout), true);
			if (!finished) return false;

			if (m_Task.Exception != null) throw Patterns.ResolveTaskException(m_Task);
			return true;
		}

		/// <summary>Waits for task to finish for a given time. 
		/// If task ended with exception the exception will be rethrown.</summary>
		/// <param name="timeout">The timeout.</param>
		/// <returns><c>true</c> if task has finished, <c>false</c> otherwise.</returns>
		public bool TryWait(TimeSpan timeout)
		{
			// wait for task to finish, if it throws exception it means it has been finished
			// (thus finished = true) it just failed.
			var finished = Patterns.IgnoreException(() => m_Task.Wait(timeout), true);
			if (!finished) return false;

			if (m_Task.Exception != null) throw Patterns.ResolveTaskException(m_Task);
			return true;
		}

		/// <summary>
		/// Waits for acton to finish.
		/// Please note, if for any reason action did not finish successfully this method will call an exception.
		/// </summary>
		/// <exception cref="CanceledException">Thrown if action has been canceled.</exception>
		/// <exception cref="AggregateException">Thrown if action has failed with exception.</exception>
		public void Wait()
		{
			Patterns.IgnoreException(() => m_Task.Wait());
			ProcessResult();
		}

		/// <summary>
		/// Waits for acton to finish.
		/// Please note, if for any reason action did not finish successfully this method will call an exception.
		/// </summary>
		/// <exception cref="TimeoutException">Thrown if waiting time expired.</exception>
		/// <exception cref="CanceledException">Thrown if action has been canceled.</exception>
		/// <exception cref="AggregateException">Thrown if action has failed with exception.</exception>
		public void Wait(int millisecondsTimeout)
		{
			Patterns.IgnoreException(() => m_Task.Wait(millisecondsTimeout));
			ProcessResult();
		}

		/// <summary>
		/// Waits for acton to finish.
		/// Please note, if for any reason action did not finish successfully this method will call an exception.
		/// </summary>
		/// <exception cref="TimeoutException">Thrown if waiting time expired.</exception>
		/// <exception cref="CanceledException">Thrown if action has been canceled.</exception>
		/// <exception cref="AggregateException">Thrown if action has failed with exception.</exception>
		public void Wait(TimeSpan timeout)
		{
			Patterns.IgnoreException(() => m_Task.Wait(timeout));
			ProcessResult();
		}

		#endregion

		#region private implementation

        /// <summary>
        /// Throws an exception if needed.
        /// </summary>
        ///
        /// <exception cref="ResolveTaskException"> Thrown when a Resolve Task error condition occurs. </exception>
		private void ProcessResult()
		{
			var exception = Patterns.ResolveTaskException(m_Task);
			if (exception != null) throw exception;
		}

		#endregion
	}
}
