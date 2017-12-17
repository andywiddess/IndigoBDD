using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Indigo.CrossCutting.Utilities.Linq
{
    public static class LinqBatchExtensions
    {
        /// <summary>
        /// Batches the specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int size)
        {
            return Batch(source, size, x => x);
        }

        /// <summary>
        /// Batches the specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="size">The size.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> Batch<TSource, TResult>(this IEnumerable<TSource> source, int size, Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            Guard.AgainstNull(source, "source");
            Guard.GreaterThan(0, size, "size");
            Guard.AgainstNull(resultSelector, "resultSelector");

            return BatchImpl(source, size, resultSelector);
        }

        /// <summary>
        /// Batches the implementation.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="size">The size.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <returns></returns>
        private static IEnumerable<TResult> BatchImpl<TSource, TResult>(this IEnumerable<TSource> source, int size, Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            Debug.Assert(source != null);
            Debug.Assert(size > 0);
            Debug.Assert(resultSelector != null);

            TSource[] items = null;
            int count = 0;

            foreach (var item in source)
            {
                if (items == null)
                    items = new TSource[size];

                items[count++] = item;

                if (count != size)
                    continue;

                yield return resultSelector(items);

                items = null;
                count = 0;
            }

            if (items != null && count > 0)
                yield return resultSelector(items.Take(count));
        }
    }
}