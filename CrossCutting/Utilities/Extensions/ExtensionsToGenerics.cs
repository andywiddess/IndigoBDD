using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	public static class ExtensionsToGenerics
	{
		public static IEnumerable<Type> GetGenericTypeDeclarations(this object obj, Type genericType)
		{
			Guard.AgainstNull(obj, "obj");

			return obj.GetType().GetGenericTypeDeclarations(genericType);
		}

		public static IEnumerable<Type> GetGenericTypeDeclarations(this Type objectType, Type genericType)
		{
			Guard.AgainstNull(objectType, "objectType");
			Guard.AgainstNull(genericType, "genericType");
			Guard.IsTrue(x => x.IsGenericTypeDefinition, genericType, "genericType", "Must be an open generic type");

			Type matchedType;
			if (objectType.ImplementsGeneric(genericType, out matchedType))
			{
				foreach (Type argument in matchedType.GetGenericArguments())
				{
					yield return argument;
				}
			}
		}
	}
}