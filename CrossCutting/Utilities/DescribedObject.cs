using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// A class representing an object which has a description which the user will see at runtime. Typically used for wrappering a Type in a combo box.
    /// </summary>
    //[DataContract(IsReference=true, Namespace="http://www.sepura.co.uk/IA")]
    public class DescribedObject<TObject>
    {
        [System.Runtime.Serialization.DataMember]
        public TObject Object { get; set; }
        [System.Runtime.Serialization.DataMember]
        public string Description { get; set; }

        public DescribedObject(TObject containedObject, string description)
        {
            this.Object = containedObject;
            this.Description = description;
        }

        public override string ToString()
        {
            return this.Description + " [" + this.Object.ToString() + "]";
        }
    }
}
