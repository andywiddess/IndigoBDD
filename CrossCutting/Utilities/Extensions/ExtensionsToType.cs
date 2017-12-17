using System;
using System.Linq;
using Indigo.CrossCutting.Utilities.Reflection;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	public static class ExtensionsToType
	{
		public static object GetDefaultValue(this Type type)
		{
			return type.AllowsNullValue() ? null : FastActivator.Create(type);
		}

		public static bool AllowsNullValue(this Type type)
		{
			return (!type.IsValueType || type.IsNullableValueType());
		}

		public static bool IsNullableValueType(this Type type)
		{
			return Nullable.GetUnderlyingType(type) != null;
		}

		public static bool IsCompatibleWith<T>(this object value)
		{
			return (value is T || (value == null && typeof (T).AllowsNullValue()));
		}

		public static string ToShortTypeName(this Type type)
		{
			if (type.IsGenericType)
			{
				string name = type.GetGenericTypeDefinition().Name;
				name = name.Substring(0, name.IndexOf('`'));

				Type[] arguments = type.GetGenericArguments();
				string innerTypeName = string.Join(",", arguments.Select(x => x.ToShortTypeName()).ToArray());

				return "{0}<{1}>".FormatWith(name, innerTypeName);
			}

			return type.Name;
		}
	}
}