using System;
using System.Diagnostics;

namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// Check class for verifying the condition of items included in interface contracts
    /// </summary>
    public static class Check
    {
        #region Member Variables
        /// <summary>
        /// Do we wish to use Exceotions when performing check conditions
        /// </summary>
        private static bool useExceptions = true;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether [use exceptions].
        /// </summary>
        /// <value><c>true</c> if [use exceptions]; otherwise, <c>false</c>.</value>
        public static bool UseExceptions
        {
            get
            {
                return useExceptions;
            }
            set
            {
                useExceptions = value;
            }
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Parameters the specified object under check.
        /// </summary>
        /// <param name="objectUnderCheck">The object under check.</param>
        /// <returns></returns>
        public static ObjectCheck Parameter(object objectUnderCheck)
        {
            return new ObjectCheck(objectUnderCheck);
        }

        /// <summary>
        /// Parameters the specified string under check.
        /// </summary>
        /// <param name="stringUnderCheck">The string under check.</param>
        /// <returns></returns>
        public static StringCheck Parameter(string stringUnderCheck)
        {
            return new StringCheck(stringUnderCheck);
        }

        /// <summary>
        /// Throws the argument null exception.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void ThrowArgumentNullException(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();
            else
                throw new ArgumentNullException("", message);
        }

        /// <summary>
        /// Requires the specified assertion.
        /// </summary>
        /// <param name="assertion">if set to <c>true</c> [assertion].</param>
        /// <param name="message">The message.</param>
        public static void Require(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new PreconditionException(message);
            }
            else
            {
                Trace.Assert(assertion, "Precondition: " + message);
            }
        }

        /// <summary>
        /// Ensures the specified assertion.
        /// </summary>
        /// <param name="assertion">if set to <c>true</c> [assertion].</param>
        /// <param name="message">The message.</param>
        public static void Ensure(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new PostconditionException(message);
            }
            else
            {
                Trace.Assert(assertion, "Postcondition: " + message);
            }
        }

        /// <summary>
        /// Thats the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="expression">The expression.</param>
        public static void That(object obj, ICheckExpression expression)
        {
            if (!expression.Validate(obj))
                throw new CheckException(string.Format("Expectation Not Met:{0}", expression));
        }

        /// <summary>
        /// Thats the specified obj.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="expression">The expression.</param>
        public static void That<TException>(object obj, ICheckExpression expression) where TException : Exception
        {
            if (!expression.Validate(obj))
                throw (TException)Activator.CreateInstance(typeof(TException), "Expectation Not Met: " + expression);
        }

        /// <summary>
        /// Arguments the specified param name.
        /// </summary>
        /// <param name="paramName">StateName of the param.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="expression">The expression.</param>
        public static void Argument(string paramName, object obj, ICheckExpression expression)
        {
            if (!expression.Validate(obj))
            {
                if (expression is NullCheckExpression)
                    throw new ArgumentNullException(paramName, expression.ToString());
                else
                    throw new ArgumentException(expression.ToString(), paramName);
            }
        }
        #endregion
    }
}