using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// Precondition Exception Implementation
    /// </summary>
    public class PreconditionException
        : CheckException
    {
        #region Constructors
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException()
        {
        }

        /// <summary>
        /// Precondition Exception.
        /// </summary>
        /// <param name="message">The message.</param>
        public PreconditionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Precondition Exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public PreconditionException(string message, Exception inner)
            : base(message, inner)
        {
        }
        #endregion
    }
}
