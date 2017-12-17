using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Client Information Object
    /// </summary>
    [DataContract(Namespace = "http://www.sepura.co.uk/Logging/2014/07/"), Serializable]
    public class ClientInfo
        : IClientInfo,
          IExtensibleDataObject
    {
        #region Members
        /// <summary>
        /// Gets or sets the structure that contains extra data.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Runtime.Serialization.ExtensionDataObject"/>
        /// that contains data that is not recognized as belonging to the data contract.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ExtensionDataObject ExtensionData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the log that the log entry
        /// is to be written under. <seealso cref="ILogEntry"/>
        /// </summary>
        /// <value>The name of the log.</value>
        [DataMember]
        public string LogName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the user that the request for the log entry
        /// was made under.
        /// </summary>
        /// <value>The name of the user.</value>
        [DataMember]
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the machine that the request for the log entry
        /// was made.
        /// </summary>
        /// <value>The name of the machine.</value>
        [DataMember]
        public string MachineName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL from where the log request was made.
        /// </summary>
        /// <value>The URL of the log request.</value>
        [DataMember]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the ip address of the client.
        /// </summary>
        /// <value>The ip address of the client.</value>
        [DataMember]
        public string IPAddress
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientInfo"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        public ClientInfo(IClientInfo info)
        {
            LogName = info.LogName;
            MachineName = info.MachineName;
            Url = info.Url;
            UserName = info.UserName;
            IPAddress = info.IPAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientInfo"/> class.
        /// </summary>
        public ClientInfo()
        {
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Log Name: {0}, Username: {1}, Machine Name: {2}, URL: {3}, IP Address: {4}",
                                 LogName, UserName, MachineName, Url, IPAddress);
        }
        #endregion
    }
}
