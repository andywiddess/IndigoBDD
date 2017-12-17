using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Indigo.CrossCutting.Utilities.Text;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// TextWriter which indents CDATA section in XML file. Note that indenting
	/// CDATA is actually change to CDATA content, so your code has to handle
	/// it properly. Don't use this class if you don't understand what it does,
	/// normal StreamWriter is much safer.
	/// </summary>
	public class CDataIndentWriter
        : TextWriter
	{
		#region consts

		/// <summary>
		/// Default encoding if not provided (UTF8).
		/// </summary>
		public static readonly Encoding DefaultEncoding = Encoding.UTF8; // make it ASCII as long as possible

		/// <summary>
		/// Default indent if not provided (4 spaces).
		/// </summary>
		public static readonly string DefaultIndent = new string(' ', 4); // 4 spaces

		/// <summary>
		/// Default buffer size if not provided (1 kB).
		/// </summary>
		public const int DefaultBufferSize = 1024; // 1kB

		#endregion

		#region fields

		private TextWriter m_Writer;
		private StringBuilder m_Line = null;
		private StringBuilder m_CData = null;

		private string m_CDataIndent;
		private string m_Indent = new string(' ', 4);

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="CDataIndentWriter"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public CDataIndentWriter(Stream stream)
			: this(stream, null, null, -1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CDataIndentWriter"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="cdataIndent">The CDATA indent.</param>
		public CDataIndentWriter(Stream stream, string cdataIndent)
			: this(stream, cdataIndent, null, -1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CDataIndentWriter"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="cdataIndent">The CDATA indent.</param>
		/// <param name="encoding">The encoding.</param>
		public CDataIndentWriter(Stream stream, string cdataIndent, Encoding encoding)
			: this(stream, cdataIndent, encoding, -1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CDataIndentWriter"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="cdataIndent">The CDATA indent.</param>
		/// <param name="encoding">The encoding.</param>
		/// <param name="bufferSize">Size of the buffer.</param>
		public CDataIndentWriter(Stream stream, string cdataIndent, Encoding encoding, int bufferSize)
		{
			if (stream == null)
				throw new ArgumentNullException("stream", "stream is null.");

			if (cdataIndent == null) cdataIndent = DefaultIndent;
			if (encoding == null) encoding = DefaultEncoding;
			if (bufferSize < 0) bufferSize = DefaultBufferSize;

			m_Writer = new StreamWriter(stream, encoding, bufferSize);
			m_Indent = cdataIndent;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CDataIndentWriter"/> class.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="cdataIndent">The CDATA indent.</param>
		public CDataIndentWriter(TextWriter writer, string cdataIndent)
		{
			if (writer == null)
				throw new ArgumentNullException("writer", "writer is null.");
			m_Writer = writer;
			m_Indent = cdataIndent;
		}

		#endregion

		#region overrides

		/// <summary>
		/// When overridden in a derived class, returns the <see cref="T:System.Text.Encoding"/> in which the output is written.
		/// </summary>
		/// <value></value>
		/// <returns>The Encoding in which the output is written.</returns>
		public override Encoding Encoding
		{
			get { return m_Writer.Encoding; }
		}

		/// <summary>
		/// Closes the current writer and releases any system resources associated with the writer.
		/// </summary>
		public override void Close()
		{
			ProcessLine(true);
			m_Writer.Close();
		}

		/// <summary>
		/// Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.
		/// Note: if Flush() is called in the middle of CDATA section it WILL be written to undarlaying stream as is, and WILL NOT
		/// be indented. I assume Flush() is more important then indentation.
		/// </summary>
		public override void Flush()
		{
			// if Flush is called in CDATA it won't be adjusted
			// 
			if (m_CData != null)
			{
				m_Writer.Write(m_CData.ToString());
				m_CData = null;
			}

			if (m_Line != null)
			{
				m_Writer.Write(m_Line.ToString());
				m_Line = null;
			}

			m_Writer.Flush();
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.IO.TextWriter"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			m_Writer.Dispose();
		}

		/// <summary>
		/// Gets an object that controls formatting.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.IFormatProvider"/> object for a specific culture, or the formatting of the current culture if no other culture is specified.</returns>
		public override IFormatProvider FormatProvider
		{
			get { return m_Writer.FormatProvider; }
		}

		/// <summary>
		/// Gets or sets the line terminator string used by the current TextWriter.
		/// </summary>
		/// <value></value>
		/// <returns>The line terminator string for the current TextWriter.</returns>
		public override string NewLine
		{
			get { return m_Writer.NewLine; }
			set { m_Writer.NewLine = value; }
		}

		/// <summary>
		/// Writes a character to the text stream.
		/// </summary>
		/// <param name="value">The character to write to the text stream.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter"/> is closed. </exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		public override void Write(char value)
		{
			if (m_Line == null) m_Line = new StringBuilder();
			m_Line.Append(value);

			if (value == '\n')
			{
				ProcessLine(false);
			}
		}

		#endregion

		#region processing

		private static Regex s_CDataHeadRx = new Regex(
				@"^(?<indent>\s*)(?<head>.*\<\!\[CDATA\[)\s*$",
				RegexOptions.ExplicitCapture | RegexOptions.Compiled);
		private static Regex s_CDataTailRx = new Regex(
				@"^(?<text>.*)(?<tail>\]\]\>.*)$",
				RegexOptions.ExplicitCapture | RegexOptions.Compiled);

		/// <summary>
		/// Processes the line.
		/// </summary>
		/// <param name="eof">if set to <c>true</c> processes line as the last one.</param>
		private void ProcessLine(bool eof)
		{
			string line = m_Line.ToString();

			if (m_CData == null)
			{
				Match match = s_CDataHeadRx.Match(line);

				if (match.Success)
				{
					m_CDataIndent = match.Groups["indent"].Value;
					m_CData = new StringBuilder();
				}

				m_Writer.Write(line);
			}
			else
			{
				Match match = s_CDataTailRx.Match(line);
				if (match.Success)
				{
					m_CData.Append(match.Groups["text"].Value);
					m_Writer.Write(
							Indent.ReindentBlock(
									m_Indent + m_CDataIndent,
									Indent.NormalizeNewLine(m_CData.ToString(), false, false),
									false));
					m_Writer.WriteLine();
					m_Writer.Write(Indent.IndentBlock(m_CDataIndent, match.Groups["tail"].Value));
					m_CData = null;
				}
				else
				{
					m_CData.Append(line);

					if (eof)
					{
						m_Writer.Write(m_CData.ToString());
					}
				}
			}

			m_Line = null;
		}

		#endregion
	}
}
