using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public static class ExtensionsForGenericArguments
	{
		public static IEnumerable<Type> GetDeclaredGenericArguments(this object obj)
		{
			if (obj == null)
				yield break;

			foreach (Type type in obj.GetType().GetDeclaredGenericArguments())
			{
				yield return type;
			}
		}

		public static IEnumerable<Type> GetDeclaredGenericArguments(this Type type)
		{
			bool atLeastOne = false;
			Type baseType = type;
			while (baseType != null)
			{
				if (baseType.IsGenericType)
				{
					foreach (Type declaredType in baseType.GetGenericArguments())
					{
						yield return declaredType;

						atLeastOne = true;
					}
				}

				baseType = baseType.BaseType;
			}

			if (atLeastOne)
				yield break;

			foreach (Type interfaceType in type.GetInterfaces())
			{
				if (!interfaceType.IsGenericType)
					continue;

				foreach (Type declaredType in interfaceType.GetGenericArguments())
				{
					if (declaredType.IsGenericParameter)
						continue;

					yield return declaredType;
				}
			}
		}

		public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
		{
			const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

			foreach (PropertyInfo propertyInfo in type.GetProperties(bindingFlags))
			{
				yield return propertyInfo;
			}

			if (type.IsInterface)
			{
				foreach (PropertyInfo propertyInfo in type.GetInterfaces()
					.SelectMany(interfaceType => interfaceType.GetProperties(bindingFlags)))
				{
					yield return propertyInfo;
				}
			}
		}
	}
}