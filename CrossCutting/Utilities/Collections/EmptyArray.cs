namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Empty array of given type.</summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public static class EmptyArray<T>
	{
		/// <summary>Default instance of empty array (and the only one you need).</summary>
		public static readonly T[] Instance = new T[0];
	}
}
