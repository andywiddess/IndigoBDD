using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    public static class SafeStringUtilities
    {

        /// <summary>
        /// Return the passed dictionary encoded as url query string, dealing with null values.
        /// </summary>
        /// <param name="query">The dictionary of name value pairs </param>
        /// <returns></returns>
        public static string EncodeAsQueryString(Dictionary<string, string> query)
        {
            string queryStringStartChar = "?";
            string queryStringSeparatorChar = "&";
            string queryValueSeperatorChar = "=";
            StringBuilder sb = new StringBuilder();
            foreach (string key in query.Keys)
            {
                if (! string.IsNullOrEmpty(key))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(queryStringSeparatorChar);
                    }
                    sb.Append(System.Web.HttpUtility.UrlEncode(key));
                    sb.Append(queryValueSeperatorChar);
                    if (!string.IsNullOrEmpty(query[key]))
                    {
                        sb.Append(System.Web.HttpUtility.UrlEncode(query[key]));
                    }
                    
                }
            }
            if (sb.Length > 0) sb.Insert(0, queryStringStartChar);
            return sb.ToString();
        }


        /// <summary>
        /// Concatenate the passed objects to stings, and seperate with the passed seperator. The seperator is not added to the last instance of the string. Child objects which are 
        /// null are replaced with a blank, and no seperator, as if they had not been passed
        /// </summary>
        /// <param name="delimiter"></param>
        /// <param name="children"></param>
        /// <returns></returns>
        public static string ToDelimitedString(string delimiter, List<object> children)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object item in children)
            {
                if (item == null || item.GetType().IsArray)
                {
                    // Skip
                }
                else
                {
                    if (string.IsNullOrEmpty(item.ToString()))
                    {
                        // Skip
                    }
                    else
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(delimiter);
                        }
                        sb.Append(item.ToString());
                        
                    }
                }
                
            }
            return sb.ToString();
        }

        /// <summary>
        /// Concatenate the passed objects to stings, and seperate with the international List Seperator relevant to the current culture.
        /// The seperator is not added to the last instance of the string. Child objects which are
        /// null are replaced with a blank, and no seperator, as if they had not been passed.
        /// </summary>
        /// <param name="children">The children.</param>
        /// <returns></returns>
        public static string ToDelimitedString(params object[] children)
        {
            return ToDelimitedString(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator, children.ToList());
        }

        /// <summary>
        /// Concatenate the passed objects to stings, and seperate with the international List Seperator relevant to the current culture.
        /// The seperator is not added to the last instance of the string. Child objects which are
        /// null are replaced with a blank, and no seperator, as if they had not been passed.
        /// </summary>
        /// <param name="children">The children.</param>
        /// <returns></returns>
        public static string ToDelimitedString(List<object> children)
        {
            return ToDelimitedString(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator, children);
        }
    }
}
