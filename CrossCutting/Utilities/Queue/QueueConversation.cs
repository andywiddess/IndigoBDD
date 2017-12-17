using System;

namespace Indigo.CrossCutting.Utilities.Queue
{
    /// <summary>
    /// Class which represents a conversation happening on a queue.
    /// This is not serializable because typically, these will be dynamic, transient objects which are populated
    /// from the queue.
    /// </summary>
    public class QueueConversation
    {
        /// <summary>
        /// The service this conversation is happening on
        /// </summary>
        private SepuraQueueServices svc;
        /// <summary>
        /// The service this conversation is happening on
        /// </summary>
        public SepuraQueueServices Service
        {
            get { return svc; }
        }
        /// <summary>
        /// The conversation handle/ID
        /// </summary>
        private Guid handle;
        /// <summary>
        /// The conversation handle/ID
        /// </summary>
        public Guid Handle
        {
            get { return handle; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueConversation"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="handle">The handle.</param>
        /// <exception cref="System.ArgumentException">Handle cannot be empty.</exception>
        public QueueConversation(SepuraQueueServices service, Guid handle)
        {
            if (handle == Guid.Empty)
                throw new ArgumentException("Handle cannot be empty.");
            this.svc = service;
            this.handle = handle;
        }
    }
}
