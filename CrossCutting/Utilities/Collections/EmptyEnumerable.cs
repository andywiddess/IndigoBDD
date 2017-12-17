using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Implements IEnumerable which is always empty.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class EmptyEnumerable<T>: IEnumerable<T>
	{
		#region static fields

		/// <summary>
		/// Default instance. Because EmptyEnumerable has no data nor state it can be shared by many processes.
		/// </summary>
		public static readonly EmptyEnumerable<T> Default = new EmptyEnumerable<T>();

		#endregion

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns an enumerator that iterates through the empty collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return EmptyEnumerator<T>.Default;
		}

		/// <summary>
		/// Returns an enumerator that iterates through a empty collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
