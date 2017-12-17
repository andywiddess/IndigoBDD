using System;
using Indigo.CrossCutting.Utilities.Extensions;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Guard Against Class
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Againsts the null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AgainstNull<T>(T value)
            where T : class
        {
            if (value == null)
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Againsts the null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AgainstNull<T>(T value, string paramName)
            where T : class
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Againsts the null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AgainstNull<T>(T value, string paramName, string message)
            where T : class
        {
            if (value == null)
                throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Againsts the null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AgainstNull<T>(T? value)
            where T : struct
        {
            if (!value.HasValue)
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Againsts the null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AgainstNull<T>(T? value, string paramName)
            where T : struct
        {
            if (!value.HasValue)
                throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Againsts the null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AgainstNull<T>(T? value, string paramName, string message)
            where T : struct
        {
            if (!value.HasValue)
                throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Againsts the null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AgainstNull(string value)
        {
            if (value.IsNull())
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Againsts the null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AgainstNull(string value, string paramName)
        {
            if (value.IsNull())
                throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Againsts the null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AgainstNull(string value, string paramName, string message)
        {
            if (value.IsNull())
                throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Againsts the empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentException">string value must not be empty</exception>
        public static void AgainstEmpty(string value)
        {
            if (value.IsEmpty())
                throw new ArgumentException("string value must not be empty");
        }

        /// <summary>
        /// Againsts the empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentException">string value must not be empty</exception>
        public static void AgainstEmpty(string value, string paramName)
        {
            if (value.IsEmpty())
                throw new ArgumentException("string value must not be empty", paramName);
        }

        /// <summary>
        /// Againsts the empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void AgainstEmpty(string value, string paramName, string message)
        {
            if (value.IsEmpty())
                throw new ArgumentException(message, paramName);
        }

        /// <summary>
        /// Greaters the than.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lowerLimit">The lower limit.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void GreaterThan<T>(T lowerLimit, T value)
            where T : IComparable<T>
        {
            if (value.CompareTo(lowerLimit) <= 0)
                throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Greaters the than.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lowerLimit">The lower limit.</param>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void GreaterThan<T>(T lowerLimit, T value, string paramName)
            where T : IComparable<T>
        {
            if (value.CompareTo(lowerLimit) <= 0)
                throw new ArgumentOutOfRangeException(paramName);
        }

        /// <summary>
        /// Greaters the than.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lowerLimit">The lower limit.</param>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void GreaterThan<T>(T lowerLimit, T value, string paramName, string message)
            where T : IComparable<T>
        {
            if (value.CompareTo(lowerLimit) <= 0)
                throw new ArgumentOutOfRangeException(paramName, message);
        }


        /// <summary>
        /// Lesses the than.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="upperLimit">The upper limit.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void LessThan<T>(T upperLimit, T value)
            where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) >= 0)
                throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Lesses the than.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="upperLimit">The upper limit.</param>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void LessThan<T>(T upperLimit, T value, string paramName)
            where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) >= 0)
                throw new ArgumentOutOfRangeException(paramName);
        }

        /// <summary>
        /// Lesses the than.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="upperLimit">The upper limit.</param>
        /// <param name="value">The value.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void LessThan<T>(T upperLimit, T value, string paramName, string message)
            where T : IComparable<T>
        {
            if (value.CompareTo(upperLimit) >= 0)
                throw new ArgumentOutOfRangeException(paramName, message);
        }

        /// <summary>
        /// Determines whether the specified condition is true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">The condition.</param>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentException">condition was not true</exception>
        public static void IsTrue<T>(Func<T, bool> condition, T target)
        {
            if (!condition(target))
                throw new ArgumentException("condition was not true");
        }

        /// <summary>
        /// Determines whether the specified condition is true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">The condition.</param>
        /// <param name="target">The target.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentException">condition was not true</exception>
        public static void IsTrue<T>(Func<T, bool> condition, T target, string paramName)
        {
            if (!condition(target))
                throw new ArgumentException("condition was not true", paramName);
        }

        /// <summary>
        /// Determines whether the specified condition is true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">The condition.</param>
        /// <param name="target">The target.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void IsTrue<T>(Func<T, bool> condition, T target, string paramName, string message)
        {
            if (!condition(target))
                throw new ArgumentException(message, paramName);
        }


        /// <summary>
        /// Determines whether [is type of] [the specified object].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">{0} is not an instance of type {1}.FormatWith(obj.GetType().Name, typeof (T).Name)</exception>
        public static T IsTypeOf<T>(object obj)
        {
            AgainstNull(obj);

            if(obj is T)
                return (T) obj;

            throw new ArgumentException("{0} is not an instance of type {1}".FormatWith(obj.GetType().Name, typeof (T).Name));
        }

    }
}