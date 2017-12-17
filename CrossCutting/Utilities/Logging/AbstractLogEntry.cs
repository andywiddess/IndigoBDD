using System;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Log Entry Abstraction
    /// </summary>
    [DataContract(Namespace = "http://www.sepura.co.uk/Logging/2014/07/"),
     Serializable]
    public abstract class AbstractLogEntry
        : LogEntryData
    {
        #region Members
        /// <summary>
        /// Gets or sets the principal identity.
        /// </summary>
        /// <value>The principal identity.</value>
        [DataMember]
        public string PrincipalIdentity
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLogEntry"/> class.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        protected AbstractLogEntry(LogLevelType logLevel, string message)
        {
            LogLevel = logLevel;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLogEntry"/> class.
        /// </summary>
        /// <param name="logEntryData">The log entry data.</param>
        protected AbstractLogEntry(LogEntryData logEntryData)
            : base(logEntryData)
        {
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns a <see cref="T:System.String"/>
        /// that represents the current <see cref="AbstractLogEntry"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="AbstractLogEntry"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, PrincipalIdentity: {1}", base.ToString(), PrincipalIdentity);
        }
        #endregion
    }
}
