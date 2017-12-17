using System;
using System.Collections.Generic;
using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>Event to abserve.</summary>
	/// <typeparam name="E">Type of event args</typeparam>
	public class ObservableEvent<E> where E: EventArgs
	{
		#region consts

		/// <summary>Number of calls between purging dead event handlers (if any).</summary>
		private const int MAX_PURGE_DELAY = 128;
		/// <summary>Number of dead event handlers which forces premature purge.</summary>
		private const int MAX_DEAD_OBSERVERS = 16;

		#endregion

		#region fields

		/// <summary>Number of event calls.</summary>
		private int m_InvokeCounter = MAX_PURGE_DELAY;

		/// <summary>Collection of observers.</summary>
		private LinkedList<IEventObserver<E>> m_Observers;

		/// <summary>Sync root.</summary>
		private readonly object m_SyncRoot = new object();

		#endregion

		#region properties

		/// <summary>Gets the observers.</summary>
		private LinkedList<IEventObserver<E>> Observers
		{
			get
			{
				if (m_Observers == null) m_Observers = new LinkedList<IEventObserver<E>>();
				return m_Observers;
			}
		}

		#endregion

		#region public interface

		/// <summary>Gets a value indicating whether this instance has observers.</summary>
		/// <value><c>true</c> if this instance has observers; otherwise, <c>false</c>.</value>
		public bool HasObservers
		{
			get { lock (m_SyncRoot) { return m_Observers != null && m_Observers.Count != 0; } }
		}

		/// <summary>Adds the specified observer.</summary>
		/// <param name="observer">The observer.</param>
		/// <param name="weak">if set to <c>true</c> weak reference is created.</param>
		public void Add(IEventObserver<E> observer, bool weak = true)
		{
			if (weak)
			{
				observer = new WeakEventObserver<E>(observer);
			}

			lock (m_SyncRoot)
			{
				Observers.AddLast(observer);
			}
		}

		/// <summary>Removes the specified observer.</summary>
		/// <param name="observer">The observer.</param>
		public void Remove(IEventObserver<E> observer)
		{
			lock (m_SyncRoot)
			{
				if (m_Observers == null || m_Observers.Count == 0) return;
				var current = m_Observers.First;

				while (current != null)
				{
					if (Match(current.Value, observer))
					{
						m_Observers.Remove(current);
						break;
					}
					current = current.Next;
				}
			}
		}

		/// <summary>Determines whether event is already observed by specified observer.</summary>
		/// <param name="observer">The observer.</param>
		/// <returns><c>true</c> if event is already observed by specified observer; otherwise, <c>false</c>.</returns>
		public bool Contains(IEventObserver<E> observer)
		{
			lock (m_SyncRoot)
			{
				if (m_Observers == null || m_Observers.Count == 0) return false;
				var current = m_Observers.First;

				while (current != null)
				{
					if (Match(current.Value, observer)) return true;
					current = current.Next;
				}
			}

			return false;
		}

		/// <summary>Flushes dead observers.</summary>
		public void Flush()
		{
			lock (m_SyncRoot)
			{
				if (m_Observers == null || m_Observers.Count == 0) return;
				var current = m_Observers.First;

				while (current != null)
				{
					if (IsAlive(current.Value))
					{
						var next = current.Next;
						m_Observers.Remove(current);
						current = next;
					}
					else
					{
						current = current.Next;
					}
				}
			}
		}

		/// <summary>Invokes the event.</summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The event args.</param>
		public void Invoke(object sender, E eventArgs)
		{
			IEventObserver<E>[] observers;

			lock (m_SyncRoot)
			{
				if (m_Observers == null || m_Observers.Count == 0) return;
				observers = new IEventObserver<E>[m_Observers.Count];
				m_Observers.CopyTo(observers, 0);
				m_InvokeCounter = Math.Max(0, m_InvokeCounter - 1);
			}

			int deadObservers = 0;
			foreach (var observer in observers)
			{
				if (IsAlive(observer))
				{
					observer.OnInvoked(sender, eventArgs);
				}
				else
				{
					deadObservers++;
				}
			}

			if (deadObservers > MAX_DEAD_OBSERVERS || (deadObservers > 0 && m_InvokeCounter <= 0))
			{
				Patterns.Fork(Flush);
				m_InvokeCounter = MAX_PURGE_DELAY;
			}
		}

		#endregion

		#region private implementation

		/// <summary>Determines whether the specified observer is alive.</summary>
		/// <param name="observer">The observer.</param>
		/// <returns><c>true</c> if the specified observer is alive; otherwise, <c>false</c>.</returns>
		private static bool IsAlive(IEventObserver<E> observer)
		{
			if (observer == null) return false;
			var weakObserver = observer as WeakEventObserver<E>;
			return weakObserver == null || weakObserver.IsAlive;
		}

		/// <summary>Determines if given observer matches the specified stored observer.
		/// Note, it has been introduced because some observers are weak introducing new layer of indirection,
		/// so simple "ReferenceEquals" is not enough.</summary>
		/// <param name="storedObserver">The stored observer.</param>
		/// <param name="givenObserver">The given observer.</param>
		/// <returns><c>true</c> if given observer matches stored observer.</returns>
		private static bool Match(IEventObserver<E> storedObserver, IEventObserver<E> givenObserver)
		{
			if (object.ReferenceEquals(storedObserver, givenObserver)) return true;

			var weakObserver = storedObserver as WeakEventObserver<E>;
			if (weakObserver != null && weakObserver.Match(givenObserver)) return true;

			return false;
		}

		#endregion
	}
}
