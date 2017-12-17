namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Interface simplyfying access to indexed properties.</summary>
	/// <typeparam name="K">Key type.</typeparam>
	/// <typeparam name="V">Value type.</typeparam>
	public interface IIndexed<K, V>
	{
		/// <summary>Gets or sets the value at the specified index.</summary>
		V this[K index] { get; set; }
	}
}