using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Linq
{
	public static class LinqMergeExtensions
	{
        /// <summary>
        /// Merges the specified first.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <typeparam name="TSecond">The type of the second.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <returns></returns>
		public static IEnumerable<TResult> Merge<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
		{
			Guard.AgainstNull(first, "first");
			Guard.AgainstNull(second, "second");
			Guard.AgainstNull(resultSelector, "resultSelector");

			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
				while (e1.MoveNext())
				{
					if (!e2.MoveNext())
						yield break;

					yield return resultSelector(e1.Current, e2.Current);
				}
		}


        /// <summary>
        /// Merges the balanced.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <typeparam name="TSecond">The type of the second.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">
        /// Second sequence ran out before first
        /// or
        /// First sequence ran out before second
        /// </exception>
		public static IEnumerable<TResult> MergeBalanced<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
		{
			Guard.AgainstNull(first, "first");
			Guard.AgainstNull(second, "second");
			Guard.AgainstNull(resultSelector, "resultSelector");

			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			{
				while (e1.MoveNext())
				{
					if (!e2.MoveNext())
						throw new InvalidOperationException("Second sequence ran out before first");

					yield return resultSelector(e1.Current, e2.Current);
				}
				if (e2.MoveNext())
					throw new InvalidOperationException("First sequence ran out before second");
			}
		}

        /// <summary>
        /// Merges the padded.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <typeparam name="TSecond">The type of the second.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <returns></returns>
		public static IEnumerable<TResult> MergePadded<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
		{
			Guard.AgainstNull(first, "first");
			Guard.AgainstNull(second, "second");
			Guard.AgainstNull(resultSelector, "resultSelector");

			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			{
				while (e1.MoveNext())
				{
					if (e2.MoveNext())
					{
						yield return resultSelector(e1.Current, e2.Current);
					}
					else
					{
						do
						{
							yield return resultSelector(e1.Current, default(TSecond));
						} while (e1.MoveNext());
						yield break;
					}
				}
				if (e2.MoveNext())
				{
					do
					{
						yield return resultSelector(default(TFirst), e2.Current);
					} while (e2.MoveNext());
					yield break;
				}
			}
		}
	}
}