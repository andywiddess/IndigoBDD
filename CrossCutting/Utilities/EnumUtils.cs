#region Header
// ---------------------------------------------------------------------------
// Sepura - Commercially Confidential.
// 
// Indigo.CrossCutting.Utilities.EnumUtils
// Enumeration Extension Methods
//
// Copyright (c) 2016 Sepura Plc
// All Rights reserved.
//
// $Id:  $ :
// ---------------------------------------------------------------------------
#endregion

using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Indigo.CrossCutting.Utilities
{
	/// <summary>
	/// enum utilities. 
	/// - converts from a [Description()] to an enum value
	/// - grabs the [Description()] from an enum value
	/// </summary>
    public class EnumUtils<T>
    {
        #region Public Methods
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="defDesc">The definition desc.</param>
        /// <returns></returns>
        public static string GetDescription(T enumValue, string defDesc)
        {
            string result = defDesc;

            try
            {
                FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());
                if (fi != null)
                {
                    object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (attrs != null &&
                        attrs.Length > 0)
                        result =((DescriptionAttribute)attrs[0]).Description;
                }
            }
            catch (Exception)
            {
                // TOOD: Log Exception
            }

            return result;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns></returns>
        public static string GetDescription(T enumValue)
        {
            return GetDescription(enumValue, string.Empty);
        }

        /// <summary>
        /// Froms the description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public static T FromDescription(string description)
        {
            T result = default(T);

            try
            {
                Type t = typeof(T);
                foreach (FieldInfo fi in t.GetFields())
                {
                    object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (attrs != null &&
                        attrs.Length > 0)
                    {
                        foreach (DescriptionAttribute attr in attrs)
                            if (attr.Description.Equals(description))
                                result = (T)fi.GetValue(null);
                    }
                }
            }
            catch (Exception)
            {
                // TOOD: Log Exception
            }

            return result;
        }

        /// <summary>
        /// Parses the enum.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T ParseEnum(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        #endregion
    }
}