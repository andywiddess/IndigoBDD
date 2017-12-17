#region Header
// ---------------------------------------------------------------------------
// Sepura - Commercially Confidential.
// 
// Indigo.CrossCutting.Utilities.StringUtils
// String Extension Methods
//
// Copyright (c) 2016 Sepura Plc
// All Rights reserved.
//
// $Id:  $ :
// ---------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// String Utilities
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Retrieves the left element of the string
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns></returns>
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }

        /// <summary>
        /// Get substring of specified number of characters on the right.
        /// </summary>
        public static string Right(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) 
                return string.Empty;

            return value.Length <= length 
                    ? value 
                    : value.Substring(value.Length - length);
        }

        /// <summary>
        /// Multiples the replace.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="replacements">The replacements.</param>
        /// <returns></returns>
        public static string MultipleReplace(this string text, Dictionary<string, string> replacements)
        {
            string retVal = text;
            foreach (string textToReplace in replacements.Keys)
            {
                retVal = retVal.Replace(textToReplace, replacements[textToReplace]);
            }
            return retVal;
        }
    }
}
