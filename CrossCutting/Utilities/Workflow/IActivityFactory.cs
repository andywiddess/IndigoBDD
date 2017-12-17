using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Workflow
{
    /// <summary>
    /// Interface implemented by creators of activitites, and called by the static ActivityCreationFactory.
    /// </summary>
    public interface IActivityFactory
    {
        /// <summary>
        /// Create a new instance of the specified activity in order to add it to a workflow.
        /// </summary>
        /// <param name="activityType"></param>
        /// <returns></returns>
        IWorkflowActivity CreateActivity(Type activityType);
    }
}
