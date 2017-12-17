using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// True Boolean Check Expression
    /// </summary>
    public class TrueCheckExpression
        : ICheckExpression
    {
        #region Member Variables
        /// <summary>
        /// Is True Expected for this type of expression
        /// </summary>
        private readonly bool isTrueExpected;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TrueCheckExpression"/> class.
        /// </summary>
        /// <param name="trueExpected">if set to <c>true</c> [true expected].</param>
        public TrueCheckExpression(bool trueExpected)
        {
            isTrueExpected = trueExpected;
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
            return isTrueExpected ? "Expression must be true" : "Expression must by false";
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
            return ((obj != null && ((bool)obj)) ? isTrueExpected : !isTrueExpected);
        }
        #endregion
    }
}