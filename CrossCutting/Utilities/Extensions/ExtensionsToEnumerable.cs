using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	public static class ExtensionsToEnumerable
	{
		/// <summary>
		/// Enumerates a collection, calling the specified action for each entry in the collection
		/// </summary>
		/// <typeparam name="T">The type of the enumeration</typeparam>
		/// <param name="collection">The collection to enumerate</param>
		/// <param name="callback">The action to call for each entry in the collection</param>
		/// <returns>The collection that was enumerated</returns>
		public static IEnumerable<T> Each<T>(this IEnumerable<T> collection, Action<T> callback)
		{
			foreach (T item in collection)
			{
				callback(item);
			}

			return collection;
		}

		/// <summary>
		/// Enumerates a collection, calling the callback until false is returned
		/// </summary>
		/// <typeparam name="T">The type of item being enumerated</typeparam>
		/// <param name="collection">The collection to enumerate</param>
		/// <param name="callback">The callback to call for each element</param>
		/// <returns>True if all of the elements were enumerated, otherwise false</returns>
		public static bool WhileTrue<T>(this IEnumerable collection, Func<T, bool> callback)
		{
			foreach (T item in collection)
			{
				if (item == null)
					continue;

				if (callback(item) == false)
					return false;
			}

			return true;
		}

		public static bool WhileTrue<T>(this IEnumerable<T> collection, Func<T, bool> callback)
		{
			foreach (T item in collection)
			{
				if (callback(item) == false)
					return false;
			}
			return true;
		}
	}
}