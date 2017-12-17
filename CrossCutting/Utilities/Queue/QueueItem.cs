using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.Diagnostics;

namespace Indigo.CrossCutting.Utilities.Queue
{
    /// <summary>
    /// This is a wrapper class for objects being placed on the queue.
    /// The items put in the queue must implement the IQueueableItem interface. This class
    /// stores additional data about the objects - their security tokens, creation date etc.
    /// This class allows us to place multiple types of objects in the queue.
    /// </summary>
    [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
    public abstract class QueueItem
    {
        /// <summary>
        /// Is this a potentially long-running queue item?
        /// Typically this flag will be used by any item processor to decide whether to deal with an item
        /// there and then, or do it asynchronously.
        /// </summary>
        public virtual bool IsRequest
        {
            get { return false; }
        }
        /// <summary>
        /// The guid of the current Service Broker conversation handle.
        /// This is used when items are being processed to ensure thatConversations are completeld correctly (by calling END CONVERSATION).
        /// Not doing this would cause the items to remain on the queue forever.
        /// It is NOT a persistant value and should not be serialised.
        /// </summary>
        public Guid CurrentConversationHandle = Guid.Empty;
        /// <summary>
        /// The date this queue item was created.
        /// </summary>
        [DataMember]
        public DateTime CreationDate;
        /// <summary>
        /// The execution status of this queue item
        /// </summary>
        protected Indigo.CrossCutting.Utilities.CallStatus callStatus;
        /// <summary>
        /// The execution status of this queue item
        /// </summary>
        [DataMember]
        public Indigo.CrossCutting.Utilities.CallStatus CallStatus
        {
            get
            {
                if (callStatus == null)
                    callStatus = new Indigo.CrossCutting.Utilities.CallStatus();
                return callStatus;
            }
            set
            {
                callStatus = value;
            }

        }
        /// <summary>
        /// Request identifier for this queue item.
        /// Typically, new items which are a request will be allocated new IDs.
        /// All other items which are about the status of a request will have the ID of that request.
        /// </summary>
        [DataMember]
        public Guid RequestID;
        /// <summary>
        /// Constructor.
        /// </summary>
        protected QueueItem()
        {
            RequestID = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="securityToken">The security token</param>
        protected QueueItem(string securityToken)
        {
            RequestID = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="requestId">The ID of the requested this item relates to</param>
        /// <param name="status">The status.</param>
        protected QueueItem(Guid requestId, CallStatus status = null)
        {
            RequestID = requestId;
            CreationDate = DateTime.Now;
            this.callStatus = status;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestId">The ID of the requested this item relates to</param>
        /// <param name="securityToken">The security token</param>
        protected QueueItem(Guid requestId, string securityToken)
        {
            RequestID = requestId;
            CreationDate = DateTime.Now;
        }
    }
}
