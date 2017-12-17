using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities
{
	public class RangeBuilder<T>
	{
		internal bool _includeLowerBound;
		internal bool _includeUpperBound;
		internal T _lowerBound;
		internal T _upperBound;

		public RangeBuilder(T lowerBound)
		{
			_lowerBound = lowerBound;
			_upperBound = lowerBound;
			_includeLowerBound = true;
			_includeUpperBound = true;
		}

		/// <summary>
		/// Specifies the upper bound for the range
		/// </summary>
		/// <param name="upperBound"></param>
		/// <returns></returns>
		public RangeBuilder<T> Through(T upperBound)
		{
			_upperBound = upperBound;
			return this;
		}

		public RangeBuilder<T> IncludeLowerBound()
		{
			_includeLowerBound = true;
			return this;
		}

		public RangeBuilder<T> IncludeUpperBound()
		{
			_includeUpperBound = true;
			return this;
		}

		public RangeBuilder<T> ExcludeLowerBound()
		{
			_includeLowerBound = false;
			return this;
		}

		public RangeBuilder<T> ExcludeUpperBound()
		{
			_includeUpperBound = false;
			return this;
		}

		public static implicit operator Range<T>(RangeBuilder<T> builder)
		{
			return new Range<T>(builder._lowerBound, builder._upperBound, builder._includeLowerBound, builder._includeUpperBound);
		}
	}

	public static class RangeBuilderExt
	{
		public static RangeBuilder<int> Through(this int start, int end)
		{
			return new RangeBuilder<int>(start).Through(end);
		}
		public static RangeBuilder<long> Through(this long start, long end)
		{
			return new RangeBuilder<long>(start).Through(end);
		}
		public static RangeBuilder<DateTime> Through(this DateTime start, DateTime end)
		{
			return new RangeBuilder<DateTime>(start).Through(end);
		}
		public static RangeBuilder<char> Through(this char start, char end)
		{
			return new RangeBuilder<char>(start).Through(end);
		}
	}
}