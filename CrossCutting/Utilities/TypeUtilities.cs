using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Collection of utilities for Type interrogation
    /// </summary>
    public static class TypeUtilities
    {
        /// <summary>
        /// Return whether this Type is a numeric. Disassembled from Microsoft.VisualBasic.Information.IsNumeric()
        /// </summary>
        /// <param name="typ"></param>
        /// <returns></returns>
        public static bool IsNumericType(Type typ)
        {
            if ((typ.IsArray))
            {
                return false;
            }
            switch (Type.GetTypeCode(typ))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            };
            return false;
        }


    }
}
