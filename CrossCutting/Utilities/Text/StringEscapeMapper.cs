using System;

namespace Indigo.CrossCutting.Utilities.Text
{
	/// <summary>
	/// Simple implementation of <see cref="IStringEscapeMapper"/>.
	/// </summary>
	public class StringEscapeMapper
        : IStringEscapeMapper
	{
		#region fields

		private readonly bool m_EscapeQuote /* = false */;
		private readonly bool m_EscapeDoubleQuote /* = false */;
		private readonly bool m_EscapeControl /* = false */;
		private readonly bool m_EscapeUnicode /* = false */;
		private readonly bool m_EscapeGlyph /* = false */;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="StringEscapeMapper"/> class.
		/// </summary>
		/// <param name="escapeQuote">if set to <c>true</c> quotes will be escaped.</param>
		/// <param name="escapeDoubleQuote">if set to <c>true</c> double quotes will be escaped.</param>
		/// <param name="escapeControl">if set to <c>true</c> control characters [0,31] will be escaped.</param>
		/// <param name="escapeUnicode">if set to <c>true</c> unicode characters [256,...] will be escaped.</param>
		/// <param name="escapeGlyph">if set to <c>true</c> glyph characters [128,255] will be escaped.</param>
		public StringEscapeMapper(bool escapeQuote, bool escapeDoubleQuote, bool escapeControl, bool escapeUnicode, bool escapeGlyph)
		{
			m_EscapeQuote = escapeQuote;
			m_EscapeDoubleQuote = escapeDoubleQuote;
			m_EscapeControl = escapeControl;
			m_EscapeUnicode = escapeUnicode;
			m_EscapeGlyph = escapeGlyph;
		}

		#endregion

		#region IStringEscapeMapper Members

		/// <summary>
		/// Decides if character should be escaped or not.
		/// </summary>
		/// <param name="character">The character.</param>
		/// <returns></returns>
		public bool NeedEscape(char character)
		{
			uint code = character;

			return
					(character == '\\') ||
					((character == '\'') && m_EscapeQuote) ||
					((character == '\"') && m_EscapeDoubleQuote) ||
					((code < 32) && m_EscapeControl) ||
					((code > 127) && (code <= 255) && m_EscapeGlyph) ||
					((code > 255) && m_EscapeUnicode);
		}

		#endregion
	}
}
