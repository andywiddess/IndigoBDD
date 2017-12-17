using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepura.Apps.KPIReporting.CrossCutting.Utilities
{
    /// <summary>
    /// SSO Authentication Failed
    /// </summary>
    public class SSOAuthenticationFailedException
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
        /// Initializes a new instance of the <see cref="SSOAuthenticationFailedException"/> class.
        /// </summary>
        /// <param name="developerMessage">The developer message that may contain
        /// detailed information regarding the exception.</param>
        /// <param name="userMessage">The localized user message that is appropriate
        /// for displaying to a user. Can be <code>null</code>.</param>
        public SSOAuthenticationFailedException(string developerMessage, string userMessage)
            : base(developerMessage)
        {
            UserMessage = userMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SSOAuthenticationFailedException"/> class.
        /// </summary>
        /// <param name="developerMessage">The developer message that may contain
        /// detailed information regarding the exception.</param>
        /// <param name="userMessage">The localized user message that is appropriate
        /// for displaying to a user. Can be <code>null</code>.</param>
        /// <param name="innerException">The inner exception.</param>
        public SSOAuthenticationFailedException(string developerMessage, string userMessage, Exception innerException)
            : base(developerMessage, innerException)
        {
            UserMessage = userMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SSOAuthenticationFailedException"/> class.
        /// </summary>
        /// <param name="developerMessage">The developer message that may contain
        /// detailed information regarding the exception.</param>
        public SSOAuthenticationFailedException(string developerMessage)
            : base(developerMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SSOAuthenticationFailedException"/> class.
        /// </summary>
        /// <param name="developerMessage">The developer message that may contain
        /// detailed information regarding the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public SSOAuthenticationFailedException(string developerMessage, Exception innerException)
            : base(developerMessage, innerException)
        {
        }
        #endregion
    }
}
