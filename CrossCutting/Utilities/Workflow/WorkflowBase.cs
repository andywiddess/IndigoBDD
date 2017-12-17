using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Workflow
{
    /// <summary>
    /// Base class for all types of workflow. Instantiators can hook the ExecutionCompleted event if they need to know that the entire workflow has completed, or the ActivityCompleted
    /// event to be told when an individual Activity has completed within the workflow.
    /// </summary>
    public abstract class WorkflowBase : IWorkflowActivity, IThreadSensitiveActivity
    {
        /// <summary>
        /// The parameters set for this server workflow.
        /// </summary>
        public Dictionary<string, object> WorkflowParameters = new Dictionary<string,object>();

        public static bool EnableTracing = true;

        /// <summary>
        /// The activity factory to use to create instances of activities during this workflow execution.
        /// </summary>
        protected IActivityFactory ActivityFactory { get; private set; }


        public WorkflowBase(IActivityFactory activityFactory)
        {
            this.ActivityFactory = activityFactory;
            if (WorkflowBase.EnableTracing) System.Diagnostics.Trace.WriteLine(string.Format("Workflow {0} Initialized ", this.GetType().Name));
        }

        /// <summary>
        /// Execute the workflow, and await the ExecutionCompleted event to signal completion.
        /// </summary>
        public virtual void Execute()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validate that all mandatory parameters are present for the workflow.
        /// </summary>
        public abstract void Validate();

        /// <summary>
        /// Execute the passed Activity and wait until it has completed. This spins up a seperate thread to execute the task on.
        /// </summary>
        /// <param name="activity">The activity.</param>
        public void WaitOnCompletion(IWorkflowActivity activity)
        {
            if (EnableTracing) System.Diagnostics.Trace.WriteLine(string.Format("Workflow {0} Starting Activity {1}" , this.GetType().Name,activity.GetType().Name));
            System.Threading.AutoResetEvent are = new System.Threading.AutoResetEvent(false); 

            EventHandler callCompletedEventHandler = null;
            callCompletedEventHandler = delegate(object sender, EventArgs e)
            {
                activity.ExecutionCompleted -= callCompletedEventHandler;
                are.Set(); // Triggers the main thread to continue executing
            };

            activity.ExecutionCompleted += callCompletedEventHandler;

            IThreadSensitiveActivity threadSensitiveActivity = activity as IThreadSensitiveActivity;
            if (threadSensitiveActivity != null) threadSensitiveActivity.UserInterfaceThreadObject = this.UserInterfaceThreadObject;

            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(
                    delegate(object target)
                    {
                        activity.Execute();
                    }));

            // Wait until the action has completed
            are.WaitOne();

            if (EnableTracing) System.Diagnostics.Trace.WriteLine(string.Format("Workflow {0} Completed Activity {1}" , this.GetType().Name,activity.GetType().Name));

            this.RaiseActivityExecutionCompleted(activity);

            this.InheritActivityStatus(activity);
        }

        /// <summary>
        /// Raise the ExecutionCompleted event
        /// </summary>
        protected void RaiseExecutionCompleted()
        {
            if (Indigo.CrossCutting.Utilities.Workflow.WorkflowBase.EnableTracing) System.Diagnostics.Trace.WriteLine(string.Format("Workflow {0} Completed " ,this.GetType().Name));
            if (this.ExecutionCompleted != null) this.ExecutionCompleted(this, new EventArgs());
        }

        /// <summary>
        /// Returns TRUE if the execution completed event is hooked by a consumer
        /// </summary>
        /// <returns></returns>
        public bool IsExecutionCompletedEventHooked()
        {
            return this.ExecutionCompleted != null;
        }

        /// <summary>
        /// Returns TRUE if the activity completed event is hooked by a consumer
        /// </summary>
        /// <returns></returns>
        public bool IsActivityCompletedEventHooked()
        {
            return this.ActivityCompleted != null;
        }
        /// <summary>
        /// Event raised when the workflow has completed
        /// </summary>
        public event EventHandler ExecutionCompleted;

        public class ActivityCompletedEventArgs : EventArgs
        {
            public ActivityCompletedEventArgs(IWorkflowActivity activity)
            {
                this.Activity = activity;
            }

            /// <summary>
            /// Get the activity that has just completed within the workflow Sender
            /// </summary>
            public IWorkflowActivity Activity { get; protected set; }
        }

        /// <summary>
        /// Event raised when an individual activity in the workflow has completed
        /// </summary>
        public event EventHandler<ActivityCompletedEventArgs> ActivityCompleted;

        /// <summary>
        /// Raise the ActivityCompleted event
        /// </summary>
        protected void RaiseActivityExecutionCompleted(IWorkflowActivity activity)
        {
            if (this.ActivityCompleted != null) this.ActivityCompleted(this, new ActivityCompletedEventArgs(activity));
        }

        protected Indigo.CrossCutting.Utilities.CallStatus activityStatus = new Indigo.CrossCutting.Utilities.CallStatus();

        public Indigo.CrossCutting.Utilities.CallStatus GetActivityStatus()
        {
            return activityStatus;
        }

        /// <summary>
        /// Inherit the passed activity status to become this new workflow status
        /// </summary>
        /// <param name="activity"></param>
        protected void InheritActivityStatus(IWorkflowActivity activity)
        {
            //It is possible that the the activity status is already faulted - in that case do not reset it - but just append the messages 
            if (this.activityStatus.Result == CallStatus.enumCallResult.Success)
            {
                this.activityStatus.Result = activity.GetActivityStatus().Result;
            }
            //Append the error messages
            this.activityStatus.Messages.AddRange(activity.GetActivityStatus().Messages);
        }

        /// <summary>
        /// Throw an exception if the passed activity status is not Success
        /// </summary>
        /// <param name="activity"></param>
        protected void ThrowExceptionOnFailure(IWorkflowActivity activity)
        {
            if (activity.GetActivityStatus().Result != CallStatus.enumCallResult.Success)
            {
                throw new Exception(activity.GetActivityStatus().Messages.FirstOrDefault().Message);
            }
        }

        /// <summary>
        /// When participating in a user interface thread model, this object will be the user interface thread, (in silverlight, this will be a Dispatcher object).
        /// </summary>
        public object UserInterfaceThreadObject
        {
            get; set;
        }


        /// <summary>
        /// The type loader to use to load Types specified in CustomBehaviours on workflows.
        /// </summary>
        public ILateBoundTypeLoader LateBoundTypeLoader { get; set; }

        /// <summary>
        /// Ensures that the parameter name passed exists in this workflow parameter list ,and is not null.
        /// </summary>
        /// <param name="parameterName"></param>
        public void AssureMandatoryParameter(string parameterName)
        {
            if (!this.WorkflowParameters.ContainsKey(parameterName) || this.WorkflowParameters[parameterName] == null) throw new ArgumentException("Cannot be null or empty", parameterName);
        }
    }
}
