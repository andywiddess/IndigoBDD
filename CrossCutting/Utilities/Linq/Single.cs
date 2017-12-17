using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Linq
{
	public static class LinqSingleExtensions
	{
        /// <summary>
        /// Singles the or.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="onMissing">The on missing.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Sequence contains more than one element</exception>
		public static T SingleOr<T>(this IEnumerable<T> source, Func<T> onMissing)
		{
			Guard.AgainstNull(source, "source");
			Guard.AgainstNull(onMissing, "onMissing");

			using (IEnumerator<T> iterator = source.GetEnumerator())
			{
				if (!iterator.MoveNext())
					return onMissing();

				T first = iterator.Current;

				if (iterator.MoveNext())
					throw new InvalidOperationException("Sequence contains more than one element");

				return first;
			}
		}
	}
}