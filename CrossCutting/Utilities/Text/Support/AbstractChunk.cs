#region Header
// --------------------------------------------------------------------------------------
// Indigo.CrossCutting.Utilities.Text.Support.AbstractChunk.cs
// --------------------------------------------------------------------------------------
//
// Base class for chunks in Templatestring.
//
// Copyright (c) 2011 Sepura Plc
//
// Sepura Confidential
//
// Created: Milosz Krajewski
//
// --------------------------------------------------------------------------------------
#endregion

namespace Indigo.CrossCutting.Utilities.Text.Support
{
    /// <summary>
    /// Base class for chunks in <see cref="TemplateString"/>.
    /// </summary>
	public abstract class AbstractChunk
	{
		/// <summary>Gets a value indicating whether this chunk is multiline.</summary>
		/// <value><c>true</c> if this instance is multiline; otherwise, <c>false</c>.</value>
		public abstract bool IsMultiline { get; }

		/// <summary>Produces (converts chunk to string) the chunk to specified context.</summary>
		/// <param name="context">The context.</param>
		public abstract void Produce(ProduceContext context);

		/// <summary>Resolves (expands) the chunk to specified context.</summary>
		/// <param name="context">The context.</param>
		public abstract void Resolve(ResolveContext context);

        /// <summary>Escapes the specified text. Little utility to make debugging easier.</summary>
        /// <param name="text">The text.</param>
        /// <returns>Escaped text.</returns>
        public static string Escape(string text) => StringEscape.Encode(text, StringEscape.StringEscapeMapper);
    }
}
