using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Indigo.CrossCutting.Utilities.Text
{
	/// <summary>
	/// String which may contain expandable macros, like: <c>$(appfolder)\data\filename.ext</c>
	/// </summary>
	public class MacroString
	{
		#region data members

		private IDictionary<string, object> m_Dictionary = new Dictionary<string, object>();
		private string m_Template = string.Empty;

		private const string m_MacroHead = "$(";
		private const string m_MacroTail = ")";

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the dictionary. Dictionary contains string,object pair which
		/// are macro name and macro value.
		/// </summary>
		/// <value>The dictionary.</value>
		public IDictionary<string, object> Dictionary
		{
			get => m_Dictionary;
            set => m_Dictionary = value;
        }

		/// <summary>
		/// Gets or sets the template string.
		/// </summary>
		/// <value>The template.</value>
		public string Template
		{
			get => m_Template;
		    set => m_Template = value;
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MacroString"/> class.
		/// </summary>
		public MacroString()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MacroString"/> class with given template string.
		/// </summary>
		/// <param name="tempate">The tempate.</param>
		public MacroString(string tempate)
		{
			m_Template = tempate;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MacroString"/> class with given template string and macro dictionary.
		/// </summary>
		/// <param name="template">The template.</param>
		/// <param name="dictionary">The dictionary.</param>
		public MacroString(string template, IDictionary<string, object> dictionary)
		{
			m_Template = template;
			m_Dictionary = dictionary;
		}

		#endregion

		#region method Union

        /// <summary>
        /// Unions.
        /// </summary>
        ///
        /// <param name="appendTo">     The append to. </param>
        /// <param name="appendFrom">   The append from. </param>
        /// <param name="overwrite">    if set to <c>true</c> existing keys will be overwritten. </param>
        ///
        /// <returns>
        /// An IDictionary&lt;string,object&gt;
        /// </returns>
		private static IDictionary<string, object> Union(
				IDictionary<string, object> appendTo, IDictionary<string, object> appendFrom,
				bool overwrite)
		{
			foreach (var kv in appendFrom)
			{
				if ((!overwrite) && (appendTo.ContainsKey(kv.Key)))
				{
					continue;
				}
				appendTo[kv.Key] = kv.Value;
			}
			return appendTo;
		}

		#endregion

		#region method Add & Remove

		/// <summary>
		/// Adds new macro. If macro already exists, old value is replaced.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>this. You can chain multiple operations like: <c>s.Remove(...).Add(...).Expand(...);</c></returns>
		public MacroString Add(string key, object value)
		{
			m_Dictionary[key] = value;
			return this;
		}

		/// <summary>
		/// Removes the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>this. You can chain multiple operations like: <c>s.Remove(...).Add(...).Expand(...);</c></returns>
		public MacroString Remove(string key)
		{
			m_Dictionary.Remove(key);
			return this;
		}

		#endregion

		#region method Append

		/// <summary>
		/// Appends the specified dictionary. If specific key already exists old value is preserved.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <returns>this. You can chain multiple operations like: <c>s.Remove(...).Add(...).Expand(...);</c></returns>
		public MacroString Append(IDictionary<string, object> dictionary)
		{
			return Append(dictionary, true);
		}


		/// <summary>
		/// Appends the specified dictionary. If specific key already exists you can choose 
		/// if you want to preserve original value or overwrite value.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="overwrite">if set to <c>true</c> existing keys will be overwritten.</param>
		/// <returns>this. You can chain multiple operations like: <c>s.Remove(...).Add(...).Expand(...);</c></returns>
		public MacroString Append(IDictionary<string, object> dictionary, bool overwrite)
		{
			Union(m_Dictionary, dictionary, overwrite);
			return this;
		}

		#endregion

		#region method Expand

		/// <summary>
		/// Expands the specified template with given dictionary.
		/// Note: This method is not optimized to reduce amount of calls
		/// to dictionary values. So, for every dictionary value you can
		/// expect many calls of <c>ToString()</c> method.
		/// </summary>
		/// <param name="template">The template.</param>
		/// <param name="dictionary">The dictionary.</param>
		/// <returns>Expanded string.</returns>
		public static string Expand(string template, IDictionary<string, object> dictionary)
		{
			string oldvalue = template;
			string newvalue;

			bool changed = true;
			while (changed)
			{
				changed = false;
				foreach (var kv in dictionary)
				{
					string macro = string.Format("{0}{1}{2}", m_MacroHead, kv.Key, m_MacroTail);
					newvalue = oldvalue.Replace(macro, kv.Value.ToString());
					if (newvalue != oldvalue)
					{
						changed = true;
						oldvalue = newvalue;
						continue;
					}
				}
			}

			return oldvalue;
		}

        /// <summary>
        /// Expands the specified template with internal dictionary.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns>Expanded string.</returns>
        public string Expand(string template) => Expand(template, m_Dictionary);

        /// <summary>
        /// Expands the internal template with specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>Expanded string.</returns>
        public string Expand(IDictionary<string, object> dictionary) => Expand(m_Template, dictionary);

        /// <summary>
        /// Expands internal template with internal dictionary.
        /// </summary>
        /// <returns>Expanded string.</returns>
        public string Expand() => Expand(m_Template, m_Dictionary);

        #endregion

        #region CopyFromRegex

        /// <summary>Copies macros from regular expression.</summary>
        /// <param name="match">The match.</param>
        /// <param name="names">The names.</param>
        public void CopyFromRegex(Match match, params string[] names)
		{
			foreach (string name in names)
			{
				m_Dictionary[name] = match.Groups[name].Value;
			}
		}

        #endregion

        #region overrides, operators

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        ///
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() => Expand();

        /// <summary>
        /// Performs an implicit conversion from
        /// <see cref="Indigo.CrossCutting.Utilities.Text.MacroString"/> to
        /// <see cref="System.String"/>.
        /// </summary>
        ///
        /// <param name="macro">    The macro. </param>
        ///
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(MacroString macro) => macro?.Expand();

	    #endregion
	}
}
