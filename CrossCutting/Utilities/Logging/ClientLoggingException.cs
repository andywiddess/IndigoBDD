using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Client Services Exception Logging
    /// </summary>
    public class ClientLoggingException
        : ApplicationException
    {
        #region Members
        /// <summary>
        /// Gets or sets the localized user message, that is appropriate
        /// for displaying to a user.
        /// </summary>
        /// <value>The user message.</value>
        public string UserMessage
        {
            get;
            protected set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLoggingException"/> class.
        /// </summary>
        /// <param name="developerMessage">The developer message that may contain
        /// detailed information regarding the exception.</param>
        /// <param name="userMessage">The localized user message that is appropriate
        /// for displaying to a user. Can be <code>null</code>.</param>
        public ClientLoggingException(string developerMessage, string userMessage)
            : base(developerMessage)
        {
            UserMessage = userMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLoggingException"/> class.
        /// </summary>
        /// <param name="developerMessage">The developer message that may contain
        /// detailed information regarding the exception.</param>
        /// <param name="userMessage">The localized user message that is appropriate
        /// for displaying to a user. Can be <code>null</code>.</param>
        /// <param name="innerException">The inner exception.</param>
        public ClientLoggingException(string developerMessage, string userMessage, Exception innerException)
            : base(developerMessage, innerException)
        {
            UserMessage = userMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLoggingException"/> class.
        /// </summary>
        /// <param name="developerMessage">The developer message that may contain
        /// detailed information regarding the exception.</param>
        public ClientLoggingException(string developerMessage)
            : base(developerMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLoggingException"/> class.
        /// </summary>
        /// <param name="developerMessage">The developer message that may contain
        /// detailed information regarding the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public ClientLoggingException(string developerMessage, Exception innerException)
            : base(developerMessage, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLoggingException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public ClientLoggingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            UserMessage = info.GetString("UserMessage");
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether the UserMessage is <code>null</code>.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the UserMessage is not <code>null</code>;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool UserMessagePresent
        {
            get
            {
                return UserMessage != null;
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        /// </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UserMessage", UserMessage);
            base.GetObjectData(info, context);
        }
        #endregion
    }
}
