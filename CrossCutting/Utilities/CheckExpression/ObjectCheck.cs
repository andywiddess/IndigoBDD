using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    public class ObjectCheck
    {
        #region Member Variables
        /// <summary>
        /// Object being Checked
        /// </summary>
        private readonly object underCheck;

        /// <summary>
        /// Message
        /// </summary>
        private string message;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCheck"/> class.
        /// </summary>
        /// <param name="objectUnderCheck">The object under check.</param>
        public ObjectCheck(object objectUnderCheck)
        {
            underCheck = objectUnderCheck;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Determines whether [is not null].
        /// </summary>
        public void IsNotNull()
        {
            if (underCheck == null)
                Check.ThrowArgumentNullException(message);
        }

        /// <summary>
        /// Withes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public ObjectCheck WithMessage(string message)
        {
            this.message = message;
            return this;
        }
        #endregion
    }
}
