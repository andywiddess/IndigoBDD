using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Implements empty list of any type.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class EmptyList<T>: EmptyCollection<T>, IList<T>
	{
		#region static fields

		/// <summary>
		/// Default instance. Because EmptyList has no data not state it can be shared by many processes.
		/// </summary>
		public static readonly new EmptyList<T> Default = new EmptyList<T>();

		#endregion

		#region utilities

		/// <summary>Creates ready to throw <see cref="ArgumentOutOfRangeException"/> exception.</summary>
		/// <param name="index">The index.</param>
		/// <returns><see cref="ArgumentOutOfRangeException"/> (does not throw it).</returns>
		protected static ArgumentOutOfRangeException InvalidIndex(int index)
		{
			return new ArgumentOutOfRangeException(
				"index", string.Format("Index {0} is out of range", index));
		}

		#endregion

		#region IList<T> Members

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <returns>
		/// The index of <paramref name="item"/> if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf(T item)
		{
			return -1;
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
		public void Insert(int index, T item)
		{
			throw NotSupported("Insert");
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
		public void RemoveAt(int index)
		{
			throw InvalidIndex(index);
		}

		/// <summary>
		/// Gets or sets the value at the specified index.
		/// </summary>
		/// <value></value>
		public T this[int index]
		{
			get { throw InvalidIndex(index); }
			set { throw InvalidIndex(index); }
		}

		#endregion
	}
}
