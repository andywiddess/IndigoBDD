using System;

namespace Indigo.CrossCutting.Utilities.Cryptography
{
	/// <summary>
	/// RSA key flags.
	/// </summary>
	[Flags]
	public enum RsaKeyFlags
	{
		/// <summary>Key is empty.</summary>
		None = 0x00,
		/// <summary>Key has a public component.</summary>
		Public = 0x01,
		/// <summary>Key has private component.</summary>
		Private = 0x02,
	}
}
