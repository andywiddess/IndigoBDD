using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.DataTypes
{
    [DataContract(Namespace = "http://www.sepura.co.uk/IA")]
    public enum enumRecurrencePattern
    {
        [EnumMember]
        NotApplicable = 0,
        [EnumMember]
        First = 1,
        [EnumMember]
        Second = 2,
        [EnumMember]
        Third = 3,
        [EnumMember]
        Fourth = 4,
        [EnumMember]
        Last = 5
    }
}
