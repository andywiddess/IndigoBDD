using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// String Checks
    /// </summary>
    public class StringCheck
    {
        #region Member Variables
        /// <summary>
        /// String being checked
        /// </summary>
        private readonly string underCheck;

        /// <summary>
        /// Message
        /// </summary>
        private string message;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StringCheck"/> class.
        /// </summary>
        /// <param name="stringUnderCheck">The string under check.</param>
        public StringCheck(string stringUnderCheck)
        {
            underCheck = stringUnderCheck;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Determines whether [is not null or empty].
        /// </summary>
        public void IsNotNullOrEmpty()
        {
            if (string.IsNullOrEmpty(underCheck))
                Check.ThrowArgumentNullException(message);
        }

        /// <summary>
        /// Withes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public StringCheck WithMessage(string message)
        {
            this.message = message;
            return this;
        }
        #endregion
    }
}