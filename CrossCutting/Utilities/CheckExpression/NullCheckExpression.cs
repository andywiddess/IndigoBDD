namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// Null Check Expression
    /// </summary>
    public class NullCheckExpression
        : ICheckExpression
    {
        #region Member Variables
        /// <summary>
        /// Is Null Expected for this type of expression
        /// </summary>
        private readonly bool isNullExpected;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NullCheckExpression"/> class.
        /// </summary>
        /// <param name="nullExpected">if set to <c>true</c> [null expected].</param>
        public NullCheckExpression(bool nullExpected)
        {
            isNullExpected = nullExpected;
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
            return isNullExpected ? "Expression must be null" : "Expression must not be null";
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
            return ((obj == null) ? isNullExpected : !isNullExpected);
        }
        #endregion
    }
}