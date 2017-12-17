using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Base interface for all log entries.
    /// </summary>
    public interface ILogEntry
        : IClientInfo
    {
        /// <summary>
        /// Gets the entry id.
        /// </summary>
        /// <value>The entry id.</value>
        Guid EntryId
        {
            get;
        }

        /// <summary>
        /// Gets the log level of the log entry. This is the intended level that the log entry should be written.
        /// This may cause the log entry to be ignored if the current has a higher threshold log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        LogLevelType LogLevel
        {
            get;
        }

        /// <summary>
        /// Gets the message to be logged.
        /// </summary>
        /// <value>The message.</value>
        string Message
        {
            get;
        }

        /// <summary>
        /// Gets the code location of the call to log.
        /// </summary>
        /// <value>The code location.</value>
        CodeLocation CodeLocation
        {
            get;
        }

        /// <summary>
        /// Gets or sets the name of the thread
        /// in which the request to log was made.
        /// </summary>
        /// <value>The name of the thread.</value>
        string ThreadName
        {
            get;
        }

        /// <summary>
        /// Gets or sets the ManagedThreadId of the thread
        /// in which the request to log was made.
        /// </summary>
        /// <value>The name of the thread.</value>
        int ManagedThreadId
        {
            get;
        }

        /// <summary>
        /// Gets the time that the log request was made.
        /// </summary>
        /// <value>The time that the log request was made.</value>
        DateTime OccuredAt
        {
            get;
        }

        /// <summary>
        /// Gets the custom properties.
        /// </summary>
        /// <value>The custom properties.</value>
        IDictionary<string, object> ListOfProperties
        {
            get;
        }
    }
}
