using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Utility
{
    public class XmlDeserializeException
        : Exception
    {
        #region Members
        string _file = "";
        string _type = "";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDeserializeException" /> class.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="typeString"></param>
        public XmlDeserializeException(string fileName, string typeString)
        {
            _file = fileName;
            _type = typeString;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                return "Deserialize object from \""+_file+"\" as type \""+_type+"\" fail.Mybe the xml document no correct.";
            }
        }
        #endregion
    }
}
