using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Interface implemented by Silverlight and non-Silverlight late bound type resolvers. These classes take a loosely defined type name such as 
    /// "IAServerWorkflow.MyClass, IAServerWorkflow" and return the Type that this points to. In Silverlight this process is more complex since we need to 
    /// parse the AssemblyParts that have been dynamically downloaded as part of the XAP.
    /// </summary>
    public interface ILateBoundTypeLoader
    {
        Type LoadType(string looselyDefinedTypeName);
    }
}
