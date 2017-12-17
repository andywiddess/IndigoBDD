using System.IO;
using System.Security.Cryptography;
using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Extensions;
using Indigo.CrossCutting.Utilities.Streams;

namespace Indigo.CrossCutting.Utilities.Cryptography
{
	/// <summary>Encapsulation of AES key.</summary>
	public class AesKey: IBinarySerializable
	{
		/// <summary>Gets or sets the key.</summary>
		/// <value>The key.</value>
		public byte[] Key { get; set; }

		/// <summary>Initializes a new instance of the <see cref="AesKey"/> class.
		/// Does not create key.</summary>
		public AesKey()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="AesKey"/> class. 
		/// Creates new random key.</summary>
		/// <param name="keySize">Size of the key.</param>
		public AesKey(int keySize)
		{
			Create(keySize);
		}

		/// <summary>Initializes a new instance of the <see cref="AesKey"/> class.</summary>
		/// <param name="create">if set to <c>true</c> key gets create.</param>
		/// <param name="keySize">Size of the key.</param>
		public AesKey(bool create, int keySize = 256)
		{
			if (create) Create(keySize);
		}

		/// <summary>Initializes a new instance of the <see cref="AesKey"/> class.
		/// Key is initializad with given bytes.</summary>
		/// <param name="key">The key.</param>
		public AesKey(byte[] key)
		{
			Key = key;
		}

		/// <summary>Initializes a new instance of the <see cref="AesKey"/> class.
		/// Key is copied from given provider.</summary>
		/// <param name="provider">The AES provider.</param>
		public AesKey(Aes provider)
		{
			Key = provider.Key;
		}

		/// <summary>Creates new random key with specified key size.</summary>
		/// <param name="keySize">Size of the key.</param>
		public void Create(int keySize = 256)
		{
			using (var aes = AlgorithmProvider.NewAES())
			{
				aes.KeySize = keySize;
				aes.GenerateKey();
				Key = aes.Key;
			}
		}

		/// <summary>Gets a value indicating whether this instance is empty.</summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		public bool IsEmpty { get { return Key == null || Key.Length == 0; } }

		/// <summary>Encrypts the specified message using the key.</summary>
		/// <param name="message">The message.</param>
		/// <returns>Encrypted message.</returns>
		public byte[] Encrypt(byte[] message)
		{
			if (message == null) return null;

			using (var aes = AlgorithmProvider.NewAES())
			{
				aes.Key = Key;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;

				return Patterns.Bufferize(w =>
				{
					aes.GenerateIV();
					w.WritePacket(aes.IV);
					using (var encryptor = aes.CreateEncryptor())
					{
						w.WritePacket(encryptor.TransformFinalBlock(message, 0, message.Length));
					}
				});
			}
		}

		/// <summary>Decrypts the specified message.</summary>
		/// <param name="message">The encrypted message.</param>
		/// <returns>Decrypted message.</returns>
		public byte[] Decrypt(byte[] message)
		{
			if (message == null) return null;

			using (var aes = AlgorithmProvider.NewAES())
			{
				aes.Key = Key;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;

				return Patterns.Debufferize(message, r =>
				{
					aes.IV = r.ReadPacket();
					using (var decryptor = aes.CreateDecryptor())
					{
						var buffer = r.ReadPacket();
						return decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
					}
				});
			}
		}

#if DEBUG

		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format("AesKey({0})", Key == null ? "null" : Key.ToBase64());
		}

#endif

		#region IBinarySerializable Members

		/// <summary>Serializes object to stream.</summary>
		/// <param name="writer">The writer.</param>
		public void BinarySerialize(BinaryWriter writer)
		{
			writer.WritePacket(Key);
		}

		/// <summary>Deserialize object from stream.</summary>
		/// <param name="reader">The reader.</param>
		public void BinaryDeserialize(BinaryReader reader)
		{
			Key = reader.ReadPacket();
		}

		#endregion
	}
}
