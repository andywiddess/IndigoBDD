using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// ReadOnly Check Expression
    /// </summary>
    public class ReadOnlyCheckExpression
        : ICheckExpression
    {
        #region Member Variables
        /// <summary>
        /// Is ReadOnly Expected for this type of expression
        /// </summary>
        private readonly bool isReadOnlyExpected;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyCheckExpression"/> class.
        /// </summary>
        /// <param name="readOnlyExpected">if set to <c>true</c> [read only expected].</param>
        public ReadOnlyCheckExpression(bool readOnlyExpected)
        {
            isReadOnlyExpected = readOnlyExpected;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return isReadOnlyExpected ? "Expression must be read-only" : "Expression must not be read-only";
        }
        #endregion

        #region ICheckExpression Implementation
        /// <summary>
        /// Validates the specified object
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <returns>bool, true if the expression is valid</returns>
        public bool Validate(object obj)
        {
            return (obj != null && (obj.GetType().Equals(typeof(IEnumerable)) || obj.GetType().Equals(typeof(IList))) && ((IList)obj).IsReadOnly ? isReadOnlyExpected : !isReadOnlyExpected);
        }
        #endregion
    }
}