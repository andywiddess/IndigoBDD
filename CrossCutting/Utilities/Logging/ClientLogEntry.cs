using System;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Client Log Entry Object
    /// </summary>
    [DataContract(Namespace = "http://www.sepura.co.uk/Logging/2014/07/"),
     Serializable]
    public sealed class ClientLogEntry
        : AbstractLogEntry,
          IClientLogEntry
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLogEntry"/> class.
        /// </summary>
        /// <param name="logEntryData">The log entry data.</param>
        public ClientLogEntry(LogEntryData logEntryData)
            : base(logEntryData)
        {
            ExceptionLog = logEntryData.ExceptionLog;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLogEntry"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="logEntryData">The log entry data.</param>
        public ClientLogEntry(Guid id, LogEntryData logEntryData)
            : base(logEntryData)
        {
            EntryId = id;
            ExceptionLog = logEntryData.ExceptionLog;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the exception log, representing
        /// an exception that is to be logged.
        /// May be null.
        /// </summary>
        /// <value>The exception log.</value>
        [DataMember]
        public new IExceptionLog ExceptionLog
        {
            get
            {
                return base.ExceptionLog;
            }
            set
            {
                base.ExceptionLog = (ExceptionLog)value;
            }
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
            string result = string.Format("{0}, Exception: {1}", base.ToString(), ExceptionLog);
            return result;
        }
        #endregion
    }
}
