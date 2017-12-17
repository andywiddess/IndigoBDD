#region Header
// --------------------------------------------------------------------------------------
// Sepura.IO.Text.StringUtils.cs
// --------------------------------------------------------------------------------------
// 
// Core string manipulation functions
//
// Copyright (c) 2008 Sepura Plc
//
// Sepura Confidential (c)
//
// Created: August 2008 : Milosz Krajewski and Simon Hirst
// 
//
// --------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Reflection;
using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Text
{
	/// <summary>
	/// Core string manipulation functions
	/// </summary>
	public class StringUtils
	{
		/// <summary>
		/// IFormatProvider for numbers, which does not use locale. It uses no currency symbol,
		/// it uses '.' as decimal place, and no group separator.
		/// </summary>
		public static readonly NumberFormatInfo RawNumberFormatProvider = new NumberFormatInfo();

		/// <summary>
		/// Initializes the <see cref="StringUtils"/> class.
		/// Sets <see cref="RawNumberFormatProvider"/> up.
		/// </summary>
		static StringUtils()
		{
			RawNumberFormatProvider.CurrencyDecimalSeparator = ".";
			RawNumberFormatProvider.CurrencyGroupSeparator = "";
			RawNumberFormatProvider.CurrencyNegativePattern = 5; // -n$
			RawNumberFormatProvider.CurrencyPositivePattern = 1; // n$
			RawNumberFormatProvider.CurrencySymbol = "";
			RawNumberFormatProvider.DigitSubstitution = DigitShapes.None;
			RawNumberFormatProvider.NegativeSign = "-";
			RawNumberFormatProvider.NumberDecimalSeparator = ".";
			RawNumberFormatProvider.NumberGroupSeparator = "";
			RawNumberFormatProvider.NumberNegativePattern = 1;
			RawNumberFormatProvider.PositiveSign = "+";
		}

		/// <summary>
		/// Slices the specified string. 
		/// </summary>
		/// <param name="value">The string.</param>
		/// <param name="index">The starting index.</param>
		/// <param name="length">The maximum length.</param>
		/// <returns>Sliced string.</returns>
		public static string Slice(string value, int index, int length)
		{
			if (value != null)
			{
				int currentLength = value.Length;
				index = Math.Min(Math.Max(index, 0), currentLength);
				length = Math.Min(currentLength - index, length);
				return value.Substring(index, length);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Returns string containing given number of spaces.
		/// </summary>
		/// <param name="length">The length.</param>
		/// <returns>String containing given number of spaces</returns>
		public static string Spaces(int length)
		{
			if (length <= 0)
            {
                return string.Empty;
            }

            return length == 1 ? " " : new string(' ', length);
		}

		/// <summary>
		/// Pads the specified text filling it with spaces. 
		/// If <paramref name="head"/> is <c>true</c> spaces will be added before text. 
		/// If <paramref name="tail"/> is <c>true</c> spaces will be added after text.
		/// if both of these parameters are <c>true</c> spaces will be added before and 
		/// after text, effectively centering text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="length">The length.</param>
		/// <param name="head">if set to <c>true</c> spaces will be added before text.</param>
		/// <param name="tail">if set to <c>true</c> spaces will be added after text.</param>
		/// <returns></returns>
		public static string Pad(string text, int length, bool head, bool tail)
		{
			length = length - text.Length;
			if (length <= 0) return text;

			int headSpaces = 0;
			int tailSpaces = 0;

			if (head && tail)
			{
				headSpaces = length / 2;
				tailSpaces = length - headSpaces;
			}
			else if (head)
			{
				headSpaces = length;
			}
			else if (tail)
			{
				tailSpaces = length;
			}

			return string.Format("{0}{1}{2}", Spaces(headSpaces), text, Spaces(tailSpaces));
		}

		/// <summary>
		/// Trims the specified text. If given text is <c>null</c> returns <c>null</c>.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>Trimmed text or <c>null</c></returns>
		public static string Trim(string text)
		{
		    return text == null ? null : text.Trim();
		}

		/// <summary>
		/// Ensures that string is not <c>null</c>. Returns <c>string.Empty</c> in such cases.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>Given text or empty string</returns>
		public static string NotNull(string text)
		{
			return NotNull(text, string.Empty);
		}

		/// <summary>
		/// Ensures that string is not <c>null</c>. Returns <paramref name="defaultText"/> in such cases.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="defaultText">The default text.</param>
		/// <returns>Given text or <paramref name="defaultText"/></returns>
		public static string NotNull(string text, string defaultText)
		{
		    return text ?? defaultText;
		}

		/// <summary>
		/// Ensures that string is not <c>null</c> nor empty. Returns <paramref name="defaultText"/> in such cases.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="defaultText">The default text.</param>
		/// <returns>Given text or <paramref name="defaultText"/></returns>
		public static string NotEmpty(string text, string defaultText)
		{
			if (string.IsNullOrEmpty(text))
			{
				return defaultText;
			}
			return text;
		}

		/// <summary>
		/// Determines whether the string is null or empty, before or after a trim to remove any spaces.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>
		/// 	<c>true</c> if is null or empty before or after a trim; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrEmptyTrim(string text)
		{
			return string.IsNullOrEmpty(text) || text.Trim().Length == 0;
		}

		#region ConvertTo helpers

		[DebuggerStepThrough]
		private static T ConvertTo_Cast<T>(object v)
		{
			return (T)v;
		}

		[DebuggerStepThrough]
		private static T ConvertTo_ChangeType<T>(object v)
		{
			return (T)Convert.ChangeType(v, typeof(T));
		}

		[DebuggerStepThrough]
		private static T ConvertTo_IntToEnum<T>(object v)
		{
			return (T)(object)ConvertTo<int>(v);
		}

		[DebuggerStepThrough]
		private static T ConvertTo_IntToBool<T>(object v)
		{
			return (T)(object)(ConvertTo<int>(v) != 0);
		}

		[DebuggerStepThrough]
		private static T ConvertTo_StringToEnum<T>(object v)
		{
			return (T)Enum.Parse(typeof(T), ConvertTo<string>(v));
		}

		[DebuggerStepThrough]
		private static T ConvertTo_StringToBool<T>(object v)
		{
			return (T)(object)bool.Parse(ConvertTo<string>(v));
		}

		[DebuggerStepThrough]
		private static T ConvertTo_AnyToString<T>(object v)
		{
			return (T)(object)v.ToString();
		}

		[DebuggerStepThrough]
		private static T ConvertTo_IntParse<T>(object v)
		{
			return (T)(object)int.Parse(ConvertTo<string>(v));
		}

		[DebuggerStepThrough]
		private static T ConvertTo_LongParse<T>(object v)
		{
			return (T)(object)long.Parse(ConvertTo<string>(v), CultureInfo.InvariantCulture);
		}

		[DebuggerStepThrough]
		private static T ConvertTo_DoubleParse<T>(object v)
		{
			return (T)(object)double.Parse(ConvertTo<string>(v), CultureInfo.InvariantCulture);
		}

		[DebuggerStepThrough]
		private static T ConvertTo_DecimalParse<T>(object v)
		{
			return (T)(object)decimal.Parse(ConvertTo<string>(v), CultureInfo.InvariantCulture);
		}


		#endregion

		/// <summary>Converts to given type. Note, this function has very limited functionality, it handles 
		/// enum, int, bool, strings, double and decimal only.</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns>Converted value.</returns>
		[DebuggerStepThrough]
		public static T ConvertTo<T>(object value)
		{
			Type typeofT = typeof(T);

			// IMP:MAK this method got messy, it will needs to be improved
			// it does what it should do, but we need some pretty 'extension method' based
			// solution
			if (value == null && !typeofT.IsValueType)
			{
				return default(T);
			}
			else if (value != null && typeofT.IsAssignableFrom(value.GetType()))
			{
				return (T)value;
			}
			else if (typeofT.IsEnum)
			{
				return Patterns.SafeConvert<object, T>(
					value,
					ConvertTo_Cast<T>,
					ConvertTo_ChangeType<T>,
					ConvertTo_IntToEnum<T>,
					ConvertTo_StringToEnum<T>);
			}
			else if (typeofT == typeof(bool))
			{
				return Patterns.SafeConvert<object, T>(
					value,
					ConvertTo_Cast<T>,
					ConvertTo_ChangeType<T>,
					ConvertTo_IntToBool<T>,
					ConvertTo_StringToBool<T>);
			}
			else if (typeofT == typeof(string))
			{
				return Patterns.SafeConvert<object, T>(
					value,
					ConvertTo_Cast<T>,
					ConvertTo_ChangeType<T>,
					ConvertTo_AnyToString<T>);
			}
			else if (typeofT == typeof(long))
			{
				return Patterns.SafeConvert<object, T>(
					value,
					ConvertTo_Cast<T>,
					ConvertTo_ChangeType<T>,
					ConvertTo_LongParse<T>);
			}
			else if (typeofT == typeof(int))
			{
				return Patterns.SafeConvert<object, T>(
					value,
					ConvertTo_Cast<T>,
					ConvertTo_ChangeType<T>,
					ConvertTo_IntParse<T>);
			}
			else if (typeofT == typeof(double))
			{
				return Patterns.SafeConvert<object, T>(
					value,
					ConvertTo_Cast<T>,
					ConvertTo_ChangeType<T>,
					ConvertTo_DoubleParse<T>);
			}
			else if (typeofT == typeof(decimal))
			{
				return Patterns.SafeConvert<object, T>(
					value,
					ConvertTo_Cast<T>,
					ConvertTo_ChangeType<T>,
					ConvertTo_DecimalParse<T>);
			}
			else
			{
				return Patterns.SafeConvert<object, T>(
					value,
					ConvertTo_Cast<T>,
					ConvertTo_ChangeType<T>);
			}
		}

		/// <summary>
		/// Converts value to given type. If <paramref name="value"/> 
		/// is <c>null</c> or <c>DBNull</c> returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <typeparam name="T">Output type.</typeparam>
		/// <param name="value">The value.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Converted value.</returns>
		public static T NotNull<T>(object value, T defaultValue)
		{
			if (value == null)
            {
                return defaultValue;
            }

            if (value is DBNull) return defaultValue;
			return ConvertTo<T>(value);
		}

		/// <summary>
		/// Converts value to given type. If <paramref name="value"/> 
		/// is <c>null</c> or <c>DBNull</c> or an empty string returns <paramref name="defaultValue"/>.
		/// </summary>
		/// <typeparam name="T">Output type.</typeparam>
		/// <param name="value">The value.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Converted value.</returns>
		public static T NotEmpty<T>(object value, T defaultValue)
		{
			if (value is DBNull) return defaultValue;
			if (string.IsNullOrEmpty(value as string)) return defaultValue;

			return ConvertTo<T>(value);
		}

        /// <summary>
        /// Database version of Trim(). Allows to specify maximum length of the field.
        /// Return empty string if text is <c>null</c>.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="length">The maximum length.</param>
        /// <returns>Sliced and trimmed text; returns empty string if <paramref name="text"/> is <c>null</c></returns>
        public static string DbTrim(string text, int length) => DbTrim(text, length, false);

        /// <summary>
        /// Database version of Trim(). Allows to specify maximum length of the field.
        /// If <paramref name="text"/> is <c>null</c> and <paramref name="allowNull"/> 
        /// is <c>true</c> returns <c>null</c>, empty string otherwise.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="length">The maximum length.</param>
        /// <param name="allowNull">if set to <c>true</c> may return <c>null</c>.</param>
        /// <returns>Sliced and trimmed text.</returns>
        public static string DbTrim(string text, int length, bool allowNull)
        {
            return text == null ? (allowNull ? null : string.Empty) : Slice(text.Trim(), 0, length);
        }

		/// <summary>
		/// Joins multiple strings.
		/// </summary>
		/// <param name="strings">The strings.</param>
		/// <returns>Joined string.</returns>
		public static string Join(params string[] strings)
		{
			return string.Join("", strings);
		}

		/// <summary>
		/// Replaces the Regex inside string with given replacement.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="replacement">The replacement.</param>
		/// <returns>Replaced string.</returns>
		public static string ReplaceRx(string input, string pattern, string replacement)
		{
			var rx = new Regex(pattern);
			return rx.Replace(input, replacement);
		}

		/// <summary>
		/// Replaces the Regex inside string using given replacement routine.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="evaluator">The evaluator.</param>
		/// <returns>Replaced string.</returns>
		public static string ReplaceRx(string input, string pattern, MatchEvaluator evaluator)
		{
			var rx = new Regex(pattern);
			return rx.Replace(input, evaluator);
		}

		/// <summary>
		/// Works as .ToString() method, but does not throw exception, so can be safely used
		/// for logging.
		/// </summary>
		/// <param name="subject">The subject.</param>
		/// <returns>String representation of subject.</returns>
		public static string SafeToString(object subject)
		{
			try
			{
				if (object.ReferenceEquals(subject, null))
				{
					return "<null>";
				}
				else if (subject is string)
				{
					return $"\"{StringEscape.Encode((string) subject, StringEscape.StringEscapeMapper)}\"";
				}
				else if (subject is char)
				{
					return
					    $"'{StringEscape.Encode(((char) subject).ToString(CultureInfo.InvariantCulture), StringEscape.CharEscapeMapper)}'";
				}
				else
				{
					return $"{subject.GetType().Name}(\"{StringEscape.Encode(subject.ToString(), StringEscape.StringEscapeMapper)}\")";
				}
			}
			catch
			{
				return "<exception>";
			}
		}

		/// <summary>
		/// Converts a string value to a boolean
		/// All cases use the "defaultWhenNullOrEmpty" except for 'false', '0' and 'no' (case independent)
		/// </summary>
		/// <param name="value">the input string, expected values are empty string, -1, 0, TRUE, FALSE</param>
		/// <param name="defaultWhenNullOrEmpty">this is the default returned when the value is null or empty.</param>
		/// <returns>
		/// boolean corresponding to the input
		/// </returns>
		public static bool StringToBool(string value, bool defaultWhenNullOrEmpty = true)
		{
			bool rtnVal = defaultWhenNullOrEmpty;

			try
			{
				// NOTE: All cases are true except for 'false', '0' and 'no' (case independent)
				if (!string.IsNullOrWhiteSpace(value))
				{
					string lower = value.ToLower();
					rtnVal = !(lower.Equals("false") || lower.Equals("0") || lower.Equals("no"));
				}

				return rtnVal;
			}
			catch
			{
				// Swallow exception?
				return rtnVal;
			}
		}

		#region Split methods
		/// <summary>
		/// Split Helper - takes a string array and returns IEnumerable of string, Trimmed and empty items removed
		/// </summary>
		/// <param name="srcStringArray">The string array to process</param>
		/// <returns>IList of string</returns>
		private static IList<string> SplitHelper(string[] srcStringArray)
		{
			List<string> result = new List<string>();

			for (int i = 0; i < srcStringArray.Length; i++)
			{
				string srcItem = srcStringArray[i].Trim();
				// Don't return empty strings
				if (!string.IsNullOrEmpty(srcItem))
					result.Add(srcItem);
			}
			return result;
		}

        /// <summary>
        /// Splits and trims each entry as well as removing empty entries.
        /// </summary>
        ///
        /// <param name="separator">    string[] separator. </param>
        /// <param name="srcString">    the source string. </param>
        ///
        /// <returns>
        /// IList of strings.
        /// </returns>
		public static IList<string> SplitTrimRemoveEmpty(string[] separator, string srcString)
		{
			if (!string.IsNullOrEmpty(srcString))
			{
				string[] srcStringArray = srcString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				return SplitHelper(srcStringArray);
			}
			else
			{
				return new List<string>();
			}
		}

        /// <summary>
        /// Splits and trims each entry as well as removing empty entries
        /// </summary>
        /// <param name="separator">string separator, only one</param>
        /// <param name="srcString">the source string</param>
        /// <returns>IList of strings</returns>
        public static IList<string> SplitTrimRemoveEmpty(string separator, string srcString) => SplitTrimRemoveEmpty(new string[] { separator }, srcString);

        /// <summary>
        /// Splits and trims each entry as well as removing empty entries
        /// </summary>
        /// <param name="separator">char[] separator</param>
        /// <param name="srcString">the source string</param>
        /// <returns>IList of strings</returns>
        public static IList<string> SplitTrimRemoveEmpty(char[] separator, string srcString)
		{
			if (!string.IsNullOrEmpty(srcString))
			{
				string[] srcStringArray = srcString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				return SplitHelper(srcStringArray);
			}
			else
			{
				return new List<string>();
			}
		}

		/// <summary>
		/// Splits and trims each entry as well as removing empty entries
		/// </summary>
		/// <param name="separator">char separator, only one</param>
		/// <param name="srcString">the source string</param>
		/// <returns>IList of strings</returns>
		public static IList<string> SplitTrimRemoveEmpty(char separator, string srcString) => SplitTrimRemoveEmpty(new char[] { separator }, srcString);

        /// <summary>
        /// Splits string on , and trims each entry as well as removing empty entries
        /// </summary>
        /// <param name="srcString">the source string</param>
        /// <returns>IList of strings</returns>
        public static IList<string> SplitTrimRemoveEmpty(string srcString) => SplitTrimRemoveEmpty(',', srcString);
        #endregion

        /// <summary>
        /// Calculates MD5 for given text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>MD5 (128 bits, 16 bytes)</returns>
        public static byte[] MD5(string text) => System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(text));

        #region NormalizeLineEndings

        /// <summary>
        /// Regular expression to find the line endings
        /// </summary>
        private readonly static Regex s_LineEndingsRx = new Regex(@"(?<body>.*?)((?<eol>(\r\n)|(\r)|(\n))|(?<eof>$))",
			RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Normalizes the line endings.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="lineEnd">The line ending in output.</param>
        /// <returns>
        /// Collection of lines with proper line endings.
        /// </returns>
        public static IEnumerable<string> NormalizeLineEndings(string text, string lineEnd) => NormalizeLineEndings(text, lineEnd, false);

        /// <summary>
        /// Normalizes the line endings.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="newLineOnEoF">if set to <c>true</c> new line will also be included before EoF, otherwise preserves original last line.</param>
        /// <returns>
        /// Collection of lines with proper line endings.
        /// </returns>
        public static IEnumerable<string> NormalizeLineEndings(string text, bool newLineOnEoF) => NormalizeLineEndings(text, Environment.NewLine, newLineOnEoF);

	    /// <summary>
		/// Normalizes the line endings.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="lineEnd">The line ending in output.</param>
		/// <param name="newLineOnEoF">if set to <c>true</c> new line will also be included before EoF, otherwise preserves original last line.</param>
		/// <returns>
		/// Collection of lines with proper line endings.
		/// </returns>
		public static IEnumerable<string> NormalizeLineEndings(string text, string lineEnd, bool newLineOnEoF)
		{
			int position = 0;
			int length = text.Length;

			while (position < length)
			{
				Match m = s_LineEndingsRx.Match(text, position);
				// the thing is, with given Regex it's not possible to raise this error, but this check will at least check if it was modified
				if (!m.Success) throw new ArgumentException("Given text is not valid text file", "text");

				string body = m.Groups["body"].Value;
				string eol = (m.Groups["eol"].Success | newLineOnEoF) ? lineEnd : string.Empty;

				yield return string.Concat(body, eol);
				position += m.Length;
			}
		}

		/// <summary>
		/// Normalizes the line endings. Uses <see cref="Environment.NewLine"/> as line ending.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>Collection of lines with proper line endings.</returns>
		public static IEnumerable<string> NormalizeLineEndings(string text) => NormalizeLineEndings(text, Environment.NewLine, false);

	    #endregion

		#region CamelCaseToString

		private readonly static Regex s_CamelCaseRx = new Regex(
			@"((?<=[A-Z])(?=[A-Z][a-z]))|((?<=[^A-Z])(?=[A-Z]))|((?<=[A-Za-z])(?=[^A-Za-z]))", 
			RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        /// <summary>Converts camel case string to user facing string..</summary>
        /// <param name="text">The text.</param>
        /// <returns>User facing string.</returns>
        public static string CamelCaseToString(string text) => s_CamelCaseRx.Replace(text, " ");

        #endregion

        #region NumericalStringSort

        /// <summary>
        /// Performs Numerical string comparison
        /// Based on Numeric String sort from http://www.codeproject.com/Articles/11016/Numeric-String-Sort-in-C
        /// </summary>
        /// <param name="s1">The first string</param>
        /// <param name="s2">The second string</param>
        /// <returns>
        /// Returns 0 if the strings are identical.
        /// Returns 1 if the string s1 has a greater value than s2.
        /// Returns -1 if the string s1 has a lesser value than s2.
        /// </returns>
        public static int NumericStringCompare(string s1, string s2)
		{
			//get rid of special cases
			switch (s1)
			{
			    case null when (s2 == null):
			        return 0;

			    case null:
			        return -1;

			    default:
			        if (s2 == null) return 1;
			        break;
			}

			if ((s1.Equals(string.Empty) && (s2.Equals(string.Empty)))) return 0;
			else if (s1.Equals(string.Empty)) return -1;
			else if (s2.Equals(string.Empty)) return -1;

			//Windows Explorer style, special case
			bool sp1 = Char.IsLetterOrDigit(s1, 0);
			bool sp2 = Char.IsLetterOrDigit(s2, 0);
			if (sp1 && !sp2) return 1;
			if (!sp1 && sp2) return -1;

			int i1 = 0, i2 = 0; //current index
			int r = 0; // temp result
			while (true)
			{
				bool c1 = Char.IsDigit(s1, i1);
				bool c2 = Char.IsDigit(s2, i2);
				if (!c1 && !c2)
				{
					bool letter1 = Char.IsLetter(s1, i1);
					bool letter2 = Char.IsLetter(s2, i2);
					if ((letter1 && letter2) || (!letter1 && !letter2))
					{
						if (letter1 && letter2)
						{
							r = Char.ToLower(s1[i1]).CompareTo(Char.ToLower(s2[i2]));
						}
						else
						{
							r = s1[i1].CompareTo(s2[i2]);
						}
						if (r != 0) return r;
					}
					else if (!letter1 && letter2) return -1;
					else if (letter1 && !letter2) return 1;
				}
				else if (c1 && c2)
				{
					r = CompareNum(s1, ref i1, s2, ref i2);
					if (r != 0) return r;
				}
				else if (c1)
				{
					return -1;
				}
				else if (c2)
				{
					return 1;
				}
				i1++;
				i2++;
				if ((i1 >= s1.Length) && (i2 >= s2.Length))
				{
					return 0;
				}
				else if (i1 >= s1.Length)
				{
					return -1;
				}
				else if (i2 >= s2.Length)
				{
					return -1;
				}
			}
		}

		/// <summary>
		/// Helper function for the Compare routine
		/// </summary>
		/// <param name="s1">First string</param>
		/// <param name="i1">ref parameter</param>
		/// <param name="s2">Second string</param>
		/// <param name="i2">ref parameter</param>
		/// <returns>int</returns>
		private static int CompareNum(string s1, ref int i1, string s2, ref int i2)
		{
			int nzStart1 = i1, nzStart2 = i2; // nz = non zero
			int end1 = i1, end2 = i2;

			ScanNumEnd(s1, i1, ref end1, ref nzStart1);
			ScanNumEnd(s2, i2, ref end2, ref nzStart2);
			int start1 = i1; i1 = end1 - 1;
			int start2 = i2; i2 = end2 - 1;

			int nzLength1 = end1 - nzStart1;
			int nzLength2 = end2 - nzStart2;

			if (nzLength1 < nzLength2) return -1;
			else if (nzLength1 > nzLength2) return 1;

			for (int j1 = nzStart1, j2 = nzStart2; j1 <= i1; j1++, j2++)
			{
				int r = s1[j1].CompareTo(s2[j2]);
				if (r != 0) return r;
			}
			// the nz parts are equal
			int length1 = end1 - start1;
			int length2 = end2 - start2;
			if (length1 == length2) return 0;
			if (length1 > length2) return -1;
			return 1;
		}

		/// <summary>
		/// Helper function for the CompareNum routine
		/// </summary>
		/// <param name="s">Source string</param>
		/// <param name="start">Starting character</param>
		/// <param name="end">End character</param>
		/// <param name="nzStart">nzStart</param>
		private static void ScanNumEnd(string s, int start, ref int end, ref int nzStart)
		{
			nzStart = start;
			end = start;
			bool countZeros = true;
			while (Char.IsDigit(s, end))
			{
				if (countZeros && s[end].Equals('0'))
				{
					nzStart++;
				}
				else countZeros = false;
				end++;
				if (end >= s.Length) break;
			}
		}

		#endregion

		/// <summary>
		/// Returns the string present in the description attribute
		/// of enum values
		/// </summary>
		/// <param name="value">Enum value</param>
		/// <returns>Description of a enum value</returns>
		public static string GetEnumDescription(Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes != null && attributes.Length > 0)
				return attributes[0].Description;
			else
				return value.ToString();
		}
	}
}
