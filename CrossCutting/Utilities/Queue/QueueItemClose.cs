using System;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Queue
{
    /// <summary>
    /// Special type of queue item.
    /// This item gets sent when an item is being closed (with success).
    /// </summary>
    [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
    public class QueueItemClose : QueueItem
    {
        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <remarks>
        /// These have blank RequestIDs. 
        /// The Request ID cannot be sent as part of a Close/CloseWithError, so it's set to NULL instead.
        /// </remarks>
        public QueueItemClose() : base(Guid.Empty)
        {
        }
    }
}
