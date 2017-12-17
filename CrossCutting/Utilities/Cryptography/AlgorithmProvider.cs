using System;
using System.Security.Cryptography;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Cryptography
{
	internal static class AlgorithmProvider
	{
		#region static fields

		/// <summary>Flag indicating if SHA-512 is available from CSP.</summary>
		private static int _hasSHA512 = -1;

		/// <summary>Flag indicating if AES is available from CSP.</summary>
		private static int _hasAES = -1;

		#endregion

		/// <summary>Creates new RandomNumberGenerator.</summary>
		/// <returns>RandomNumberGenerator.</returns>
		public static RandomNumberGenerator NewRNG()
		{
			return RNGCryptoServiceProvider.Create();
		}

		/// <summary>Create new MD5 hash algorithm.</summary>
		/// <returns>MD5 hash algorithm.</returns>
		public static HashAlgorithm NewMD5()
		{
			return MD5CryptoServiceProvider.Create();
		}

		/// <summary>Creates new SHA1 hash algorithm.</summary>
		/// <returns>SHA1 hash algorithm.</returns>
		public static HashAlgorithm NewSHA1()
		{
			return SHA1CryptoServiceProvider.Create();
		}

		/// <summary>Creates new SHA512 hash algorithm.</summary>
		/// <returns>SHA512 hash algorithm.</returns>
		public static HashAlgorithm NewSHA512()
		{
			if (_hasSHA512 == 0)
				return SHA512Managed.Create();

			try
			{
				return SHA512CryptoServiceProvider.Create();
			}
			catch (PlatformNotSupportedException)
			{
				Interlocked.Exchange(ref _hasSHA512, 0);
				return NewSHA512();
			}
		}

		/// <summary>Creates new RC2 encryption algorithm.</summary>
		/// <returns>RC2 algorithm.</returns>
		public static SymmetricAlgorithm NewRC2()
		{
			return RC2CryptoServiceProvider.Create();
		}

		/// <summary>Creates new AES encryption algorithm.</summary>
		/// <returns>AES algorithm.</returns>
		public static SymmetricAlgorithm NewAES()
		{
			if (_hasAES == 0)
				return RijndaelManaged.Create();

			try
			{
				return AesCryptoServiceProvider.Create();
			}
			catch (PlatformNotSupportedException)
			{
				Interlocked.Exchange(ref _hasAES, 0);
				return NewAES();
			}
		}
	}
}
