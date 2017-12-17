using Indigo.CrossCutting.Utilities.Collections;
using System.Security.Cryptography;


namespace Indigo.CrossCutting.Utilities.Extensions
{
	/// <summary>
	/// Helper methods for security related classes.
	/// </summary>
	public static class ExtensionsToSecurity
	{
		/// <summary>Transforms the block. 
		/// Does not require offset (0 is assumes) and length (whole buffer is assumed).</summary>
		/// <param name="md5">The MD5.</param>
		/// <param name="buffer">The buffer.</param>
		/// <returns>Result of underlying TransformBlock.</returns>
		public static int TransformBlock(this MD5 md5, byte[] buffer)
		{
			return md5.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
		}

		/// <summary>Transforms the final empty block.</summary>
		/// <param name="md5">The MD5.</param>
		/// <returns>Result of underlying TransformFinalBlock.</returns>
		public static byte[] TransformFinalBlock(this MD5 md5)
		{
			return md5.TransformFinalBlock(EmptyArray<byte>.Instance, 0, 0);
		}
	}
}
