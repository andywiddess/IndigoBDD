using System;
using System.Linq.Expressions;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public static class ExtensionsToSafeProperty
	{
		public static SafeProperty CreateSafeProperty<T,V>(this Expression<Func<T, V>> expression)
		{
			return SafeProperty.Create(expression);
		}

		public static SafeProperty CreateSafeProperty<T,V>(this Expression<Func<T, int, V>> expression)
		{
			return SafeProperty.Create(expression);
		}
	}
}