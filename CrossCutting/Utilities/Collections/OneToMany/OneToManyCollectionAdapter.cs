using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections.OneToMany
{
	#region class OneToManyCollectionAdapter

	/// <summary>Helper class to create <see cref="IOneToManyCollectionAdapter{TContainer,TItem}"/> instances.</summary>
	public class OneToManyCollectionAdapter
	{
		/// <summary>
		/// Creates adapter from simple container (<see cref="IOneToManyCollectionSimpleContainer{TContainer,TItem}"/>) 
		/// and simple item (<see cref="IOneToManyCollectionSimpleItem{TContainer,TItem}"/>).
		/// </summary>
		/// <typeparam name="TContainer">The type of the container.</typeparam>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <returns></returns>
		public static IOneToManyCollectionAdapter<TContainer, TItem> Make<TContainer, TItem>()
			where TContainer: class, IOneToManyCollectionSimpleContainer<TContainer, TItem>
			where TItem: class, IOneToManyCollectionSimpleItem<TContainer, TItem>
		{
			return new OneToManyCollectionAdapter<TContainer, TItem>(
				new OneToManyCollectionSimpleContainerAdapter<TContainer, TItem>(),
				new OneToManyCollectionSimpleItemAdapter<TContainer, TItem>());
		}

		/// <summary>Creates the adapter for two separate adapter.</summary>
		/// <typeparam name="TContainer">The type of the container.</typeparam>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="containerAdapter">The container adapter.</param>
		/// <param name="itemAdapter">The item adapter.</param>
		/// <returns></returns>
		public static IOneToManyCollectionAdapter<TContainer, TItem> Make<TContainer, TItem>(
			IOneToManyCollectionContainerAdapter<TContainer, TItem> containerAdapter,
			IOneToManyCollectionItemAdapter<TContainer, TItem> itemAdapter)
			where TContainer: class
			where TItem: class
		{
			return new OneToManyCollectionAdapter<TContainer, TItem>(containerAdapter, itemAdapter);
		}
	}

	#endregion

	#region class OneToManyCollectionAdapter<TContainer, TItem>

	/// <summary>Create <see cref="IOneToManyCollectionAdapter{TContainer,TItem}"/> with two adapters, for container and for the item.</summary>
	/// <typeparam name="TContainer">The type of the container.</typeparam>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	public class OneToManyCollectionAdapter<TContainer, TItem>: IOneToManyCollectionAdapter<TContainer, TItem>
		where TContainer: class 
		where TItem: class
	{
		#region fields

		readonly IOneToManyCollectionContainerAdapter<TContainer, TItem> m_ContainerAdapter;
		readonly IOneToManyCollectionItemAdapter<TContainer, TItem> m_ItemAdapter;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="OneToManyCollectionAdapter&lt;TContainer, TItem&gt;"/> class.</summary>
		/// <param name="containerAdapter">The container adapter.</param>
		/// <param name="itemAdapter">The item adapter.</param>
		public OneToManyCollectionAdapter(
			IOneToManyCollectionContainerAdapter<TContainer, TItem> containerAdapter,
			IOneToManyCollectionItemAdapter<TContainer, TItem> itemAdapter)
		{
			if (containerAdapter == null)
				throw new ArgumentNullException("containerAdapter", "containerAdapter is null.");
			if (itemAdapter == null)
				throw new ArgumentNullException("itemAdapter", "itemAdapter is null.");

			m_ContainerAdapter = containerAdapter;
			m_ItemAdapter = itemAdapter;
		}

		#endregion

		#region IOneToManyCollectionAdapter<TContainer,TItem> Members

		/// <summary>Gets the field responsible for holding a collection on container side.</summary>
		/// <param name="container">The container.</param>
		/// <returns>Collection of items.</returns>
		public ICollection<TItem> RawGetItems(TContainer container)
		{
			return m_ContainerAdapter.GetItems(container);
		}

		/// <summary>Gets the field responsible from holding a reference to container on item side.</summary>
		/// <param name="item">The item.</param>
		/// <returns>Reference to container.</returns>
		public TContainer RawGetContainer(TItem item)
		{
			return m_ItemAdapter.GetContainer(item);
		}

		/// <summary>Set the container reference on item side.</summary>
		/// <param name="item">The item.</param>
		/// <param name="container">The container.</param>
		public void RawSetContainer(TItem item, TContainer container)
		{
			m_ItemAdapter.SetContainer(item, container);
		}

		/// <summary>Adds the item to container.</summary>
		/// <param name="container">The container.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item is now contained by container (this includes the when it already was); 
		/// <c>false</c> if for some reason it failed (unlikely).</returns>
		public bool RawAddItem(TContainer container, TItem item)
		{
			return m_ContainerAdapter.RawAddItem(container, item);
		}

		/// <summary>Removes the item from container.</summary>
		/// <param name="container">The container.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if item is now not in container (this includes th case when it never was); 
		/// <c>false</c> is for some reason removing failed (unlikely).</returns>
		public bool RawRemoveItem(TContainer container, TItem item)
		{
			return m_ContainerAdapter.RawRemoveItem(container, item);
		}

		#endregion
	}

	#endregion
}
