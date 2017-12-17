using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using Indigo.CrossCutting.Utilities.Collections;
using Indigo.CrossCutting.Utilities.Extensions;
using Indigo.CrossCutting.Utilities.Streams;

namespace Indigo.CrossCutting.Utilities.Cryptography
{
	/// <summary>Encapsulation of RSA key.</summary>
	public class RsaKey: IBinarySerializable
	{
		#region static field

		/// <summary>Minimum key size.</summary>
		private static int? MinKeySize;

		/// <summary>
		/// Key dispenser. RSA key creation takes long time, so let's reuse keys as much as possible.
		/// </summary>
		private static readonly Pool<RSACryptoServiceProvider> _keyPool;

		private static readonly string SHA1OID = CryptoConfig.MapNameToOID("SHA1");

		#endregion

		#region fields

		/// <summary>RSA parameters.</summary>
		private RSAParameters m_RsaParams;

		#endregion

		#region static constructor

		/// <summary>Initializes the <see cref="RsaKey"/> class. Initializes 
		/// RSA Crypto Service Provider dispenser.</summary>
		static RsaKey()
		{
			_keyPool = new Pool<RSACryptoServiceProvider>(
				8,
				() => ProduceCryptoServiceProvider(),
				(p) => RecycleCryptoServiceProvider(p));
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of empty the <see cref="RsaKey"/> class.</summary>
		public RsaKey()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="RsaKey"/> class and creates random key.</summary>
		/// <param name="keySize">Size of the key (in bits).</param>
		public RsaKey(int keySize)
		{
			Create(keySize);
		}

		/// <summary>Initializes a new instance of the <see cref="RsaKey"/> class.</summary>
		/// <param name="create">if set to <c>true</c> random key will be created.</param>
		/// <param name="keySize">Size of the key (in bits).</param>
		public RsaKey(bool create, int keySize = 2048)
		{
			if (create) Create(keySize);
		}

		/// <summary>Initializes a new instance of the <see cref="RsaKey"/> class
		/// and loads provided RSA key.</summary>
		/// <param name="rsaParams">The RSA params.</param>
		public RsaKey(RSAParameters rsaParams)
		{
			m_RsaParams = rsaParams;
		}

		#endregion

		#region public interface

		/// <summary>Creates new key which contains only public component.</summary>
		/// <returns>Public component of the key.</returns>
		public RsaKey GetPublicKey()
		{
			return new RsaKey(new RSAParameters
			{
				Exponent = m_RsaParams.Exponent,
				Modulus = m_RsaParams.Modulus,
			});
		}

		/// <summary>Returns GUID derived from public key, as public keys are considered
		/// unique they can serve as identifiers.
		/// The probability of pubic key collision is smaller than probability 
		/// of regular GUID collition, so calculating MD5 and casting it as GUID does 
		/// not increase probability of collision.</summary>
		/// <returns>GUID derived from public key.</returns>
		public Guid GetGuid()
		{
			using (var md5 = new MD5CryptoServiceProvider())
			{
				return new Guid(md5.ComputeHash(m_RsaParams.Modulus));
			}
		}

		/// <summary>Creates the key with specified key size.</summary>
		/// <param name="keySize">Size of the key (in bits).</param>
		private void Create(int keySize)
		{
			var cspParams = CreateCspParameters(null, false, true);
			using (var rsaProvider = CreateCryptoServiceProvider(keySize, cspParams, false))
			{
				m_RsaParams = rsaProvider.ExportParameters(true);
			}
		}

		/// <summary>Gets the flags.</summary>
		public RsaKeyFlags Flags
		{
			get
			{
				var result = RsaKeyFlags.None;
				var hasPublic = 
					m_RsaParams.Modulus != null && 
					m_RsaParams.Exponent != null;
				var hasPrivate = 
					hasPublic &&
					m_RsaParams.Q != null && 
					m_RsaParams.P != null &&
					m_RsaParams.D != null && 
					m_RsaParams.DP != null &&
					m_RsaParams.DQ != null &&
					m_RsaParams.InverseQ != null;
				if (hasPublic) result |= RsaKeyFlags.Public;
				if (hasPrivate) result |= RsaKeyFlags.Private;
				return result;
			}
		}

		/// <summary>Gets a value indicating whether this key is empty.</summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		public bool IsEmpty { get { return Flags == RsaKeyFlags.None; } }

		/// <summary>Gets a value indicating whether this instance is private.</summary>
		/// <value><c>true</c> if this key contains private component; otherwise, <c>false</c>.</value>
		public bool IsPrivate { get { return (Flags & RsaKeyFlags.Private) != 0; } }

		/// <summary>Gets or sets the parameters.</summary>
		/// <value>The parameters.</value>
		public RSAParameters Parameters
		{
			get { return m_RsaParams; }
			set { m_RsaParams = value; }
		}

		#region serialization

		/// <summary>Loads existing named key.</summary>
		/// <param name="keyName">Name of the key.</param>
		/// <param name="useMachineStore">if set to <c>true</c> uses machine key store.</param>
		/// <returns>Archived key. Throws an <see cref="CryptographicException"/> when 
		/// key does not exist in CSP.</returns>
		public RsaKey Load(string keyName, bool useMachineStore)
		{
			var cspParams = CreateCspParameters(keyName, useMachineStore, false);

			using (var rsaProvider = CreateCryptoServiceProvider(cspParams, false))
			{
				m_RsaParams = rsaProvider.ExportParameters(true);
			}

			return this;
		}

		/// <summary>Loads existing named key or creates it.</summary>
		/// <param name="keyName">Name of the key.</param>
		/// <param name="useMachineStore">if set to <c>true</c> uses machine key store.</param>
		/// <param name="keySize">Size of the key.</param>
		/// <returns>Archived key.</returns>
		public RsaKey CreateOrLoad(string keyName, bool useMachineStore, int keySize = 2048)
		{
			var cspParams = CreateCspParameters(keyName, useMachineStore, true);

			using (var rsaProvider = CreateCryptoServiceProvider(keySize, cspParams, true))
			{
				m_RsaParams = rsaProvider.ExportParameters(true);
			}

			return this;
		}

		#endregion

		/// <summary>Checks if thwo keys are equal. May compare both private or public 
		/// or just public component.</summary>
		/// <param name="other">The other key.</param>
		/// <param name="includePrivateKey">if set to <c>true</c> private component 
		/// is also compared.</param>
		/// <returns><c>true</c> if keys are equal, <c>false</c> otherwise.</returns>
		public bool EqualsTo(RsaKey other, bool includePrivateKey = false)
		{
			return
				Equals(m_RsaParams.Modulus, other.m_RsaParams.Modulus) &&
				Equals(m_RsaParams.Exponent, other.m_RsaParams.Exponent) &&
				(!includePrivateKey || (
					Equals(m_RsaParams.D, other.m_RsaParams.D) &&
					Equals(m_RsaParams.P, other.m_RsaParams.P) &&
					Equals(m_RsaParams.Q, other.m_RsaParams.Q) &&
					Equals(m_RsaParams.DP, other.m_RsaParams.DP) &&
					Equals(m_RsaParams.DQ, other.m_RsaParams.DQ) &&
					Equals(m_RsaParams.InverseQ, other.m_RsaParams.InverseQ) &&
					true
				));
		}

		/// <summary>Checks if two byte arrays are identical</summary>
		/// <param name="a">Array A.</param>
		/// <param name="b">Array B.</param>
		/// <returns><c>true</c> if arrays are equal, <c>false</c> otherwise.</returns>
		private static bool Equals(byte[] a, byte[] b)
		{
			if (a == b) return true;
			// if (a == null && b == null) return true;
			if (a == null || b == null) return false;
			if (a.Length != b.Length) return false;
			var length = a.Length;
			for (int i = 0; i < length; i++) if (a[i] != b[i]) return false;
			return true;
		}

		#region encryption

		/// <summary>Encrypts the message with the key.</summary>
		/// <param name="message">The message.</param>
		/// <returns>Encrypted message.</returns>
		public byte[] Encrypt(byte[] message)
		{
			return WithKnownKey(rsa => rsa.Encrypt(message, false));
		}

		/// <summary>Decrypts the specified message (requires private key).</summary>
		/// <param name="message">The encrypted message.</param>
		/// <returns>Decrypted message.</returns>
		public byte[] Decrypt(byte[] message)
		{
			return WithKnownKey(rsa => rsa.Decrypt(message, false));
		}

		/// <summary>Signs the message.</summary>
		/// <param name="message">The message.</param>
		/// <returns>Hash.</returns>
		public byte[] Sign(byte[] message)
		{
			return WithKnownKey(rsa => rsa.SignData(message, 0, message.Length, SHA1OID));
		}

		/// <summary>Verifies the specified message.</summary>
		/// <param name="message">The message.</param>
		/// <param name="signature">The signature.</param>
		/// <returns><c>true</c> if message matches the hash, <c>false</c> otherwise.</returns>
		public bool Verify(byte[] message, byte[] signature)
		{
			return WithKnownKey(rsa =>
			{
				using (var halg = AlgorithmProvider.NewSHA1())
				{
					if (!rsa.VerifyData(message, SHA1OID, signature))
						return false; // check is data matches signature
					var hash = halg.ComputeHash(message);
					if (!rsa.VerifyHash(hash, SHA1OID, signature))
						return false; // check if signature is valid
				}
				return true;
			});
		}

		#endregion

		#endregion

		#region private implementation

		private static void UpdateMinKeySize(RSACryptoServiceProvider provider)
		{
			lock (typeof(RsaKey)) // not nice to lock on type, but c'mon no way it would deal-lock
			{
				if (!MinKeySize.HasValue)
				{
					MinKeySize = provider.LegalKeySizes.Min(ks => ks.MinSize);
				}
			}
		}

		/// <summary>Creates the CSP parameters. Please note, this is not 
		/// general purpose method, it just contains some commonalities
		/// of CreateOrLoad and Load.</summary>
		/// <param name="keyName">Name of the key.</param>
		/// <param name="useMachineStore">if set to <c>true</c> use machine store.</param>
		/// <param name="allowCreate">if set to <c>true</c> creating new key is allowed.</param>
		/// <returns>CSP parameters.</returns>
		private static CspParameters CreateCspParameters(
			string keyName,
			bool useMachineStore,
			bool allowCreate)
		{
			var cspParams = new CspParameters
			{
				ProviderType = 1, // RSA
				Flags = allowCreate 
					? CspProviderFlags.UseArchivableKey 
					: CspProviderFlags.UseExistingKey,
				KeyContainerName = keyName,
			};

			if (useMachineStore)
				cspParams.Flags |= CspProviderFlags.UseMachineKeyStore;

			return cspParams;
		}

		/// <summary>Creates the crypto service provider.</summary>
		/// <param name="parameters">The parameters.</param>
		/// <param name="persist">if set to <c>true</c> key will automatically persisted.</param>
		/// <returns>New crypto service provider.</returns>
		private static RSACryptoServiceProvider CreateCryptoServiceProvider(
			CspParameters parameters,
			bool persist = false)
		{
			var provider = 
				new RSACryptoServiceProvider(parameters) { PersistKeyInCsp = persist };
			UpdateMinKeySize(provider);
			return provider;
		}

		/// <summary>Creates the crypto service provider.</summary>
		/// <param name="keySize">Size of the key.</param>
		/// <param name="parameters">The parameters.</param>
		/// <param name="persist">if set to <c>true</c> key will automatically persisted.</param>
		/// <returns>New crypto service provider.</returns>
		private static RSACryptoServiceProvider CreateCryptoServiceProvider(
			int keySize,
			CspParameters parameters,
			bool persist = false)
		{
			var provider = 
				new RSACryptoServiceProvider(keySize, parameters) { PersistKeyInCsp = persist };
			UpdateMinKeySize(provider);
			return provider;
		}

		/// <summary>Produces the crypto service provider.
		/// Built-in crypto service provider has an annoying feature of creating new
		/// random key it gets created even if it is going to be replaced with some
		/// other key 3ms later. It may even take up to 2s so it becomes a performance 
		/// bottleneck. It is annoying in case we already know the key, and there is no
		/// need to create new one.
		/// There is no easy solution, but we can at list ensure the created key
		/// is as small as possible and reused. 
		/// So on first run we cache minimum key size and following calls we use it to 
		/// create smallest keys.
		/// </summary>
		/// <returns>Newly created key with small random key.</returns>
		private static RSACryptoServiceProvider ProduceCryptoServiceProvider()
		{
			RSACryptoServiceProvider provider = null;

			lock (typeof(RsaKey))
			{
				if (!MinKeySize.HasValue)
				{
					provider = new RSACryptoServiceProvider();
					MinKeySize = provider.LegalKeySizes.Min(ks => ks.MinSize);
				}
			}

			if (provider == null)
				provider = new RSACryptoServiceProvider(MinKeySize.Value);

			provider.PersistKeyInCsp = false;

			return provider;
		}

		/// <summary>Recycles the crypto service provider before returning it to dispenser.</summary>
		/// <param name="provider">The provider.</param>
		/// <returns><c>true</c> if provider has been properly recycled.</returns>
		private static bool RecycleCryptoServiceProvider(RSACryptoServiceProvider provider)
		{
			provider.PersistKeyInCsp = false;
			// that's quite ugly but removes private part from key
			// Dispose and Clear prevent provider from being reused
			provider.ImportCspBlob(provider.ExportCspBlob(false));
			return true;
		}

		/// <summary>Gets "clean" crypto service provider and execute the action with it.</summary>
		/// <typeparam name="T">Type of result.</typeparam>
		/// <param name="action">The action.</param>
		/// <returns>Result of action.</returns>
		private T WithKnownKey<T>(Func<RSACryptoServiceProvider, T> action)
		{
			return _keyPool.Use(k =>
			{
				k.ImportParameters(m_RsaParams);
				return action(k);
			});
		}

		#endregion

		#region overrides

#if DEBUG

		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format("RsaKey(Public: {0}, Private: {1})",
				!IsEmpty ? GetGuid().ToByteArray().ToBase64() : "null",
				IsPrivate);
		}

#endif

		#endregion

		#region IBinarySerializable Members

		/// <summary>Serializes object to stream.</summary>
		/// <param name="writer">The writer.</param>
		public void BinarySerialize(BinaryWriter writer)
		{
			var flags = Flags;

			writer.Write((byte)flags);
			if ((flags & RsaKeyFlags.Public) != 0)
			{
				writer.WritePacket(m_RsaParams.Exponent);
				writer.WritePacket(m_RsaParams.Modulus);
			}
			if ((flags & RsaKeyFlags.Private) != 0)
			{
				writer.WritePacket(m_RsaParams.P);
				writer.WritePacket(m_RsaParams.Q);
				writer.WritePacket(m_RsaParams.D);
				writer.WritePacket(m_RsaParams.DP);
				writer.WritePacket(m_RsaParams.DQ);
				writer.WritePacket(m_RsaParams.InverseQ);
			}
		}

		/// <summary>Deserialize object from stream.</summary>
		/// <param name="reader">The reader.</param>
		public void BinaryDeserialize(BinaryReader reader)
		{
			var flags = (RsaKeyFlags)reader.ReadByte();
			if ((flags & RsaKeyFlags.Public) != 0)
			{
				m_RsaParams.Exponent = reader.ReadPacket();
				m_RsaParams.Modulus = reader.ReadPacket();
			}
			if ((flags & RsaKeyFlags.Private) != 0)
			{
				m_RsaParams.P = reader.ReadPacket();
				m_RsaParams.Q = reader.ReadPacket();
				m_RsaParams.D = reader.ReadPacket();
				m_RsaParams.DP = reader.ReadPacket();
				m_RsaParams.DQ = reader.ReadPacket();
				m_RsaParams.InverseQ = reader.ReadPacket();
			}
		}

		#endregion
	}
}
