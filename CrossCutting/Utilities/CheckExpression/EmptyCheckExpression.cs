namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    internal class EmptyCheckExpression
        : ICheckExpression
    {
        #region Member Variables
        /// <summary>
        /// Are we expecting the expression to be empty
        /// </summary>
        private readonly bool isEmptyExpected;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyCheckExpression"/> class.
        /// </summary>
        /// <param name="emptyExpected">if set to <c>true</c> [empty expected].</param>
        public EmptyCheckExpression(bool emptyExpected)
        {
            this.isEmptyExpected = emptyExpected;
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
            if (obj == null)
                return isEmptyExpected;

            if (obj is string)
                return (string.IsNullOrEmpty((string)obj) ? isEmptyExpected : !isEmptyExpected);

            if (obj is int)
                return ((int)obj <= 0 ? isEmptyExpected : !isEmptyExpected);

            return !isEmptyExpected;
        }
        #endregion
    }
}