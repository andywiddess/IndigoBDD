using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities
{
    [CollectionDataContract(Namespace="SepuraResearch")]
    public class ValidationErrorCollection : List<ValidationError>
    {
        public void AddMessage(string message)
        {
            this.Add(new ValidationError(message));
        }

        public void Add(string message)
        {
            this.Add(new ValidationError(message));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ValidationError error in this)
            {
                sb.AppendLine(error.Message);
            }
            return sb.ToString();
        }
    }
}
