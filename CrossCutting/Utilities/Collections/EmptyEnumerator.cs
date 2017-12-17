using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Implements always empty enumerator.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class EmptyEnumerator<T>: IEnumerator<T>
	{
		#region static fields

		/// <summary>
		/// Default object. Because empty enumerator has no data nor state can be 
		/// shared by all processes.
		/// </summary>
		public static readonly EmptyEnumerator<T> Default = new EmptyEnumerator<T>();

		#endregion

		#region IEnumerator<T> Members

		/// <summary>
		/// Gets the element in the collection at the current position of the enumerator.
		/// Always throws an exception.
		/// </summary>
		/// <value></value>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		public T Current
		{
			get { throw new InvalidOperationException("EmptyEnumerator does not have current item"); }
		}

		/// <summary>
		/// Gets the element in the collection at the current position of the enumerator.
		/// Always throws an exception.
		/// </summary>
		/// <value></value>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		object System.Collections.IEnumerator.Current
		{
			get { return Current; }
		}

		/// <summary>
		/// Advances the enumerator to the next element of the collection.
		/// Always returns false.
		/// </summary>
		/// <returns>
		/// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
		/// </returns>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		public bool MoveNext()
		{
			return false;
		}

		/// <summary>
		/// Sets the enumerator to its initial position, which is before the first element in the collection.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		public void Reset()
		{
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			// this object can be shared, so Dispose SHOULD NOT do anything
		}

		#endregion
	}
}
