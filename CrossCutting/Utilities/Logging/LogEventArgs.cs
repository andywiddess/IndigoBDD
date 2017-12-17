using System;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Contains information regarding a logging event.
    /// </summary>
    public class LogEventArgs
        : EventArgs
    {
        #region Members
        /// <summary>
        /// Gets or sets the log entry.
        /// </summary>
        /// <value>The log entry.</value>
        public ILogEntry LogEntry
        {
            get;
            internal set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEventArgs"/> class.
        /// </summary>
        public LogEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEventArgs"/> class.
        /// </summary>
        /// <param name="logEntry">The log entry data.</param>
        public LogEventArgs(ILogEntry logEntry)
        {
            LogEntry = logEntry;
        }
        #endregion
    }
}
