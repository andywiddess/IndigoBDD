using System;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Lazy proxy with weak reference. Object is stored in proxy but it is weakly refereced so it can be garbage collected.
	/// If object is needed again it will be recreated using factory.
	/// </summary>
	/// <typeparam name="T">Type of object.</typeparam>
	public class WeakLazyProxy<T> where T: class
	{
		#region fields

		private Func<T> m_Factory;
		private WeakReference m_Reference;

		#endregion

		#region properties

		/// <summary>Gets or sets the factory.</summary>
		/// <value>The factory.</value>
		public Func<T> Factory
		{
			get { return m_Factory; }
			set
			{
				if (m_Factory != value)
				{
					m_Factory = value;
					m_Reference = null;
				}
			}
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WeakLazyProxy&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		public WeakLazyProxy(Func<T> factory)
		{
			m_Factory = factory;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WeakLazyProxy&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="factory">The factory.</param>
		public WeakLazyProxy(T value, Func<T> factory)
			: this(factory)
		{
			if (value != null)
				m_Reference = new WeakReference(value);
		}

		#endregion

		#region public interface

		/// <summary>
		/// Gets a value indicating whether this instance is alive.
		/// </summary>
		/// <value><c>true</c> if this instance is alive; otherwise, <c>false</c>.</value>
		public bool IsAlive
		{
			get { return m_Reference != null && m_Reference.IsAlive; }
		}

		/// <summary>Gets the value.</summary>
		public T Value
		{
			get
			{
				var result = m_Reference == null ? null : (T)m_Reference.Target;
				if (result == null)
				{
					m_Reference = null;
					result = m_Factory == null ? null : m_Factory();
					if (result != null) m_Reference = new WeakReference(result);
				}
				return result;
			}
		}

		#endregion
	}
}
