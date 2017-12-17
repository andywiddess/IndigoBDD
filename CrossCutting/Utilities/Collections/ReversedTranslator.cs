using System;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Implemente reversed translation to specified one.
	/// </summary>
	/// <typeparam name="S">Source type (becomes target).</typeparam>
	/// <typeparam name="T">Target type (becomes source).</typeparam>
	public class ReversedTranslator<S, T>: IObjectTranslator<S, T>
	{
		#region fields

		/// <summary>
		/// Original translator.
		/// </summary>
		private IObjectTranslator<T, S> m_Translator;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ReversedTranslator&lt;S, T&gt;"/> class.
		/// </summary>
		/// <param name="translator">The original translator.</param>
		public ReversedTranslator(IObjectTranslator<T, S> translator)
		{
			if (translator == null)
				throw new ArgumentNullException("translator", "translator is null.");
			m_Translator = translator;
		}

		#endregion

		#region IObjectTranslator<S,T> Members

		/// <summary>
		/// Converts source to target.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>Converted object.</returns>
		public T SourceToTarget(S source)
		{
			return m_Translator.TargetToSource(source);
		}

		/// <summary>
		/// Converts targets to source.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns>Converted object.</returns>
		public S TargetToSource(T target)
		{
			return m_Translator.SourceToTarget(target);
		}

		#endregion
	}
}
