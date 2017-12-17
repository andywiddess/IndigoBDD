using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Workflow
{
    /// <summary>
    /// Interface supported by a single client workflow task.
    /// </summary>
    public interface IWorkflowActivity
    {
        /// <summary>
        /// Executes the activity
        /// </summary>
        void Execute();

        /// <summary>
        /// Event raised when the activity has completed
        /// </summary>
        event EventHandler ExecutionCompleted;

        /// <summary>
        /// Get the current status of the activity, 
        /// </summary>
        /// <returns></returns>
        Indigo.CrossCutting.Utilities.CallStatus GetActivityStatus();

        /// <summary>
        /// Returns TRUE if the execution completed event is hooked by a consumer
        /// </summary>
        /// <returns></returns>
        bool IsExecutionCompletedEventHooked();
        
    }
}
