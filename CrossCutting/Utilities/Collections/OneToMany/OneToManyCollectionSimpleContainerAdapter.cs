using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections.OneToMany
{
	#region class OneToManyCollectionSimpleContainerAdapter<TContainer, TItem>

	/// <summary>
	/// <see cref="IOneToManyCollectionContainerAdapter{TContainer,TItem}"/> implementation
	/// for <see cref="IOneToManyCollectionSimpleContainer{TContrainer,TItem}"/>.
	/// </summary>
	/// <typeparam name="TContainer">The type of the container.</typeparam>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	public class OneToManyCollectionSimpleContainerAdapter<TContainer, TItem>:
		IOneToManyCollectionContainerAdapter<TContainer, TItem>
		where TContainer: class, IOneToManyCollectionSimpleContainer<TContainer, TItem>
		where TItem: class
	{
		#region IOneToManyCollectionContainerAdapter<TContainer,TItem> Members

		/// <summary>Gets the field responsible for holding a collection on container side.</summary>
		/// <param name="container">The container.</param>
		/// <returns>Collection of items.</returns>
		public ICollection<TItem> GetItems(TContainer container)
		{
			if (container == null) return null;
			return container.Items;
		}

		/// <summary>Adds the item to container.</summary>
		/// <param name="container">The container.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item is now contained by container (this includes the when it already was); 
		/// <c>false</c> if for some reason it failed (unlikely).</returns>
		public bool RawAddItem(TContainer container, TItem item)
		{
			return container.RawAddItem(item);
		}

		/// <summary>Removes the item from container.</summary>
		/// <param name="container">The container.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item is now not in container (this includes th case when it never was); 
		/// <c>false</c> is for some reason removing failed (unlikely).</returns>
		public bool RawRemoveItem(TContainer container, TItem item)
		{
			return container.RawRemoveItem(item);
		}

		#endregion
	}

	#endregion
}
