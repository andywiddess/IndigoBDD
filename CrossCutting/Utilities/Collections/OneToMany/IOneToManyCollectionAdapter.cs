using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections.OneToMany
{
	/// <summary>Adapter used with <see cref="OneToManyCollection{TContainer,TItem}"/>.</summary>
	/// <typeparam name="TContainer">The type of the container.</typeparam>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	public interface IOneToManyCollectionAdapter<TContainer, TItem>
		where TContainer: class
		where TItem: class
	{
		/// <summary>Gets the field responsible for holding a collection on container side.</summary>
		/// <param name="container">The container.</param>
		/// <returns>Collection of items.</returns>
		ICollection<TItem> RawGetItems(TContainer container);

		/// <summary>Gets the field responsible from holding a reference to container on item side.</summary>
		/// <param name="item">The item.</param>
		/// <returns>Reference to container.</returns>
		TContainer RawGetContainer(TItem item);

		/// <summary>Set the container reference on item side.</summary>
		/// <param name="item">The item.</param>
		/// <param name="container">The container.</param>
		void RawSetContainer(TItem item, TContainer container);

		/// <summary>Adds the item to container.</summary>
		/// <param name="container">The container.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item is now contained by container (this includes the when it already was); 
		/// <c>false</c> if for some reason it failed (unlikely).</returns>
		bool RawAddItem(TContainer container, TItem item);

		/// <summary>Removes the item from container.</summary>
		/// <param name="container">The container.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item is now not in container (this includes th case when it never was); 
		/// <c>false</c> is for some reason removing failed (unlikely).</returns>
		bool RawRemoveItem(TContainer container, TItem item);
	}
}
