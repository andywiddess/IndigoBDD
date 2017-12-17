using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	public static class ExtensionsToObject
	{
		const int RecursionLimit = 10;

        /// <summary>
        /// An object extension method that stringifies the given value.
        /// </summary>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
		public static string Stringify(this object value)
		{
			try
			{
				return StringifyInternal(value, RecursionLimit);
			}
			catch (InvalidOperationException)
			{
				return value.ToString();
			}
		}

        /// <summary>
        /// Stringify internal.
        /// </summary>
        ///
        /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
        ///                                                 invalid. </exception>
        ///
        /// <param name="value">            The value. </param>
        /// <param name="recursionLevel">   The recursion level. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
		static string StringifyInternal(object value, int recursionLevel)
		{
			if (value == null)
				return "null";

			if (recursionLevel < 0)
				throw new InvalidOperationException();

			if (value is string || value is char)
				return "\"" + value + "\"";

			var collection = value as IEnumerable;
			if (collection != null)
				return StringifyCollection(collection, recursionLevel);

			if (value.GetType().IsValueType)
				return value.ToString();

			return StringifyObject(value, recursionLevel);
		}


        /// <summary>
        /// Stringify collection.
        /// </summary>
        ///
        /// <param name="collection">       The collection. </param>
        /// <param name="recursionLevel">   The recursion level. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
		static string StringifyCollection(IEnumerable collection, int recursionLevel)
		{
			string[] elements = collection.Cast<object>()
				.Select(x => StringifyInternal(x, recursionLevel - 1))
				.ToArray();

			return "[" + string.Join(", ", elements) + "]";
		}

        /// <summary>
        /// Stringify object.
        /// </summary>
        ///
        /// <param name="value">            The value. </param>
        /// <param name="recursionLevel">   The recursion level. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
		static string StringifyObject(object value, int recursionLevel)
		{
			string[] elements = value
				.GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				.Select(x => "{0} = {1}".FormatWith(x.Name, StringifyInternal(x.GetValue(value, null), recursionLevel - 1)))
				.ToArray();

			return "{" + string.Join(", ", elements) + "}";
		}

        /// <summary>
        /// An object extension method that cast as.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
        ///                                                 are null. </exception>
        /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
        ///                                                 invalid. </exception>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="input">    The input to act on. </param>
        ///
        /// <returns>
        /// A T.
        /// </returns>
		public static T CastAs<T>(this object input)
			where T : class
		{
			if (input == null)
				throw new ArgumentNullException("input");

			var result = input as T;
			if (result == null)
				throw new InvalidOperationException("Unable to convert from " + input.GetType().FullName + " to "
				                                    + typeof(T).FullName);

			return result;
		}

		/// <summary>
		/// Returns the value of the instance member, or the default value if the instance is null
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="instance"></param>
		/// <param name="accessor"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static TValue ValueOrDefault<T, TValue>(this T instance, Func<T, TValue> accessor, TValue defaultValue)
			where T : class
		{
			if(null == instance)
				return defaultValue;

			return accessor(instance);
		}


        /// <summary>
        /// Used to execute actions on possibly <c>null</c> object.
        /// Safe version of <c>list.Clear</c> is <c>list.SafeExec(l =&gt; l.Clear)</c>.
        /// Note, this is much slower than code with checks, use it when performance is not an issue.
        /// </summary>
        /// <typeparam name="V">Type of value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="extractor">The extractor.</param>
        public static void SafeExec<V>(this V value, Action<V> extractor)
        {
            if (!object.ReferenceEquals(value, null)) extractor(value);
        }

        /// <summary>
        /// Used to execute actions on possibly <c>null</c> object.
        /// Safe version of <c>p.Left.Left</c> (which may casue <c>NullReferenceException</c>) is
        /// <c>p.SafeExec(x =&gt; x.Left).SafeExec(x =&gt; x.Left)</c>. Safe version of <c>list.Count</c> is
        /// <c>list.SafeExec(l =&gt; l.Count)</c>.
        /// Note, this is much slower than code with checks, use it when performance is not an issue.
        /// </summary>
        /// <typeparam name="V">Type of value.</typeparam>
        /// <typeparam name="R">Type of result</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="extractor">The extractor.</param>
        /// <param name="defaultValue">The default value returned.</param>
        /// <returns>
        /// Result of <paramref name="extractor"/> or <paramref name="defaultValue"/>.
        /// </returns>
        public static R SafeExec<V, R>(this V value, Func<V, R> extractor, R defaultValue = default(R))
        {
            return object.ReferenceEquals(value, null)
                ? defaultValue
                : extractor(value);
        }

        /// <summary>
        /// Gets property value.
        /// </summary>
        ///
        /// <param name="data">         The data. </param>
        /// <param name="propertyName"> Name of the property. </param>
        ///
        /// <returns>
        /// The property value.
        /// </returns>
        private static object GetPropertyValue(object data, string propertyName)
        {
            if (data == null) return null;

            var access =
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static;

            var type = data.GetType();
            var name = propertyName;

            var property = ResolvePropertyInfo(type, name, type.GetProperties(access));
            if (property != null)
                return property.GetValue(data, null);

            var field = ResolvePropertyInfo(type, name, type.GetFields(access));
            if (field != null)
                return field.GetValue(data);

            return null;
        }

        /// <summary>
        /// Type distance.
        /// </summary>
        ///
        /// <param name="parent">   The parent. </param>
        /// <param name="child">    The child. </param>
        ///
        /// <returns>
        /// An int?
        /// </returns>
        private static int? TypeDistance(Type parent, Type child)
        {
            if (parent == null) return null;
            if (child == null) return null;
            if (parent == child) return 0;
            var result = TypeDistance(parent, child.BaseType);
            return result == null ? result : result + 1;
        }

        /// <summary>
        /// Resolve property information.
        /// </summary>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="type">         The type. </param>
        /// <param name="name">         The name. </param>
        /// <param name="propertyInfo"> Information describing the property. </param>
        ///
        /// <returns>
        /// A T.
        /// </returns>
        private static T ResolvePropertyInfo<T>(Type type, string name, T[] propertyInfo)
            where T : MemberInfo
        {
            return propertyInfo
                .Where(p => string.CompareOrdinal(name, p.Name) == 0)
                // as we don't get whole hierarchy we don't need sorting so far
                // .OrderBy(p => TypeDistance(p.DeclaringType, type))
                .FirstOrDefault();
        }

        private static readonly char[] _separator = new[] { '.' };

        /// <summary>Resolves the property value. Allows to use 
        /// Note, this method is quite slow (comparing to direct access) so consider 
        /// using more advanced approach (for example: LWCG) if you thisnk it is too slow.</summary>
        /// <param name="data">The data.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <returns>Value of property.</returns>
        public static object ResolvePropertyValue(this object data, string propertyPath)
        {
            var result = data;
            foreach (var name in propertyPath.Split(_separator))
            {
                if (!string.IsNullOrEmpty(name))
                    result = GetPropertyValue(result, name);
            }
            return result;
        }

        /// <summary>
        /// Creates the hash code for the required object
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static ulong CreateHashCode(this object obj)
	    {
	        ulong hash = 0;
	        Type objType = obj.GetType();

	        if (objType.IsValueType || obj is string)
	        {
	            unchecked
	            {
	                hash = (uint)obj.GetHashCode() * 397;
	            }

	            return hash;
	        }

	        unchecked
	        {
	            foreach (PropertyInfo property in obj.GetType().GetProperties())
	            {
	                object value = property.GetValue(obj, null);
	                hash ^= value.CreateHashCode();
	            }
	        }

	        return hash;
	    }
    }
}