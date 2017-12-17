using System;
using System.IO;
using System.Security.Cryptography;
using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Extensions;
using Indigo.CrossCutting.Utilities.Streams;

namespace Indigo.CrossCutting.Utilities.Cryptography
{
	/// <summary>
	/// RC2 40-bit key derived from password.
	/// Please note, this is not cryptographicly secure, it's used rather as obfuscation
	/// tool. It is needed to communicate with Delphi subsystems (Programming Client).
	/// </summary>
	public class RC2Key40
        : IBinarySerializable
	{
		#region consts

		/// <summary>40 bits.</summary>
		private const int KEY_LENGTH = 40 >> 3;

		/// <summary>64 bits.</summary>
		private const int IV_LENGTH = 64 >> 3;

		/// <summary>64 bits.</summary>
		private const int BLOCK_SIZE = 64;

		#endregion

		#region properties

		/// <summary>Gets the key.</summary>
		public byte[] Key { get; private set; }

		/// <summary>Gets the IV.</summary>
		public byte[] IV { get; private set; }

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="RC2Key40"/> class.</summary>
		/// <param name="password">The password used for key initialization.</param>
		public RC2Key40(string password)
		{
			var md5 = AlgorithmProvider.NewMD5();
			var kv = md5.ComputeHash(password.ToUTF8());
			var key = new byte[KEY_LENGTH];
			var iv = new byte[IV_LENGTH];
			Array.Copy(kv, 0, key, 0, KEY_LENGTH);
			Array.Copy(kv, KEY_LENGTH, iv, 0, IV_LENGTH);
			Key = key;
			IV = iv;
		}

		#endregion

		#region public interface

		/// <summary>Encrypts the specified message using the key.</summary>
		/// <param name="message">The message.</param>
		/// <returns>Encrypted message.</returns>
		public byte[] Encrypt(byte[] message)
		{
			if (message == null) 
				return null;

			using (var rc2 = AlgorithmProvider.NewRC2())
			{
				rc2.BlockSize = BLOCK_SIZE;
				rc2.Key = Key;
				rc2.IV = IV;
				rc2.Mode = CipherMode.CBC;
				rc2.Padding = PaddingMode.PKCS7;

				return Patterns.Bufferize(w =>
				{
					using (var encryptor = rc2.CreateEncryptor())
					{
						w.Write(encryptor.TransformFinalBlock(message, 0, message.Length));
					}
				});
			}
		}

		/// <summary>Decrypts the specified message.</summary>
		/// <param name="message">The encrypted message.</param>
		/// <returns>Decrypted message.</returns>
		public byte[] Decrypt(byte[] message)
		{
			if (message == null) 
				return null;

			using (var rc2 = AlgorithmProvider.NewRC2())
			{
				rc2.BlockSize = BLOCK_SIZE;
				rc2.Key = Key;
				rc2.IV = IV;
				rc2.Mode = CipherMode.CBC;
				rc2.Padding = PaddingMode.PKCS7;

				return Patterns.Debufferize(message, r =>
				{
					using (var decryptor = rc2.CreateDecryptor())
					{
						return decryptor.TransformFinalBlock(message, 0, message.Length);
					}
				});
			}
		}

		#endregion

		#region IBinarySerializable Members

		/// <summary>Serializes object to stream.</summary>
		/// <param name="writer">The writer.</param>
		public void BinarySerialize(BinaryWriter writer)
		{
			if (Key.Length != KEY_LENGTH)
				throw new ArgumentException("RC2Key40 key is not 40-bit long");
			if (IV.Length != IV_LENGTH)
				throw new ArgumentException("RC2Key40 IV is not 64-bit long");
			writer.Write(Key);
			writer.Write(IV);
		}

		/// <summary>Deserialize object from stream.</summary>
		/// <param name="reader">The reader.</param>
		public void BinaryDeserialize(BinaryReader reader)
		{
			Key = reader.ReadBytes(KEY_LENGTH);
			IV = reader.ReadBytes(IV_LENGTH);
		}

		#endregion
	}
}
