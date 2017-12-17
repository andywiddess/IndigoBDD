using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;
using System.IO;
using System.Net;
using System.Web;

namespace Indigo.CrossCutting.Utilities.EventMonitoring
{
    /// <summary>
    /// Define the observer base class to sending email
    /// </summary>
    public abstract class AbstractTemplatedMailEventObserver<T>
        : AbstractEventTypebaseObserver<T>
        where T : AbstractEvent
    {
        #region Members
        private string subject = "User registered information";

        /// <summary>
        /// Gets/Sets the email template virtual path.
        /// </summary>
        /// <remarks>
        /// When template loaded the observer will auto format the template with {username} and {password}.
        /// </remarks>
        public virtual string Templated { get; set; }

        /// <summary>
        /// Get/Sets the email subject
        /// </summary>
        public virtual string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        #endregion

        #region Methods
        protected abstract string GetDestinationAddress(T e);

        /// <summary>
        /// Called when [setting mail].
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="mail">The mail.</param>
        protected virtual void OnSettingMail(T e, MailMessage mail)
        { 
        }

        /// <summary>
        /// Format the html templated email body. This method executed when the email template file was set.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="html">The html text from email template file.</param>
        protected abstract void OnFormatHtmlBody(T e, ref string html);

        /// <summary>
        /// Format the text body for email content.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="text">The text.</param>
        protected abstract void OnFormatTextBody(T e, ref string text);

        /// <summary>
        /// Processes the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        public override void Process(T e)
        {
            var webConfig = WebConfigurationManager.OpenWebConfiguration("~/web.config");
            var smtp = webConfig.GetSection("system.net/mailSettings/smtp") as SmtpSection;

            if (smtp != null)
            {
                if (smtp.Network != null)
                {
                    var from = smtp.From;
                    var to = GetDestinationAddress(e);
                    if (string.IsNullOrEmpty(from) && (string.IsNullOrEmpty(to)))
                        return;

                    var mail = new MailMessage(from, to);
                    mail.Subject = Subject;

                    OnSettingMail(e, mail);

                    if (!string.IsNullOrEmpty(Templated))
                    {
                        mail.IsBodyHtml = true;
                        var bodyHtml = "";
                        bodyHtml = ReadTemplateContent(e);
                        OnFormatHtmlBody(e, ref bodyHtml);
                        mail.Body = bodyHtml;
                    }
                    else
                    {
                        mail.IsBodyHtml = false;
                        var bodyText = "";
                        OnFormatTextBody(e, ref bodyText);
                        mail.Body = bodyText.ToString();
                    }

                    var smtpClient = new SmtpClient();
                    smtpClient.SendAsync(mail, null);
                }
            }
        }

        /// <summary>
        /// Reads the content of the template.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        protected string ReadTemplateContent(T e)
        {
            if (Templated.EndsWith(".cshtml", StringComparison.OrdinalIgnoreCase))
                return ReadWebPageTemplate(e);
            else
                return ReadHtmlTemplate(e);
        }

        /// <summary>
        /// Reads the HTML template.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        protected string ReadHtmlTemplate(T e)
        {
            string fileName = e.HttpContext.Server.MapPath(Templated);

            var bodyHtml = "";
            if (File.Exists(fileName))
            {
                try
                {
                    using (var reader = File.OpenText(fileName))
                    {
                        bodyHtml = reader.ReadToEnd();
                    }
                }
                catch 
                { 
                    return "";
                }
            }

            return bodyHtml;
        }

        /// <summary>
        /// Reads the web page template.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        protected string ReadWebPageTemplate(T e)
        {
            var httpRequest = e.HttpContext.Request;
            string url = httpRequest.Url.Scheme + "://" + httpRequest.Url.Authority;
            if (!httpRequest.ApplicationPath.Equals("/"))
                url += httpRequest.ApplicationPath;

            if (Templated.StartsWith("~/"))
                url = Templated.Replace("~", url);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers["Accept-Encoding"] = "gzip";
            request.Headers["Accept-Language"] = "en-us";
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.AutomaticDecompression = DecompressionMethods.GZip;
            var response = request.GetResponse();
            var htmlContent = "";

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                htmlContent = reader.ReadToEnd();
            }

            return htmlContent;
        }
        #endregion
    }
}
