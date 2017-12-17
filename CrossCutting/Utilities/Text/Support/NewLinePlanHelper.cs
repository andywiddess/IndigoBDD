using System;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Text.Support
{
	internal class NewLinePlanHelper
	{
        #region Members
        private readonly string m_Text;
		private readonly LinkedList<AbstractChunk> m_Plan;

		private int m_Start;
		private int m_IndentLength;
		private int m_TextLength;
		private bool m_Active;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="text"> The text. </param>
        /// <param name="plan"> The plan. </param>
        public NewLinePlanHelper(string text, LinkedList<AbstractChunk> plan)
		{
			m_Text = text;
			m_Plan = plan;
		}
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether this object is active.
        /// </summary>
        ///
        /// <value>
        /// True if this object is active, false if not.
        /// </value>
        public bool IsActive
		{
			get { return m_Active; }
		}
        #endregion

        #region Implementation
        /// <summary>
        /// Resets this object.
        /// </summary>
        ///
        /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
        ///                                                 invalid. </exception>
        ///
        /// <param name="start">        The start. </param>
        /// <param name="indentLength"> Length of the indent. </param>
        public void Reset(int start, int indentLength)
		{
			if (m_Active)
			{
				throw new InvalidOperationException("Cannot initialize active chunk");
			}

			m_Active = true;
			m_Start = start;
			m_IndentLength = indentLength;
			m_TextLength = 0;
		}

        /// <summary>
        /// Expands.
        /// </summary>
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        ///
        /// <param name="start">        The start. </param>
        /// <param name="textLength">   Length of the text. </param>
		public void Expand(int start, int textLength)
		{
			if (start != m_Start + m_IndentLength + m_TextLength)
			{
				throw new ArgumentException("Cannot merge not adjacent chunks");
			}

			m_TextLength += textLength;
		}

        /// <summary>
        /// Flushes this object.
        /// </summary>
		internal void Flush()
		{
			if (m_Active)
			{
				m_Plan.AddLast(new NewLineChunk(m_Text, m_Start, m_IndentLength, m_TextLength));
				m_Active = false;
			}
		}
        #endregion
    }
}
