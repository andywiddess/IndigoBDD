using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Interval encapsulates interval defined by [min, max).
	/// Notice, that this formula describes a set which actually 
	/// contains min, yet does not contain max.
	/// </summary>
	public class Interval: ICloneable, IEnumerable<int>, IEquatable<Interval>
	{
		#region fields

		private int m_Min;
		private int m_Max;

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating whether this <see cref="Interval"/> is empty.
		/// </summary>
		/// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
		public bool Empty
		{
			get { return m_Min >= m_Max; }
		}

		/// <summary>
		/// Gets or sets the minimum value.
		/// </summary>
		/// <value>The min.</value>
		public int Min
		{
			get { return m_Min; }
			set
			{
				if (Empty)
					throw new NotSupportedException("Cannot set Min for empty Interval");
				m_Min = value;
				AfterEdit();
			}
		}

		/// <summary>
		/// Gets or sets the maximum value. Note: Maximum value is NOT included in interval.
		/// </summary>
		/// <value>The max.</value>
		public int Max
		{
			get { return m_Max; }
			set
			{
				if (Empty)
					throw new NotSupportedException("Cannot set Max for empty Interval");
				m_Max = value;
				AfterEdit();
			}
		}

		/// <summary>
		/// Gets or sets the width of interval.
		/// </summary>
		/// <value>The width.</value>
		public int Width
		{
			get
			{
				if (Empty)
					return 0;
				else
					return m_Max - m_Min;
			}
			set
			{
				if (Empty)
					throw new NotSupportedException("Cannot set Width for empty Interval");
				m_Max = m_Min + value;
				AfterEdit();
			}
		}

		private void AfterEdit()
		{
			if (Empty) Reset();
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Interval"/> class.
		/// </summary>
		/// <param name="min">The min.</param>
		/// <param name="max">The max.</param>
		/// <param name="includeMax">if set to <c>true</c> set will include maximum value, making it: [min, max+1).</param>
		public Interval(int min, int max, bool includeMax)
		{
			if (includeMax)
			{
				Set(min, max + 1);
			}
			else
			{
				Set(min, max);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Interval"/> class.
		/// </summary>
		/// <param name="min">The min.</param>
		/// <param name="max">The max.</param>
		public Interval(int min, int max)
			: this(min, max, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Interval"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Interval(int value)
			: this(value, value, true)
		{
		}

		/// <summary>
		/// Initializes an empty instance of the <see cref="Interval"/> class.
		/// </summary>
		public Interval()
		{
			Reset();
		}

		/// <summary>
		/// Initializes instance of the <see cref="Interval"/> class by copying it.
		/// </summary>
		/// <param name="other">The other.</param>
		public Interval(Interval other)
		{
			m_Min = other.m_Min;
			m_Max = other.m_Max;
		}

		#endregion

		#region Set, Reset

		/// <summary>
		/// Sets interval range.
		/// </summary>
		/// <param name="min">The min.</param>
		/// <param name="max">The max.</param>
		public void Set(int min, int max)
		{
			if (min < max)
			{
				m_Min = min;
				m_Max = max;
			}
			else
			{
				Reset();
			}
		}

		/// <summary>
		/// Resets the interval making it empty.
		/// </summary>
		public void Reset()
		{
			m_Min = int.MaxValue;
			m_Max = int.MinValue;
		}

		#endregion

		#region Testing

		/// <summary>
		/// Determines whether interval contains specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if interval contains specified value; otherwise, <c>false</c>.</returns>
		public bool Contains(int value)
		{
			return (!Empty) && (value >= m_Min) && (value < m_Max);
		}

		/// <summary>
		/// Determines whether interval contains other interval.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if interval contains other interval; otherwise, <c>false</c>.</returns>
		public bool Contains(Interval other)
		{
			return
					(!Empty) && (!other.Empty) &&
					(other.m_Min >= m_Min) && (other.m_Max <= m_Max);
		}

		/// <summary>
		/// Tests if interval is equal to other interval.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns></returns>
		public bool Equals(Interval other)
		{
			return
					(Empty && other.Empty) ||
					((m_Min == other.m_Min) && (m_Max == other.m_Max));
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
		public override bool Equals(object obj)
		{
			if (obj is Interval)
				return Equals((Interval)obj);
			else
				return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		public override int GetHashCode()
		{
			return HashCode.Hash(m_Min, m_Min);
		}

		/// <summary>
		/// Tests if interval does not overlap with other interval.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if they do not overlap.</returns>
		public bool Disjoint(Interval other)
		{
			return
				(Empty) || (other.Empty) ||
				(m_Max <= other.m_Min) || (m_Min >= other.m_Max);
		}

		/// <summary>
		/// Tests if interval intersects with other interval.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if they intersect.</returns>
		public bool Intersects(Interval other)
		{
			return
				(!Empty) && (!other.Empty) &&
				(m_Max > other.m_Min) && (m_Min < other.m_Max);
		}

		/// <summary>
		/// Tests if interval "touches" other interval. By touching I mean "it ends exacly when other starts".
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if they touch.</returns>
		public bool Touches(Interval other)
		{
			return
				(!Empty) && (!other.Empty) &&
				((m_Min == other.m_Max) || (other.m_Min == m_Max));
		}

		/// <summary>
		/// Tests if interval touches or intersects other interval.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if they touch or intersect.</returns>
		public bool Interacts(Interval other)
		{
			return
				(!Empty) && (!other.Empty) &&
				(m_Max >= other.m_Min) && (m_Min <= other.m_Max);
		}

		#endregion

		#region Union & Intersection

		/// <summary>
		/// Makes union of two intervals. If they are disjoint it will throw an exception.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns>Interval which is an union of given intervals.</returns>
		public Interval Union(Interval other)
		{
			if (Empty) return other;
			if (other.Empty) return this;

			if (Interacts(other))
			{
				return new Interval(
						System.Math.Min(m_Min, other.m_Min),
						System.Math.Max(m_Max, other.m_Max));
			}
			else
			{
				throw new NotSupportedException("Disjoint Intervals Union not supported");
			}
		}

		/// <summary>
		/// Makes an intersection of two intervals. Throws an exception if they are disjoint.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns>Intersection.</returns>
		public Interval Intersection(Interval other)
		{
			if (Intersects(other))
			{
				return new Interval(
					System.Math.Max(m_Min, other.m_Min),
					System.Math.Min(m_Max, other.m_Max));
			}
			else
			{
				throw new NotSupportedException("Disjoint Intervals Intersection not supported");
			}
		}

		/// <summary>
		/// Makes the difference of two intervals.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns>One or multiple intervals.</returns>
		public Interval[] Difference(Interval other)
		{
			if (Intersects(other))
			{
				Interval resultA = new Interval(m_Min, other.m_Min);
				Interval resultB = new Interval(other.m_Max, m_Max);

				int count = (resultA.Empty ? 0 : 1) + (resultB.Empty ? 0 : 1);
				Interval[] result = new Interval[count];

				if (!resultB.Empty)
					result[--count] = resultB;
				if (!resultA.Empty)
					result[--count] = resultA;

				return result;
			}
			else
			{
				return new Interval[] { (Interval)Clone() };
			}
		}

		#endregion

		#region ICloneable Members

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns></returns>
		public Interval Clone()
		{
			return new Interval(m_Min, m_Max);
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		#endregion

		#region ToString override

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			if (Empty)
				return string.Format("{0}()", GetType().FullName);
			else
				return string.Format("{0}({1}, {2})", GetType().FullName, m_Min, m_Max);
		}

		#endregion

		#region Eumerate

		/// <summary>
		/// Enumerates all numbers in interval.
		/// </summary>
		/// <returns>Stream if ints.</returns>
		private IEnumerable<int> Enumerate()
		{
			for (int i = m_Min; i < m_Max; i++) yield return i;
		}

		#endregion

		#region IEnumerable<int> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<int> GetEnumerator()
		{
			return Enumerate().GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}