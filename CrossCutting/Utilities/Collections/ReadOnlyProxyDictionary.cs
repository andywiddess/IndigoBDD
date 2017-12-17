using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Dictionary with readonly proxy.
	/// </summary>
	/// <typeparam name="K">Key.</typeparam>
	/// <typeparam name="V">Value.</typeparam>
	public class ReadOnlyProxyDictionary<K, V>: LazyProxy<IDictionary<K, V>, ReadOnlyDictionary<K, V>>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyProxyDictionary&lt;K, V&gt;"/> class.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		public ReadOnlyProxyDictionary(IDictionary<K, V> dictionary)
			: base(dictionary, CreateProxy)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyProxyDictionary&lt;K, V&gt;"/> class.
		/// </summary>
		/// <param name="factory">The factory producing original object on demand.</param>
		public ReadOnlyProxyDictionary(Func<IDictionary<K, V>> factory)
			: base(factory, CreateProxy)
		{
		}

		/// <summary>
		/// Creates the proxy.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <returns>Generated proxy.</returns>
		protected static ReadOnlyDictionary<K, V> CreateProxy(IDictionary<K, V> dictionary)
		{
			return new ReadOnlyDictionary<K, V>(dictionary);
		}
	}
}
