using System;
using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Collections
{
	#region class LazyProxy

	/// <summary>Helper class to make access to LazyProxy's constructors easier.</summary>
	public static class LazyProxy
	{
		/// <summary>Makes the lazy proxy of specified type with given factory.</summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="factory">The factory.</param>
		/// <returns>LazyProxy</returns>
		public static LazyProxy<T> Make<T>(Func<T> factory)
		{
			return new LazyProxy<T>(factory);
		}
	}

	#endregion

	#region class LazyProxy<V, P>

	/// <summary>
	/// Helper class to build properties with proxies.
	/// Used for dual access: internal and external, when object is, for example,
	/// mutable internally and immutable externally.
	/// </summary>
	/// <typeparam name="V">Type of original value.</typeparam>
	/// <typeparam name="P">Type of proxy.</typeparam>
	/// <example>
	/// LazyProxy&lt;List, ReadOnlyList&gt; lp = new LazyProxy&lt;List, ReadOnlyList&gt;();
	/// lp.Value = RealList;
	/// lp.Proxy.Add(...); // won't compile ReadOnlyList does not have .Add method
	/// </example>
	public class LazyProxy<V, P>
	{
		#region fields

		private readonly object m_SyncRoot = new object();
		private readonly Func<V> m_Factory;
		private Converter<V, P> m_Converter;

		private V m_Value;
		private P m_Proxy;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the converter. Reseting converter resets proxy as well.
		/// </summary>
		/// <value>The converter.</value>
		public Converter<V, P> Converter
		{
			get { return m_Converter; }
			set
			{
				if (m_Converter == value) return;
				m_Converter = value;
				m_Proxy = default(P);
			}
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public V Value
		{
			get
			{
				lock (m_SyncRoot)
				{
					if (object.Equals(m_Value, default(V)) && m_Factory != null)
					{
						m_Value = m_Factory();
					}
					return m_Value;
				}
			}
			set
			{
				lock (m_SyncRoot)
				{
					if (object.Equals(m_Value, value)) return;
					m_Value = value;
					m_Proxy = default(P);
				}
			}
		}

		/// <summary>
		/// Gets the proxy.
		/// </summary>
		/// <value>The proxy.</value>
		public P Proxy
		{
			get { return Ensure(); }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="LazyProxy&lt;V, P&gt;"/> class.
		/// </summary>
		/// <param name="converter">The proxy generator.</param>
		public LazyProxy(Converter<V, P> converter)
		{
			m_Value = default(V);
			m_Proxy = default(P);

			m_Factory = null;
			m_Converter = converter;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LazyProxy&lt;V, P&gt;"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="converter">The proxy generator.</param>
		public LazyProxy(Func<V> factory, Converter<V, P> converter)
		{
			m_Value = default(V);
			m_Proxy = default(P);

			m_Factory = factory;
			m_Converter = converter;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LazyProxy&lt;V, P&gt;"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="factory">The factory.</param>
		/// <param name="converter">The proxy generator.</param>
		public LazyProxy(V value, Func<V> factory, Converter<V, P> converter)
		{
			m_Value = value;
			m_Proxy = default(P);

			m_Factory = factory;
			m_Converter = converter;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LazyProxy&lt;V, P&gt;"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="converter">The proxy generator.</param>
		public LazyProxy(V value, Converter<V, P> converter)
		{
			m_Value = value;
			m_Proxy = default(P);

			m_Factory = null;
			m_Converter = converter;
		}

		#endregion

		#region Ensure

		/// <summary>Ensures that proxy is set.</summary>
		/// <returns>Proxy object.</returns>
		public P Ensure()
		{
			lock (m_SyncRoot)
			{
				if (object.Equals(m_Proxy, default(P)) && m_Converter != null)
				{
					m_Proxy = m_Converter(Value);
				}
				return m_Proxy;
			}
		}

		/// <summary>Changes the value and ensures proxy is set.</summary>
		/// <param name="value">The value.</param>
		/// <returns>Proxy object.</returns>
		public P Ensure(V value)
		{
			lock (m_SyncRoot)
			{
				Value = value;
				return Ensure();
			}
		}

		#endregion
	}

	/// <summary>Helper class to build properties with proxies.
	/// Used for dual access: internal and external, when object is, for example,
	/// mutable internally and immutable externaly.</summary>
	/// <typeparam name="T">Type of original value.</typeparam>
	/// <example>
	/// LazyProxy&lt;List, ReadOnlyList&gt; lp = new LazyProxy&lt;List, ReadOnlyList&gt;();
	/// lp.Value = RealList;
	/// lp.Proxy.Add(...); // won't compile ReadOnlyList does not have .Add method
	/// </example>
	public class LazyProxy<T>: LazyProxy<T, T>
	{
		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="LazyProxy&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		public LazyProxy(Func<T> factory)
			: base(factory, Patterns.Pass<T>)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LazyProxy&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="converter">The proxy generator.</param>
		public LazyProxy(Converter<T, T> converter)
			: base(converter)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LazyProxy&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="converter">The proxy generator.</param>
		public LazyProxy(T value, Converter<T, T> converter)
			: base(value, converter)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LazyProxy&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="factory">The factory.</param>
		/// <param name="converter">The proxy generator.</param>
		public LazyProxy(T value, Func<T> factory, Converter<T, T> converter)
			: base(value, factory, converter)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LazyProxy&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <param name="converter">The proxy generator.</param>
		public LazyProxy(Func<T> factory, Converter<T, T> converter)
			: base(factory, converter)
		{
		}

		#endregion
	}

	#endregion
}