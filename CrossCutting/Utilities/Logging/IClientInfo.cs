using System;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Client AbstractInfo Interface
    /// </summary>
    public interface IClientInfo
    {
        /// <summary>
        /// Gets or sets the URL of the client.
        /// </summary>
        /// <value>The URL of the client.</value>
        string Url
        {
            get;
        }

        /// <summary>
        /// Gets the name of the log that the log entry
        /// is to be written under. <seealso cref="ILogEntry"/>
        /// </summary>
        /// <value>The name of the log.</value>
        string LogName
        {
            get;
        }

        /// <summary>
        /// Gets the name of the user that the request for the log entry
        /// was made under.
        /// </summary>
        /// <value>The name of the user.</value>
        string UserName
        {
            get;
        }

        /// <summary>
        /// Gets the name of the machine that the request for the log entry
        /// was made.
        /// </summary>
        /// <value>The name of the machine.</value>
        string MachineName
        {
            get;
        }

        /// <summary>
        /// Gets the ip address of the client.
        /// </summary>
        /// <value>The ip address of the client.</value>
        string IPAddress
        {
            get;
        }
    }
}
