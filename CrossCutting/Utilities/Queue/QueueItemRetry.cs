using System;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Queue
{
    /// <summary>
    /// Special type of queue item.
    /// This is a retry request for an item previously sent on the queue.
    /// </summary>
    [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
    public class QueueItemRetry : QueueItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="retryItemId">ID of the item to retry</param>
        public QueueItemRetry(Guid retryItemId)
            : base(retryItemId)
        {
        }

    }
}
