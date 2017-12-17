using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections.OneToMany
{
	#region interface IOneToManyCollectionSimpleContainer<TContainer, TItem>

	/// <summary>Container adapter from simple relation.</summary>
	/// <typeparam name="TContainer">The type of the container.</typeparam>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	public interface IOneToManyCollectionSimpleContainer<TContainer, TItem>
		where TContainer: class
		where TItem: class
	{
		/// <summary>Gets the raw items collection.</summary>
		ICollection<TItem> Items { get; }

		/// <summary>Adds the item.</summary>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item is now contained by container; 
		/// <c>false</c> if for some reason it failed (unlikely).</returns>
		bool RawAddItem(TItem item);

		/// <summary>Removes the item.</summary>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item is now not in container (this includes th case when it never was); 
		/// <c>false</c> is for some reason removing failed (unlikely).</returns>
		bool RawRemoveItem(TItem item);

	}

	#endregion
}
