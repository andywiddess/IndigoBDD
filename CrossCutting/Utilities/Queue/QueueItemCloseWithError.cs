using System;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Queue
{
    /// <summary>
    /// Specialised type of queue item.
    /// Typically this will get sent if an item on the queue has failed and it's been requested to be closed, mostly by SSOS and
    /// internal to SQL Server itself rather than any application code issues.
    /// </summary>
    [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
    public class QueueItemCloseWithError : QueueItemClose
    {
        /// <summary>
        /// The error code of the error.
        /// </summary>
        [DataMember]
        public int ErrorCode
        {
            get;
            set;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="errorMessage">Error message</param>
        public QueueItemCloseWithError(int errorCode, string errorMessage)
            : base()
        {
            this.ErrorCode = errorCode;
            this.CallStatus.Result = CallStatus.enumCallResult.Fault;
            this.CallStatus.Messages.Add(errorMessage);
        }
    }
}
