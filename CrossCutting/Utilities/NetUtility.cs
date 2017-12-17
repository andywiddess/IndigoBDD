using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace Indigo.CrossCutting.Utilities.Utility
{
    public class NetUtility
    {
        /// <summary>
        /// Download the specified url as string
        /// </summary>
        /// <param name="url">Specified the target url to download</param>
        /// <returns>Return the download result as string.</returns>
        public static string Download(Uri url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers["Accept-Encoding"] = "gzip";
                request.Headers["Accept-Language"] = "en-us";
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (var response = request.GetResponse())
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }
    }
}