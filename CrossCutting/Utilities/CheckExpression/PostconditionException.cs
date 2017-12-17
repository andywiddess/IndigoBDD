using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// Exception raised when a postcondition fails.
    /// </summary>
    public class PostconditionException
        : CheckException
    {
        #region Constructors
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException()
        {
        }

        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        /// <param name="message">The message.</param>
        public PostconditionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public PostconditionException(string message, Exception inner)
            : base(message, inner)
        {
        }
        #endregion
    }
}
