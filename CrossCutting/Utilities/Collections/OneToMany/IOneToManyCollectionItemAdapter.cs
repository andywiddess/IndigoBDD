using System;

namespace Indigo.CrossCutting.Utilities.Collections.OneToMany
{
	/// <summary>
	/// Item side adapter for <see cref="OneToManyCollection{TContainer,TItem}"/>.
	/// </summary>
	/// <typeparam name="TContainer">The type of the container.</typeparam>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	public interface IOneToManyCollectionItemAdapter<TContainer, TItem>
	{
		/// <summary>Gets the container for specified item.</summary>
		/// <param name="item">The item.</param>
		/// <returns>The assigned container.</returns>
		TContainer GetContainer(TItem item);

		/// <summary>Sets the container for specified item.</summary>
		/// <param name="item">The item.</param>
		/// <param name="container">The container to be assigned.</param>
		void SetContainer(TItem item, TContainer container);
	}
}
