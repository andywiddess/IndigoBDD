using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Collection with ReadOnly proxy.
	/// </summary>
	/// <typeparam name="T">Item type.</typeparam>
	public class ReadOnlyProxyCollection<T>: LazyProxy<ICollection<T>, ReadOnlyCollection<T>>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyProxyCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public ReadOnlyProxyCollection(ICollection<T> collection)
			: base(collection, CreateProxy)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyProxyCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="factory">The factory generating original object on demand.</param>
		public ReadOnlyProxyCollection(Func<ICollection<T>> factory)
			: base(factory, CreateProxy)
		{
		}

		/// <summary>
		/// Creates the proxy.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <returns>Generated proxy.</returns>
		protected static ReadOnlyCollection<T> CreateProxy(ICollection<T> collection)
		{
			return new ReadOnlyCollection<T>(collection);
		}
	}
}
