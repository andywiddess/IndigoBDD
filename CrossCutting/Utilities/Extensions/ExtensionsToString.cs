using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;
using Indigo.CrossCutting.Utilities.Text;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	public static class ExtensionsToString
	{
		/// <summary>
		/// Checks if a string is not null or empty
		/// </summary>
		/// <param name="value">A string instance</param>
		/// <returns>True if the string has a value</returns>
		public static bool IsNotEmpty(this string value)
		{
			return !string.IsNullOrEmpty(value);
		}

		/// <summary>
		/// Check if a string is null or empty
		/// </summary>
		/// <param name="value">A string instance</param>
		/// <returns>True if the string is null or empty, otherwise false</returns>
		public static bool IsEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

		/// <summary>
		/// Returns true if a string is null (the string can, however, be empty)
		/// </summary>
		/// <param name="value">A string value</param>
		/// <returns>True if the string value is null, otherwise false</returns>
		public static bool IsNull(this string value)
		{
			return value == null;
		}

		/// <summary>
		/// Uses the string as a template and applies the specified arguments
		/// </summary>
		/// <param name="format">The format string</param>
		/// <param name="args">The arguments to pass to the format provider</param>
		/// <returns>The formatted string</returns>
		public static string FormatWith(this string format, params object[] args)
		{
			return format.IsEmpty() ? format : string.Format(format, args);
		}

		/// <summary>
		/// Returns the UTF-8 encoded string from the specified byte array
		/// </summary>
		/// <param name="data">The byte array</param>
		/// <returns>The UTF-8 string</returns>
		public static string ToUtf8String(this byte[] data)
		{
			return Encoding.UTF8.GetString(data);
		}

	    /// <summary>
	    /// Formats this string using the values <paramref name="args"/>
	    /// </summary>
	    /// <param name="me">This string</param>
	    /// <param name="args">Values to format this string with</param>
	    /// <returns>The formatted string</returns>
	    public static string Frmt(this string me, params object[] args)
	    {
	        return string.Format(me, args);
	    }

	    /// <summary>
	    /// Splits PascalCase text into separate words based on upper case chars.  E.g. "PascalCase" = "Pascal Case"
	    /// </summary>
	    /// <param name="me">This string</param>
	    /// <returns></returns>
	    public static string PascalCaseToWords(this string me)
	    {
	        return Regex.Replace(me.ToString(), "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
	    }
	    /// <summary>
	    /// Extension method that replaces keys in a string with the values of matching object properties.
	    /// <remarks>Uses <see cref="Format"/> internally; custom formats should match those used for that method.</remarks>
	    /// </summary>
	    /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
	    /// <param name="injectionObject">The object whose properties should be injected in the string</param>
	    /// <returns>A version of the formatString string with keys replaced by (formatted) key values.</returns>
	    public static string Inject(this string formatString, object injectionObject)
	    {
	        return formatString.Inject(GetPropertyHash(injectionObject));
	    }

	    /// <summary>
	    /// Extension method that replaces keys in a string with the values of matching dictionary entries.
	    /// taken from http://mo.notono.us/2008/07/c-stringinject-format-strings-by-key.html
	    /// <remarks>Uses <see cref="Format"/> internally; custom formats should match those used for that method.</remarks>
	    /// </summary>
	    /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
	    /// <param name="dictionary">An <see cref="IDictionary"/> with keys and values to inject into the string</param>
	    /// <returns>A version of the formatString string with dictionary keys replaced by (formatted) key values.</returns>
	    public static string Inject(this string formatString, IDictionary dictionary)
	    {
	        return formatString.Inject(new Hashtable(dictionary));
	    }

	    /// <summary>
	    /// Extension method that replaces keys in a string with the values of matching hashtable entries.
	    /// <remarks>Uses <see cref="Format"/> internally; custom formats should match those used for that method.</remarks>
	    /// </summary>
	    /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
	    /// <param name="attributes">A <see cref="Hashtable"/> with keys and values to inject into the string</param>
	    /// <returns>A version of the formatString string with hastable keys replaced by (formatted) key values.</returns>
	    public static string Inject(this string formatString, Hashtable attributes)
	    {
	        string result = formatString;
	        if (attributes == null || formatString == null)
	            return result;

	        foreach (string attributeKey in attributes.Keys)
	        {
	            result = result.InjectSingleValue(attributeKey, attributes[attributeKey]);
	        }
	        return result;
	    }

	    /// <summary>
	    /// Replaces all instances of a 'key' (e.g. {foo} or {foo:SomeFormat}) in a string with an optionally formatted value, and returns the result.
	    /// </summary>
	    /// <param name="formatString">The string containing the key; unformatted ({foo}), or formatted ({foo:SomeFormat})</param>
	    /// <param name="key">The key name (foo)</param>
	    /// <param name="replacementValue">The replacement value; if null is replaced with an empty string</param>
	    /// <returns>The input string with any instances of the key replaced with the replacement value</returns>
	    public static string InjectSingleValue(this string formatString, string key, object replacementValue)
	    {
	        string result = formatString;
	        //regex replacement of key with value, where the generic key format is:
	        //Regex foo = new Regex("{(foo)(?:}|(?::(.[^}]*)}))");
	        Regex attributeRegex = new Regex("{(" + key + ")(?:}|(?::(.[^}]*)}))");  //for key = foo, matches {foo} and {foo:SomeFormat}

	        //loop through matches, since each key may be used more than once (and with a different format string)
	        foreach (Match m in attributeRegex.Matches(formatString))
	        {
	            string replacement = m.ToString();
	            if (m.Groups[2].Length > 0) //matched {foo:SomeFormat}
	            {
	                //do a double string.Format - first to build the proper format string, and then to format the replacement value
	                string attributeFormatString = string.Format(CultureInfo.InvariantCulture, "{{0:{0}}}", m.Groups[2]);
	                replacement = string.Format(CultureInfo.CurrentCulture, attributeFormatString, replacementValue);
	            }
	            else //matched {foo}
	            {
	                replacement = (replacementValue ?? string.Empty).ToString();
	            }
	            //perform replacements, one match at a time
	            result = result.Replace(m.ToString(), replacement);  //attributeRegex.Replace(result, replacement, 1);
	        }
	        return result;

	    }


	    /// <summary>
	    /// Creates a HashTable based on current object state.
	    /// <remarks>Copied from the MVCToolkit HtmlExtensionUtility class</remarks>
	    /// </summary>
	    /// <param name="properties">The object from which to get the properties</param>
	    /// <returns>A <see cref="Hashtable"/> containing the object instance's property names and their values</returns>
	    private static Hashtable GetPropertyHash(object properties)
	    {
	        Hashtable values = null;
	        if (properties != null)
	        {
	            values = new Hashtable();
	            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(properties);
	            foreach (PropertyDescriptor prop in props)
	            {
	                values.Add(prop.Name, prop.GetValue(properties));
	            }
	        }
	        return values;
	    }

        /// <summary>
        /// Repeats the specified count.
        /// </summary>
        /// <param name="me">Me.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string Repeat(this string me, int count)
	    {
	        return string.Join("", Enumerable.Repeat(me, count).ToArray());
	    }

        /// <summary>
        /// Repeats the specified separator.
        /// </summary>
        /// <param name="me">Me.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string Repeat(this string me, string separator, int count)
	    {
	        return string.Join(separator, Enumerable.Repeat(me, count).ToArray());
	    }

        #region SafeToString

        /// <summary>Safe <see cref="object.ToString()"/>.</summary>
        /// <param name="subject">The subject.</param>
        /// <returns>String representation of the object. Returns "null" if object is <c>null</c>, 
        /// and "exception" if <see cref="object.ToString()"/> caused the exception</returns>
        public static string SafeToString(this object subject)
        {
            string result;

            if (subject == null)
            {
                result = "<null>";
            }
            else
            {
                try
                {
                    result = subject.ToString();
                }
                catch (Exception)
                {
                    result = "<exception>";
                }
            }

            return result;
        }

        #endregion

        #region NotNull, NotEmpty, NotBlank

        /// <summary>Forces string to be not <c>null</c>. Returns <see cref="String.Empty"/> in such cases.</summary>
        /// <param name="value">The string value.</param>
        /// <returns>Passed value or <see cref="String.Empty"/></returns>
        public static string NotNull(this string value)
        {
            return value ?? String.Empty;
        }

        /// <summary>Forces string to be not <c>null</c>. Returns <paramref name="defaultValue"/> in such cases.</summary>
        /// <param name="value">The string value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Passed value or <paramref name="defaultValue"/></returns>
        public static string NotNull(this string value, string defaultValue)
        {
            return value ?? defaultValue;
        }

        /// <summary>Forces string to be not empty or <c>null</c>. 
        /// Returns <paramref name="defaultValue"/> is such cases.</summary>
        /// <param name="value">The string value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Passed value or <paramref name="defaultValue"/></returns>
        public static string NotEmpty(this string value, string defaultValue)
        {
            return String.IsNullOrEmpty(value) ? defaultValue : value;
        }

        /// <summary>Forces string to be not blank or <c>null</c>. Blank it this case means being null, empty or consist only
        /// of whitespaces.</summary>
        /// <param name="value">The string value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Passed value or <see cref="String.Empty"/></returns>
        public static string NotBlank(this string value, string defaultValue)
        {
            return String.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        #endregion

        #region Format

        /// <summary>Formats the specified the string with given arguments.</summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="argument1">The argument.</param>
        /// <returns>Formatted string.</returns>
        public static string With(this string pattern, object argument1)
        {
            return String.Format(pattern, argument1);
        }

        /// <summary>Formats the specified pattern.</summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="argument1">The argument.</param>
        /// <param name="argument2">The argument.</param>
        /// <returns>Formatted string.</returns>
        public static string With(this string pattern, object argument1, object argument2)
        {
            return String.Format(pattern, argument1, argument2);
        }

        /// <summary>Formats the specified pattern.</summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="argument1">The argument.</param>
        /// <param name="argument2">The argument.</param>
        /// <param name="argument3">The argument.</param>
        /// <returns>Formatted string.</returns>
        public static string With(this string pattern, object argument1, object argument2, object argument3)
        {
            return String.Format(pattern, argument1, argument2, argument3);
        }

        /// <summary>Formats the specified the string with given arguments.</summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>Formatted string.</returns>
        public static string With(this string pattern, params object[] arguments)
        {
            return String.Format(pattern, arguments);
        }


        #endregion

        #region Slice

        /// <summary>Returns slice of specified string. </summary>
        /// <param name="value">The string.</param>
        /// <param name="index">The starting index.</param>
        /// <param name="length">The maximum length.</param>
        /// <returns>Sliced string.</returns>
        public static string Slice(this string value, int index, int length = int.MaxValue)
        {
            if (value != null)
            {
                int currentLength = value.Length;
                if (index < 0) index = currentLength + index;
                index = Math.Min(Math.Max(index, 0), currentLength);
                length = Math.Min(currentLength - index, length);

                return
                    index <= 0 && length >= currentLength
                    ? value
                    : value.Substring(index, length);
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Check Structure

        /// <summary>
        /// Determines whether the specified string is capitalised.
        /// </summary>
        /// <param name="val">The string to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified string is capitalised; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCapitalised(this string val)
        {
            return !String.IsNullOrWhiteSpace(val) && Char.IsUpper(val[0]);
        }

        /// <summary>
        /// Makes the string capitalised, effectively capitalising the first character in the string.
        /// Note, this is NOT title-case, which is converting the first character of each word to capitals.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static string MakeCapitalised(this string val)
        {
            string rtnVal;

            if (!val.IsCapitalised())
                rtnVal = String.Concat(Char.ToUpper(val[0]).ToString(CultureInfo.CurrentCulture), val.Substring(1));
            else
                rtnVal = val;

            return rtnVal;
        }

        /// <summary>
        /// Checks if a string contains only numbers and letters
        /// </summary>
        /// <param name="val">The string to check</param>
        /// <returns><c>True</c> if it is alphanumeric, <c>False</c> otherwise</returns>
        public static bool IsAlphanumeric(this string val)
        {
            return Regex.IsMatch(val, @"^[a-zA-Z0-9]+$");
        }

        /// <summary>
        /// Checks if a string contains only numbers
        /// </summary>
        /// <param name="val">The string to check</param>
        /// <returns><c>True</c> if it is numeric, <c>False</c> otherwise</returns>
        public static bool IsNumeric(this string val)
        {
            return Regex.IsMatch(val, @"^[0-9]+$");//@"^\d$");
        }

        #endregion

        #region Expand (with TemplateString)

        /// <summary>Resolves the template using <see cref="TemplateString.Expand(string,Func{string,object})"/>.</summary>
        /// <param name="template">The template.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns>Resolved String.</returns>
        public static string Expand(this string template, Func<string, object> resolver)
        {
            return TemplateString.Expand(template, resolver);
        }

        /// <summary>Resolves the template using <see cref="TemplateString.Expand{T}(string,IDictionary{string,T})"/>.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template">The template.</param>
        /// <param name="data">The data dictionary.</param>
        /// <returns>Resolved String.</returns>
        public static string Expand<T>(this string template, IDictionary<string, T> data)
        {
            return TemplateString.Expand(template, data);
        }

        /// <summary>Resolves the template using <see cref="TemplateString.Expand(string,object)"/>.</summary>
        /// <param name="template">The template.</param>
        /// <param name="data">The data object.</param>
        /// <returns>Resolved String.</returns>
        public static string Expand(this string template, object data)
        {
            return TemplateString.Expand(template, data);
        }

        #endregion

        #region Parse methods

        /// <summary>
        /// Parses the int.
        /// </summary>
        /// <param name="val">The value to parse</param>
        /// <param name="defaultValue">The default value used if the parse failed</param>
        /// <returns>The parsed value or the default</returns>
        public static int ParseInt(this string val, int defaultValue = 0)
        {
            int rtnVal;
            if (!int.TryParse(val, out rtnVal))
                rtnVal = defaultValue;

            return rtnVal;
        }

        /// <summary>
        /// Parses the UInt.
        /// </summary>
        /// <param name="val">The value to parse</param>
        /// <param name="defaultValue">The default value used if the parse failed</param>
        /// <returns>The parsed value or the default</returns>
        public static uint ParseUInt(this string val, uint defaultValue = 0)
        {
            uint rtnVal;
            if (!uint.TryParse(val, out rtnVal))
                rtnVal = defaultValue;

            return rtnVal;
        }

        #endregion

        #region Join

        /// <summary>Joins the specified collection of strings using the separator.</summary>
        /// <param name="values">The values.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>Joined string</returns>
        public static string Join(this IEnumerable<string> values, string separator = "")
        {
            return string.Join(separator, values);
        }

        #endregion

        #region ToEnum

        private static char[] EnumSeparators = new char[] { '|', ':', ',', ';', '+' };

        private static string FixEnum<T>(string value, string separator = ", ")
        {
            var names = Enum.GetNames(typeof(T));
            return value.Split(EnumSeparators)
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => names.FirstOrDefault(n => string.Compare(n, v, true) == 0))
                .Where(v => v != null)
                .Join(separator);
        }

        /// <summary>Converts string to enumeration.</summary>
        /// <typeparam name="T">Type of enumeration.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> case of string is ignored.</param>
        /// <returns>Converted value.</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase = false)
            where T : struct
        {
            if (ignoreCase)
                value = FixEnum<T>(value);

            T result;
            if (!Enum.TryParse<T>(value, out result))
            {
                throw new ArgumentException(
                    "Value '{0}' cannot be converted to {1}".With(value.SafeToString(), typeof(T).Name));
            }
            return result;
        }

        /// <summary>Converts string to enumeration.</summary>
        /// <typeparam name="T">Type of enumeration.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Converted value or default value.</returns>
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            T result;
            if (!Enum.TryParse<T>(value, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        #endregion

        #region Base16, Base64

        /// <summary>Converts buffer to base64 encoded string.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="lineBreaks">Create line breaks at 76 characters.</param>
        /// <returns>Base85 encoded string.</returns>
        public static string ToBase64(this byte[] buffer, bool lineBreaks = false)
        {
            return Convert.ToBase64String(
                buffer, lineBreaks ? Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None);
        }

        /// <summary>Converts base64 encoded string into byte buffer.</summary>
        /// <param name="encoded">The base64 encoded string.</param>
        /// <returns>Decoded byte buffer.</returns>
        public static byte[] FromBase64(this string encoded)
        {
            return Convert.FromBase64String(encoded);
        }

        /// <summary>
        /// CHecks if a string is Base64 encoded
        /// </summary>
        /// <param name="value">The string to check</param>
        /// <returns><c>True</c> if the string is Base64 <c>false</c> otherwise</returns>
        public static bool IsBase64String(this string value)
        {
            value = value.Trim();
            return (value.Length % 4 == 0) && Regex.IsMatch(value, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        /// <summary>
        /// Converts buffer to hex encoded string.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useCapitals">if set to <c>true</c> use capitals, uses lowercase otherwise.</param>
        /// <returns>Hex encoded string.</returns>
        public static string ToHex(this byte[] buffer, bool useCapitals = false)
        {
            return HexConvert.ToString(buffer, useCapitals);
        }

        /// <summary>Converts hex encoded string into byte buffer.</summary>
        /// <param name="encoded">The hex encoded string.</param>
        /// <returns>Decoded byte buffer.</returns>
        public static byte[] FromHex(this string encoded)
        {
            return HexConvert.FromString(encoded);
        }

        #endregion

        #region To/FromUTF8

        /// <summary>Converts a string to its UTF-8 representation.</summary>
        /// <param name="text">The text.</param>
        /// <returns>Byte array with UTF-8 representation.</returns>
        public static byte[] ToUTF8(this string text)
        {
            if (text == null)
                return null;
            return Encoding.UTF8.GetBytes(text);
        }

        /// <summary>Converts UTF-8 byte array to string.</summary>
        /// <param name="utf8">The UTF-8 byte array.</param>
        /// <returns>A string.</returns>
        public static string FromUTF8(this byte[] utf8)
        {
            if (utf8 == null)
                return null;
            return Encoding.UTF8.GetString(utf8);
        }

        #endregion

        #region GuidOf

        /// <summary>Calculates MD5 of given bytes and returns it as GUID.</summary>
        /// <param name="data">The data.</param>
        /// <returns>MD5 as GUID.</returns>
        public static Guid GuidOf(this byte[] data)
        {
            return new Guid(MD5.Create().ComputeHash(data));
        }

        /// <summary>Calculates MD5 of given string (UTF8 encoded) and returns it as GUID.</summary>
        /// <param name="text">The text.</param>
        /// <returns>MD5 as GUID.</returns>
        public static Guid GuidOf(this string text)
        {
            return GuidOf(text.ToUTF8());
        }

        /// <summary>
        /// Removes the first character of a string if it matches the specified value
        /// </summary>
        /// <param name="value">The string to trim</param>
        /// <param name="character">The target character to remove</param>
        /// <returns>The trimmed string</returns>
        public static string TrimFirstCharacter(this string value, string character)
        {
            return value.StartsWith(character) ? value.Remove(0, 1) : value;
        }

        /// <summary>
        /// Retrieves the first N characters from the string
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="len">The length.</param>
        /// <returns></returns>
        public static string SelectN(this string value, int len)
	    {
	        return value.Substring(0, (value.Length > len ? len : value.Length));
	    }

        /// <summary>
        /// Appends copies of the specified strings to this instance
        /// </summary>
        /// <param name="builder">The StringBuilder instance</param>
        /// <param name="strings">The strings to appends</param>
        public static void AppendMany(this StringBuilder builder, params string[] strings)
        {
            strings.ForEach(s => builder.Append(s));
        }

        #endregion
    }
}