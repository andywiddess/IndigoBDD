using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Encapsulation for <see cref="IEnumerator"/> making items typed.</summary>
	/// <typeparam name="T"></typeparam>
	public class TypedAsEnumerator<T>: IEnumerator<T>
	{
		#region fields

		/// <summary>Internal enumerator.</summary>
		private readonly IEnumerator m_Enumerator;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="TypedAsEnumerator&lt;T&gt;"/> class.</summary>
		/// <param name="enumerator">The enumerator.</param>
		public TypedAsEnumerator(IEnumerator enumerator)
		{
			if (enumerator == null)
				throw new ArgumentNullException("enumerator", "enumerator is null.");
			m_Enumerator = enumerator;
		}

		#endregion

		#region IEnumerator<T> Members

		/// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		public T Current
		{
			get { return (T)m_Enumerator.Current; }
		}

		#endregion

		#region IDisposable Members

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
			// nothing to do
		}

		#endregion

		#region IEnumerator Members

		/// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		object IEnumerator.Current
		{
			get { return m_Enumerator.Current; }
		}

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		public bool MoveNext()
		{
			return m_Enumerator.MoveNext();
		}

		/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		public void Reset()
		{
			m_Enumerator.Reset();
		}

		#endregion
	}
}
