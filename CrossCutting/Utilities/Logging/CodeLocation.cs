using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Specifies a place in the source code or class,
    /// where an logging event occurred.
    /// </summary>
    [DataContract(Namespace = "http://www.sepura.co.uk/Logging/2014/07/"), Serializable]
    public class CodeLocation
        : ICodeLocation,
          IExtensibleDataObject
    {
        #region Members
        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        [DataMember]
        public string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        [DataMember]
        public string MethodName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        [DataMember]
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the line number of the location.
        /// </summary>
        /// <value>The line number.</value>
        [DataMember]
        public int LineNumber
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

        #region Public Methods
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents
        /// the current <see cref="CodeLocation"/>.
        /// The format is &lt;FileName&gt;(&lt;LineNumber&gt;):&lt;ClassName&gt;.&lt;MethodName&gt;
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="CodeLocation"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}({1}): {2}.{3}", FileName, LineNumber, ClassName, MethodName);
        }
        #endregion
    }
}
