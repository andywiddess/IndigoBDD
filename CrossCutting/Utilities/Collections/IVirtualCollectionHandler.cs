using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Virtual collection of items.
	/// </summary>
	/// <typeparam name="D">Discriminator.</typeparam>
	/// <typeparam name="T">Type of item.</typeparam>
	public interface IVirtualCollectionHandler<D, T>
	{
		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="item">The item.</param>
		void Add(D discriminator, T item);

		/// <summary>
		/// Clears collection.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		void Clear(D discriminator);

		/// <summary>
		/// Determines whether collection contains specified item.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="item">The item.</param>
		/// <returns>
		/// 	<c>true</c> if collection contains specified item; otherwise, <c>false</c>.
		/// </returns>
		bool Contains(D discriminator, T item);

		/// <summary>
		/// Copies items to array.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Starting index in the array.</param>
		void CopyTo(D discriminator, T[] array, int arrayIndex);

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <returns>Number of items in collection.</returns>
		int GetCount(D discriminator);

		/// <summary>
		/// Determines whether collection is read only.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <returns>
		/// 	<c>true</c> if collection is read only; otherwise, <c>false</c>.
		/// </returns>
		bool IsReadOnly(D discriminator);

		/// <summary>
		/// Removes the item from collection.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item has been removed.</returns>
		bool Remove(D discriminator, T item);

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <returns>Enumerator.</returns>
		IEnumerator<T> GetEnumerator(D discriminator);
	}
}
