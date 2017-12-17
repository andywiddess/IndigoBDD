using System;

namespace Indigo.CrossCutting.Utilities.Collections.OneToMany
{
	/// <summary>
	/// Methods extending <see cref="IOneToManyCollectionAdapter{TContainer,TItem}"/> with some helpful methods.
	/// Oposite to original adapter they are fully implemented.
	/// </summary>
	public static class OneToManyCollectionHelper
	{
		/// <summary>Adds the the item to container using specified adapter.</summary>
		/// <typeparam name="TContainer">The type of the container.</typeparam>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="adapter">The adapter.</param>
		/// <param name="container">The container.</param>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public static bool Add<TContainer, TItem>(
			this IOneToManyCollectionAdapter<TContainer, TItem> adapter, 
			TContainer container, TItem item)
			where TContainer: class
			where TItem: class
		{
			bool result;

			var current = adapter.RawGetContainer(item);
			if (current == null)
			{
				result = adapter.RawAddItem(container, item);
				adapter.RawSetContainer(item, container);
			}
			else
			{
				throw new ArgumentException();
			}

			return result;
		}

		/// <summary>Removes the the item from container using specified adapter.</summary>
		/// <typeparam name="TContainer">The type of the container.</typeparam>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="adapter">The adapter.</param>
		/// <param name="container">The container.</param>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public static bool Remove<TContainer, TItem>(
			this IOneToManyCollectionAdapter<TContainer, TItem> adapter, 
			TContainer container, TItem item)
			where TContainer: class
			where TItem: class
		{
			bool result;

			var current = adapter.RawGetContainer(item);
			if (object.ReferenceEquals(current, container))
			{
				result = adapter.RawRemoveItem(container, item);
				adapter.RawSetContainer(item, null);
			}
			else
			{
				result = true;
			}

			return result;
		}

		/// <summary>Moves item from one container to another using specified adapter.</summary>
		/// <typeparam name="TContainer">The type of the container.</typeparam>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="adapter">The adapter.</param>
		/// <param name="item">The item.</param>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static bool MoveTo<TContainer, TItem>(
			this IOneToManyCollectionAdapter<TContainer, TItem> adapter,
			TItem item, TContainer container)
			where TContainer: class
			where TItem: class
		{
			bool result = false;

			var current = adapter.RawGetContainer(item);
			if (object.ReferenceEquals(current, container))
			{
				result = true;
			}
			else
			{
				if (current != null && adapter.RawRemoveItem(current, item))
				{
					adapter.RawSetContainer(item, null);
				}
				if (container != null && adapter.RawRemoveItem(container, item))
				{
					adapter.RawSetContainer(item, container);
					result = true;
				}
			}

			return result;
		}

	}
}
