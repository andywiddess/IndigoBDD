using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities
{
    [DataContract(IsReference=true, Namespace="http://www.sepura.co.uk/IA")]
    public class ValidationError
    {
        [DataMember]
        public string Message = "";

        public ValidationError(string message)
        {
            this.Message = message;
        }

        public override string ToString()
        {
            return this.Message;
        }
    }
}
