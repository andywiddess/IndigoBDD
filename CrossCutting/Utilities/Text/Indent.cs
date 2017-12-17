#region Header
// --------------------------------------------------------------------------------------
// Indigo.CrossCutting.Utilities.Text.Indent.cs
// --------------------------------------------------------------------------------------
//
// Class for manage string indentaton.
//
// Copyright (c) 2011 Sepura Plc
//
// Sepura Confidential
//
// Created: Milosz Krajewski
//
// --------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Indigo.CrossCutting.Utilities.Text
{
	/// <summary>
	/// Class for manage string indentaton.
	/// </summary>
	public class Indent
	{
		#region method IndentBlock

		/// <summary>
		/// Indents the block of text.
		/// </summary>
		/// <param name="indent">The indent.</param>
		/// <param name="block">The block.</param>
		/// <returns>Indented block.</returns>
		public static string IndentBlock(string indent, string block)
		{
			if ((indent == null) || (indent.Length == 0))
				return block;

			var result = new StringBuilder();
			foreach (string line in ExtractLines(block))
			{
				if (IsEmptyLine(line))
				{
					if (!line.EndsWith("\n"))
					{
						// it is the last line (does not end with \n) and it is empty
						// do not add \n to result
					}
					else
					{
						result.AppendLine();
					}
				}
				else
				{
					result.Append(indent).Append(line);
				}
			}
			return result.ToString();
		}

        /// <summary>
        /// Reindents the block. Effectively, it deindents the block completely then indents it back.
        /// </summary>
        /// <param name="indent">The indent.</param>
        /// <param name="block">The block.</param>
        /// <param name="raiseError">if set to <c>true</c> error is raised when deindentation fails.</param>
        /// <returns></returns>
        public static string ReindentBlock(string indent, string block, bool raiseError) => IndentBlock(indent, DeindentBlock(FindIndent(block), block, raiseError));

        #endregion

        #region tabs to spaces

        /// <summary>
        /// Converts tabs to spaces in a single line.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="tabSize">Size of the tab.</param>
        /// <returns>Same string with tabs converted to spaces.</returns>
        public static string TabsToSpacesLine(string line, int tabSize)
		{
			var result = new StringBuilder();
			int length = line.Length;
			int column = 0;

			for (int i = 0; i < length; i++)
			{
				char c = line[i];

				if (c != '\t')
				{
					result.Append(c);
					column++;
				}
				else
				{
					int target = (column + tabSize) / tabSize * tabSize;
					result.Append(' ', target - column);
					column = target;
				}
			}
			return result.ToString();
		}

		/// <summary>
		/// Converts tabses to spaces in a block of text.
		/// </summary>
		/// <param name="block">The block of text.</param>
		/// <param name="tabSize">Size of the tab.</param>
		/// <returns>Same string with tabs converted to spaces.</returns>
		public static string TabsToSpaces(string block, int tabSize)
		{
			var result = new StringBuilder();
			foreach (string line in ExtractLines(block))
			{
				result.Append(TabsToSpacesLine(line, tabSize));
			}
			return result.ToString();
		}

		#endregion

		#region method DeindentBlock

		/// <summary>
		/// Deindents the block by a given indent.
		/// </summary>
		/// <param name="indent">The indent.</param>
		/// <param name="block">The block.</param>
		/// <param name="raiseError">if set to <c>true</c> raises error if deindent cannot be applied.</param>
		/// <returns>Deindented text block.</returns>
		public static string DeindentBlock(string indent, string block, bool raiseError)
		{
			var result = new StringBuilder();
			foreach (string line in ExtractLines(block))
			{
				result.Append(DeindentLine(indent, line, raiseError));
			}
			return result.ToString();
		}

		/// <summary>
		/// Deindents the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns>Deindented text block.</returns>
		public static string DeindentBlock(string block)
		{
			string indent = FindIndent(block);
			return DeindentBlock(indent, block, false);
		}

		#endregion

		#region method FindIndent

		private static readonly Regex indentRx = new Regex(@"^\s*", RegexOptions.Compiled);

		/// <summary>
		/// Finds the indent in given block of text.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns>Indentation of text.</returns>
		public static string FindIndent(string block)
		{
			string result = null;

			foreach (string line in ExtractLines(block))
			{
				if (!IsEmptyLine(line))
				{
					var m = indentRx.Match(line);
					if (m.Success)
					{
						string found = m.Value;
						if ((result == null) || (found.Length < result.Length))
						{
							result = found;
						}
					}
				}
			}
			return result;
		}

        #endregion

        #region method IdentLine

        /// <summary>
        /// Idents the line.
        /// </summary>
        /// <param name="indent">The indent.</param>
        /// <param name="line">The line.</param>
        /// <returns>Indented line.</returns>
        public static string IdentLine(string indent, string line) => indent + line;

        #endregion

        #region method DeindentLine

        /// <summary>
        /// Deindents the line.
        /// </summary>
        /// <param name="indent">The indent.</param>
        /// <param name="line">The line.</param>
        /// <param name="raiseError">if set to <c>true</c> [raise error].</param>
        /// <returns></returns>
        public static string DeindentLine(string indent, string line, bool raiseError)
		{
			if (line.StartsWith(indent))
			{
				return line.Remove(0, indent.Length);
			}
			else
			{
				if (raiseError && !IsEmptyLine(line))
				{
					throw new ArgumentException("Given line is not properly indented");
				}
				return line;
			}
		}

		#endregion

		#region method ExtractLine & ExtractLines

        /// <summary>
        /// The raw line receive.
        /// </summary>
		private static readonly Regex s_RawLineRx = new Regex(
				@"((.*?)((\r\n)|(\n)))|((.+?)$)",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Singleline);

		/// <summary>
		/// Extracts single line startting at given index.
		/// </summary>
		/// <param name="block">The block of text.</param>
		/// <param name="startIndex">The start index.</param>
		/// <returns>Text up to EoL or EoF</returns>
		public static string ExtractLine(string block, int startIndex)
		{
			var m = s_RawLineRx.Match(block, startIndex);
			if (!m.Success) return null;
			return m.Value;
		}

		/// <summary>
		/// Enumerates lines in given block of text.
		/// </summary>
		/// <param name="block">The block of text.</param>
		/// <returns>Collection of text lines.</returns>
		public static IEnumerable<string> ExtractLines(string block)
		{
			int index = 0;

			while (true)
			{
				string line = ExtractLine(block, index);
				if (line == null) break;
				index += line.Length;
				yield return line;
			}
		}

		#endregion

		#region method EndsWithNewLine

        /// <summary>
        /// The starts with newline.
        /// </summary>
		private static readonly Regex s_StartsWithNL = new Regex(
				@"^[ \t]*\r?\n", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        /// <summary>
        /// The ends with newline.
        /// </summary>
		private static readonly Regex s_EndsWithNL = new Regex(
				@"\r?\n[ \t]*$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        /// <summary>
        /// Checks if line starts with new line characters (<c>\n</c> or <c>\r\n</c>).
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="trimSpaces">if set to <c>true</c> spaces are trimmed.</param>
        /// <returns><c>true</c> is line starts with new line character.</returns>
        public static bool StartsWithNewLine(string line, bool trimSpaces) => trimSpaces
                ? s_StartsWithNL.Match(line).Success
                : line.StartsWith("\r\n") || line.StartsWith("\n");

        /// <summary>
        /// Checks if line ends with new line characters (<code>\n</code> or <code>\r\n</code>).
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="trimSpaces">if set to <c>true</c> spaces are trimmed.</param>
        /// <returns><c>true</c> is line ends with new line character.</returns>
        public static bool EndsWithNewLine(string line, bool trimSpaces) => trimSpaces
                ? s_EndsWithNL.Match(line).Success
                : line.EndsWith("\n");
        #endregion

        #region method IsEmptyLine & IsWhiteLine

        /// <summary>
        /// The empty line receive.
        /// </summary>
        private static readonly Regex s_EmptyLineRx = new Regex(
				@"^\s*$", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Determines whether given line is empty line. Note: Whitespaces are considered as empty.
        /// </summary>
        ///
        /// <param name="line"> The line. </param>
        ///
        /// <returns>
        /// <c>true</c> if given line is empty line; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmptyLine(string line) => string.IsNullOrEmpty(line) || (s_EmptyLineRx.Match(line).Success);

        #endregion

        #region method ForceNewLine

        /// <summary>
        /// Forces the new line on the end. It is not added if it is already there.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Given text with new line on the end.</returns>
        public static string ForceNewLine(string text) => ForceNewLine(text, false, true, Environment.NewLine);

        /// <summary>
        /// Forces the new line on the end. It is not added if it is already there.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="newLine">The new line combination.</param>
        /// <returns>Given text with new line on the end.</returns>
        public static string ForceNewLine(string text, string newLine) => ForceNewLine(text, false, true, newLine);

        /// <summary>
        /// Forces the new line.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="head">if set to <c>true</c> new line will be enforced before first line.</param>
        /// <param name="tail">if set to <c>true</c> new line will be enforced after last line.</param>
        /// <returns>Text with new line added.</returns>
        public static string ForceNewLine(string text, bool head, bool tail) => ForceNewLine(text, head, tail, Environment.NewLine);

        /// <summary>
        /// Forces the new line.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="head">if set to <c>true</c> new line will be enforced before first line.</param>
        /// <param name="tail">if set to <c>true</c> new line will be enforced after last line.</param>
        /// <param name="newLine">The new line.</param>
        /// <returns>Text with new line added.</returns>
        private static string ForceNewLine(string text, bool head, bool tail, string newLine)
		{
			if (text == null) return null;
			if (text.Length == 0)
			{
				if (head || tail)
					return newLine;
				return text;
			}

			head = head && !StartsWithNewLine(text, true);
			tail = tail && !EndsWithNewLine(text, true);

			return string.Format("{0}{2}{1}",
					head ? newLine : string.Empty,
					tail ? newLine : string.Empty,
					text);
		}

        #endregion

        #region method MakeIndent

        /// <summary>
        /// Makes the indent.
        /// </summary>
        ///
        /// <param name="length">       The length. </param>
        /// <param name="indentChar">   The indent char. </param>
        ///
        /// <returns>
        /// <paramref name="indentChar"/> repeated <paramref name="length"/> times.
        /// </returns>
        public static string MakeIndent(int length, char indentChar) => new string(c: indentChar, count: length);

        #endregion

		#region method NormalizeNewLine

        /// <summary>
        /// The trimmed line.
        /// </summary>
		private readonly static Regex s_TrimmedLine = new Regex(
				@"^(?<head>([ \t]*\r?\n)+)?(?<body>.*?)[ \t]*(?<tail>(\r?\n[ \t]*)+)?$",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Singleline);

        /// <summary>
        /// Conditional new line.
        /// </summary>
        ///
        /// <param name="force">    The force. </param>
        /// <param name="group">    The group. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
		private static string ConditionalNewLine(bool? force, Group group) => force.GetValueOrDefault(@group.Success)
		    ? "\n"
		    : string.Empty;

	    /// <summary>
		/// Normalizes line by adding new line at the beginning and/or at the end.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="head">Set to <c>true</c> if you need new line, set to <c>false</c> if you don't, set to <c>null</c> if you don't care (leave as is).</param>
		/// <param name="tail">Set to <c>true</c> if you need new line, set to <c>false</c> if you don't, set to <c>null</c> if you don't care (leave as is).</param>
		/// <returns>Line with normalized new lines.</returns>
		public static string NormalizeNewLine(string text, bool? head, bool? tail)
		{
			var m = s_TrimmedLine.Match(text);
			if (m.Success)
			{
				var ghead = m.Groups["head"];
				var gbody = m.Groups["body"];
				var gtail = m.Groups["tail"];

				return string.Format(
						"{0}{1}{2}",
						ConditionalNewLine(head, ghead),
						gbody.Value,
						ConditionalNewLine(tail, gtail));
			}
			else
			{
				throw new ArgumentException(string.Format("Text '{0}' cannot be trimmed.", text));
			}
		}

        #endregion

        #region NormalizeText

        /// <summary>
        /// Normalizes the text. Removes \n from the beginning and end of text 
        /// block (kind of trim) and unindents as much as possible.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Trimmed and unindented text.</returns>
        public static string NormalizeText(string text) => DeindentBlock(NormalizeNewLine(text, false, false));

        #endregion

        #region ScanForLineStart & ScanForLineEnd

        /// <summary>
        /// Scans text for line start. It scans backwards starting at given position.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="index">The starting index.</param>
        /// <returns>First character in a line.</returns>
        public static int ScanForLineStart(string input, int index)
		{
			while (true)
			{
				char c = (index < 0) ? '\n' : input[index];

				if (c == '\n')
				{
					index++;
					break;
				}

				index--;
			}

			return index;
		}

		/// <summary>
		/// Scans text for line end. Scans forward from given position.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static int ScanForLineEnd(string input, int index)
		{
			int inputLength = input.Length;

			while (true)
			{
				char c = (index >= inputLength) ? '\n' : input[index];

				if (c == '\n')
				{
					index--;
					break;
				}

				index++;
			}

			return index;
		}

		#endregion
	}
}
