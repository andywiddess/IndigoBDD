using System;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Queue
{
    /// <summary>
    /// Special type of queue item.
    /// One of these will get sent when a queue item has completed successfully.
    /// </summary>
    [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
    public class QueueItemSuccess : QueueItem
    {
        public QueueItemSuccess(Guid id, CallStatus context)
            : base(id, context)
        {
        }
    }
}
