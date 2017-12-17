using System;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Utility class for validating method parameters.
    /// </summary>
    public static class ArgumentValidator
    {
        #region Static Methods
        /// <summary>
        /// Ensures the specified value is not null.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to test.</param>
        /// <param name="parameterName">StateName of the parameter.</param>
        /// <returns>The specified value.</returns>
        /// <exception cref="ArgumentNullException">Occurs if the specified value
        /// is <code>null</code>.</exception>
        public static T AssertNotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);

            return value;
        }

        /// <summary>
        /// Ensures the specified value is not <code>null</code> or empty (a zero length string).
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="parameterName">StateName of the parameter.</param>
        /// <returns>The specified value.</returns>
        /// <exception cref="ArgumentNullException">Occurs if the specified value 
        /// is <code>null</code> or empty (a zero length string).</exception>
        public static string AssertNotNullOrEmpty(string value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);

            if (value.Length < 1)
                throw new ArgumentException("Parameter should not be an empty string.", parameterName); // TODO: Make localisable resource.

            return value;
        }

        /// <summary>
        /// Ensures the specified value is not <code>null</code>
        /// and that it is of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to test.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The value to test.</returns>
        /// <exception cref="ArgumentNullException">Occurs if the specified value
        /// is <code>null</code> or of type not assignable from the specified type.</exception>
        /// <example>
        /// public DoSomething(object message)
        /// {
        /// this.message = ArgumentValidator.AssertNotNullAndOfType&lt;string&gt;(message, "message");
        /// }
        /// </example>
        public static T AssertNotNullAndOfType<T>(object value, string parameterName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);

            var result = value as T;
            if (result == null)
                throw new ArgumentException(string.Format("Expected argument of type " + typeof(T) + ", but was " + value.GetType(), typeof(T), value.GetType()),parameterName);

            return result;
        }

        /// <summary>
        /// Asserts the greater than.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="greaterThan">The greater than.</param>
        /// <param name="parameterName">StateName of the parameter.</param>
        /// <returns></returns>
        public static int AssertGreaterThan(int value, int greaterThan, string parameterName)
        {
            if (value < greaterThan)
                throw new ArgumentOutOfRangeException("Parameter should be greater than " + greaterThan, parameterName); // TODO: Make localisable resource.

            return value;
        }

        /// <summary>
        /// Asserts the greater than.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mustBeGreaterThan">The must be greater than.</param>
        /// <param name="parameterName">StateName of the parameter.</param>
        /// <returns></returns>
        public static double AssertGreaterThan(double value, double mustBeGreaterThan, string parameterName)
        {
            if (value < mustBeGreaterThan)
                throw new ArgumentOutOfRangeException("Parameter should be greater than " + mustBeGreaterThan, parameterName); // TODO: Make localisable resource.

            return value;
        }

        /// <summary>
        /// Asserts the less than.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mustBeLessThan">The must be less than.</param>
        /// <param name="parameterName">StateName of the parameter.</param>
        /// <returns></returns>
        public static double AssertLessThan(double value, double mustBeLessThan, string parameterName)
        {
            if (value > mustBeLessThan)
                throw new ArgumentOutOfRangeException("Parameter should be less than " + mustBeLessThan, parameterName); // TODO: Make localisable resource.

            return value;
        }

        /// <summary>
        /// Asserts the greater than.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="mustBeGreaterThan">The must be greater than.</param>
        /// <param name="parameterName">StateName of the parameter.</param>
        /// <returns></returns>
        public static long AssertGreaterThan(long value, long mustBeGreaterThan, string parameterName)
        {
            if (value < mustBeGreaterThan)
                throw new ArgumentOutOfRangeException("Parameter should be greater than " + mustBeGreaterThan, parameterName); // TODO: Make localisable resource.
            
            return value;
        }
        #endregion
    }
}

