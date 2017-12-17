using System;

namespace Indigo.CrossCutting.Utilities.Text.Support
{
	/// <summary><see cref="TemplateString"/>'s chunk holding information about plan text.</summary>
	public class TextChunk
        : AbstractChunk
	{
		#region fields

		// these fields are readonly for a purpose
		// one NewLineChunk can be shared between multiple templates
		// it important to make sure that once created it will never change

		/// <summary>Text.</summary>
		private readonly string m_Text;

		/// <summary>Start index.</summary>
		private readonly int m_StartIndex;

		/// <summary>Length of chunk.</summary>
		private readonly int m_ChunkLength;

		/// <summary>Caches information if chunk is multiline.</summary>
		private bool? m_IsMultiline;

		#endregion

		#region properties

		/// <summary>Gets a value indicating whether this chunk is multiline.</summary>
		/// <value><c>true</c> if this instance is multiline; otherwise, <c>false</c>.</value>
		public override bool IsMultiline
		{
			get
			{
				if (!m_IsMultiline.HasValue)
				{
					m_IsMultiline = CheckIsMultiline();
				}
				return m_IsMultiline.Value;
			}
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TextChunk"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="chunkLength">Length of the chunk.</param>
		public TextChunk(string text, int startIndex = 0, int chunkLength = int.MaxValue)
		{
			m_Text = text;
			m_StartIndex = startIndex;
			m_ChunkLength = Math.Min(chunkLength, text.Length - m_StartIndex);
		}

        #endregion

        #region public interface

        /// <summary>Produces (converts chunk to string) the chunk to specified context.</summary>
        /// <param name="context">The context.</param>
        public override void Produce(ProduceContext context) => context.Append(m_Text, m_StartIndex, m_ChunkLength);

        /// <summary>Resolves (expands) the chunk to specified context.</summary>
        /// <param name="context">The context.</param>
        public override void Resolve(ResolveContext context) => context.Add(this);

        #endregion

        #region private implementation

        /// <summary>Checks if chunk is multiline.</summary>
        /// <returns></returns>
        private bool CheckIsMultiline() => m_Text.LastIndexOf('\n', m_StartIndex + m_ChunkLength - 1, m_ChunkLength) >= 0;

        #endregion

        #region object overrides

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
		{
			return string.Format(
				"{0}({1})",
				GetType().Name,
				Escape(m_Text.Substring(m_StartIndex, m_ChunkLength)));
		}

		#endregion
	}
}
