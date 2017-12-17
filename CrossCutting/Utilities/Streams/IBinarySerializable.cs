using System.IO;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// Interface for object which can be serialized to binary. It's different than
	/// built-in Serializable as does not provide type safety (you need to know upfront 
	/// what type it was) although is vary compact and fast.
	/// </summary>
	public interface IBinarySerializable
	{
		/// <summary>Serializes object to stream.</summary>
		/// <param name="writer">The writer.</param>
		void BinarySerialize(BinaryWriter writer);

		/// <summary>Deserialize object from stream.</summary>
		/// <param name="reader">The reader.</param>
		void BinaryDeserialize(BinaryReader reader);
	}
}
