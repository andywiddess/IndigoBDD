namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Virtual list handler.
	/// </summary>
	/// <typeparam name="D">Discriminator.</typeparam>
	/// <typeparam name="T">Type of item.</typeparam>
	public interface IVirtualListHandler<D, T>: IVirtualCollectionHandler<D, T>
	{
		/// <summary>
		/// Indexes the given item.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="item">The item.</param>
		/// <returns>Index of given item or negative value if item not found.</returns>
		int IndexOf(D discriminator, T item);

		/// <summary>
		/// Inserts the specified item at given index.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		void Insert(D discriminator, int index, T item);

		/// <summary>
		/// Removes item at given index.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="index">The index.</param>
		void RemoveAt(D discriminator, int index);

		/// <summary>
		/// Gets item at given index.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		T GetAt(D discriminator, int index);

		/// <summary>
		/// Sets intem at given index.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		void SetAt(D discriminator, int index, T value);
	}
}
