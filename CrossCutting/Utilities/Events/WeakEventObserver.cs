using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>Event observer weak reference.</summary>
	/// <typeparam name="E">Type of event args.</typeparam>
	internal class WeakEventObserver<E>: IEventObserver<E> where E: EventArgs
	{
		#region fields

		/// <summary>Weak reference to 'real' observer.</summary>
		private readonly WeakReference m_Reference;

		#endregion

		#region properties

		/// <summary>Gets a value indicating whether observer is alive.</summary>
		/// <value><c>true</c> if observer is alive; otherwise, <c>false</c>.</value>
		public bool IsAlive
		{
			get { return m_Reference != null && m_Reference.IsAlive; }
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="WeakEventObserver&lt;E&gt;"/> class.</summary>
		/// <param name="observer">The observer.</param>
		public WeakEventObserver(IEventObserver<E> observer)
		{
			if (observer == null)
				throw new ArgumentNullException("observer", "observer is null.");
			m_Reference = new WeakReference(observer);
		}

		#endregion

		#region IEventObserver<E> Members

		/// <summary>Called when event is invoked.</summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The event args.</param>
		public void OnInvoked(object sender, E eventArgs)
		{
			var observer = m_Reference == null ? null : (IEventObserver<E>)m_Reference.Target;
			if (observer != null) observer.OnInvoked(sender, eventArgs);
		}

		#endregion

		#region internal interface

		/// <summary>Determines if observer matches the specified given observer (weak observer match other observer 
		/// if it calls it).</summary>
		/// <param name="givenObserver">The given observer.</param>
		/// <returns><c>true</c> if it matches.</returns>
		internal bool Match(IEventObserver<E> givenObserver)
		{
			var observer = m_Reference == null ? null : (IEventObserver<E>)m_Reference.Target;
			return object.ReferenceEquals(observer, givenObserver);
		}

		#endregion
	}
}
