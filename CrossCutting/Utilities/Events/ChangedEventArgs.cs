using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Represents an event argument for the holding the old and new value of a generic type
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ChangedEventArgs<T> : EventArgs
	{
		#region fields

		private T m_OldValue;
		private T m_NewValue;

		#endregion

		#region properties

		/// <summary>
		/// Gets the old value.
		/// </summary>
		/// <value>The old value.</value>
		public T OldValue
		{
			get { return m_OldValue; }
			protected internal set { m_OldValue = value; }
		}

		/// <summary>
		/// Gets the new value.
		/// </summary>
		/// <value>The new value.</value>
		public T NewValue
		{
			get { return m_NewValue; }
			protected internal set { m_NewValue = value; }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgs&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		public ChangedEventArgs(T oldValue, T newValue)
		{
			m_OldValue = oldValue;
			m_NewValue = newValue;
		}

		#endregion
	}
}
