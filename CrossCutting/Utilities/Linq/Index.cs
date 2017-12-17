using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Linq
{
	public static class LinqIndexExtensions
	{
        /// <summary>
        /// Indexes the specified first.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <returns></returns>
		public static IEnumerable<TResult> Index<TFirst, TResult>(this IEnumerable<TFirst> first, Func<TFirst, int, TResult> resultSelector)
		{
			return first.Merge(Index(), resultSelector);
		}

        /// <summary>
        /// Indexes the specified first.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="start">The start.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <returns></returns>
		public static IEnumerable<TResult> Index<TFirst, TResult>(this IEnumerable<TFirst> first, int start, Func<TFirst, int, TResult> resultSelector)
		{
			return first.Merge(Index(start), resultSelector);
		}

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
		public static IEnumerable<int> Index()
		{
			return GenerateByIndexImpl(0, x => x);
		}

        /// <summary>
        /// Indexes the specified start.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <returns></returns>
		public static IEnumerable<int> Index(int start)
		{
			return GenerateByIndexImpl(start, x => x);
		}

        /// <summary>
        /// Indexes the specified selector.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
		public static IEnumerable<TResult> Index<TResult>(Func<int, TResult> selector)
		{
			Guard.AgainstNull(selector, "selector");

			return GenerateByIndexImpl(0, selector);
		}

        /// <summary>
        /// Indexes the specified start.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="start">The start.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
		public static IEnumerable<TResult> Index<TResult>(int start, Func<int, TResult> selector)
		{
			Guard.AgainstNull(selector, "selector");

			return GenerateByIndexImpl(start, selector);
		}

        /// <summary>
        /// Generates the by index implementation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="start">The start.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
		private static IEnumerable<TResult> GenerateByIndexImpl<TResult>(int start, Func<int, TResult> selector)
		{
			for (int i = start; i < int.MaxValue; i++)
			{
				yield return selector(i);
			}

			yield return selector(int.MaxValue);
		}
	}
}