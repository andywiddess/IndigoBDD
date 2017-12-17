using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// The Connection is not Empty Expression
    /// </summary>
    public class CollectionEmptyCheckConstraint
        : ICheckExpression
    {
        #region Member Variables
        /// <summary>
        /// Is Count == 0 Expected for this type of expression
        /// </summary>
        private readonly bool isCountZeroExpected;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NullCheckExpression"/> class.
        /// </summary>
        /// <param name="isCountZeroExpected">if set to <c>true</c> [is count zero expected].</param>
        public CollectionEmptyCheckConstraint(bool isCountZeroExpected)
        {
            this.isCountZeroExpected = isCountZeroExpected;
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
            return isCountZeroExpected ? "Expression Collection must be empty" : "Expression Collection must not be empty";
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
            return ((obj == null) ? isCountZeroExpected : !isCountZeroExpected);
        }

        /// <summary>
        /// Validates the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public bool Validate(IList obj)
        {
            return ((obj == null || obj.Count == 0) ? isCountZeroExpected : !isCountZeroExpected);
        }
        #endregion
    }
}