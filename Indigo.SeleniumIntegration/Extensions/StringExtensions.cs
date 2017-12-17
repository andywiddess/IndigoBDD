using System;
using System.Text;
using System.Xml.Linq;
using System.Xml;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenQA.Selenium
{
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces the first.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="search">The search.</param>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }

            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        /// <summary>
        /// Determines whether [is null or empty].
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified target]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }

        /// <summary>
        /// Removes the line breaks.
        /// </summary>
        /// <param name="theString">The string.</param>
        /// <returns></returns>
        public static string RemoveLineBreaks(this string theString)
        {
            return theString.Replace("\r", string.Empty).Replace("\n", " ").Replace("<strong>", string.Empty).Replace("</strong>", string.Empty);
        }

        /// <summary>
        /// Cleans the json.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static string CleanJson(this string target)
        {
            try
            {
                var expectedJson = JObject.Parse(target.Trim());
                return JsonConvert.SerializeObject(expectedJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception cleaning Json.");
                throw ex;
            }
        }

        /// <summary>
        /// Cleans the XML.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static string CleanXml(this string target)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(sb))
                {
                    XDocument xml = XDocument.Parse(target.Trim());
                    xml.Save(writer);
                    writer.Flush();
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception cleaning Xml.");
                throw ex;
            }
        }
    }
}
