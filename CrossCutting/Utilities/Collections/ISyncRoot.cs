namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Interface for objects having SyncRoot.</summary>
	public interface ISyncRoot
	{
		/// <summary>Exposes synchronisation root.</summary>
		object SyncRoot { get; }
	}
}
