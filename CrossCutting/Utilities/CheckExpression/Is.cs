namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// Is Predicate
    /// </summary>
    public static class Is
    {
        #region Properties
        /// <summary>
        /// Gets the not.
        /// </summary>
        /// <value>The not.</value>
        public static CheckConstraint Not
        {
            get
            {
                return NewConstraint(false);
            }
        }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public static ICheckExpression Empty
        {
            get
            {
                return new CheckConstraint(true).Empty;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// News the constraint.
        /// </summary>
        /// <param name="isTrue">if set to <c>true</c> [is true].</param>
        /// <returns></returns>
        private static CheckConstraint NewConstraint(bool isTrue)
        {
            return new CheckConstraint(isTrue);
        }
        #endregion
    }
}