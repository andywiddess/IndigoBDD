using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Indigo.CrossCutting.Utilities.Utility
{
    public static class StringExtension
    {
        /// <summary>
        /// Use the current thread's culture info for conversion
        /// </summary>
        public static string ToTitleCase(this string str)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Overload which uses the culture info with the specified name
        /// </summary>
        public static string ToTitleCase(this string str, string cultureInfoName)
        {
            var cultureInfo = new CultureInfo(cultureInfoName);
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Overload which uses the specified culture info
        /// </summary>
        public static string ToTitleCase(this string str, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// To the base64 encoding.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string ToBase64Encoding(this string str)
        {
            return !string.IsNullOrEmpty(str) ? System.Convert.ToBase64String(new System.Text.UTF8Encoding().GetBytes(Regex.Replace(str, @"\r\n?|\n", "<br/>"))) : str;
        }

        /// <summary>
        /// Froms the base64 encoding.
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <returns></returns>
        public static string FromBase64Encoding(this byte[] arr)
        {
            return (arr != null && arr.Length > 0) ? new System.Text.UTF8Encoding().GetString(arr) : string.Empty;
        }
    }
}
