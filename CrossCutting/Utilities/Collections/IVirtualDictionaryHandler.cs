using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Virtual dictionary handler.
	/// </summary>
	/// <typeparam name="D">Discriminator.</typeparam>
	/// <typeparam name="K">Key type.</typeparam>
	/// <typeparam name="V">Value type.</typeparam>
	public interface IVirtualDictionaryHandler<D, K, V>: IVirtualCollectionHandler<D, KeyValuePair<K, V>>
	{
		/// <summary>
		/// Adds KV pair to dictionary..
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		void Add(D discriminator, K key, V value);

		/// <summary>
		/// Determines whether dictionary contains key.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="key">The key.</param>
		/// <returns>
		/// 	<c>true</c> if the specified discriminator contains key; otherwise, <c>false</c>.
		/// </returns>
		bool ContainsKey(D discriminator, K key);

		/// <summary>
		/// Gets the keys.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <returns>Keys collection.</returns>
		ICollection<K> GetKeys(D discriminator);

		/// <summary>
		/// Removes item from dictionary.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="key">The key.</param>
		/// <returns><c>true</c> if key has been removed.</returns>
		bool Remove(D discriminator, K key);

		/// <summary>
		/// Tries the get value from dictionary.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value found or default if not found.</param>
		/// <returns><c>true</c> if key has been found</returns>
		bool TryGetValue(D discriminator, K key, out V value);

		/// <summary>
		/// Gets the values collection.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <returns>Values collection.</returns>
		ICollection<V> GetValues(D discriminator);

		/// <summary>
		/// Gets value by key.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="key">The key.</param>
		/// <returns>Value found.</returns>
		V GetByKey(D discriminator, K key);

		/// <summary>
		/// Sets value the by key.
		/// </summary>
		/// <param name="discriminator">The discriminator.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		void SetByKey(D discriminator, K key, V value);
	}
}
