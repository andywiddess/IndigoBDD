using System;
using System.Linq.Expressions;


namespace Indigo.CrossCutting.Utilities.Reflection
{
	public static class SafeProperty<T>
		where T : class
	{
		public static GetProperty<T, TProperty> GetGetProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
		{
			var factory = new SafeGetPropertyFactory<T, TProperty>(propertyExpression);

			return factory.GetProperty;
		}
	}
}