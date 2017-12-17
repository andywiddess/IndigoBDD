using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Server Log Entry Object
    /// </summary>
    [DataContract(Namespace = "http://www.sepura.co.uk/Logging/2014/07/"),
     Serializable]
    public sealed class ServerLogEntry
        : AbstractLogEntry,
          IServerLogEntry
    {
        #region Members
        /// <summary>
        /// Gets or sets the exception that is to be logged.
        /// May be null.
        /// </summary>
        /// <value>The exception that occured.</value>
        [XmlIgnore]
        public Exception Exception
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the app domain that the log request was made.
        /// </summary>
        /// <value>The app domain.</value>
        [DataMember]
        public string AppDomain
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets string representation of the current thread's principal identity.
        /// </summary>
        /// <value>
        /// The string representation of the current thread's principal identity.
        /// </value>
        [DataMember]
        public string Identity
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerLogEntry"/> class.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        public ServerLogEntry(LogLevelType logLevel, string message)
            : base(logLevel, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerLogEntry"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        public ServerLogEntry(Guid id, LogLevelType logLevel, string message)
            : this(logLevel, message)
        {
            EntryId = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerLogEntry"/> class.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public ServerLogEntry(LogLevelType logLevel, string message, Exception exception)
            : base(logLevel, message)
        {
            Exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerLogEntry"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public ServerLogEntry(Guid id, LogLevelType logLevel, string message, Exception exception)
            : this(logLevel, message, exception)
        {
            EntryId = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerLogEntry"/> class.
        /// </summary>
        /// <param name="logEntryData">The log entry data.</param>
        public ServerLogEntry(LogEntryData logEntryData)
            : base(logEntryData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerLogEntry"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="logEntryData">The log entry data.</param>
        public ServerLogEntry(Guid id, LogEntryData logEntryData)
            : this(logEntryData)
        {
            EntryId = id;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, AppDomain: {1}, Identity: {2}", base.ToString(), AppDomain, Identity);
        }
        #endregion
    }
}
