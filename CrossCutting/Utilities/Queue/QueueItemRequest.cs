using System;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Queue
{
    /// <summary>
    /// Special type of queue item.
    /// This is a base class for items that are updates on other items (e.g. status updates, success, failure, retry message).
    /// </summary>
    [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
    public abstract class QueueItemRequest 
        : QueueItem
    {
        /// <summary>
        /// The number of times we should attempt to retry processing requests.
        /// </summary>
        static public int MaxRetryAttempts = 5;
        /// <summary>
        /// Is this a potentially long-running queue item?
        /// Typically this flag will be used by any item processor to decide whether to deal with an item
        /// there and then, or do it asynchronously.
        /// </summary>
        public override bool IsRequest
        {
            get { return true; }
        }
        /// <summary>
        /// The security token associated with the request
        /// </summary>
        [DataMember]
        public object SecurityToken; // Indigo.CrossCutting.Utilities.Security.SerialisedToken
        /// <summary>
        /// The configuration server URL associated with the queueing server.
        /// This will allow the queue agent to track down the business database the request is to be sent to.
        /// </summary>
        [DataMember]
        public string ConfigurationServerURL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configurationServer">The configuration server.</param>
        protected QueueItemRequest(string configurationServer) : base()
        {
            ConfigurationServerURL = configurationServer;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configurationServer">The configuration server.</param>
        /// <param name="securityToken">The security token.</param>
        protected QueueItemRequest(string configurationServer, object securityToken)
            : this(configurationServer)
        {
            this.SecurityToken = securityToken;
        }
    }
}
