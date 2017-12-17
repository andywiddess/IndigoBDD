using System;
using System.Text.RegularExpressions;

namespace Indigo.CrossCutting.Utilities.Text
{

    /// <summary>
    /// Lazy wildcard. It just a wildcard which is actually evaluated when it is first used.
    /// Additinally, when it does not contain any pattern ('*' or '?') it is not compiled
    /// as Regex and uses string comparision instead. It is used for performance reasons.
    /// </summary>
    public class LazyWildcard
    {
        #region const data members
        /// <summary>
        /// Empty pattern.
        /// </summary>
        public static readonly LazyWildcard Empty = new LazyWildcard(string.Empty, false);
        #endregion

        #region private data members
        /// <summary>
        /// Pattern text.
        /// </summary>
        private string m_Text;

        /// <summary>
        /// Text used when pattern does not contain pattern specific characters (like '*' and '?')
        /// </summary>
        private string m_TextForCompare;

        /// <summary>
        /// Indicates if comparision is case insensitive.
        /// </summary>
        private bool m_IgnoreCase;

        /// <summary>
        /// Compiled wildcard.
        /// </summary>
        private Wildcard m_Wildcard;

        /// <summary>
        /// Determines if pattern is a wildcard.
        /// </summary>
        private bool? m_IsWildcard = null;
        #endregion

        #region properties
        /// <summary>
        /// Gets the pattern.
        /// </summary>
        /// <value>The text.</value>
        public string Text => m_Text;

        /// <summary>
        /// Gets a value indicating whether case is ignored.
        /// </summary>
        /// <value><c>true</c> if case is ignored; otherwise, <c>false</c>.</value>
        public bool IgnoreCase => m_IgnoreCase;

        /// <summary>
        /// Gets a value indicating whether pattern contains wildcards.
        /// </summary>
        /// <value><c>true</c> if pattern contains wildcards; otherwise, <c>false</c>.</value>
        public bool IsWildcard
        {
            get
            {
                if (!m_IsWildcard.HasValue)
                {
                    m_IsWildcard = Wildcard.IsPattern(m_Text);
                }
                return m_IsWildcard.Value;
            }
        }

        /// <summary>
        /// Gets the compiled wildcard.
        /// </summary>
        /// <value>The wildcard.</value>
        public Wildcard Wildcard => m_Wildcard ?? (m_Wildcard = new Wildcard(m_Text, m_IgnoreCase));

        /// <summary>
        /// Gets the text for compare.
        /// </summary>
        /// <value>The text for compare.</value>
        private string TextForCompare => m_TextForCompare ?? (m_TextForCompare = m_IgnoreCase ? m_Text.ToLower() : m_Text);

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyWildcard"/> class.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="ignoreCase">if set to <c>true</c> case is ignored.</param>
        public LazyWildcard(string pattern, bool ignoreCase)
        {
            m_IgnoreCase = ignoreCase;
            m_Text = pattern ?? throw new ArgumentNullException("Argument 'pattern' cannot be null");
        }
        #endregion

        #region method Match
        /// <summary>
        /// Matches the specified text to a pattern.
        /// </summary>
        /// <param name="other">The text to match.</param>
        /// <returns><c>true</c> if text matches pattern</returns>
        public bool Match(string other)
        {
            if (IsWildcard)
            {
                return Wildcard.Match(other).Success;
            }

            return string.Compare(m_Text, other, m_IgnoreCase) == 0;
        }
        #endregion

        #region method GetHashCode, Equals
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        ///
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return TextForCompare.GetHashCode() ^ m_IgnoreCase.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current
        /// <see cref="T:System.Object"/>.
        /// </summary>
        ///
        /// <exception cref="T:System.NullReferenceException">  The <paramref name="obj"/> parameter is
        ///                                                     null. </exception>
        ///
        /// <param name="obj">  The <see cref="T:System.Object"/> to compare with the current
        ///                     <see cref="T:System.Object"/>. </param>
        ///
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current
        /// <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
		public override bool Equals(object obj)
        {
            if (obj == this) return true;
            if (!(obj is LazyWildcard other)) return false;
            return
                    (m_IgnoreCase == other.m_IgnoreCase) &&
                    (TextForCompare == other.TextForCompare);
        }
        #endregion
    }
}
