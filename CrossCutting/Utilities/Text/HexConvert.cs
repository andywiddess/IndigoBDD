using System;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Text
{
	/// <summary>
	/// Routines to encode binary data as hexadecimal string.
	/// </summary>
	public class HexConvert
	{
		#region ToChar, FromChar

		/// <summary>
		/// Converts 4bit value to character representation.
		/// Note: even if methods allows to use any character as 'A' you should not use characters
		/// other than 'A' or 'a'. It has been implemented like this for performance reasons.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="achar">The character representing character 'A'. Use 'a' or 'A'.</param>
		/// <returns></returns>
		public static char ToChar(int value, char achar)
		{
			value &= 0x0F;

			return (value > 9)
				? ((char)((value - 10) + (int)achar)) // abcdef
				: ((char)(value + (int)'0')); // 0123456789
		}

		/// <summary>
		/// Converts a character to 4bit value. It does not any checking,
		/// so passing invalid character would lead to invalid results.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>4bit value.</returns>
		public static int FromChar(char value)
		{
			if (value >= 'a') return ((int)value - (int)'a' + 10) & 0x0F;
			if (value >= 'A') return ((int)value - (int)'A' + 10) & 0x0F;
			return ((int)value - (int)'0') & 0x0F;
		}

		#endregion

		#region ToCharArray, FromCharArray

		/// <summary>
		/// Converts a byte to 2-character array.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="upper">if set to <c>true</c> uppercase digits are used.</param>
		/// <returns>2-character array with representation of given byte.</returns>
		public static char[] ToCharArray(Byte value, bool upper)
		{
			char achar = upper ? 'A' : 'a';

			return new char[] { 
                ToChar((int) (value >> 4), achar), 
                ToChar((int) (value), achar) 
            };
		}

		/// <summary>
		/// Converts a 16bit integer to 4-character array.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="upper">if set to <c>true</c> uppercase digits are used.</param>
		/// <returns>4-character array with representation of given byte.</returns>
		public static char[] ToCharArray(UInt16 value, bool upper)
		{
			char achar = upper ? 'A' : 'a';

			return new char[] { 
                ToChar((int) (value >> 0x0C), achar), 
                ToChar((int) (value >> 0x08), achar), 
                ToChar((int) (value >> 0x04), achar), 
                ToChar((int) (value), achar) 
            };
		}

        /// <summary>
        /// Converts a byte buffer to character array. Uses uppercase characters.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>Character array with hex representation of byte buffer.</returns>
        public static char[] ToCharArray(byte[] buffer) => ToCharArray(buffer, true);

        /// <summary>
        /// Converts a byte buffer to character array.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="upper">if set to <c>true</c> uppercase digits are used.</param>
        /// <returns>Character array with hex representation of byte buffer.</returns>
        public static char[] ToCharArray(byte[] buffer, bool upper)
		{
			char[] result = new char[buffer.Length << 1]; // *2
			char achar = upper ? 'A' : 'a';

			int inoffset = 0;
			int outoffset = 0;
			int left = buffer.Length;

			while (left > 0)
			{
				byte value = buffer[inoffset++];
				result[outoffset++] = ToChar(value >> 4, achar);
				result[outoffset++] = ToChar(value, achar);
				left--;
			}

			return result;
		}

		/// <summary>
		/// Converts character array with hex digits into byte buffer.
		/// </summary>
		/// <param name="buffer">The character array.</param>
		/// <returns>Decoded byte buffer.</returns>
		public static byte[] FromCharArray(char[] buffer)
		{
			byte[] result = new byte[buffer.Length >> 1];
			int inoffset = 0;
			int outoffset = 0;
			int left = result.Length;

			while (left > 0)
			{
				int value = 0;
				value += FromChar(buffer[inoffset++]) << 4;
				value += FromChar(buffer[inoffset++]);
				result[outoffset++] = (byte)value;
				left--;
			}

			return result;
		}

		#endregion

		#region ToString, FromString

		/// <summary>
		/// Convert byte buffer into hex string. Uses uppercase digits.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <returns>Hex string.</returns>
		public static string ToString(byte[] buffer) => ToString(buffer, true);

        /// <summary>
        /// Convert byte buffer into hex string.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="upper">if set to <c>true</c> uppercase digits are used.</param>
        /// <returns>Hex string.</returns>
        public static string ToString(byte[] buffer, bool upper) => new string(ToCharArray(buffer, upper));

        /// <summary>
        /// Converts hex string into byte buffer.
        /// </summary>
        /// <param name="buffer">Hex string.</param>
        /// <returns>Byte buffer.</returns>
        public static byte[] FromString(string buffer) => FromCharArray(buffer.ToCharArray());

        /// <summary>
        /// Converts hex string into integer value.
        /// </summary>
        /// <param name="buffer">Hex string.</param>
        /// <returns>Integer value.</returns>
        public static UInt64 FromStringToInt(string buffer) => FromStringToInt(buffer, 0, buffer.Length);

        /// <summary>
        /// Converts hex string starting at given index into integer value.
        /// </summary>
        /// <param name="buffer">Hex string.</param>
        /// <param name="start">The starting index.</param>
        /// <returns>Integer value.</returns>
        public static UInt64 FromStringToInt(string buffer, int start) => FromStringToInt(buffer, start, buffer.Length - start);

        /// <summary>
        /// Converts hex string starting at given index and with given length into integer value.
        /// </summary>
        /// <param name="buffer">Hex string.</param>
        /// <param name="start">The starting index.</param>
        /// <param name="length">The number of characters to take.</param>
        /// <returns>Integer value.</returns>
        public static UInt64 FromStringToInt(string buffer, int start, int length)
		{
			UInt64 result = 0;
			int end = Math.Min(start + length, buffer.Length);

			for (int i = start; i < end; i++)
			{
				result = (result << 4) + (uint)FromChar(buffer[i]);
			}

			return result;
		}

		#endregion

		#region Check

		/// <summary>
		/// Checks if given character is valid haxadecimal digit.
		/// </summary>
		/// <param name="digit">The digit character.</param>
		/// <returns><c>true</c> if character is a valid digit; <c>false</c> otherwise.</returns>
		public static bool Check(char digit)
		{
			return
				((digit >= 'a') && (digit <= 'f')) ||
				((digit >= 'A') && (digit <= 'F')) ||
				((digit >= '0') && (digit <= '9'));
		}

		/// <summary>
		/// Checks if given string is valid hexadecimal string.
		/// </summary>
		/// <param name="buffer">The haxadecimal string.</param>
		/// <returns><c>true</c> if string is a valid hexadecimal string; <c>false</c> otherwise.</returns>
		public static bool Check(string buffer)
		{
			int len = buffer.Length;
			for (int i = 0; i < len; i++)
			{
				if (!Check(buffer[i])) return false;
			}
			return true;
		}

		#endregion
	}
}