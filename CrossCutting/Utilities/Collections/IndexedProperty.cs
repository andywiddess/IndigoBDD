using System;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Simplifies access to indexed properties.</summary>
	/// <typeparam name="K">Key type.</typeparam>
	/// <typeparam name="V">Value type.</typeparam>
	public class IndexedProperty<K, V>: IIndexed<K, V>
	{
		#region fields

		/// <summary>Setter.</summary>
		private readonly Action<K, V> m_Setter;

		/// <summary>Getter.</summary>
		private readonly Func<K, V> m_Getter;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="IndexedProperty&lt;K, V&gt;"/> class.</summary>
		/// <param name="getter">The getter.</param>
		/// <param name="setter">The setter.</param>
		public IndexedProperty(Func<K, V> getter, Action<K, V> setter)
		{
			m_Getter = getter;
			m_Setter = setter;
		}

		#endregion

		#region IIndexed<K,V> Members

		/// <summary>Gets or sets the value at the specified index.</summary>
		public V this[K index]
		{
			get { return m_Getter(index); }
			set { m_Setter(index, value); }
		}

		#endregion
	}
}
