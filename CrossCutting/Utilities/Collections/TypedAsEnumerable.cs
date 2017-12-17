using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Encapsulation for <see cref="IEnumerable"/> making items typed.</summary>
	/// <typeparam name="T">Type of element.</typeparam>
	public class TypedAsEnumerable<T>: IEnumerable<T>
	{
		#region fields

		/// <summary>Internal enumerable.</summary>
		private readonly IEnumerable m_Enumerable;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="TypedAsEnumerable&lt;T&gt;"/> class.</summary>
		/// <param name="enumerable">The enumerable.</param>
		public TypedAsEnumerable(IEnumerable enumerable)
		{
			if (enumerable == null)
				throw new ArgumentNullException("enumerable", "enumerable is null.");
			m_Enumerable = enumerable;
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return new TypedAsEnumerator<T>(m_Enumerable.GetEnumerator());
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Enumerable.GetEnumerator();
		}

		#endregion
	}
}
