namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Object translator. Provides ability to translate one object type to another.
	/// </summary>
	/// <typeparam name="S">Source type.</typeparam>
	/// <typeparam name="T">Target type.</typeparam>
	public interface IObjectTranslator<S, T>
	{
		/// <summary>
		/// Converts source to target.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>Converted object.</returns>
		T SourceToTarget(S source);

		/// <summary>
		/// Converts targets to source.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns>Converted object.</returns>
		S TargetToSource(T target);
	}
}
