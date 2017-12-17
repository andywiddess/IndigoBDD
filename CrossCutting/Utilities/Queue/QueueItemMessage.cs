using System;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Queue
{
    /// <summary>
    /// Special type of queue item.
    /// This sends a message request to the queue processor
    /// It's for information only.
    /// </summary>
    [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
    public class QueueItemMessage : QueueItemRequest
    {

        /// <summary>
        /// The message (if any) on completion
        /// </summary>
        [DataMember]
        public int StatusCode
        {
            get;
            set;
        }
        /// <summary>
        /// The message (if any) on completion
        /// </summary>
        [DataMember]
        public string Message
        {
            get;
            set;
        }
        /// <summary>
        /// Constructor. Creates a new message request.
        /// </summary>
        /// <param name="statusCode">Status code</param>
        /// <param name="configServerURL">The configuration server URL.</param>
        /// <param name="message">Message</param>
        public QueueItemMessage(int statusCode, string configServerURL, string message) 
            : base(configServerURL)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }
    }
}
