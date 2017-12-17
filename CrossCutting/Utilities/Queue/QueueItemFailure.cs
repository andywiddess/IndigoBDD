using System;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Queue
{
    /// <summary>
    /// Specialised type of queue item.
    /// Typically this will get sent if an item on the queue has failed.
    /// </summary>
    [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
    public class QueueItemFailure : QueueItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="context">The context.</param>
        public QueueItemFailure(Guid id, CallStatus context)
            : base(id, context)
        {
        }
    }
}
