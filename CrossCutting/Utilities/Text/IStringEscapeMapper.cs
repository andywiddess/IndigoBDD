#region Header
// --------------------------------------------------------------------------------------
// Indigo.CrossCutting.Utilities.Text.IStringEscapeMapper.cs
// --------------------------------------------------------------------------------------
//
// Provides information for StringEscape class.
// Allows to decide which characters should be escaped.
//
// Copyright (c) 2011 Sepura Plc
//
// Sepura Confidential
//
// Created: Milosz Krajewski
//
// --------------------------------------------------------------------------------------
#endregion

namespace Indigo.CrossCutting.Utilities.Text
{
	/// <summary>
	/// Provides information for StringEscape class.
	/// Allows to decide which characters should be escaped.
	/// </summary>
	public interface IStringEscapeMapper
	{
		/// <summary>
		/// Decides if character should be escaped or not.
		/// </summary>
		/// <param name="character">The character.</param>
		/// <returns></returns>
		bool NeedEscape(char character);
	}
}
