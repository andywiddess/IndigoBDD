using System;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Helper class for <see cref="DisposableLazyProxy{T}"/></summary>
	public class DisposableLazyProxy
	{
		/// <summary>Makes the dispasable lazy proxy for given value, with given factory.</summary>
		/// <typeparam name="T">Oject type.</typeparam>
		/// <param name="value">The value.</param>
		/// <param name="factory">The factory.</param>
		/// <returns>Proxy.</returns>
		public static DisposableLazyProxy<T> Make<T>(T value, Func<T> factory)
			where T: class, IDisposable
		{
			return new DisposableLazyProxy<T>(value, factory);
		}

		/// <summary>Makes the dispasable lazy proxy with given factory.</summary>
		/// <typeparam name="T">Oject type.</typeparam>
		/// <param name="factory">The factory.</param>
		/// <returns>Proxy.</returns>
		public static DisposableLazyProxy<T> Make<T>(Func<T> factory)
			where T: class, IDisposable
		{
			return new DisposableLazyProxy<T>(null, factory);
		}
	}

	/// <summary>
	/// Proxy object holding reference to an object and having a factory of such objects.
	/// If proxy is called and object was not initialized (null) it will be created, if object has been passed
	/// it will be returned.
	/// When proxy is disposed it will dispose underlaying object if has been created by proxy or not if it has been passed.
	/// It's not a bid deal, but it simplifies using sections when object can be given (not dispose needed) or created on 
	/// demand (it should be disposed then).
	/// </summary>
	/// <typeparam name="T">Type of object.</typeparam>
	public class DisposableLazyProxy<T>: IDisposable
		where T: class, IDisposable
	{
		#region fields

		/// <summary>Proxied value.</summary>
		private T m_Value;

		/// <summary>Factory.</summary>
		private readonly Func<T> m_Factory;

		/// <summary>Should object be disposed.</summary>
		private bool m_Dispose;

		#endregion

		#region properties

		/// <summary>Gets the value. Returns precached one or uses factory to generate it.</summary>
		public T Value
		{
			get
			{
				if (object.ReferenceEquals(m_Value, null))
				{
					m_Value = m_Factory == null ? null : m_Factory();
					m_Dispose = true;
				}
				return m_Value;
			}
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="DisposableLazyProxy&lt;T&gt;"/> class.</summary>
		/// <param name="value">The value.</param>
		/// <param name="factory">The factory.</param>
		public DisposableLazyProxy(T value, Func<T> factory)
		{
			m_Value = value;
			m_Factory = factory;
		}

		#endregion

		#region IDisposable Members

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
			if (m_Dispose)
			{
				m_Dispose = false;
				var value = m_Value;
				m_Value = null;
				if (value != null) value.Dispose();
			}
			else
			{
				m_Value = null;
			}
		}

		#endregion

		#region operators

		/// <summary>Performs an implicit conversion from <see cref="Indigo.CrossCutting.Utilities.Collections.DisposableLazyProxy&lt;T&gt;"/> to <typeparamref name="T"/>.</summary>
		/// <param name="proxy">The proxy.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T(DisposableLazyProxy<T> proxy)
		{
			return proxy == null ? null : proxy.Value;
		}

		#endregion
	}
}
