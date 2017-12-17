using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.Exceptions
{
    /// <summary>
    /// An exception thrown when the <c>Messages</c> collection has been locked and can no longer be edited.
    /// </summary>
    public class MessagesLockedException 
        : Exception
    {
        private static readonly string s_LockedMessage = "Messages object is locked.";

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesLockedException"/> class.
        /// </summary>
        public MessagesLockedException()
            : base(s_LockedMessage)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesLockedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessagesLockedException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesLockedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MessagesLockedException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesLockedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected MessagesLockedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}
