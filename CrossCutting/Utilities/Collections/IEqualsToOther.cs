namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Object equality extended interface.</summary>
	/// <typeparam name="T">Same type as the object you are implementing it in.</typeparam>
	public interface IEqualsToOther<T>
	{
		/// <summary>
		/// Tests if one object is equal to other object. 
		/// NOTE, you cont have to check <paramref name="other"/> to be <c>null</c> or <c>this</c>,
		/// if this method got called it has been ruled out already.
		/// NOTE, implement this as 'explicit' implementation.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns><c>true</c> if object is equal to other; <c>false</c> otherwise</returns>
		bool EqualsToOther(T other);
	}
}
