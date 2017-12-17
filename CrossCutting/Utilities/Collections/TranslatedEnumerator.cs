using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Translates one enumerator into another.
	/// </summary>
	/// <typeparam name="S">Source item type.</typeparam>
	/// <typeparam name="T">Target item type.</typeparam>
	public class TranslatedEnumerator<S, T> : IEnumerator<T>
	{
		#region fields

		/// <summary>
		/// Original enumerator.
		/// </summary>
		private IEnumerator<S> m_Enumerator;

		/// <summary>
		/// Translator.
		/// </summary>
		private IObjectTranslator<S, T> m_Translator;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslatedEnumerator{S,T}"/> class.
		/// </summary>
		/// <param name="enumerator">The enumerator.</param>
		public TranslatedEnumerator(IEnumerator<S> enumerator)
			: this(enumerator, (IObjectTranslator<S, T>) null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslatedEnumerator{S,T}"/> class.
		/// </summary>
		/// <param name="enumerator">The enumerator.</param>
		/// <param name="translator">The translator.</param>
		public TranslatedEnumerator(IEnumerator<S> enumerator, IObjectTranslator<S, T> translator)
		{
			if (enumerator == null)
				throw new ArgumentNullException("enumerator", "enumerator is null.");
			if (translator == null)
				translator = CastObjectTranslator<S, T>.Default;

			m_Enumerator = enumerator;
			m_Translator = translator;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslatedEnumerator{S,T}"/> class.
		/// </summary>
		/// <param name="enumerator">The enumerator.</param>
		/// <param name="sourceToTarget">The source to target converter.</param>
		public TranslatedEnumerator(IEnumerator<S> enumerator, Func<S, T> sourceToTarget)
			: this(enumerator, new CallbackObjectTranslator<S, T>(sourceToTarget, null))
		{
		}

		#endregion

		#region translation

		/// <summary>
		/// Translates source to target.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>Translated item.</returns>
		protected T SourceToTarget(S source)
		{
			return m_Translator.SourceToTarget(source);
		}

		#endregion

		#region IEnumerator<T> Members

		/// <summary>
		/// Gets the element in the collection at the current position of the enumerator.
		/// </summary>
		/// <value></value>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		public T Current
		{
			get { return SourceToTarget(m_Enumerator.Current); }
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			m_Enumerator.Dispose();
		}

		#endregion

		#region IEnumerator Members

		/// <summary>
		/// Gets the element in the collection at the current position of the enumerator.
		/// </summary>
		/// <value></value>
		/// <returns>The element in the collection at the current position of the enumerator.</returns>
		object System.Collections.IEnumerator.Current
		{
			get { return this.Current; }
		}

		/// <summary>
		/// Advances the enumerator to the next element of the collection.
		/// </summary>
		/// <returns>
		/// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
		/// </returns>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		public bool MoveNext()
		{
			return m_Enumerator.MoveNext();
		}

		/// <summary>
		/// Sets the enumerator to its initial position, which is before the first element in the collection.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
		public void Reset()
		{
			m_Enumerator.Reset();
		}

		#endregion
	}
}
