using System.Collections.Generic;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Text.Support
{
	/// <summary>
	/// Context used in production (<see cref="TemplateString"/> to <c>string</c> conversion).
	/// </summary>
	public class ProduceContext
	{
		#region fields

		/// <summary><see cref="IndentationString"/> cached value.</summary>
		private string m_IndentationString;

		#endregion

		#region properties

		/// <summary>Result of production (so far).</summary>
		public StringBuilder Result { get; internal set; }

		/// <summary>Indentation stack.</summary>
		public Stack<NewLineChunk> IndentStack { get; internal set; } // = null;

		/// <summary>Indentation stack top. Please note, this indentation is not applied, it will be applied when multiline 
		/// expansion is inserted.</summary>
		public NewLineChunk IndentStackTop { get; internal set; } // = null;

		/// <summary>Current indentation.</summary>
		public StringBuilder Indentation { get; internal set; }

		/// <summary>Current indentation. It it the same value as <see cref="Indentation"/>. There are two fields, 
		/// <see cref="StringBuilder"/> and <see cref="string"/> for performance reason, but in general they hold same 
		/// value.</summary>
		public string IndentationString
		{
			get
			{
				if (m_IndentationString == null)
				{
					m_IndentationString = Indentation.ToString();
				}
				return m_IndentationString;
			}
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="ProduceContext"/> class.</summary>
		public ProduceContext()
		{
			Result = new StringBuilder();
			IndentStack = new Stack<NewLineChunk>();
			IndentStackTop = null;
			Indentation = new StringBuilder();
		}

        #endregion

        #region public interface

        /// <summary>Indents production.</summary>
        public void Indent() => IndentStackTop.Push(this);

        /// <summary>Unindents production.</summary>
        public void Unindent() => NewLineChunk.Pop(this);

        /// <summary>Appends the specified text.</summary>
        /// <param name="text">The text.</param>
        public void Append(string text) => Append(text, 0, text.Length);

        #endregion

        #region internal implementation

        /// <summary>Appends the specified text.</summary>
        /// <param name="text">The text.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="chunkLength">Length of the chunk.</param>
        internal void Append(string text, int startIndex, int chunkLength)
		{
			if (chunkLength == 0) return;

			Result.Append(text, startIndex, chunkLength);

			if (Indentation.Length > 0 && text[startIndex + chunkLength - 1] == '\n')
			{
				Result.Append(IndentationString);
			}
		}

        /// <summary>
        /// Notifies that indentations has changed. Resets <see cref="IndentationString"/> cache. Used
        /// internally.
        /// </summary>
		internal void IndentationChanged() => m_IndentationString = null;

        #endregion
    }
}
