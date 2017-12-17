using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// List with readonly proxy.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class ReadOnlyProxyList<T>: LazyProxy<IList<T>, ReadOnlyList<T>>
	{
		/// <summary>Initializes a new instance of the <see cref="ReadOnlyProxyList&lt;T&gt;"/> class.</summary>
		public ReadOnlyProxyList()
			: base(CreateValue, CreateProxy)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyProxyCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public ReadOnlyProxyList(IList<T> collection)
			: base(collection, CreateProxy)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyProxyCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="factory">The factory generating original object on demand.</param>
		public ReadOnlyProxyList(Func<IList<T>> factory)
			: base(factory, CreateProxy)
		{
		}

		/// <summary>Creates the value (list).</summary>
		/// <returns>List used to physically store items.</returns>
		protected static IList<T> CreateValue()
		{
			return new List<T>();
		}

		/// <summary>
		/// Creates the proxy.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <returns>Generated proxy.</returns>
		protected static ReadOnlyList<T> CreateProxy(IList<T> collection)
		{
			return new ReadOnlyList<T>(collection);
		}
	}
}
