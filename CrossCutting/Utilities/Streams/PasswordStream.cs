using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// Provides simple access to encrypted files. It uses RC2 with 40bit key. Read notes below.
	/// NOTE, was AES-128 (Rijandel) but has been change due to UK/CSG export restrictions.
	/// NOTE, was DES-56 but has been changed due to US export restrictions and not supported by Windows' CryptoAPI in some countries.
	/// </summary>
	/// <example><code><![CDATA[
	/// using (FileStream fileStream = new FileStream("c:\\file.ext"))
	/// using (PasswordStream stream = new PasswordStream(fileStream, "NoMoreSecrets", CryptoStreamMode.Write))
	/// {
	///   StreamWriter writer = new StreamWriter(stream);
	///   writer.WriteLine("Confidential information");
	/// }
	/// ]]></code></example>
	public class PasswordStream
        : CryptoStream
	{
		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PasswordStream"/> class.
		/// Creates encrypted stream over existing stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="password">The password. Password is used to generate encryption key.</param>
		/// <param name="mode">Stream mode (Read or Write).</param>
		public PasswordStream(Stream stream, string password, CryptoStreamMode mode)
			: base(stream, BuildTransform(password, mode), mode)
		{
		}

		#endregion

		#region Key & IV

		private static byte[] ExtendHash(byte[] password, int keyBytes, int ivBytes)
		{
			if (password.Length >= keyBytes + ivBytes)
				return password;
			throw new NotImplementedException("Hash extension has not been implemented");
		}

		private static SymmetricAlgorithm SetupAlgorithm(
			byte[] password,
			SymmetricAlgorithm encryptionAlgorithm,
			int keyBytes, int ivBytes)
		{
			password = ExtendHash(password, keyBytes, ivBytes);

			byte[] key = new byte[keyBytes];
			byte[] iv = new byte[ivBytes];

			Array.Copy(password, 0, key, 0, keyBytes);
			Array.Copy(password, keyBytes, iv, 0, ivBytes);
			encryptionAlgorithm.Key = key;
			encryptionAlgorithm.IV = iv;

			return encryptionAlgorithm;
		}

		private static SymmetricAlgorithm SetupAlgorithm(
			string password,
			SymmetricAlgorithm encryptionAlgorithm,
			HashAlgorithm hashAlgorithm,
			int keyBytes, int ivBytes)
		{
			return SetupAlgorithm(
				hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password)),
				encryptionAlgorithm,
				keyBytes, ivBytes);
		}

		private static SymmetricAlgorithm SetupRC2_40(string password)
		{
			return SetupAlgorithm(
				password,
				RC2CryptoServiceProvider.Create(),
				MD5CryptoServiceProvider.Create(),
				5, 8 // 40bit Key, 64bit IV
			);
		}

		/*
		private static SymmetricAlgorithm SetupDES_56(string password)
		{
		    return SetupAlgorithm(
		        password,
		        DESCryptoServiceProvider.Create(),
		        MD5CryptoServiceProvider.Create(),
		        8, 8 // 64bit Key, 64bit IV
			);
		}

		private static SymmetricAlgorithm SetupAES_128(string password)
		{
		    return SetupAlgoritm(
		        RijndaelManaged.Create(),
		        SHA256Managed.Create(),
		        16, 16 // 128bit Key, 128bit IV
			);
		}
		*/

		#endregion

		#region utility

		private static ICryptoTransform BuildTransform(string password, CryptoStreamMode mode)
		{
			// SymmetricAlgorithm algorithm = SetupAES_128(password);
			var algorithm = SetupRC2_40(password);

			switch (mode)
			{
				case CryptoStreamMode.Read: return algorithm.CreateDecryptor();
				case CryptoStreamMode.Write: return algorithm.CreateEncryptor();
				default: throw new ArgumentException(string.Format("Unknown CryptoStreamMode: {0}", mode));
			}
		}

		#endregion
	}
}
