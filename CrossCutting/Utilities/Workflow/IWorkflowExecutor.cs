using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Workflow
{
    /// <summary>
    /// Wrapper interface for executing a workflow
    /// </summary>
    public interface IWorkflowExecutor
    {
        /// <summary>
        /// Execute the workflow 
        /// </summary>
        void Execute(Indigo.CrossCutting.Utilities.Workflow.WorkflowBase workflowToExecute);

        /// <summary>
        /// Execute the workflow and wait till the workflow has been executed
        /// </summary>
        void ExecuteAndWait(Indigo.CrossCutting.Utilities.Workflow.WorkflowBase workflowToExecute);

        /// <summary>
        /// Event raised when the workflow has completed
        /// </summary>
        event EventHandler ExecutionCompleted;

        /// <summary>
        /// Event raised when an activity in the workflow has completed.
        /// </summary>
        event EventHandler<WorkflowBase.ActivityCompletedEventArgs> ActivityCompleted;
    }
}
