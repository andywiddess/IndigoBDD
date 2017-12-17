using System;
using System.Collections.Generic;
using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Events;

namespace Indigo.CrossCutting.Utilities.Collections.OneToMany
{
	#region class OneToManyCollection

	/// <summary>Helper class to create <see cref="OneToManyCollection{TContainer,TItem}"/> instances.</summary>
	public static class OneToManyCollection
	{
		/// <summary>Creates collection inside given container.</summary>
		/// <typeparam name="TContainer">The type of the container.</typeparam>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="container">The container.</param>
		/// <param name="adapter">The adapter.</param>
		/// <returns>New collection.</returns>
		public static OneToManyCollection<TContainer, TItem> Make<TContainer, TItem>(
			TContainer container, IOneToManyCollectionAdapter<TContainer, TItem> adapter)
			where TContainer: class
			where TItem: class
		{
			return new OneToManyCollection<TContainer, TItem>(container, adapter);
		}
	}

	#endregion

	#region class OneToManyCollection<TContainer, TItem>

	/// <summary>Class handling one-to-many relation based on collection. Ensures that references are updated on both sides.
	/// This class sacrifices speed for convenience. If lot of processing is going to be done on such collection it's probably
	/// not the best option to use it.
	/// Note, It requires an adapter which handles both sides (<see cref="IOneToManyCollectionAdapter{TContainer,TItem}"/>, 
	/// but sometimes it's easier to write two adapters, for the container (<see cref="IOneToManyCollectionContainerAdapter{TContainer,TItem}"/>) 
	/// and for the item (<see cref="IOneToManyCollectionItemAdapter{TContainer,TItem}"/>).
	/// </summary>
	/// <typeparam name="TContainer">The type of the container.</typeparam>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	public class OneToManyCollection<TContainer, TItem>
		where TContainer: class
		where TItem: class
	{
		#region fields

		/// <summary>Adapter (mediator).</summary>
		private readonly IOneToManyCollectionAdapter<TContainer, TItem> m_Adapter;

		/// <summary>Container (object which has owns this collection).</summary>
		private readonly TContainer m_Container;

		/// <summary>Fixed collection. Even if original collection has been recreated this reference is still valid.</summary>
		private readonly FixedRefCollection<TItem> m_Fixed;

		/// <summary>Monitored collection.</summary>
		private readonly LazyProxy<MonitoredCollection<TItem>> m_MonitoredCollection;

		/// <summary>Read only collection.</summary>
		private readonly LazyProxy<ReadOnlyCollection<TItem>> m_ReadOnlyCollection;

		#endregion

		#region properties

		/// <summary>Gets the read only collection.</summary>
		public ReadOnlyCollection<TItem> ReadOnly
		{
			get { return m_ReadOnlyCollection.Proxy; }
		}

		/// <summary>Gets the monitored. Please note, it is essential to NOT suspend events.</summary>
		public MonitoredCollection<TItem> Monitored
		{
			get { return m_MonitoredCollection.Proxy; }
		}

		/// <summary>Gets or sets a value indicating whether source collection is volatile.</summary>
		/// <value><c>true</c> if volatile; otherwise, <c>false</c>.</value>
		public bool Volatile
		{
			get { return m_Fixed.Volatile; }
			set { m_Fixed.Volatile = value; }
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="OneToManyCollection&lt;TContainer, TItem&gt;"/> class.</summary>
		/// <param name="container">The container.</param>
		/// <param name="adapter">The adapter.</param>
		public OneToManyCollection(
			TContainer container, IOneToManyCollectionAdapter<TContainer, TItem> adapter)
		{
			if (adapter == null)
				throw new ArgumentNullException("adapter", "adapter is null.");

			m_Container = container;
			m_Adapter = adapter;

			m_Fixed = new FixedRefCollection<TItem>(RefreshOriginalReference, null);
			m_Fixed.BindingChanged += BindingChanged;
			m_MonitoredCollection = LazyProxy.Make(() => m_Fixed.Monitor(CollectionModified));
			m_ReadOnlyCollection = LazyProxy.Make(() => m_Fixed.ReadOnly());
		}

		#endregion

		#region public interface

		/// <summary>Invalidates source collection.</summary>
		public void Invalidate()
		{
			m_Fixed.Invalidate();
		}

		#endregion

		#region internal interface

		/// <summary>Adds the specified item.</summary>
		/// <param name="item">The item.</param>
		internal void Add(TItem item)
		{
			m_MonitoredCollection.Proxy.Add(item);
		}

		/// <summary>Removes the specified item.</summary>
		/// <param name="item">The item.</param>
		internal void Remove(TItem item)
		{
			m_MonitoredCollection.Proxy.Remove(item);
		}

		#endregion

		#region private implmentation

		/// <summary>Refreshes the original reference.</summary>
		/// <returns>Reference to original collection.</returns>
		public ICollection<TItem> RefreshOriginalReference()
		{
			return m_Adapter.RawGetItems(m_Container);
		}

		/// <summary>Triggered when reference to original collection has been changed.</summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The instance containing the event data.</param>
		private void BindingChanged(object sender, ChangedEventArgs<ICollection<TItem>> e)
		{
			if (m_MonitoredCollection != null)
			{
				// suspend/resume to refresh
				// do not use Proxy (which would create object)
				// rather use Value (if it's null, nobody can listen anyway)
				var weakMonitored = m_MonitoredCollection.Value;
				if (weakMonitored != null)
				{
					using (weakMonitored.EventLock()) Patterns.NoOp();
				}
			}
		}

		/// <summary>Triggered when collections is modified. It is essential to handler this event properly.</summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The instance containing the event data.</param>
		private void CollectionModified(object sender, MonitoredCollectionEventArgs<TItem> e)
		{
			if (e.Canceled) return;

			switch (e.EventType)
			{
				case MonitoredCollectionEventType.Adding:
					TContainer currentContainer = m_Adapter.RawGetContainer(e.Item);
					if (currentContainer != null && currentContainer != m_Container)
						throw new ArgumentException();
					break;

				case MonitoredCollectionEventType.Added:
					m_Adapter.RawSetContainer(e.Item, m_Container);
					break;

				case MonitoredCollectionEventType.ClearApproved:
					m_Adapter.RawGetItems(m_Container).ForEach((i) => m_Adapter.RawSetContainer(i, null));
					break;

				case MonitoredCollectionEventType.Removed:
					m_Adapter.RawSetContainer(e.Item, null);
					break;

				case MonitoredCollectionEventType.Clearing:
				case MonitoredCollectionEventType.Cleared:
				case MonitoredCollectionEventType.Suspended:
				case MonitoredCollectionEventType.Resumed:
				case MonitoredCollectionEventType.Removing:
				case MonitoredCollectionEventType.Cancelled:
				case MonitoredCollectionEventType.None:
				default:
					break;
			}
		}

		#endregion
	}

	#endregion
}
