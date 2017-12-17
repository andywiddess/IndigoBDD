using System;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Provides information to allow the recreation
    /// of an <see cref="Exception"/>.
    /// </summary>
    public interface IExceptionLog
    {
        /// <summary>
        /// Gets or sets the name of the <see cref="Type"/> of exception.
        /// </summary>
        /// <value>The name of the type of exception.</value>
        string TypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the message property of the exception.
        /// </summary>
        /// <value>The message.</value>
        string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source property of the exception.
        /// </summary>
        /// <value>The exception Source.</value>
        string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the stack trace of the exception.
        /// </summary>
        /// <value>The exception stack trace.</value>
        string StackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HelpLink of the exception.
        /// </summary>
        /// <value>The exception HelpLink.</value>
        string HelpLink
        {
            get;
            set;
        }
    }
}
