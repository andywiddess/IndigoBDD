using System;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Represents regular log entries that are not from a e.g. web client,
    /// and that are made within the application.
    /// <seealso cref="IClientLogEntry"/>
    /// </summary>
    public interface IServerLogEntry
        : ILogEntry
    {
        /// <summary>
        /// Gets or sets the exception that is to be logged.
        /// May be null.
        /// </summary>
        /// <value>The exception that occurred.</value>
        Exception Exception
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the app domain that the log request was made.
        /// </summary>
        /// <value>The app domain.</value>
        string AppDomain
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
        string Identity
        {
            get;
            set;
        }
    }
}
