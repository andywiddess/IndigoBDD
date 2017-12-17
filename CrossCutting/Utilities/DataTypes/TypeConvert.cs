using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.DataTypes
{
    /// <summary>
    /// Utility class used for converting objects from one type to another - optionally culture-specific too
    /// </summary>
    public static class TypeConvert
    {
        /// <summary>
        /// Returns a System Object with the specified system Type whose value is equivalent to specified object
        /// Supports custom data types declared in this assembly
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType)
        {
            if (value == null)
            {
                return null;
            }
            // intercept booleans and convert
            if (conversionType == typeof(bool) || conversionType == typeof(Boolean))
            {
                if (value.ToString() == "1")
                {
                    value = true;
                }
                else if (value.ToString() == "0")
                {
                    value = false;
                }
            }

            // Rather than extend the system types to understand our custom types 
            // eplicitly intercept convert here for custom types
            if (conversionType == typeof(Date))
            {
                return Date.FromObject(value);
            }
            if (conversionType == typeof(Time))
            {
                return Time.FromObject(value);
            }
            if (conversionType == typeof(RichText) && value != null)
            {
                return value.ToString();
            }
            if (conversionType == typeof(enumRecurrenceType))
            {
                return (enumRecurrenceType)Enum.Parse(typeof(enumRecurrenceType), value.ToString(), true);
            }
            if (conversionType == typeof(enumRecurrencePattern))
            {
                return (enumRecurrencePattern)Enum.Parse(typeof(enumRecurrencePattern), value.ToString(), true);
            }

            if (conversionType.IsValueType)
            {
                //Silverlight does not support ChangeType overload with 2 arguments - hence setting the 3rd parameter as CurrentCulture (As per Reflector)
                return Convert.ChangeType(value, conversionType, System.Threading.Thread.CurrentThread.CurrentCulture);
            }
            else
            {
                return value; // The caller will have to assume this object can be directly cast onto the relevant reference type.
            }
        }

        /// <summary>
        /// Returns a System Object with the specified system Type whose value is equivalent to specified object
        /// Supports custom data types declared in this assembly
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <param name="culture">Typically passed in as Current UI Culture when parsing data typed into a text box.</param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            if (value.ToString() == "") return null;

            // intercept booleans and convert
            if (conversionType == typeof(bool) || conversionType == typeof(Boolean))
            {
                if (value.ToString() == "1")
                {
                    value = true;
                }
                else if (value.ToString() == "0")
                {
                    value = false;
                }
            }


            // Rather than extend the system types to understand our custom types 
            // eplicitly intercept convert here for custom types
            if (conversionType == typeof(Date))
            {
                return Date.FromObject(value);
            }
            if (conversionType == typeof(Time))
            {
                return Time.FromObject(value);
            }
            if (conversionType == typeof(RichText) && value != null)
            {
                return value.ToString();
            }
            if (conversionType.IsValueType)
            {

                //Silverlight does not support ChangeType overload with 2 arguments - hence setting the 3rd parameter as CurrentCulture (As per Reflector)
                return Convert.ChangeType(value, conversionType, culture);
            }
            else
            {
                return value; // The caller will have to assume this object can be directly cast onto the relevant reference type.
            }
        }

        /// <summary>
        /// Gets the System.Type with the specified Name
        /// This is effecticely a shim for Type.GetType - but it will additioanlly provide the types declared in this assembly 
        /// when a fully qualified name is not provided. 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns>The type</returns>
        /// <remarks>
        /// Replacing calls to this function with direct calls to Type.GetType will NOT work in all cases because of its behaviour regarding
        /// names which are not fully qualified.
        /// </remarks>
        public static Type GetType(string typeName)
        {
            return Type.GetType(typeName);
        }

    
    }
}
