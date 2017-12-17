using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Workflow
{
    /// <summary>
    /// Enumeration of  supported dataSources
    /// </summary>
    [DataContract(Namespace = "http://www.sepura.co.uk/IA")]
    public enum DataSourceTypeEnum
    {
        [EnumMember]
        SQL = 0,
        [EnumMember]
        CSV,
        [EnumMember]
        XML,
        [EnumMember]
        ODBC,
        [EnumMember]
        UNKNOWN
    }
}
