using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    public static class CommonEnum
    {

        /// <summary>
        /// Converts a object data item to an enum of the given type, or returns the 
        /// NullEquivalent for the enum if the type cannot cast correctly.
        /// 
        /// Comment : Enum.Parse uses reflection and so is very slow. 
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum to convert</typeparam>
        /// <param name="data">The data from which to get the enum value or name.</param>
        /// <returns>The data parameter converted to the appropriate enumeration</returns>
        public static TEnum EnumFromObject<TEnum>(object data)
        {
            return EnumFromObject<TEnum>(data, false);
        }

        /// <summary>
        /// Converts a object data item to an enum of the given type, or returns the 
        /// NullEquivalent for the enum if the type cannot cast correctly.
        /// 
        /// Comment : Enum.Parse uses reflection and so is very slow. 
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum to convert</typeparam>
        /// <param name="data">The data from which to get the enum value or name.</param>
        /// <param name="ignoreCase">Indicates if the case of <paramref name="data"/> should be ignored</param>
        /// <returns>The data parameter converted to the appropriate enumeration</returns>
        public static TEnum EnumFromObject<TEnum>(object data, bool ignoreCase)
        {
            return (TEnum)EnumFromObject(typeof(TEnum), data, ignoreCase);
        }

        /// <summary>
        /// Converts a object data item to an enum of the given type, or returns the 
        /// NullEquivalent for the enum if the type cannot cast correctly.
        /// 
        /// Comment : Enum.Parse uses reflection and so is very slow. 
        /// </summary>
        /// <param name="enumType">The type of the enumeration</param>
        /// <param name="data">The data from which to get the enum value or name.</param>
        /// <param name="ignoreCase">Indicates if the case of <paramref name="data"/> should be ignored</param>
        /// <returns>The data parameter converted to the appropriate enumeration</returns>
        public static Object EnumFromObject(Type enumType, object data, bool ignoreCase)
        {
            if (data != null && data.GetType() == enumType)
            {   // The supply object is already the correct type, so it is pointless
                // converting it to a string and then using the expensive Enum.Parse
                // method to convert it back!
                return data;
            }

            String dataAsString = Convert.ToString(data);

            try
            {
                return Enum.Parse(enumType, dataAsString, ignoreCase);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// For the passed enum value, get its Description attribute.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="enumValueString">The default to return if it doesnt have a Description Attribute</param>
        /// <returns></returns>
        public static string GetEnumDescription(Type enumType, string enumValueString)
        {
            // Get the reflected field information of the enums Value. This will return a FieldInfo structure pointing to the 
            // enums specific value item, including its attributes.
            System.Reflection.FieldInfo fi = enumType.GetField(enumValueString);
            if (fi != null)
            {
                System.ComponentModel.DescriptionAttribute[] attributes =
                      (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(
                      typeof(System.ComponentModel.DescriptionAttribute), false);

                return (attributes.Length > 0) ? attributes[0].Description : enumValueString;
            }
            else
            {
                return enumValueString;
            }
        }
        /// <summary>
        /// Get a list of the permitted values that would be serialized to and from for the specific enum.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<string> GetEnumValues(Type enumType)
        {
            List<string> enumValues = new List<string>();
            //Enum.GetNames does not exists in Silverlight hence using reflection
            //enumValues.AddRange(Enum.GetNames(enumType));
            System.Reflection.FieldInfo[] infos; 
            infos = enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.FieldInfo fi in infos) 
            {
                enumValues.Add(fi.Name);
            }
            return enumValues;
        }

        /// <summary>
        /// Get a list of the permitted values that would be serialized to and from for the specific enum.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static List<string> EnumValuesAsString<TEnum>()
        {
            return GetEnumValues(typeof(TEnum));
        }

        /// <summary>
        /// Get a list of the enum descriptions for the passed enum type.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<string> GetEnumDescriptions(Type enumType)
        {
            //Enum.GetValues does not exists in Silverlight hence using reflection
            //Array values = Enum.GetValues(enumType);
            System.Reflection.FieldInfo[] infos;
            infos = enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            List<string> descriptions = new List<string>();
            //foreach (object value in values)
            //{
            //    descriptions.Add(
            //        GetEnumDescription((Enum)value));
            //}
            foreach (System.Reflection.FieldInfo fi in infos)
            {
                descriptions.Add(
                    GetEnumDescription((Enum)fi.GetValue(null)));
            }
            return descriptions;
        }

        /// <summary>
        /// Get the enum Value based on the passed DescriptionAttribute value;
        /// </summary>
        /// <param name="enumType">The Type of enum</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// NULL if the passed description does not match a Description attribute on the enum
        /// </returns>
        public static object GetEnumValueFromDescription(Type enumType, string description)
        {
            //Enum.GetValues does not exists in Silverlight hence using reflection
            System.Reflection.FieldInfo[] infos;
            infos = enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.FieldInfo fi in infos)
            {
                string enumDescription = GetEnumDescription(enumType, fi.GetValue(null).ToString());
                if (enumDescription == description)
                {
                    return fi.GetValue(null);
                }
            }
            //foreach (object enumValue in Enum.GetValues(enumType))
            //{
            //    string enumDescription = GetEnumDescription(enumType, enumValue.ToString());
            //    if (enumDescription == description)
            //    {
            //        return enumValue;
            //    }
            //}
            return null;
        }

        /// <summary>
        /// For the passed enum value, get its Description attribute. This enables Enums to be modelled in the Combo Box.
        /// </summary>
        /// <param name="value">The enum value to get the Description attribute for.</param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            return GetEnumDescription(value.GetType(), value.ToString());
        }

    }
}
