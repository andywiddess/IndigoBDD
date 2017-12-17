using System;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	public static class ExtensionsToRange
	{
		public static void MustBeInRange<T>(this T value, RangeBuilder<T> rangeBuilder)
		{
			Range<T> range = rangeBuilder;

			value.MustBeInRange(range);
		}

		public static void MustBeInRange<T>(this T value, Range<T> range)
		{
			if (!range.Contains(value))
				throw new ArgumentException();
		}

		public static void Times(this int count, Action action)
		{
			for (int i = 0; i < count; i++)
			{
				action();
			}
		}
	}
}