namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// Check Expression
    /// </summary>
    public class CheckExpression
    {
        #region Properties
        /// <summary>
        /// create a new null expression
        /// </summary>
        /// <value>The null expression.</value>
        public ICheckExpression Null
        {
            get
            {
                return new NullCheckExpression(false);
            }
        }

        /// <summary>
        /// Gets the read only.
        /// </summary>
        /// <value>The read only.</value>
        public ICheckExpression ReadOnly
        {
            get
            {
                return new ReadOnlyCheckExpression(false);
            }
        }

        /// <summary>
        /// Create a new List Is Empty expression
        /// </summary>
        public ICheckExpression ListIsEmpty
        {
            get
            {
                return new CollectionEmptyCheckConstraint(false);
            }
        }

        /// <summary>
        /// create a new empty expression
        /// </summary>
        /// <value>The empty expression.</value>
        public ICheckExpression Empty
        {
            get
            {
                return new EmptyCheckExpression(false);
            }
        }
        #endregion
    }
}