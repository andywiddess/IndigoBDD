using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Indigo.CrossCutting.Utilities.Collections;
using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Extensions;

namespace Indigo.CrossCutting.Utilities.Cryptography
{
	/// <summary>
	/// Facade class for message encryption and decryption using AES and RSA.
	/// </summary>
	public class SecureMessage
	{
		#region encrypt

		/// <summary>Encrypts the specified message with AES key.</summary>
		/// <param name="message">The message.</param>
		/// <param name="key">The key.</param>
		/// <returns>Encrypted message.</returns>
		public static byte[] Encrypt(
			byte[] message,
			AesKey key)
		{
			if (key == null)
				throw new ArgumentException("Encryption key is required");
			return key.Encrypt(Compress(message));
		}

		/// <summary>Encrypts the specified message with RSA key and random AES key.</summary>
		/// <param name="message">The message.</param>
		/// <param name="senderPrivateKey">The sender private key.</param>
		/// <param name="receiverPublicKey">The receiver public key.</param>
		/// <param name="aesKeySize">Size of the random AES key (in bits).</param>
		/// <returns>Encrypted message.</returns>
		public static byte[] Encrypt(
			byte[] message,
			RsaKey senderPrivateKey, RsaKey receiverPublicKey,
			int aesKeySize = 256)
		{
			if (senderPrivateKey == null || (senderPrivateKey.Flags & RsaKeyFlags.Private) == 0)
				throw new ArgumentException("Private sender key is required");
			if (receiverPublicKey == null || (receiverPublicKey.Flags & RsaKeyFlags.Public) == 0)
				throw new ArgumentException("Public receiver key is required");
			if (message == null)
				throw new ArgumentException("Message cannot be null");

			byte[] encryptedKeys = null;
			byte[] encryptedMessage = null;
			byte[] messageSignature = null;

			var messageKey = new AesKey(true, aesKeySize);

			// encrypt keysize, encrypt message and sign message in parallel
			// they don't rely on each other
			Parallel.Invoke(
				() => encryptedKeys = receiverPublicKey.Encrypt(messageKey.Key),
				() => encryptedMessage = messageKey.Encrypt(Compress(message)),
				() => messageSignature = senderPrivateKey.Sign(message)
			);

			return Patterns.Bufferize(w =>
			{
				receiverPublicKey.GetPublicKey().BinarySave(w); // to whom...
				senderPrivateKey.GetPublicKey().BinarySave(w); // ...and who sends it
				w.WritePacket(encryptedKeys); // encryption key
				w.WritePacket(encryptedMessage); // the message
				w.WritePacket(messageSignature); // signature
			});
		}

		#endregion

		#region decrypt

		/// <summary>Decrypts the specified encrypted message with AES key.</summary>
		/// <param name="encrypted">The encrypted message.</param>
		/// <param name="key">The AES key.</param>
		/// <returns>Decrypted message.</returns>
		public static byte[] Decrypt(
			byte[] encrypted,
			AesKey key)
		{
			if (key == null)
				throw new ArgumentException("Encryption key is required");
			return Decompress(key.Decrypt(encrypted));
		}

		/// <summary>Decrypts the specified encrypted message with RSA key and random AES key.</summary>
		/// <param name="encrypted">The encrypted message.</param>
		/// <param name="acceptSender">The accept sender callback.</param>
		/// <param name="acceptReceiver">The accept receiver callback.</param>
		/// <returns>Decrypted message.</returns>
		public static byte[] Decrypt(
			byte[] encrypted,
			Func<RsaKey, bool> acceptSender,
			Func<RsaKey, RsaKey> acceptReceiver)
		{
			RsaKey _; // we do not care about this
			return Decrypt(encrypted, acceptSender, acceptReceiver, out _, out _);
		}

		/// <summary>Decrypts the specified encrypted message with RSA key and random AES key.</summary>
		/// <param name="encrypted">The encrypted message.</param>
		/// <param name="acceptSender">The accept sender callback.</param>
		/// <param name="acceptReceiver">The accept receiver callback.</param>
		/// <param name="usedSenderPublicKey">The used sender public key.</param>
		/// <returns>Decrypted message.</returns>
		public static byte[] Decrypt(
			byte[] encrypted,
			Func<RsaKey, bool> acceptSender,
			Func<RsaKey, RsaKey> acceptReceiver,
			out RsaKey usedSenderPublicKey)
		{
			RsaKey _; // we do not care about this
			return Decrypt(encrypted, acceptSender, acceptReceiver, out usedSenderPublicKey, out _);
		}

		/// <summary>Decrypts the specified encrypted message.</summary>
		/// <param name="encrypted">The encrypted.</param>
		/// <param name="acceptSender">The accept sender callback.</param>
		/// <param name="acceptReceiver">The accept receiver callback.</param>
		/// <param name="usedSenderPublicKey">The used sender public key.</param>
		/// <param name="usedReceiverPublicKey">The used receiver public key.</param>
		/// <returns></returns>
		public static byte[] Decrypt(
			byte[] encrypted,
			Func<RsaKey, bool> acceptSender,
			Func<RsaKey, RsaKey> acceptReceiver,
			out RsaKey usedSenderPublicKey,
			out RsaKey usedReceiverPublicKey)
		{
			if (acceptReceiver == null)
				throw new ArgumentException("acceptReceiver() callback has not been provided");

			byte[] encryptedKeys = null, encryptedMessage = null, messageSignature = null;
			RsaKey receiverPrivateKey = null, receiverPublicKey = null, senderPublicKey = null;

			Patterns.Debufferize(encrypted, r =>
			{
				receiverPublicKey = new RsaKey().BinaryLoad(r);
				receiverPrivateKey = acceptReceiver(receiverPublicKey);
				if (receiverPublicKey == null || !receiverPublicKey.EqualsTo(receiverPrivateKey, false))
					throw new InvalidOperationException("Message is not authorized for this receiver");

				senderPublicKey = new RsaKey().BinaryLoad(r);
				if (acceptSender != null && !acceptSender(senderPublicKey))
					throw new InvalidOperationException("Sender is not authorized");

				encryptedKeys = r.ReadPacket();
				encryptedMessage = r.ReadPacket();
				messageSignature = r.ReadPacket();
			});

			var messageKey = new AesKey { Key = receiverPrivateKey.Decrypt(encryptedKeys) };
			var decryptedMessage = Decompress(messageKey.Decrypt(encryptedMessage));
			if (!senderPublicKey.Verify(decryptedMessage, messageSignature))
				throw new InvalidOperationException("Signature does not match sender");

			usedSenderPublicKey = senderPublicKey;
			usedReceiverPublicKey = receiverPublicKey;

			return decryptedMessage;
		}

		#endregion

		#region compress

		/// <summary>Compresses the specified message.</summary>
		/// <param name="message">The message.</param>
		/// <returns>Compressed message.</returns>
		public static byte[] Compress(byte[] message)
		{
			byte[] compressed = null;

			using (var mem = new MemoryStream())
			{
				using (var zstream = new DeflateStream(mem, CompressionMode.Compress))
				{
					zstream.Write(message, 0, message.Length);
				}
				compressed = mem.ToArray();

				if (compressed.Length >= message.Length)
				{
					// if compressed message is not smaller, don't bother use uncompressed message
					compressed = message;
				}
			}

			return Patterns.Bufferize(w =>
			{
				w.PackedWrite((ulong)message.Length); // original length
				w.Write(compressed);
			});
		}

		/// <summary>Decompresses the specified message.</summary>
		/// <param name="message">The message.</param>
		/// <returns>Decompressed message.</returns>
		public static byte[] Decompress(byte[] message)
		{
			int originalLength = 0;
			byte[] buffer = null;

			Patterns.Debufferize(message, r =>
			{
				originalLength = (int)r.PackedRead();
				buffer = r.ReadBytes(message.Length); // that's more than available, but reader handles it correctly
			});

			if (buffer.Length == originalLength) return buffer;

			using (var mstream = new MemoryStream(buffer))
			using (var zstream = new DeflateStream(mstream, CompressionMode.Decompress))
			{
				var result = new byte[originalLength];
				zstream.Read(result, 0, originalLength);
				return result;
			}
		}

		#endregion

		#region utilities

		/// <summary>Calculates MD5.</summary>
		/// <param name="buffer">The buffer.</param>
		/// <returns>MD5 of given buffer.</returns>
		public static byte[] MD5(byte[] buffer)
		{
			return Hash(buffer, AlgorithmProvider.NewMD5());
		}

		/// <summary>Calculates MD5.</summary>
		/// <param name="text">The text.</param>
		/// <returns>MD5 of given text.</returns>
		public static byte[] MD5(string text)
		{
			return Hash(text, AlgorithmProvider.NewMD5());
		}

		/// <summary>Returns MD5 hash of given buffers.</summary>
		/// <param name="buffers">The buffers.</param>
		/// <returns>MD5 of given buffers.</returns>
		public static byte[] MD5(IEnumerable<byte[]> buffers)
		{
			return Hash(buffers, AlgorithmProvider.NewMD5());
		}

		/// <summary>Returns MD5 hash of given buffer (but pretending it's a Guid).</summary>
		/// <param name="buffer">The buffer.</param>
		/// <returns>MD5/Guid of given buffer.</returns>
		public static Guid GuidOf(byte[] buffer)
		{
			return new Guid(MD5(buffer));
		}

		/// <summary>Returns MD5 hash of given string (but pretending it's a Guid).</summary>
		/// <param name="text">The text.</param>
		/// <returns>MD5/Guid of given buffer.</returns>
		public static Guid GuidOf(string text)
		{
			return new Guid(MD5(text));
		}

		/// <summary>Returns MD5 hash of given buffers (but pretending it's a Guid).</summary>
		/// <param name="buffers">The buffers.</param>
		/// <returns>MD5/Guid of given buffers.</returns>
		public static Guid GuidOf(IEnumerable<byte[]> buffers)
		{
			return new Guid(MD5(buffers));
		}

		/// <summary>Calculates hash of given buffer.</summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="hash">The hash algorithm (SHA-512 is default).</param>
		/// <returns>Hash of byte buffer.</returns>
		public static byte[] Hash(byte[] buffer, HashAlgorithm hash = null)
		{
			return (hash ?? AlgorithmProvider.NewSHA512()).ComputeHash(buffer);
		}

		/// <summary>Calculates hash of given text.</summary>
		/// <param name="text">The text.</param>
		/// <param name="hash">The hash algorithm (SHA-512 is default).</param>
		/// <returns>Hash of text.</returns>
		public static byte[] Hash(string text, HashAlgorithm hash = null)
		{
			return Hash(Encoding.UTF8.GetBytes(text), hash);
		}

		/// <summary>Hashes the specified buffers.</summary>
		/// <param name="buffers">The buffers.</param>
		/// <param name="hash">The hash (SHA-512 is used by default).</param>
		/// <returns>Hash of byte buffers.</returns>
		public static byte[] Hash(IEnumerable<byte[]> buffers, HashAlgorithm hash = null)
		{
			hash = hash ?? AlgorithmProvider.NewSHA512();
			foreach (var buffer in buffers)
			{
				hash.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
			}
			hash.TransformFinalBlock(EmptyArray<byte>.Instance, 0, 0);
			return hash.Hash;
		}

		/// <summary>Salt value from PBKDF2.</summary>
		private static readonly byte[] Rfc2898Salt = Hash(@"DX2)$`1R5@?4qN,qjM~2/|C%w&5g^j:x");

		/// <summary>Gets bytes derived from blob.</summary>
		/// <param name="blob">The blob.</param>
		/// <param name="bytes">The bytes.</param>
		/// <returns>Byte buffer.</returns>
		public static byte[] DerivedBytes(byte[] blob, int bytes)
		{
			var hash = Hash(blob);
			var salt = Rfc2898Salt;
			using (var pdb = new Rfc2898DeriveBytes(hash, salt, 2048))
			{
				return pdb.GetBytes(bytes);
			}
		}

		/// <summary>Derives AES key from blob.</summary>
		/// <param name="blob">The blob.</param>
		/// <param name="keySize">Size of the key (in bits).</param>
		/// <returns>AES key.</returns>
		public static AesKey DerivedAesKey(byte[] blob, int keySize = 256)
		{
			return new AesKey(DerivedBytes(blob, keySize / 8));
		}

		/// <summary>Deriveds the AES key from RSA key.</summary>
		/// <param name="key">The key.</param>
		/// <param name="keySize">Size of the key (in bits).</param>
		/// <returns>AES key.</returns>
		public static AesKey DerivedAesKey(RsaKey key, int keySize = 256)
		{
			if (!key.IsPrivate)
				throw new ArgumentException("Derived AES key requires private RSA key");
			return DerivedAesKey(key.Parameters.D, keySize);
		}

		/// <summary>Creates the array of random bytes.</summary>
		/// <param name="bytes">The bytes.</param>
		/// <returns>Array of random bytes.</returns>
		public static byte[] CreateRandomBytes(int bytes = 32)
		{
			using (var rng = AlgorithmProvider.NewRNG())
			{
				var result = new byte[bytes];
				rng.GetBytes(result);
				return result;
			}
		}

		#endregion
	}
}
