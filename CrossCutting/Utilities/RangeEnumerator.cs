using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities
{
    public class RangeEnumerator<T> :
        IEnumerable<T>
    {
        readonly bool _ascending;
        readonly Range<T> _range;
        readonly Func<T, T> _step;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeEnumerator{T}"/> class.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="step">The step.</param>
        public RangeEnumerator(Range<T> range, Func<T, T> step)
        {
            _range = range;
            _step = step;

            _ascending = range.Comparer.Compare(range.LowerBound, step(range.LowerBound)) < 0;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            T first = _ascending ? _range.LowerBound : _range.UpperBound;
            T last = _ascending ? _range.UpperBound : _range.LowerBound;

            T value = first;

            IComparer<T> comparer = _range.Comparer;

            if (_range.IncludeLowerBound)
            {
                if (_range.IncludeUpperBound || comparer.Compare(value, last) < 0)
                    yield return value;
            }

            value = _step(value);

            while (comparer.Compare(value, last) < 0)
            {
                yield return value;
                value = _step(value);
            }

            if (_range.IncludeUpperBound && comparer.Compare(value, last) == 0)
                yield return value;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}