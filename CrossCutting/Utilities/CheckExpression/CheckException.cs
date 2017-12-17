using System;

namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// Check Exception
    /// </summary>
    public class CheckException
        : Exception
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckException"/> class.
        /// </summary>
        protected CheckException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CheckException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        protected CheckException(string message, Exception inner)
            : base(message, inner)
        {
        }
        #endregion
    }
}