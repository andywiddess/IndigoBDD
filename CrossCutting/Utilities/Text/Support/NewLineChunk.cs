using System;

namespace Indigo.CrossCutting.Utilities.Text.Support
{
	/// <summary><see cref="TemplateString"/>'s chunk holding information about new line.</summary>
	public class NewLineChunk: AbstractChunk
	{
		#region static fields

		/// <summary>Empty new link chunk. Can be shared by multiple objects and processes.</summary>
		public static readonly NewLineChunk Default = new NewLineChunk(string.Empty, 0, 0, 0);

		#endregion

		#region fields

		// these fields are readonly for a purpose
		// one NewLineChunk can be shared between multiple templates
		// it important to make sure that once created it will never change

		private readonly string m_Text;
		private readonly int m_StartIndex;
		private readonly int m_IndentLength;
		private readonly int m_TextLength;
		private bool? m_IsMultiline;

		#endregion

		#region properties

		/// <summary>
		/// Gets a value indicating whether this chunk is multiline.
		/// </summary>
		/// <value><c>true</c> if this instance is multiline; otherwise, <c>false</c>.</value>
		public override bool IsMultiline
		{
			get
			{
				// it is lock (this) for a reason
				// the chunk is atomic immutable object, and for performance reasons we don't need some mock 'object' dangling
				lock (this)
				{
					if (!m_IsMultiline.HasValue)
					{
						m_IsMultiline = CheckIsMultiline();
					}
					return m_IsMultiline.Value;
				}
			}
		}

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="NewLineChunk"/> class.</summary>
		/// <param name="text">The text.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="indentLength">Length of the indent.</param>
		/// <param name="extendedLength">Length of the extended.</param>
		public NewLineChunk(string text, int startIndex, int indentLength, int extendedLength = 0)
		{
			m_Text = text;
			m_StartIndex = startIndex;
			m_IndentLength = indentLength;
			m_TextLength = extendedLength;
		}

		#endregion

		#region public interface

		/// <summary>Produces (converts chunk to string) the chunk to specified context.</summary>
		/// <param name="context">The context.</param>
		public override void Produce(ProduceContext context)
		{
			context.IndentStackTop = this;
			int length = m_IndentLength + m_TextLength;
			if (length > 0)
			{
				context.Append(m_Text, m_StartIndex, length);
			}
		}

        /// <summary>Resolves (expands) the chunk to specified context.</summary>
        /// <param name="context">The context.</param>
        public override void Resolve(ResolveContext context) => context.Add(this);

        #endregion

        #region internal implementation

        /// <summary>Pushes itself onto indentation stack.</summary>
        /// <param name="context">The context.</param>
        internal void Push(ProduceContext context)
		{
			context.IndentStack.Push(this);
			context.IndentStackTop = Default;

			if (m_IndentLength > 0)
			{
				context.Indentation.Append(m_Text, m_StartIndex, m_IndentLength);
				context.IndentationChanged();
			}
		}

		/// <summary>Pops une chunk from indentation stack.</summary>
		/// <param name="context">The context.</param>
		internal static void Pop(ProduceContext context)
		{
			var top = context.IndentStack.Pop();

			if (top.m_IndentLength > 0)
			{
				context.Indentation.Length -= top.m_IndentLength;
				context.IndentationChanged();
			}

			context.IndentStackTop = top;
		}

		/// <summary>Checks if chunk is multiline.</summary>
		/// <returns><c>true</c> if multiline; <c>false</c> otherwise</returns>
		private bool CheckIsMultiline()
		{
			return
				m_TextLength == 0 
				? false 
				: m_Text.LastIndexOf('\n', m_StartIndex + m_IndentLength + m_TextLength - 1, m_TextLength) >= 0;
		}

		#endregion

		#region overrides

		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format("{0}({1})",
				GetType().Name,
				Escape(m_Text.Substring(m_StartIndex, m_IndentLength + m_TextLength)));
		}

		#endregion
	}
}
