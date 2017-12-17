using System;

namespace Indigo.CrossCutting.Utilities.Reflection
{
	public static class ExtensionsToGet
	{
		public static TValue Get<TValue>(this GetValue<TValue> getValue)
			where TValue : class
		{
			TValue value = null;
			getValue(x => value = x);
			return value;
		}

		/// <summary>
		/// Returns the value for the property, or the default for the property type if
		/// the actual property value is unreachable (due to a null reference in the property chain)
		/// </summary>
		/// <typeparam name="T">The object type referenced</typeparam>
		/// <typeparam name="TValue">The property type</typeparam>
		/// <param name="getValue"></param>
		/// <param name="obj">The object from which the value should be retrieved</param>
		/// <returns>The value of the property, or default(TValue) if it cannot be accessed</returns>
		public static TValue Get<T, TValue>(this GetProperty<T, TValue> getValue, T obj)
			where T : class
		{
			TValue value = default(TValue);
			getValue(obj, x => value = x);
			return value;
		}
	}
}