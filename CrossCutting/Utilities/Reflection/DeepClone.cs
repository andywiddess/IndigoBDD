using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Indigo.CrossCutting.Utilities.Reflection
{
    /// <summary>
    /// Performs a Deep Copy
    /// </summary>
    public static class DeepCopy
    {
        /// <summary>
        /// Clones the object.
        /// </summary>
        /// <param name="opSource">The op source.</param>
        /// <returns></returns>
        public static object CloneObject(object opSource)
        {
            //grab the type and create a new instance of that type
            Type opSourceType = opSource.GetType();
            object opTarget = Activator.CreateInstance(opSourceType);

            //grab the properties
            PropertyInfo[] opPropertyInfo = opSourceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //iterate over the properties and if it has a 'set' method assign it from the source TO the target
            foreach (PropertyInfo item in opPropertyInfo)
            {
                if (item.CanWrite)
                {
                    //value types can simply be 'set'
                    if (item.PropertyType.IsValueType || item.PropertyType.IsEnum || item.PropertyType.Equals(typeof(System.String)))
                    {
                        item.SetValue(opTarget, item.GetValue(opSource, null), null);
                    }
                    //object/complex types need to recursively call this method until the end of the tree is reached
                    else
                    {
                        object opPropertyValue = item.GetValue(opSource, null);
                        if (opPropertyValue == null)
                        {
                            item.SetValue(opTarget, null, null);
                        }
                        else
                        {
                            item.SetValue(opTarget, CloneObject(opPropertyValue), null);
                        }
                    }
                }
            }

            //return the new item
            return opTarget;
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="S">The s.</param>
        /// <param name="T">The t.</param>
        public static void CopyTo(this object S, object T)
        {
            foreach (var pS in S.GetType().GetProperties())
            {
                foreach (var pT in T.GetType().GetProperties())
                {
                    if (pT.Name != pS.Name)
                        continue;

                    try
                    {
                        (pT.GetSetMethod()).Invoke(T, new object[] { pS.GetGetMethod().Invoke(S, null) });
                    }
                    catch (Exception ex)
                    {
                        throw new KeyNotFoundException("Unable to translate data, object not found");
                    }
                }
            };
        }

        public static object CopyTo(object obj)
        {
            if (obj == null)
                return null;
            Type type = obj.GetType();

            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }
            else if (type.IsArray)
            {
                Type elementType = Type.GetType(
                     type.FullName.Replace("[]", string.Empty));
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(CopyTo(array.GetValue(i)), i);
                }

                return Convert.ChangeType(copied, obj.GetType());
            }
            else if (type.Name.Equals("PCStateMan"))
            {
            	// Ignore
            }
            else
            {
                if (type.IsClass)
                {

                    object toret = Activator.CreateInstance(obj.GetType());
                    FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (FieldInfo field in fields)
                    {
                        object fieldValue = field.GetValue(obj);
                        if (fieldValue == null)
                            continue;

                        field.SetValue(toret, CopyTo(fieldValue));
                    }

                    PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (PropertyInfo property in properties)
                    {
                        object fieldValue = property.GetValue(obj);
                        if (fieldValue == null)
                            continue;

                        property.SetValue(toret, CopyTo(fieldValue));
                    }

                    return toret;
                }
                else
                    throw new ArgumentException("Unknown type");
            }
            
            return null;
        }
    }
}
