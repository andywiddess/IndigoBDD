using System;
using System.Text.RegularExpressions;

namespace Indigo.CrossCutting.Utilities.Text
{
	/// <summary>
	/// Represents a wildcard running on the
	/// <see cref="System.Text.RegularExpressions"/> engine.
	/// </summary>
	public class Wildcard
        : Regex
	{
		#region constructors
		/// <summary>
		/// Initializes a wildcard with the given search pattern.
		/// </summary>
		/// <param name="pattern">The wildcard pattern to match.</param>
		public Wildcard(string pattern)
			: base(WildcardToRegex(pattern))
		{
		}

		/// <summary>
		/// Initializes a wildcard with the given search pattern and options.
		/// </summary>
		/// <param name="pattern">The wildcard pattern to match.</param>
		/// <param name="options">A combination of one or more
		/// <see cref="RegexOptions"/>.</param>
		public Wildcard(string pattern, RegexOptions options)
			: base(WildcardToRegex(pattern), options)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wildcard"/> class.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <param name="ignoreCase">if set to <c>true</c> character case is ignored.</param>
		public Wildcard(string pattern, bool ignoreCase)
			: base(WildcardToRegex(pattern), ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None)
		{
		}
		#endregion

		#region method WildcardToRegex
		/// <summary>
		/// Converts a wildcard pattern to a regex pattern.
		/// </summary>
		/// <param name="pattern">The wildcard pattern to convert.</param>
		/// <returns>A regex equivalent of the given wildcard.</returns>
		public static string WildcardToRegex(string pattern) => "^" + Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
	    #endregion

        #region method IsMatch
        /// <summary>
        /// Checks if input matches given pattern
        /// </summary>
        /// <param name="input">input string</param>
        /// <param name="pattern">given pattern</param>
        /// <returns><c>true</c> if the specified input matches pattern; otherwise, <c>false</c>.</returns>
        public new static bool IsMatch(string input, string pattern) => (new Wildcard(pattern)).IsMatch(input);

        /// <summary>
        /// Determines whether the specified input is matching pattern.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="ignoreCase">if set to <c>true</c> character case is ignored.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is match; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMatch(string input, string pattern, bool ignoreCase) => (new Wildcard(pattern, ignoreCase: ignoreCase)).IsMatch(input);
	    #endregion

		#region method IsPattern, IsPatternAny
		/// <summary>
		/// Determines whether the specified text is a pattern.
		/// </summary>
		/// <param name="pattern">The potential pattern.</param>
		/// <returns><c>true</c> if the specified text is pattern; otherwise, <c>false</c>.</returns>
		public static bool IsPattern(string pattern)
		{
			if (pattern == null)
				throw new ArgumentNullException("pattern");

			var length = pattern.Length;
			for (var i = 0; i < length; i++)
			{
				var c = pattern[i];
				if ((c == '*') || (c == '?')) return true;
			}
			return false;
		}

		/// <summary>
		/// Determines whether specified text is a any-pattern ('*')
		/// </summary>
		/// <param name="pattern">The potential pattern.</param>
		/// <returns><c>true</c> if test is '*' pattern; otherwise, <c>false</c>.</returns>
		public static bool IsPatternAny(string pattern)
		{
			if (pattern == null)
				throw new ArgumentNullException("pattern");

			var length = pattern.Length;
			for (var i = 0; i < length; i++)
			{
				if (pattern[i] != '*') return false;
			}
			return true;
		}
		#endregion
	}
}