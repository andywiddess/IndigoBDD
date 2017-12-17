using System;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	/// <summary>
	/// Numeric extension methods.
	/// </summary>
	public static class ExtensionsToNumerics
    {

		/// <summary>
		/// Enforce the range for an int value. This will return either the value passed in or 
		/// the minimum value if the value passed in is less than the minimum, or the maximum value
		/// if the value passed in is greater than the maximum value.
		/// </summary>
		/// <param name="value">The value to enforce the range on</param>
		/// <param name="minimum">The minimum value</param>
		/// <param name="maximum">The maximum value</param>
		/// <returns>The value within the range of <c>minimum</c> and <c>maximum</c>.</returns>
		public static int EnforceRange(this int value, int minimum, int maximum)
		{
			if (minimum > maximum)
				throw new ArgumentOutOfRangeException("minimum", "The minimum value must be equal to or less than the maximum value");

			int rtnVal = value;

			if (rtnVal < minimum)
				rtnVal = minimum;
			else if (rtnVal > maximum)
				rtnVal = maximum;

			return rtnVal;
		}

		/// <summary>
		/// Enforce the range for a double value. This will return either the value passed in or 
		/// the minimum value if the value passed in is less than the minimum, or the maximum value
		/// if the value passed in is greater than the maximum value.
		/// </summary>
		/// <param name="value">The value to enforce the range on</param>
		/// <param name="minimum">The minimum value</param>
		/// <param name="maximum">The maximum value</param>
		/// <returns>The value within the range of <c>minimum</c> and <c>maximum</c>.</returns>
		public static double EnforceRange(this double value, double minimum, double maximum)
		{
			if (minimum > maximum)
				throw new ArgumentOutOfRangeException("minimum", "The minimum value must be equal to or less than the maximum value");

			double rtnVal = value;

			if (rtnVal < minimum)
				rtnVal = minimum;
			else if (rtnVal > maximum)
				rtnVal = maximum;

			return rtnVal;
		}

		/// <summary>
		/// Enforce the minimum for an int value. This will return either the value passed in or 
		/// the minimum value if the value passed in is less than the minimum.
		/// </summary>
		/// <param name="value">The value to enforce the minimum on</param>
		/// <param name="minimum">The minimum value</param>
		/// <returns>The value when greater than the <c>minimum</c> or the <c>minimum</c> when the value is less.</returns>
		public static int EnforceMinimum(this int value, int minimum)
		{
			return Math.Max(value, minimum);
		}

		/// <summary>
		/// Enforce the maximum for an int value. This will return either the value passed in or 
		/// the maximum value if the value passed in is greater than the maximum.
		/// </summary>
		/// <param name="value">The value to enforce the maximum on</param>
		/// <param name="maximum">The maximum value</param>
		/// <returns>The value when less than the <c>maximum</c> or the <c>maximum</c> when the value is greater.</returns>
		public static int EnforceMaximum(this int value, int maximum)
		{
			return Math.Min(value, maximum);
		}

		/// <summary>
		/// Enforce the minimum for an double value. This will return either the value passed in or 
		/// the minimum value if the value passed in is less than the minimum.
		/// </summary>
		/// <param name="value">The value to enforce the minimum on</param>
		/// <param name="minimum">The minimum value</param>
		/// <returns>The value when greater than the <c>minimum</c> or the <c>minimum</c> when the value is less.</returns>
		public static double EnforceMinimum(this double value, double minimum)
		{
			return Math.Max(value, minimum);
		}

		/// <summary>
		/// Enforce the maximum for an double value. This will return either the value passed in or 
		/// the maximum value if the value passed in is greater than the maximum.
		/// </summary>
		/// <param name="value">The value to enforce the maximum on</param>
		/// <param name="maximum">The maximum value</param>
		/// <returns>The value when less than the <c>maximum</c> or the <c>maximum</c> when the value is greater.</returns>
		public static double EnforceMaximum(this double value, double maximum)
		{
			return Math.Min(value, maximum);
		}

		/// <summary>Enforces value to be not NaN.</summary>
		/// <param name="value">The value.</param>
		/// <param name="defaultValue">The default value (used when <paramref name="value"/> is NaN).</param>
		/// <returns>Given <paramref name="value"/> or <paramref name="defaultValue"/> if <paramref name="value"/> is NaN)</returns>
		public static double NotNaN(this double value, double defaultValue = 0.0)
		{
			return double.IsNaN(value) ? defaultValue : value;
		}

		/// <summary>Determines whether given value is NaN.</summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if given value is NaN; otherwise, <c>false</c>.</returns>
		public static bool IsNaN(this double value)
		{
			return double.IsNaN(value);
		}
	}
}
