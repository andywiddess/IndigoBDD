using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.DataTypes
{
    [DataContract(Namespace = "http://www.sepura.co.uk/IA")]
    public enum enumRecurrenceType
    {
        [EnumMember]
        NotApplicable = 0,
        [EnumMember]
        Daily = 1,
        [EnumMember]
        Weekly = 7,
        [EnumMember]
        Monthly = 31,
        [EnumMember]
        Yearly = 365
    }
}
