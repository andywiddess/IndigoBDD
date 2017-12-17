using System.Text;
using Indigo.CrossCutting.Utilities.Extensions;

namespace Indigo.CrossCutting.Utilities.Cryptography
{
	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class EmbeddedData
	{
		/// <summary>Converts base64 string into AesKey.</summary>
		/// <param name="encryptionKey">The encryption key used to encrypt base64 string (<c>null</c> if none).</param>
		/// <param name="base64">The base64 string.</param>
		/// <returns>AES key.</returns>
		public static AesKey ToAesKey(AesKey encryptionKey, string base64)
		{
			var bytes = base64.FromBase64();
			if (encryptionKey != null)
				bytes = encryptionKey.Decrypt(bytes);
			return new AesKey().BinaryLoad(bytes);
		}

		/// <summary>Converts base64 string into RsaKey.</summary>
		/// <param name="encryptionKey">The encryption key used to encrypt base64 string (<c>null</c> if none).</param>
		/// <param name="base64">The base64 string.</param>
		/// <returns>RSA key.</returns>
		public static RsaKey ToRsaKey(AesKey encryptionKey, string base64)
		{
			var bytes = base64.FromBase64();
			if (encryptionKey != null)
				bytes = encryptionKey.Decrypt(bytes);
			return new RsaKey().BinaryLoad(bytes);
		}

		/// <summary>Converts base64 string into string.</summary>
		/// <param name="encryptionKey">The encryption key used to encrypt base64 string (<c>null</c> if none).</param>
		/// <param name="base64">The base64 string.</param>
		/// <returns>Decrypted string.</returns>
		public static string ToString(AesKey encryptionKey, string base64)
		{
			var bytes = base64.FromBase64();
			if (encryptionKey != null)
				bytes = encryptionKey.Decrypt(bytes);
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
