using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Contains data pertaining to a <see cref="ILogEntry"/>.
    /// </summary>
    [DataContract(Namespace = "http://www.sepura.co.uk/Logging/2014/07/"),
     Serializable]
    public class LogEntryData
        : ClientInfo,
          ILogEntry
    {
        #region Members
        /// <summary>
        /// Gets the entry id.
        /// </summary>
        /// <value>The entry id.</value>
        public Guid EntryId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the log level of the log entry.
        /// This is the intended level that the log entry should be written.
        /// This may cause the log entry to be ignored if the current
        ///  has a higher threshold log level.
        /// </summary>
        /// <value>The log level.</value>
        [DataMember]
        public LogLevelType LogLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message to be logged.
        /// </summary>
        /// <value>The message.</value>
        [DataMember]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the code location of the call to log.
        /// </summary>
        /// <value>The code location.</value>
        [DataMember]
        public CodeLocation CodeLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exception log, representing
        /// an exception that is to be logged.
        /// May be null.
        /// </summary>
        /// <value>The exception log.</value>
        [DataMember]
        public virtual ExceptionLog ExceptionLog
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the thread that requested
        /// the log entry to take place.
        /// </summary>
        /// <value>The name of the thread.</value>
        [DataMember]
        public string ThreadName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the managed thread id that requested
        /// the log entry to take place.
        /// </summary>
        /// <value>The managed thread id.</value>
        [DataMember]
        public int ManagedThreadId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time that the log request was made.
        /// </summary>
        /// <value>The time that the log request was made.</value>
        [DataMember]
        public DateTime OccuredAt
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets additional custom properties.
        /// </summary>
        /// <value>The custom properties.</value>
        [DataMember]
        public IDictionary<string, object> ListOfProperties
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntryData"/> class.
        /// </summary>
        public LogEntryData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntryData"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public LogEntryData(Guid id)
        {
            EntryId = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntryData"/> class.
        /// </summary>
        /// <param name="logEntryData">The log entry data.</param>
        public LogEntryData(ILogEntry logEntryData)
            : base(logEntryData)
        {
            CodeLocation = logEntryData.CodeLocation;
            Message = logEntryData.Message;
            LogLevel = logEntryData.LogLevel;
            ThreadName = logEntryData.ThreadName;
            ManagedThreadId = logEntryData.ManagedThreadId;
            ListOfProperties = logEntryData.ListOfProperties;
            OccuredAt = logEntryData.OccuredAt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntryData"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="logEntryData">The log entry data.</param>
        public LogEntryData(Guid id, ILogEntry logEntryData)
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
            var propertiesBuilder = new StringBuilder();
            if (ListOfProperties != null)
            {
                foreach (var pair in ListOfProperties)
                {
                    propertiesBuilder.Append(pair.Key);
                    propertiesBuilder.Append('=');
                    propertiesBuilder.Append(pair.Value);
                    propertiesBuilder.Append(";");
                }
            }

            return string.Format("{0} , Message:{1}, LogLevelType: {2}, ThreadName: {3}, ManagedThreadId: {4}, ListOfProperties: {5}, {6}",
                                 CodeLocation, Message, LogLevel, ThreadName, ManagedThreadId, propertiesBuilder, base.ToString());
        }
        #endregion
    }
}
