using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Composite Exception object
    /// </summary>
    [Serializable]
    public class CompositeException 
        : Exception
    {
        #region Member Variables
        /// <summary>
        /// Gets or sets the exceptions.
        /// </summary>
        /// <value>The exceptions.</value>
        public IEnumerable<Exception> Exceptions { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exceptions">The exceptions.</param>
        public CompositeException(string message, IEnumerable<Exception> exceptions)
            : base(message)
        {
            Exceptions = exceptions;
        }
        #endregion
    }
}
