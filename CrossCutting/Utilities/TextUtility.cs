﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace Indigo.CrossCutting.Utilities.utility
{
    public static class TextUtility
    {
        /// <summary>
        /// Clears the HTML.
        /// </summary>
        /// <param name="htmlString">The HTML string.</param>
        /// <returns></returns>
        public static string ClearHtml(string htmlString)
        {
            if (string.IsNullOrEmpty(htmlString))
                return "";
            try
            {
                string output = HttpContext.Current.Server.HtmlDecode(htmlString);
                output = Regex.Replace(output, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"-->", "", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"<!--.*", "", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                output.Replace("<", "");
                output.Replace(">", "");
                output.Replace("\r\n", "");
                return output;
            }
            catch { return htmlString; }
        }

        /// <summary>
        /// Slugs the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string Slug(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            string formattedTitle = input.ToLower();
            var chars = new string[] { " ", ";", ",", "?", ">", "<", ".", "'","\\","/","\"", "~",":", "!", "@", "#", "{", "}", "[", "]", 
                "|", "_", "=", "$", "%", "^", "*", "(", ")", "+", "-", "&" ,"！","·","￥","%","…","—","（","）","＝","、","，","。",
                "‘","’","“","”","；","：","？","《","》"
            };

            foreach (var c in chars)
                formattedTitle = formattedTitle.Replace(c, "-");

            if (formattedTitle.EndsWith("-"))
            {
                if (formattedTitle.Length >= 2)
                    formattedTitle = formattedTitle.Substring(0, formattedTitle.Length - 1);
            }

            while (formattedTitle.IndexOf("--") > -1)
                formattedTitle = formattedTitle.Replace("--","-");

            return formattedTitle;
        }
    }
}