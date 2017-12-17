using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Late bound type loader mechanism for the full .net 4 Framework. An alternative implementation is required for the Silverlight framework.
    /// </summary>
    public class FullFrameworkLateBoundTypeLoader : ILateBoundTypeLoader
    {
        public Type LoadType(string looselyDefinedTypeName)
        {
            // For Type.GetType to work, we must ensure the assembly is loaded.
            string assemblyName = looselyDefinedTypeName.Split(',').Last(); //TODO: Need to ensure that the type name is not a fully qualified name
            System.Reflection.Assembly.Load(assemblyName);

            Type looseDefinedType = Type.GetType(looselyDefinedTypeName);
            if (looseDefinedType == null)
            {
                throw new TypeLoadException("Workflow library type " + looselyDefinedTypeName + " cannot be found");
            }

            return looseDefinedType;
        }
    }
}
