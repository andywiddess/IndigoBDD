using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Enumerable proxy which does not change its reference even if encapsulated collection has been changed.</summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class FixedRefEnumerable<T>: FixedRefBase<IEnumerable<T>, FixedRefEnumerable<T>>, IEnumerable<T>
	{
		#region constructor

		/// <summary>Initializes a new instance of the <see cref="FixedRefEnumerable&lt;T&gt;"/> class.</summary>
		public FixedRefEnumerable()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefEnumerable&lt;T&gt;"/> class.</summary>
		/// <param name="collection">The collection.</param>
		public FixedRefEnumerable(IEnumerable<T> collection)
			: base(collection)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefEnumerable&lt;T&gt;"/> class.</summary>
		/// <param name="getter">The getter for concrete collection.</param>
		/// <param name="setter">The setter for concrete collection.</param>
		public FixedRefEnumerable(Func<IEnumerable<T>> getter, Action<IEnumerable<T>> setter)
			: base(getter, setter)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="FixedRefEnumerable&lt;T&gt;"/> class.</summary>
		/// <param name="collection">The collection.</param>
		/// <param name="getter">The getter for concrete collection.</param>
		/// <param name="setter">The setter for concrete collection.</param>
		public FixedRefEnumerable(IEnumerable<T> collection, Func<IEnumerable<T>> getter, Action<IEnumerable<T>> setter)
			: base(collection, getter, setter)
		{
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return Data.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Data.GetEnumerator();
		}

		#endregion
	}
}
