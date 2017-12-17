using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.DataTypes
{
    /// <summary>
    /// Silverlight cannot reference System.Drawing.Color so we use this data type within the architecture to represent colours
    /// </summary>
    [DataContract(IsReference = true, Namespace = "http://www.sepura.co.uk/IA")]
    public class GenericColor
    {
        [DataMember]
        public byte A = 0;
        [DataMember]
        public byte R = 0;
        [DataMember]
        public byte G = 0;
        [DataMember]
        public byte B = 0;
    }
}
