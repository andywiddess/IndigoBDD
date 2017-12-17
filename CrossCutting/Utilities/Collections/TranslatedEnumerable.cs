using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Translates one IEnumerable into another.
	/// </summary>
	/// <typeparam name="S">Source type.</typeparam>
	/// <typeparam name="T">Target type.</typeparam>
	public class TranslatedEnumerable<S, T> : IEnumerable<T>
	{
		#region fields

		/// <summary>
		/// Original IEnumerable.
		/// </summary>
		private IEnumerable<S> m_Enumerable;

		/// <summary>
		/// Object translator.
		/// </summary>
		private IObjectTranslator<S, T> m_Translator;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslatedEnumerable&lt;S, T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="translator">The translator.</param>
		public TranslatedEnumerable(IEnumerable<S> collection, IObjectTranslator<S, T> translator)
		{
			if (collection == null)
				throw new ArgumentNullException("collection", "collection is null.");
			if (translator == null)
				translator = CastObjectTranslator<S, T>.Default;

			m_Enumerable = collection;
			m_Translator = translator;
		}

		/// <summary>Initializes a new instance of the <see cref="TranslatedEnumerable&lt;S, T&gt;"/> class.</summary>
		/// <param name="collection">The collection.</param>
		/// <param name="sourceToTarget">The source to target conversion.</param>
		public TranslatedEnumerable(IEnumerable<S> collection, Func<S, T> sourceToTarget)
			: this(collection, new CallbackObjectTranslator<S, T>(sourceToTarget, null))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslatedEnumerable&lt;S, T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public TranslatedEnumerable(IEnumerable<S> collection)
			: this(collection, (IObjectTranslator<S, T>) null)
		{
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return new TranslatedEnumerator<S, T>(m_Enumerable.GetEnumerator(), m_Translator);
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
