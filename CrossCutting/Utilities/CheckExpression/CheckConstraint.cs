using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// Check Constraint
    /// </summary>
    public class CheckConstraint
    {
        #region Member Variables
        /// <summary>
        /// Is the constraint valid
        /// </summary>
        private readonly bool isTrue;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckConstraint"/> class.
        /// </summary>
        /// <param name="isTrue">if set to <c>true</c> [is true].</param>
        public CheckConstraint(bool isTrue)
        {
            this.isTrue = isTrue;
        }
        #endregion

        #region Properties
        /// <summary>
        /// creates a null expression.
        /// </summary>
        /// <value>The null.</value>
        public ICheckExpression Null
        {
            get
            {
                return new NullCheckExpression(isTrue);
            }
        }

        /// <summary>
        /// Gets the read only status.
        /// </summary>
        /// <value>The read only.</value>
        public ICheckExpression ReadOnly
        {
            get
            {
                return new ReadOnlyCheckExpression(isTrue);
            }
        }

        /// <summary>
        /// Gets the status of the list
        /// </summary>
        /// <value>The status of the list.</value>
        public ICheckExpression ListIsEmpty
        {
            get
            {
                return new CollectionEmptyCheckConstraint(isTrue);
            }
        }

        /// <summary>
        /// creates an empty expression
        /// </summary>
        /// <value>The empty.</value>
        public ICheckExpression Empty
        {
            get
            {
                return new EmptyCheckExpression(isTrue);
            }
        }

        /// <summary>
        /// creates a boolean
        /// </summary>
        /// <value>Boolean value, true</value>
        public ICheckExpression True
        {
            get
            {
                return new TrueCheckExpression(isTrue);
            }
        }
        #endregion
    }
}
