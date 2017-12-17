using System;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Text
{
	/// <summary>Class providing ability to escape strings.</summary>
	public class StringEscape
	{
		#region static fields

		/// <summary>StringEscapeMapper escaping everything.</summary>
		public static readonly IStringEscapeMapper DefaultMapper = new StringEscapeMapper(true, true, true, true, true);

		/// <summary>Escape mapper for C-like strings.</summary>
		public static readonly IStringEscapeMapper StringEscapeMapper = new StringEscapeMapper(false, true, true, false, true);

		/// <summary>Escape mapper to C-like character constants.</summary>
		public static readonly IStringEscapeMapper CharEscapeMapper = new StringEscapeMapper(true, false, true, true, true);

		/// <summary>StringEscapeMapper for command line file names.</summary>
		public static readonly IStringEscapeMapper FileNameStringMapper = new StringEscapeMapper(false, true, false, false, false);

		#endregion

		#region fields

		private IStringEscapeMapper m_Mapper;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the mapper used for encoding.
		/// </summary>
		/// <value>The mapper.</value>
		public IStringEscapeMapper Mapper
		{
			get => Mapper1;
		    set => Mapper1 = value;
		}

        public IStringEscapeMapper Mapper1 { get => m_Mapper; set => m_Mapper = value; }

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StringEscape"/> 
        /// class with given <see cref="IStringEscapeMapper">mapper</see>.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public StringEscape(IStringEscapeMapper mapper)
		{
			Mapper1 = mapper;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StringEscape"/> class using default mapper.
		/// </summary>
		public StringEscape()
		{
			Mapper1 = new StringEscapeMapper(true, true, true, true, true);
		}

		#endregion

		#region method Escape

        /// <summary>
        /// Escape as character.
        /// </summary>
        ///
        /// <param name="character">    The character. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
		private static string EscapeAsChar(char character) => "\\" + character;

        /// <summary>
        /// Escape as hexadecimal.
        /// </summary>
        ///
        /// <param name="character">    The character. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
	    private static string EscapeAsHex(char character)
		{
			Byte code = (Byte)character;
			return "\\x" + new string(HexConvert.ToCharArray(code, true));
		}

        /// <summary>
        /// Escape as unicode.
        /// </summary>
        ///
        /// <param name="character">    The character. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
		private static string EscapeAsUnicode(char character)
		{
			UInt16 code = character;
			return "\\u" + new string(HexConvert.ToCharArray(code, true));
		}

        /// <summary>
        /// Escapes the given character.
        /// </summary>
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        ///
        /// <param name="character">    The character. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
		private static string Escape(char character)
		{
			switch (character)
			{
				case '\n': return "\\n";
				case '\r': return "\\r";
				case '\t': return "\\t";
				case '\b': return "\\b";
				case '\a': return "\\a";
				case '\f': return "\\f";
				case '\v': return "\\v";
				case '\0': return "\\0";

				case 'n':
				case 'r':
				case 't':
				case 'b':
				case 'a':
				case 'f':
				case 'v':
				case 'x':
				case 'u':
				case '0':
					return EscapeAsHex(character);

				default:
					uint code = character;

					if ((code >= 32) && (code <= 127))
					{
						return EscapeAsChar(character);
					}
					else if (code <= 0xFF)
					{
						return EscapeAsHex(character);
					}
					else if (code <= 0xFFFF)
					{
						return EscapeAsUnicode(character);
					}
					throw new ArgumentException();
			}
		}
		#endregion

		#region method Unescape
        /// <summary>
        /// Unescapes.
        /// </summary>
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        ///
        /// <param name="buffer">   The buffer. </param>
        /// <param name="index">    [in,out] Zero-based index of the. </param>
        ///
        /// <returns>
        /// A char.
        /// </returns>
		private static char Unescape(string buffer, ref int index)
		{
			if (string.IsNullOrEmpty(buffer))
				throw new ArgumentException("buffer is null or empty.", "buffer");

			char result;

			try
			{
				result = buffer[index];
			}
			catch (IndexOutOfRangeException)
			{
				throw new ArgumentException();
			}

			if (result != '\\')
			{
				index++;
				return result;
			}

			try
			{
				result = buffer[index + 1];
			}
			catch (IndexOutOfRangeException)
			{
				throw new ArgumentException();
			}

			switch (result)
			{
				case 'n': index += 2; return '\n';
				case 'r': index += 2; return '\r';
				case 't': index += 2; return '\t';
				case 'b': index += 2; return '\b';
				case 'a': index += 2; return '\a';
				case 'f': index += 2; return '\f';
				case 'v': index += 2; return '\v';
				case '0': index += 2; return '\0';

				case 'x':
					try
					{
						string code = buffer.Substring(index + 2, 2);
						if ((code.Length < 2) || (!HexConvert.Check(code)))
							throw new ArgumentException();
						result = (char)HexConvert.FromStringToInt(code);
						index += 4;
						return result;
					}
					catch
					{
						throw new ArgumentException();
					}

				case 'u':
					try
					{
						string code = buffer.Substring(index + 2, 4);
						if ((code.Length < 4) || (!HexConvert.Check(code)))
							throw new ArgumentException();
						result = (char)HexConvert.FromStringToInt(code);
						index += 6;
						return result;
					}
					catch
					{
						throw new ArgumentException();
					}

				default:
					index += 2;
					return result;
			}
		}

		#endregion

		#region method Encode

		/// <summary>
		/// Encodes give text as printable. Converts control characters to escaped characters.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="map">Printable characters map. Used to determine which character should be escaped.</param>
		/// <returns>Escaped text.</returns>
		public static string Encode(string text, IStringEscapeMapper map)
		{
			StringBuilder result = new StringBuilder();

			foreach (char c in text)
			{
				if (c == '\\' || map.NeedEscape(c))
				{
					result.Append(Escape(c));
				}
				else
				{
					result.Append(c);
				}
			}

			return result.ToString();
		}

		/// <summary>
		/// Encodes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>Escaped version of given text.</returns>
		public string Encode(string text) => Encode(text, Mapper1);

	    #endregion

		#region method Decode

		/// <summary>
		/// Decodes the specified text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public static string Decode(string text)
		{
			StringBuilder result = new StringBuilder();

			int index = 0;
			while (index < text.Length)
			{
				try
				{
					char character = Unescape(text, ref index);
					result.Append(character);
				}
				catch (ArgumentException)
				{
					result.Append(text[index]);
					index++;
				}
			}

			return result.ToString();
		}

		#endregion
	}
}
