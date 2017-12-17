using System;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Helper class to build <see cref="CallbackObjectTranslator{S,T}"/> objects.</summary>
	public class CallbackObjectTranslator
	{
		/// <summary>Makes the <see cref="CallbackObjectTranslator{S,T}"/> object using two callbacks.</summary>
		/// <typeparam name="S">Source type.</typeparam>
		/// <typeparam name="T">Target type.</typeparam>
		/// <param name="sourceToTarget">The source to target.</param>
		/// <param name="targetToSource">The target to source.</param>
		/// <returns><see cref="CallbackObjectTranslator{S,T}"/></returns>
		public static CallbackObjectTranslator<S, T> Make<S, T>(Func<S, T> sourceToTarget, Func<T, S> targetToSource)
		{
			return new CallbackObjectTranslator<S, T>(sourceToTarget, targetToSource);
		}
	}

	/// <summary>
	/// Implements <see cref="IObjectTranslator&lt;S,T&gt;"/> allowing to
	/// provide callback methods.
	/// </summary>
	/// <typeparam name="S">Source type.</typeparam>
	/// <typeparam name="T">Target type.</typeparam>
	public class CallbackObjectTranslator<S, T>: IObjectTranslator<S, T>
	{
		#region fields

		private readonly Func<S, T> m_SourceToTarget;
		private readonly Func<T, S> m_TargetToSource;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="CallbackObjectTranslator&lt;S, T&gt;"/> class.
		/// </summary>
		/// <param name="sourceToTarget">The source to target conversion method. 
		/// If <c>null</c> cast operator will be used for conversion.</param>
		/// <param name="targetToSource">The target to source conversion method. 
		/// If <c>null</c> cast operator will be used for conversion.</param>
		public CallbackObjectTranslator(Func<S, T> sourceToTarget, Func<T, S> targetToSource)
		{
			m_SourceToTarget = sourceToTarget ?? DefaultSourceToTarget;
			m_TargetToSource = targetToSource ?? DefaultTargetToSource;
		}

		#endregion

		#region utilities

		/// <summary>
		/// Default translation from source to target. It just casts object to another type.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>Translated object.</returns>
		private static T DefaultSourceToTarget(S source)
		{
			return (T)((object)source);
		}

		/// <summary>
		/// Default translation from target to source. It just casts object to another type.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns>Translated object.</returns>
		private static S DefaultTargetToSource(T target)
		{
			return (S)((object)target);
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
			return m_SourceToTarget(source);
		}

		/// <summary>
		/// Converts targets to source.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns>Converted object.</returns>
		public S TargetToSource(T target)
		{
			return m_TargetToSource(target);
		}

		#endregion
	}
}
