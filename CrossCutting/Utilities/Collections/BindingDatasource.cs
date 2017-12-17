using System.ComponentModel;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// A BindingList list extension which allows to
	/// know which item is going to be removed before it's
	/// actually removed (Microsoft forgot about such eventuality).
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class BindingDatasource<T>: BindingList<T>
	{
		#region events

		/// <summary>
		/// Occurs when item is about to be removed.
		/// </summary>
		public event ItemRemovingEvent<T> ItemRemoving;

		#endregion

		#region overrides

		/// <summary>
		/// Removes the item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">You are removing a newly added item and <see cref="P:System.ComponentModel.IBindingList.AllowRemove"/> is set to false. </exception>
		protected override void RemoveItem(int index)
		{
			bool cancel = false;
			if ((ItemRemoving != null) && (RaiseListChangedEvents))
			{
				ItemRemovingEventArgs<T> args = new ItemRemovingEventArgs<T>(index, this.Items[index]);
				ItemRemoving(this, args);
				cancel = args.Cancel;
			}

			if (!cancel)
			{
				base.RemoveItem(index);
			}
		}

		#endregion
	}

	#region class ItemRemovingEventArgs<T>

	/// <summary>
	/// Event args for ItemRemivingEvent.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ItemRemovingEventArgs<T>: CancelEventArgs
	{
		#region fields

		/// <summary>
		/// Index.
		/// </summary>
		private int m_Index;

		/// <summary>
		/// Item.
		/// </summary>
		private T m_Item;

		#endregion

		#region properties

		/// <summary>
		/// Gets the index.
		/// </summary>
		/// <value>The index.</value>
		public int Index
		{
			get { return m_Index; }
		}

		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <value>The item.</value>
		public T Item
		{
			get { return m_Item; }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ItemRemovingEventArgs&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		public ItemRemovingEventArgs(int index, T item)
		{
			m_Item = item;
			m_Index = index;
		}

		#endregion
	}

	#endregion

	#region delegate ItemRemovingEvent<T>

	/// <summary>
	/// Delegate called when item is about to be removed from BindingDatasource.
	/// </summary>
	public delegate void ItemRemovingEvent<T>(object sender, ItemRemovingEventArgs<T> args);

	#endregion
}
