using System;

namespace Indigo.CrossCutting.Utilities.Collections.OneToMany
{
	#region class OneToManyCollectionSimpleItemAdapter<TContrainer, TItem>

	/// <summary>
	/// <see cref="IOneToManyCollectionItemAdapter{TContainer,TItem}"/> implementation
	/// for <see cref="IOneToManyCollectionSimpleItem{TContrainer,TItem}"/>.
	/// </summary>
	/// <typeparam name="TContrainer">The type of the container.</typeparam>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	internal class OneToManyCollectionSimpleItemAdapter<TContrainer, TItem>:
		IOneToManyCollectionItemAdapter<TContrainer, TItem>
		where TContrainer: class
		where TItem: class, IOneToManyCollectionSimpleItem<TContrainer, TItem>
	{
		#region IOneToManyCollectionItemAdapter<TContrainer,TItem> Members

		public TContrainer GetContainer(TItem item)
		{
			if (item == null) return null;
			return item.Container;
		}

		public void SetContainer(TItem item, TContrainer container)
		{
			if (item == null) return;
			item.Container = container;
		}

		#endregion
	}

	#endregion
}
