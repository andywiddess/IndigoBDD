using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;

namespace Sepura.ApplicationServer
{
    /// <summary>
    /// Standard hard coded set of settings for WCF
    /// 
    /// In this scenario we are using binary encoding instead of WsHTTPBinding. See the link below for comparison.
    /// http://www.zamd.net/2008/06/18/SOAPMessageSizeOptimizationEncodingVsCompression.aspx
    /// </summary>
    public static class WCFSettings
    {
        /// <summary>
        /// Get the binding used in the default scenario for transmitting this entity to its host web service.
        /// This eliminates the need for a client app.config file tediously listing all the binding types for every service.
        /// 
        /// The Binding here can be used also any client consumer.
        /// 
        /// Passing in the URL is required to decide whether to use HTTPS (transport) security or HTTP (none) security.
        /// </summary>
        public static Binding Binding(string url, string securityServerUrl, bool secured)
        {
            if (url.ToLower().StartsWith("net.tcp"))
            {
                return netTcpBinding(url, securityServerUrl, secured);
            }
            else
            {
                return httpBinding(url, securityServerUrl, secured);
            }
        }

        private static Binding httpBinding(string url, string securityServerUrl, bool secured)
        {
            // HTTP Binding - tested but much bulkier than NetTCP.
            WS2007FederationHttpBinding binding = new WS2007FederationHttpBinding();
            // If it's a secured connection, then we want to bind some security in there.
            if (secured)
            {
                binding.Security.Message.IssuerAddress = new EndpointAddress(securityServerUrl);
                binding.Security.Message.IssuerMetadataAddress = new EndpointAddress(securityServerUrl + "/mex");
                ////*********************************************************************************************
                //binding.Security.Message.IssuedKeyType = System.IdentityModel.Tokens.SecurityKeyType.BearerKey;
                ////*********************************************************************************************
                WS2007HttpBinding issueBinding = new WS2007HttpBinding();
                issueBinding.Security.Message.NegotiateServiceCredential = true;
                issueBinding.UseDefaultWebProxy = true;
                issueBinding.BypassProxyOnLocal = true;
                issueBinding.Security.Message.EstablishSecurityContext = false;
                binding.Security.Message.IssuerBinding = issueBinding;
            }
            else
                binding.Security.Mode = WSFederationHttpSecurityMode.None;
            // Timeouts etc
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            // Quotas
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            // Proxy settings
            binding.UseDefaultWebProxy = true;
            binding.BypassProxyOnLocal = true;
            // If it's HTTPS, then we want the transport encrypted too
            if (url.ToUpper().StartsWith("HTTPS"))
                binding.Security.Mode = WSFederationHttpSecurityMode.TransportWithMessageCredential;
            //for now need to create a custom binding based on TCP to support claims based security
            //http://weblogs.asp.net/cibrax/archive/2008/04/21/federation-over-tcp-with-wcf.aspx
            return binding;
        }

        private static Binding netTcpBinding(string url, string securityServerUrl, bool secured)
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;

            binding.TransactionFlow = false;
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;

            binding.ReliableSession.Enabled = false;
            binding.ReliableSession.Ordered = true;
            binding.ReliableSession.InactivityTimeout = new TimeSpan(0, 10, 0);

            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = 16384;
            binding.ReaderQuotas.MaxBytesPerRead = 4096;
            
            return binding;
        }

        /// <summary>
        /// Get the binding used for the Security Server
        /// The same binding will be used for the server and client.
        ///
        /// Passing in the URL is required to decide whether to use HTTPS (transport) security or HTTP (none) security.
        /// passing in the type of message security required anon = true/false
        /// </summary>
        public static Binding BindingForSecurityServer(string url, bool anon)
        {
            // Use WS2007HttpBinding
            WS2007HttpBinding binding = new WS2007HttpBinding();
            // Timeout settings etc
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            // Quotas
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            // If it's SSL...
            if (url.ToUpper().StartsWith("HTTPS"))
            {
                binding.Security.Mode = SecurityMode.TransportWithMessageCredential;
                // The SSL certificate should have been bound to the port we're using with HttpCfg
                // SO this binding should _probably_ be MessageCredentialType.Certificate
                binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                // Need to check whether the transport credential type needs setting too!
                //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            }
            else
            {
                // We're not using transport security - it's Message only.
                binding.Security.Mode = SecurityMode.Message;
                // No transport security.
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                // And set the message client credential type
                if (anon)
                    binding.Security.Message.ClientCredentialType = MessageCredentialType.None;
                else
                    binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            }
            //TODO: Confirm from Iain about these settings
            binding.Security.Message.EstablishSecurityContext = false;
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Message.NegotiateServiceCredential = true;
            binding.UseDefaultWebProxy = true;
            binding.BypassProxyOnLocal = true;
            return binding;
        }
        /// <summary>
        /// Get the basic binding used for the Security Server
        /// This eliminates the need for a client app.config file tediously listing all the binding types for every service.
        /// 
        /// The Binding here can be used also any client consumer.
        /// 
        /// Passing in the URL is required to decide whether to use HTTPS (transport) security or HTTP (none) security.
        /// </summary>
        public static Binding BindingForSecurityServerBasic(string url)
        {
            WSHttpBinding binding = new WSHttpBinding(SecurityMode.Message);

            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.NegotiateServiceCredential = true;

            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            binding.UseDefaultWebProxy = true;
            binding.BypassProxyOnLocal = true;
            return binding;
        }
    }
}
