using System;
using System.Reflection;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>The object-in-the-middle of weak events.</summary>
	/// <typeparam name="T">Type of target object.</typeparam>
	/// <typeparam name="E">Type of EventArgs.</typeparam>
	internal class WeakEventHandler<T, E>: IWeakEventHandler<E>
		where T: class
		where E: EventArgs
	{
		/// <summary>Open delegate.</summary>
		/// <param name="this">The @this.</param>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The event args.</param>
		private delegate void OpenEventHandler(T @this, object sender, E eventArgs);

		/// <summary>Holds the reference to target object.</summary>
		private readonly WeakReference m_TargetRef;

		/// <summary>Reference to method called on target object.</summary>
		private readonly MethodInfo m_Method;

		/// <summary>Open delegate. The method and target are stored separately, because target isn't known before it is 
		/// going to be called.</summary>
		private readonly OpenEventHandler m_OpenHandler;

		/// <summary>Weak handler delegate. Always points to Invoke method in <c>this</c>.</summary>
		private readonly EventHandler<E> m_Handler;

		/// <summary>Unregister callback.</summary>
		private WeakEventUnregisterCallback<E> m_Unregister;

		/// <summary>Initializes a new instance of the <see cref="WeakEventHandler&lt;T, E&gt;"/> class.</summary>
		/// <param name="eventHandler">The event handler.</param>
		/// <param name="unregisterCallback">The unregister callback.</param>
		public WeakEventHandler(EventHandler<E> eventHandler, WeakEventUnregisterCallback<E> unregisterCallback)
		{
			m_TargetRef = new WeakReference(eventHandler.Target);
			m_Method = eventHandler.Method;
			m_OpenHandler = (OpenEventHandler)Delegate.CreateDelegate(typeof(OpenEventHandler), null, m_Method);
			m_Handler = Invoke;
			m_Unregister = unregisterCallback;
		}

		/// <summary>Inkoves the event handler.</summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The event args.</param>
		public void Invoke(object sender, E eventArgs)
		{
			T target = (T)m_TargetRef.Target;

			if (target != null)
			{
				Invoke(target, sender, eventArgs);
			}
			else
			{
				Unregister();
			}
		}

		/// <summary>Invokes the event handler of given target.</summary>
		/// <param name="target">The target.</param>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The event args.</param>
		private void Invoke(T target, object sender, E eventArgs)
		{
			m_OpenHandler.Invoke(target, sender, eventArgs);
		}

		/// <summary>Unregisters the event handler.</summary>
		public void Unregister()
		{
			if (m_Unregister != null)
			{
				m_Unregister(m_Handler);
				m_Unregister = null;
			}
		}

		/// <summary>Handler-in-the-middle.</summary>
		public EventHandler<E> Handler
		{
			get { return m_Handler; }
		}

		/// <summary>Checks if this handler calls given concreate handler.</summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if this handler calls given concreate handler, <c>false</c> otherwise.</returns>
		public bool Matches(EventHandler<E> value)
		{
			return value.Target == (T)m_TargetRef.Target && value.Method == m_Method;
		}

		//public static implicit operator EventHandler<E>(WeakEventHandler<T, E> weh)
		//{
		//    return weh.m_Handler;
		//}
	}
}
