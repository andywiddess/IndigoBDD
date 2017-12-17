namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>
	/// Default object translator. It just casts one object type onto another.
	/// </summary>
	/// <typeparam name="S">Source type.</typeparam>
	/// <typeparam name="T">Target type.</typeparam>
	public class CastObjectTranslator<S, T>: IObjectTranslator<S, T>
	{
		#region static fields

		/// <summary>
		/// Default translator. Because it has no state nor data it can be shared by many processes.
		/// </summary>
		public static readonly CastObjectTranslator<S, T> Default = new CastObjectTranslator<S, T>();

		#endregion

		#region IObjectTranslator<S,T> Members

		/// <summary>
		/// Converts source to target.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>Converted object.</returns>
		public T SourceToTarget(S source)
		{
			return (T)((object)source);
		}

		/// <summary>
		/// Converts targets to source.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns>Converted object.</returns>
		public S TargetToSource(T target)
		{
			return (S)((object)target);
		}

		#endregion
	}
}
