using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Provides information to allow the recreation
    /// of an <see cref="Exception"/>.
    /// </summary>
    [DataContract(Namespace = "http://www.sepura.co.uk/Logging/2014/07/"),
     Serializable]
    public class ExceptionLog
        : IExceptionLog,
          IExtensibleDataObject
    {
        #region Members
        /// <summary>
        /// Gets or sets the name of the type of <see cref="Exception"/>.
        /// </summary>
        /// <value>The name of the type.</value>
        [DataMember]
        public string TypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the message property of the exception.
        /// </summary>
        /// <value>The message.</value>
        [DataMember]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source property of the exception.
        /// </summary>
        /// <value>The exception Source.</value>
        [DataMember]
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the stack trace of the exception.
        /// </summary>
        /// <value>The exception stack trace.</value>
        [DataMember]
        public string StackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HelpLink of the exception.
        /// </summary>
        /// <value>The exception HelpLink.</value>
        [DataMember]
        public string HelpLink
        {
            get;
            set;
        }

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
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionLog"/> class.
        /// </summary>
        public ExceptionLog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionLog"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ExceptionLog(Exception exception)
        {
            TypeName = exception.GetType().AssemblyQualifiedName;
            Message = exception.Message;
            Source = exception.Source;
            StackTrace = exception.StackTrace;
            HelpLink = exception.HelpLink;
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
            return string.Format("{0}  Message: {1} HelpLink: {2}{3}StackTrace: {4}",
                TypeName,
                Message,
                HelpLink,
                Environment.NewLine,
                StackTrace);
        }
        #endregion
    }
}
