using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Indigo.CrossCutting.Utilities.Utility
{
    public static class UrlUtility
    {
        private static readonly Regex pingbackLinkRegex = new Regex("<link rel=\"pingback\" href=\"([^\"]+)\" ?/?>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex pingbackLinkRegex_2 = new Regex("<link href=\"([^\"]+)\" rel=\"pingback\"  ?/?>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// The href regex.
        /// </summary>
        private static readonly Regex HrefRegex = new Regex("href=\"(.*)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        ///     Regex used to find the trackback link on a remote web page.
        /// </summary>
        private static readonly Regex TrackbackLinkRegex = new Regex(
            "trackback:ping=\"([^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        ///     Regex used to find all hyperlinks.
        /// </summary>
        private static readonly Regex UrlsRegex = new Regex(
            @"<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Gets all the URLs from the specified string.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>A list of Uri</returns>
        public static IEnumerable<Uri> GetUrlsFromContent(string content)
        {
            var urlsList = new List<Uri>();
            foreach (var url in
                UrlsRegex.Matches(content).Cast<Match>().Select(myMatch => myMatch.Groups["url"].ToString().Trim()))
            {
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    urlsList.Add(uri);
                }
            }

            return urlsList;
        }

        /// <summary>
        /// Examines the web page source code to retrieve the trackback link from the RDF.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The trackback Uri</returns>
        public static Uri GetTrackBackUrlFromContent(string input)
        {
            var url = TrackbackLinkRegex.Match(input).Groups[1].ToString().Trim();
            Uri uri;

            return Uri.TryCreate(url, UriKind.Absolute, out uri) ? uri : null;
        }

        /// <summary>
        /// Gets the content of the pingback URL from.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static Uri GetPingbackUrlFromContent(string input)
        {
            var match = pingbackLinkRegex.Match(input);
            var url = "";
            if (!match.Success)
                match = pingbackLinkRegex_2.Match(input);

            if (!match.Success) 
                return null;

            url = match.Groups[1].ToString().Trim();
            Uri uri;

            return Uri.TryCreate(url, UriKind.Absolute, out uri) ? uri : null;
        }
    }
}
