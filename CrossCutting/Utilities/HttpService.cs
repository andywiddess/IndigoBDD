using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

using Indigo.CrossCutting.Utilities;
using Indigo.CrossCutting.Utilities.CheckExpression;
using Indigo.CrossCutting.Utilities.Logging;
using Indigo.CrossCutting.Utilities.PerformanceCounters;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Salesforce Unit of Work Pattern Implementation
    /// </summary>
    public static class HttpServices
    {
        #region Members
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the specified sobject.
        /// </summary>
        /// <param name="instanceUrl">The instance URL.</param>
        /// <param name="restUrlFragment">The rest URL fragment.</param>
        /// <param name="sobject">The sobject.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static string Get(string instanceUrl, string restUrlFragment, string sobject, string id)
        {
            string result = string.Empty;

            try
            {
                result = HttpGet(string.Format("{0}{1}/{2}/{3}", instanceUrl, restUrlFragment, sobject, id), string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Gets the describe.
        /// </summary>
        /// <param name="instanceUrl">The instance URL.</param>
        /// <param name="restUrlFragment">The rest URL fragment.</param>
        /// <param name="sobject">The sobject.</param>
        /// <returns></returns>
        public static string GetDescribe(string instanceUrl, string restUrlFragment, string sobject)
        {
            string result = string.Empty;

            try
            {
                result = HttpGet(string.Format("{0}{1}/{2}/describe", instanceUrl, restUrlFragment, sobject), string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Returns a list of matching Id's from the API.
        /// </summary>
        /// <param name="instanceUrl">The instance URL.</param>
        /// <param name="restUrlFragment">The rest URL fragment.</param>
        /// <param name="clause">The clause.</param>
        /// <returns></returns>
        public static string Find(string instanceUrl, string restUrlFragment, string clause)
        {
            string result = string.Empty;

            try
            {
                result = HttpGet(string.Format("{0}{1}/search/?q=FIND+%7B{2}%7D", instanceUrl, restUrlFragment, clause), string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Returns a list of matching Id's from the API.
        /// </summary>
        /// <param name="instanceUrl">The instance URL.</param>
        /// <param name="restUrlFragment">The rest URL fragment.</param>
        /// <param name="sobject">The sobject.</param>
        /// <returns></returns>
        public static string Describe(string instanceUrl, string restUrlFragment, string sobject)
        {
            string result = string.Empty;

            try
            {
                result = HttpGet(string.Format("{0}{1}/{2}/describe", instanceUrl, restUrlFragment, sobject), string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Updates a dataset from the API for the supplied JSON aka a REST patch request.
        /// </summary>
        /// <param name="instanceUrl">The instance URL.</param>
        /// <param name="restUrlFragment">The rest URL fragment.</param>
        /// <param name="sobject">The sobject.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="updatedJson">The updated json.</param>
        /// <returns>
        /// Success Indictor.
        /// </returns>
        public static string Update(string instanceUrl, string restUrlFragment, string sobject, string id, string updatedJson)
        {
            string result = "";

            try
            {
                result = HttpPatch(string.Format("{0}{1}/{2}/{3}", instanceUrl, restUrlFragment, sobject, id), updatedJson);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Updates a dataset from the API for the supplied JSON aka a REST patch request.
        /// </summary>
        /// <param name="instanceUrl">The instance URL.</param>
        /// <param name="restUrlFragment">The rest URL fragment.</param>
        /// <param name="sobject">The sobject.</param>
        /// <param name="updatedJson">The updated json.</param>
        /// <returns>
        /// Success Indictor.
        /// </returns>
        public static string Insert(string instanceUrl, string restUrlFragment, string sobject, string updatedJson)
        {
            string result = "";

            try
            {
                result = HttpPost(string.Format("{0}{1}/{2}", instanceUrl, restUrlFragment, sobject), updatedJson, "application/json");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Deletes a dataset from the API for the supplied JSON aka a REST patch request.
        /// </summary>
        /// <param name="instanceUrl">The instance URL.</param>
        /// <param name="restUrlFragment">The rest URL fragment.</param>
        /// <param name="sobject">The sobject.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Success Indictor.
        /// </returns>
        public static string Delete(string instanceUrl, string restUrlFragment, string sobject, string id)
        {
            string result = "";

            try
            {
                result = HttpDelete(string.Format("{0}{1}/{2}/{3}", instanceUrl, restUrlFragment, sobject, id), string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Updates a dataset from the API for the supplied JSON aka a REST patch request.
        /// </summary>
        /// <param name="instanceUrl">The instance URL.</param>
        /// <param name="restUrlFragment">The rest URL fragment.</param>
        /// <param name="sobject">The sobject.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="updatedJson">The updated json.</param>
        public static void Put(string instanceUrl, string restUrlFragment, string sobject, string id, string updatedJson)
        {
            try
            {
                HttpPut(string.Format("{0}{1}/{2}", instanceUrl, restUrlFragment, sobject, id), updatedJson);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// HTTPs the post.
        /// </summary>
        /// <param name="URI">The URI.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        public static string HttpPost(string URI, string Parameters, string contentType)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            System.Net.WebResponse resp;
            DateTime perfFrom = DateTime.Now;
            string result = string.Empty;

            try
            {
                Log.Debug($"Calling {URI}");

                if (string.IsNullOrEmpty(contentType))
                    contentType = "application/x-www-form-urlencoded";

                System.Net.ServicePointManager.Expect100Continue = false;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                //WebProxy webProxy = new WebProxy("localhost.", 8888);

                //req.Proxy = webProxy;

                req.ContentType = contentType;
                req.Method = "POST";
                if (!URI.Equals("https://login.sepura.com/services/oauth2/revoke", StringComparison.CurrentCultureIgnoreCase))
                    req.Headers.Add("Authorization", "OAuth " + "AccessToken");

                // Add parameters to post
                byte[] data = !string.IsNullOrEmpty(Parameters) ? System.Text.Encoding.UTF8.GetBytes(Parameters) : new byte[0];
                req.ContentLength = data.Length;
                System.IO.Stream os = req.GetRequestStream();
                os.Write(data, 0, data.Length);
                os.Close();

                // Do the post and get the response.
                resp = req.GetResponse();
                if (resp == null)
                    return null;

                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

                result = sr.ReadToEnd().Trim();
            }
            catch (WebException e)
            {
                var msg = $"{e.Message}{(e.InnerException != null ? string.Format("\n{0}", e.InnerException.Message) : "")}";
                Log.Error(msg);
                throw new Exception(msg, e);
            }

            showQueryPerformance(perfFrom, "HttpPost", URI);
            return result;
        }

        /// <summary>
        /// HTTPs the post.
        /// </summary>
        /// <param name="URI">The URI.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <param name="sessionid">The sessionid.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        public static string HttpPost(string URI, string Parameters, string sessionid, string contentType)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            System.Net.WebResponse resp;
            DateTime perfFrom = DateTime.Now;
            string result = string.Empty;

            try
            {
                Log.Debug($"Calling {URI}");

                if (string.IsNullOrEmpty(sessionid))
                    sessionid = "";
                if (string.IsNullOrEmpty(contentType))
                    contentType = "application/x-www-form-urlencoded";

                System.Net.ServicePointManager.Expect100Continue = false;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                req.ContentType = contentType;
                req.Method = "POST";

                if (!URI.Equals("https://login.sepura.com/services/oauth2/revoke", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (sessionid != string.Empty)
                    {
                        req.Headers.Add("X-SFDC-Session", sessionid);
                    }
                    else
                    {
                        req.Headers.Add("Authorization", "OAuth " + "AccessToken");
                    }
                }

                // Add parameters to post
                byte[] data = !string.IsNullOrEmpty(Parameters) ? System.Text.Encoding.UTF8.GetBytes(Parameters) : new byte[0];
                req.ContentLength = data.Length;
                System.IO.Stream os = req.GetRequestStream();
                os.Write(data, 0, data.Length);
                os.Close();

                // Do the post and get the response.
                resp = req.GetResponse();
                if (resp == null)
                    return null;

                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

                result = sr.ReadToEnd().Trim();

            }
            catch (WebException e)
            {
                var msg = $"{e.Message}{(e.InnerException != null ? string.Format("\n{0}", e.InnerException.Message) : "")}";
                Log.Error(msg);
                throw new Exception(msg, e);
            }

            showQueryPerformance(perfFrom, "HttpPost", URI);
            return result;
        }

        /// <summary>
        /// HTTPs the patch.
        /// </summary>
        /// <param name="URI">The URI.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        public static string HttpPatch(string URI, string Parameters)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI + "?_HttpMethod=PATCH");
            System.Net.WebResponse resp;
            DateTime perfFrom = DateTime.Now;
            string result = string.Empty;

            try
            {
                Log.Debug($"Calling {URI}");
                System.Net.ServicePoint sp = ServicePointManager.FindServicePoint(new Uri(URI + "?_HttpMethod=PATCH"));
                sp.Expect100Continue = false;

                req.ContentType = "application/json";
                req.Method = "POST";

                req.Headers.Add("Authorization", "OAuth " + "AccessToken");

                // Add parameters to post
                byte[] data = !string.IsNullOrEmpty(Parameters) ? System.Text.Encoding.UTF8.GetBytes(Parameters) : new byte[0];
                req.ContentLength = data.Length;
                System.IO.Stream os = req.GetRequestStream();
                os.Write(data, 0, data.Length);
                os.Close();

                // Do the post and get the response.
                resp = req.GetResponse();
                if (resp != null)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    result = sr.ReadToEnd().Trim();
                }
            }
            catch (WebException e)
            {
                var msg = $"{e.Message}{(e.InnerException != null ? string.Format("\n{0}", e.InnerException.Message) : "")}";
                Log.Error(msg);
                throw new Exception(msg, e);
            }

            showQueryPerformance(perfFrom, "HttpPatch", URI);
            return result;
        }

        /// <summary>
        /// HTTPs the delete.
        /// </summary>
        /// <param name="URI">The URI.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        public static string HttpDelete(string URI, string Parameters)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI + "?_HttpMethod=DELETE");
            System.Net.WebResponse resp;
            DateTime perfFrom = DateTime.Now;
            string result = string.Empty;

            try
            {
                Log.Debug($"Calling {URI}");
                System.Net.ServicePoint sp = ServicePointManager.FindServicePoint(new Uri(URI + "?_HttpMethod=DELETE"));
                sp.Expect100Continue = false;

                req.ContentType = "application/json";
                req.Method = "POST";

                req.Headers.Add("Authorization", "OAuth " + "AccessToken");

                // Add parameters to post
                byte[] data = !string.IsNullOrEmpty(Parameters) ? System.Text.Encoding.UTF8.GetBytes(Parameters) : new byte[0];
                req.ContentLength = data.Length;
                System.IO.Stream os = req.GetRequestStream();
                os.Write(data, 0, data.Length);
                os.Close();

                // Do the post and get the response.
                resp = req.GetResponse();
                if (resp == null)
                    return null;

                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                result = sr.ReadToEnd().Trim();
            }
            catch (WebException e)
            {
                var msg = $"{e.Message}{(e.InnerException != null ? string.Format("\n{0}", e.InnerException.Message) : "")}";
                Log.Error(msg);
                throw new Exception(msg, e);
            }

            showQueryPerformance(perfFrom, "HttpDelete", URI);
            return result;
        }


        /// <summary>
        /// HTTPs the put.
        /// </summary>
        /// <param name="URI">The URI.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        public static string HttpPut(string URI, string Parameters)
        {
            System.Net.WebResponse resp;
            DateTime perfFrom = DateTime.Now;
            string result = string.Empty;

            try
            {
                Log.Debug($"Calling {URI}");
                System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
                System.Net.ServicePoint sp = ServicePointManager.FindServicePoint(new Uri(URI));
                sp.Expect100Continue = false;

                req.Headers.Add("Authorization", "OAuth " + "AccessToken");
                req.Headers.Add("X-PrettyPrint:1");
                req.ContentType = "application/json";
                req.Method = "POST";


                // Add parameters to post
                byte[] data = !string.IsNullOrEmpty(Parameters) ? System.Text.Encoding.UTF8.GetBytes(Parameters) : new byte[0];
                req.ContentLength = data.Length;
                System.IO.Stream os = req.GetRequestStream();
                os.Write(data, 0, data.Length);
                os.Close();

                // Do the post and get the response.
                resp = req.GetResponse();
                if (resp == null)
                    return null;

                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                result = sr.ReadToEnd().Trim();
            }
            catch (WebException e)
            {
                var msg = $"{e.Message}{(e.InnerException != null ? string.Format("\n{0}", e.InnerException.Message) : "")}";
                Log.Error(msg);
                throw new Exception(msg, e);
            }

            showQueryPerformance(perfFrom, "HttpPut", URI);
            return result;
        }

        /// <summary>
        /// HTTPs the get.
        /// </summary>
        /// <param name="URI">The URI.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        public static string HttpGet(string URI, string Parameters)
        {
            DateTime perfFrom = DateTime.Now;
            string result = null;

            try
            {
                Log.Debug($"Calling {URI}");
                System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
                req.Method = "GET";
                req.Headers.Add("Authorization", "OAuth " + "AccessToken");
                req.Proxy = null;

                using (WebResponse resp = req.GetResponse())
                {
                    if (resp != null)
                    {
                        using (Stream s = resp.GetResponseStream())
                        {
                            using (var sr = new StreamReader(s, Encoding.UTF8))
                            {
                                result = sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (WebException e)
            {
                var msg = $"{e.Message}{(e.InnerException != null ? string.Format("\n{0}", e.InnerException.Message) : "")}";
                Log.Error(msg);
                throw new Exception(msg, e);
            }

            showQueryPerformance(perfFrom, "HttpGet", URI);
            return result;
        }

        /// <summary>
        /// HTTPs the get report.
        /// </summary>
        /// <param name="URI">The URI.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static string HttpGetReport(string URI, string Parameters)
        {
            DateTime perfFrom = DateTime.Now;
            string result = null;

            try
            {
                Log.Debug($"Calling {URI}");
                System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
                req.Method = "GET";
                req.Headers.Add("Authorization", "OAuth " + "AccessToken");
                req.Proxy = null;

                using (WebResponse resp = req.GetResponse())
                {
                    if (resp != null)
                    {
                        using (Stream s = resp.GetResponseStream())
                        {
                            using (var sr = new StreamReader(s, Encoding.UTF8))
                            {
                                result = sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (WebException e)
            {
                var msg = $"{e.Message}{(e.InnerException != null ? string.Format("\n{0}", e.InnerException.Message) : "")}";
                Log.Error(msg);
                throw new Exception(msg, e);
            }

            showQueryPerformance(perfFrom, "HttpGet", URI);
            return result;
        }

        /// <summary>
        /// HTTPs the get with no authorize.
        /// </summary>
        /// <param name="URI">The URI.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static string HttpGetWithNoAuthorize(string URI, string Parameters)
        {
            DateTime perfFrom = DateTime.Now;
            string result = string.Empty;

            try
            {
                Log.Debug($"Calling {URI}");
                System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
                req.Method = "GET";

                req.Proxy = null;
                using (var response = (HttpWebResponse)req.GetResponse())
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
                    result = sr.ReadToEnd().Trim();
                }
            }
            catch (WebException e)
            {
                var msg = $"{e.Message}{(e.InnerException != null ? string.Format("\n{0}", e.InnerException.Message) : "")}";
                Log.Error(msg);
                throw new Exception(msg, e);
            }

            showQueryPerformance(perfFrom, "HttpGetWithNoAuthorize", URI);
            return result;
        }

        /// <summary>
        /// HTTPs the get XML.
        /// </summary>
        /// <param name="URI">The URI.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns></returns>
        public static string HttpGetXml(string URI, string Parameters)
        {
            DateTime perfFrom = DateTime.Now;
            string result = string.Empty;

            try
            {
                Log.Debug($"Calling {URI}");
                System.Net.HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create(URI);
                req.Method = "GET";
                req.Accept = "application/xml";
                req.ContentType = "application/xml; charset=UTF-8";
                req.Headers.Add("Authorization", "OAuth " + "AccessToken");

                req.Proxy = null;
                using (var response = (HttpWebResponse)req.GetResponse())
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
                    result = sr.ReadToEnd().Trim();
                }
            }
            catch (WebException e)
            {
                var msg = $"{e.Message}{(e.InnerException != null ? string.Format("\n{0}", e.InnerException.Message) : "")}";
                Log.Error(msg);
                throw new Exception(msg, e);
            }

            showQueryPerformance(perfFrom, "HttpGetXml", URI);
            return result;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Shows the query performance.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="source">The source.</param>
        /// <param name="URI">The URI.</param>
        private static void showQueryPerformance(DateTime fromDate, string source, string URI)
        {
            TimeSpan perfTo = DateTime.Now.Subtract(fromDate);
            Log.Debug($"{source}: {perfTo.Seconds}.{perfTo.Milliseconds}ms to process {URI}");
        }
        #endregion
    }
}