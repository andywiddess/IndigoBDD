using System;
using System.Diagnostics;
using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Extensions
{
    public static class ExtensionsToDateTime
    {
        private static DateTime _origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Gets the current DateTime and adjusts it by the specified TimeSpan
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static DateTime FromNow(this TimeSpan span)
        {
            return SystemUtil.Now + span;
        }

        /// <summary>
        /// A TimeSpan extension method that initialises this object from the given from UTC now.
        /// </summary>
        ///
        /// <param name="span"> . </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime FromUtcNow(this TimeSpan span)
        {
            return SystemUtil.UtcNow + span;
        }

        /// <summary>
        /// A DateTime extension method that firsts the given value.
        /// </summary>
        ///
        /// <param name="value">    The value to act on. </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime First(this DateTime value)
        {
            return value.AddDays(1 - value.Day);
        }

        /// <summary>
        /// A DateTime extension method that firsts the given value.
        /// </summary>
        ///
        /// <param name="value">        The value to act on. </param>
        /// <param name="dayOfWeek">    The day of week. </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime First(this DateTime value, DayOfWeek dayOfWeek)
        {
            DateTime first = value.First();

            if (first.DayOfWeek != dayOfWeek)
                first = first.Next(dayOfWeek);

            return first;
        }

        /// <summary>
        /// A DateTime extension method that lasts the given value.
        /// </summary>
        ///
        /// <param name="value">    The value to act on. </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime Last(this DateTime value)
        {
            int daysInMonth = DateTime.DaysInMonth(value.Year, value.Month);

            return value.AddDays(1 - value.Day + daysInMonth - 1);
        }

        /// <summary>
        /// A DateTime extension method that lasts the given value.
        /// </summary>
        ///
        /// <param name="value">        The value to act on. </param>
        /// <param name="dayOfWeek">    The day of week. </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime Last(this DateTime value, DayOfWeek dayOfWeek)
        {
            DateTime last = value.Last();

            return last.AddDays(Math.Abs(dayOfWeek - last.DayOfWeek)*-1);
        }

        /// <summary>
        /// A DateTime extension method that move to the next item in the collection.
        /// </summary>
        ///
        /// <param name="value">        The value to act on. </param>
        /// <param name="dayOfWeek">    The day of week. </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime Next(this DateTime value, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - value.DayOfWeek;

            if (offsetDays <= 0)
                offsetDays += 7;

            return value.AddDays(offsetDays);
        }

        /// <summary>
        /// A DateTime extension method that midnights the given value.
        /// </summary>
        ///
        /// <param name="value">    The value to act on. </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime Midnight(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day);
        }

        /// <summary>
        /// A DateTime extension method that noons the given value.
        /// </summary>
        ///
        /// <param name="value">    The value to act on. </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime Noon(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 12, 0, 0);
        }

        /// <summary>
        /// A DateTime extension method that sets a time.
        /// </summary>
        ///
        /// <param name="value">    The value to act on. </param>
        /// <param name="hour">     The hour. </param>
        /// <param name="minute">   The minute. </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime SetTime(this DateTime value, int hour, int minute)
        {
            return SetTime(value, hour, minute, 0, 0);
        }

        /// <summary>
        /// A DateTime extension method that sets a time.
        /// </summary>
        ///
        /// <param name="value">    The value to act on. </param>
        /// <param name="hour">     The hour. </param>
        /// <param name="minute">   The minute. </param>
        /// <param name="second">   The second. </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime SetTime(this DateTime value, int hour, int minute, int second)
        {
            return SetTime(value, hour, minute, second, 0);
        }

        /// <summary>
        /// A DateTime extension method that sets a time.
        /// </summary>
        ///
        /// <param name="value">        The value to act on. </param>
        /// <param name="hour">         The hour. </param>
        /// <param name="minute">       The minute. </param>
        /// <param name="second">       The second. </param>
        /// <param name="millisecond">  The millisecond. </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime SetTime(this DateTime value, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(value.Year, value.Month, value.Day, hour, minute, second, millisecond);
        }

        /// <summary>
        /// A DateTime extension method that force UTC.
        /// </summary>
        ///
        /// <param name="value">    The value to act on. </param>
        ///
        /// <returns>
        /// A DateTime.
        /// </returns>
        public static DateTime ForceUtc(this DateTime value)
        {
            if (value.Kind == DateTimeKind.Utc)
                return value;

            return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, DateTimeKind.Utc);
        }


        /// <summary>
        /// Gets a Unix Timestamp from a DateTime, converting local time to UTC if necessary.
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns></returns>
        public static int ToUnixTimestamp(this DateTime date)
        {
            TimeSpan difference = ((date.Kind == DateTimeKind.Local) ? date.ToUniversalTime() : date) - _origin;

            return (int) Math.Floor(difference.TotalSeconds);
        }

        public static DateTime ConvertUnixTimestamp(int timestamp)
        {
            return _origin.AddSeconds(timestamp);
        }

        /// <summary>
        /// Find the Maximum of two DateTime.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="other">The other.</param>
        /// <returns>The maximum DateTime</returns>
        public static DateTime Max(this DateTime item, DateTime other)
        {
            if (item > other)
                return item;
            else
                return other;
        }

        /// <summary>
        /// Find the Minimum of two DateTime
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="other">The other.</param>
        /// <returns>The minimum DateTime</returns>
        public static DateTime Min(this DateTime item, DateTime other)
        {
            if (item < other)
                return item;
            else
                return other;
        }

        /// <summary>Creates a scope over stopwatch. Starts a timer and stops it when 
        /// scope is disposed.</summary>
        /// <param name="stopwatch">The stopwatch.</param>
        /// <returns>IDisposable scope.</returns>
        public static IDisposable Scope(this Stopwatch stopwatch)
        {
            if (stopwatch == null)
                return EmptyDisposable.Default;

            var result = Patterns.Scope(() => stopwatch.Stop());
            stopwatch.Start();
            return result;
        }
    }
    
}