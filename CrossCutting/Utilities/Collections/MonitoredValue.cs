using System;
using System.Collections.Generic;
using Indigo.CrossCutting.Utilities.Events;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Value holder which fires events when value is changing.
	/// NOTE, this class is NOT thread safe.
	/// </summary>
	/// <typeparam name="T">Any type.</typeparam>
	public class MonitoredValue<T>: ISuspendableEvents
	{
		#region fields

		private T m_Value;

		private IEqualityComparer<T> m_Comparer;
		private bool m_NullComparer;
		private int m_EventsSuspended;

		#endregion

		#region events

		/// <summary>
		/// Occurs when events are suspended.
		/// </summary>
		public event EventHandler EventsSuspended;

		/// <summary>
		/// Occurs when events are resumed.
		/// </summary>
		public event EventHandler EventsResumed;

		/// <summary>
		/// Occurs when (before) value is changing. Handler has a chance to cancel change to value.
		/// </summary>
		public event EventHandler<ChangingEventArgs<T>> Changing;

		/// <summary>
		/// Occurs when (after) value has been changed. Please note, although <see cref="ChangedEventArgs&lt;T&gt;"/> provides
		/// both old and new value, you cannot assume that old value is correct. After events and resumed
		/// provided old value is always equal to new value, it is not stored anywhere when events are suspended, so it is unkown
		/// when events are resumed.
		/// </summary>
		public event EventHandler<ChangedEventArgs<T>> Changed;

		/// <summary>
		/// Attaches to <see cref="Changed"/> event. Additionaly calls <paramref name="handler"/> immediately.
		/// NOTE, it does not call all the handlers, only given one.
		/// </summary>
		/// <param name="handler">The handler.</param>
		public void AttachChanged(EventHandler<ChangedEventArgs<T>> handler)
		{
			if (handler == null)
				throw new ArgumentNullException("handler", "handler is null.");
			Changed += handler;
			handler(this, new ChangedEventArgs<T>(m_Value, m_Value));
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the comparer. Comprr is used to decide if assignment really changes the value.
		/// You can use <c>null</c> to force events to be fired on every assignment.
		/// </summary>
		/// <value>The comparer.</value>
		public IEqualityComparer<T> Comparer
		{
			get { return m_NullComparer ? null : m_Comparer; }
			set
			{
				if (value == null)
				{
					m_Comparer = AlwaysFalseEqualityComparer<T>.Default;
					m_NullComparer = true;
				}
				else
				{
					m_Comparer = value;
					m_NullComparer = false;
				}
			}
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public T Value
		{
			get { return m_Value; }
			set { SetValue(value, false, false); }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredValue&lt;T&gt;"/> class.
		/// NOTE, listeners won't be notified about this value (default) unless you call <see cref="UpdateListeners"/>
		/// </summary>
		public MonitoredValue()
		{
			Comparer = EqualityComparer<T>.Default;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MonitoredValue&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="initial">The initial value. NOTE, listeners won't be notified about this value unless you call <see cref="UpdateListeners"/></param>
		public MonitoredValue(T initial)
			: this()
		{
			m_Value = initial;
		}

		#endregion

		#region utilities

		/// <summary>
		/// Determines whether current value is equal to new one.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if current value is equal to new one; otherwise, <c>false</c>.</returns>
		private bool IsValueEqual(T value)
		{
			return m_Comparer.Equals(m_Value, value);
		}

		/// <summary>
		/// Sets the value. 
		/// If <paramref name="force"/> is set to <c>true</c> assigning the value cannot be 'cancelled' by <see cref="Changing"/> event.
		/// If <paramref name="silent"/> is set to <c>true</c> events won't be triggered, this implies that argument <paramref name="force"/> is ignored (and assumed <c>true</c>).
		/// Use 'silent' mode, only if you truly understand the consequences.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="force">if set to <c>true</c> value is set forcefully.</param>
		/// <param name="silent">if set to <c>true</c> value is silently set.</param>
		public void SetValue(T value, bool force, bool silent)
		{
			if (!IsValueEqual(value) || force)
			{
				T oldValue = m_Value;

				if (!silent && m_EventsSuspended <= 0 && Changing != null)
				{
					ChangingEventArgs<T> args = new ChangingEventArgs<T>(m_Value, value);
					Changing(this, args);
					if (args.Cancel && !force) 
						return;
				}

				m_Value = value;

				if (!silent && m_EventsSuspended <= 0 && Changed != null)
				{
					Changed(this, new ChangedEventArgs<T>(oldValue, m_Value));
				}
			}
		}

		/// <summary>
		/// Updates the listeners. Triggers <see cref="EventsResumed"/> event.
		/// NOTE, it doesn't have to be really resumed.
		/// </summary>
		public void UpdateListeners()
		{
			if (EventsResumed != null)
			{
				EventsResumed(this, new EventArgs());
			}
		}

		#endregion

		#region conversion operators

		/// <summary>
		/// Performs an implicit conversion from <see cref="Indigo.CrossCutting.Utilities.Collections.MonitoredValue&lt;T&gt;"/> to <typeparamref name="T"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T(MonitoredValue<T> value)
		{
			return value.Value;
		}

		#endregion

		#region ISuspendableEvents Members

		/// <summary>
		/// Suspends events.
		/// If events become resumed listeners are notified (see: <see cref="UpdateListeners"/>).
		/// </summary>
		public void SuspendEvents()
		{
			m_EventsSuspended++;
			if (m_EventsSuspended == 1)
			{
				// events has been just suspended
				if (EventsSuspended != null)
				{
					EventsSuspended(this, new EventArgs());
				}
			}
		}

		/// <summary>
		/// Resumes events.
		/// </summary>
		public void ResumeEvents()
		{
			m_EventsSuspended--;
			if (m_EventsSuspended == 0)
			{
				// events has been just resumed
				if (EventsResumed != null)
				{
					EventsResumed(this, new EventArgs());
				}
			}
		}

		#endregion
	}

	#region class AlwaysFalseEqualityComparer<T>

	/// <summary>
	/// Implementation of <see cref="IEqualityComparer&lt;T&gt;"/> which always returns <c>false</c>.
	/// </summary>
	/// <typeparam name="T">Any type.</typeparam>
	public class AlwaysFalseEqualityComparer<T>: IEqualityComparer<T>
	{
		#region consts

		/// <summary>
		/// Default instance (aka singleton).
		/// </summary>
		public static readonly AlwaysFalseEqualityComparer<T> Default = new AlwaysFalseEqualityComparer<T>();

		#endregion

		#region IEqualityComparer<T> Members

		/// <summary>
		/// Determines whether the specified objects are equal.
		/// </summary>
		/// <param name="x">The first object of type <typeparamref name="T"/> to compare.</param>
		/// <param name="y">The second object of type <typeparamref name="T"/> to compare.</param>
		/// <returns>Always <c>false</c>.</returns>
		public bool Equals(T x, T y)
		{
			return false;
		}

		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
		/// </exception>
		public int GetHashCode(T obj)
		{
			return obj.GetHashCode();
		}

		#endregion
	}

	#endregion
}
