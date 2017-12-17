using System;

namespace Indigo.CrossCutting.Utilities.Queue
{
    /// <summary>
    /// Wrapper class for a message on a queue.
    /// </summary>
    public class QueueMessage
    {
        /// <summary>
        /// Creates a message object with given parameters.
        /// </summary>
        /// <param name="queueItem">The queue item.</param>
        /// <param name="conversationHandle">The conversation handle.</param>
        public QueueMessage(QueueItem queueItem, Guid conversationHandle)
        {
            this.item = queueItem;
            this.ConversationHandle = conversationHandle;
        }

        /// <summary>
        /// The queue item within the message.
        /// </summary>
        private QueueItem item;

        /// <summary>
        /// The queue item within the message.
        /// </summary>
        public QueueItem Item
        {
            get { return item; }
            set { item = value; }
        }

        /// <summary>
        /// The conversation this message forms part of
        /// </summary>
        private Guid conversationHandle;

        /// <summary>
        /// The conversation this message forms part of
        /// </summary>
        public Guid ConversationHandle
        {
            get { return conversationHandle; }
            set { conversationHandle = value; }
        }

        /// <summary>
        /// Sequence number of the message within a conversation
        /// </summary>
        private long sequenceNumber;

        /// <summary>
        /// Sequence number of the message within a conversation
        /// </summary>
        public long SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = value; }
        }

        /// <summary>
        /// The service to which this message was sent.
        /// </summary>
        private SepuraQueueServices service;
        /// <summary>
        /// The service to which this message was sent.
        /// </summary>
        public SepuraQueueServices Service
        {
            get { return service; }
            set { service = value; }
        }

        /// <summary>
        /// The name of the sender of the message.
        /// </summary>
        public string SenderName
        {
            get;
            set;
        }
    }
}
