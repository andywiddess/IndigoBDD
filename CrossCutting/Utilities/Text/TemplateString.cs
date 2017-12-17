using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Indigo.CrossCutting.Utilities.Collections;
using Indigo.CrossCutting.Utilities.Text.Support;

namespace Indigo.CrossCutting.Utilities.Text
{
    /// <summary>
    /// TemplateString allowing for <c>$(variable)</c> expansions which is more useful than
    /// <see cref="M:string.Format()"/>'s <c>{0}</c>
    /// There are two approaches <see cref="M:Templatestring.Resolve(string,object)"/> and
    /// <see cref="M:Templatestring.Expand(string,object)"/>. I couldn't find better names, sorry.
    /// The <c>Expand</c> does same job as <c>Resolve</c> but should be used for ad-hoc expansions.  
    /// Resolve compiles macro and caches it, ad-hoc replaces string in-place. For flat (not nested)
    /// templates used once ad-hoc expansion is twice as fast as compiled one, for templates used
    /// multiple times (for example, in loops) or nested the compiled ones are 4 times faster. This
    /// numbers depend heavily on machine you use, of course.
    /// </summary>
	public class TemplateString
	{
		#region static fields

		/// <summary>TemplateString cache.</summary>
		private static readonly Cache<string, TemplateString> m_Cache = new Cache<string, TemplateString>();

		/// <summary>Regular expression to find macros and indentiation.</summary>
		private readonly static Regex s_SplitRx = new Regex(
			@"
				(((?<=((\r?\n)|^))(?<indent>[ \t]*))?((\$\((?<name>[\w\._]+)(\:(?<format>[^)]+))?\))))|
				((?<=((\r?\n)|^))(?<indent>[ \t]*)$?)
			",
			RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);

		/// <summary>Regular expression to extract macros from quick template.</summary>
		private readonly static Regex s_QuickMacroRx = new Regex(
			@"\$\((?<name>[\w\._]+)\)",
			RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);

		/// <summary>Regular expression for find quickly if template is expandable.</summary>
		private readonly static Regex s_MacroRx = new Regex(
			@"\$\([\w\._]+(\:[^)]+)?\)",
			RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);

		/// <summary>Empty plan. Shared among empty templates.</summary>
		private readonly static LinkedList<AbstractChunk> s_EmptyPlan = new LinkedList<AbstractChunk>();

		/// <summary>Enables template caching. It might improve proformance a lot when templates are frequently reused.</summary>
		public static bool CacheTemplates = true;
		#endregion

		#region fields
		/// <summary>Template text.</summary>
		private string m_Text;

		/// <summary>Plan for expansion. Using LinkedList as it is much more light weight.</summary>
		private LinkedList<AbstractChunk> m_Plan; // = null;

		/// <summary>Indicates if template is expandable.</summary>
		private bool? m_IsExpandable;

		/// <summary>Indicates if template is multiline.</summary>
		private bool? m_IsMultiline;
        #endregion

        #region properties
        /// <summary>Gets the cache.</summary>
        internal static Cache<string, TemplateString> Cache => m_Cache;

        /// <summary>Gets not expanded template text.</summary>
        public string Text
		{
			get
			{
				if (m_Text == null)
				{
					CreateText();
				}
				return m_Text;
			}
		}

		/// <summary>Gets the template plan.</summary>
		internal IEnumerable<AbstractChunk> Plan
		{
			get
			{
				if (m_Plan == null)
				{
					CreatePlan();
				}

				return m_Plan;
			}
		}

		/// <summary>Gets a value indicating whether this instance is expandable. It gives definitive answer if
		/// <see cref="T:TemplateString"/> is expandable. It is more reliable then <see cref="P:IsSimpleString"/> but
		/// may require some processing.</summary>
		/// <value><c>true</c> if this instance is expandable; otherwise, <c>false</c>.</value>
		public bool IsExpandable
		{
			get
			{
				if (!m_IsExpandable.HasValue)
				{
					m_IsExpandable = CheckIsExpandable();
				}
				return m_IsExpandable.Value;
			}
		}

		/// <summary>Gets a value indicating whether this template is multiline.</summary>
		/// <value><c>true</c> if this instance is multiline; otherwise, <c>false</c>.</value>
		public bool IsMultiline
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

		#region constuctor
		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateString"/> class.
		/// Even if this constructor is public you should rather use <see cref="Create"/> method.
		/// It saves time if TemplateString is reused.
		/// </summary>
		/// <param name="text">The text.</param>
		public TemplateString(string text)
		{
			m_Text = text;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateString"/> class.
		/// </summary>
		/// <param name="plan">The plan.</param>
		protected TemplateString(LinkedList<AbstractChunk> plan)
		{
			m_Plan = plan;
		}

		/// <summary>Initializes a new instance of the <see cref="TemplateString"/> class using resolve context of another 
		/// template, used internal when result of <see cref="TemplateString"/>'s expansion is another TemplateString read to 
		/// expanded further.</summary>
		/// <param name="context">The context.</param>
		protected TemplateString(ResolveContext context)
		{
			m_Plan = context.Chunks;
			m_IsExpandable = context.Expandable;
		}

		/// <summary>Reparses the template. This should be used when new macros has been sneakly introduced by injecting 
		/// raw text into template</summary>
		/// <returns>New template.</returns>
		public TemplateString Reparse()
		{
			return TemplateString.Create(Text);
		}
		#endregion

		#region caching
		/// <summary>
		/// Creates template using given text or reuses one from cache, this method should be used istead of constructor.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>TemplateString object for given string.</returns>
		public static TemplateString Create(string text)
		{
			if (CacheTemplates)
			{
				return m_Cache.Get(text, TemplateStringFactory);
			}
			else
			{
				return new TemplateString(text);
			}
		}

		/// <summary>TemplateString factory. Used internaly.</summary>
		/// <param name="text">The text.</param>
		/// <returns>New Templatestring.</returns>
		private static TemplateString TemplateStringFactory(string text)
		{
			return new TemplateString(text);
		}
		#endregion

		#region plan
		private void CreateText()
		{
			if (m_Text != null)
			{
				return;
			}

			lock (this)
			{
				// two threads could enter this method, but at this point oth other one may have created the text already
				if (m_Text != null)
				{
					return;
				}

				int planLength = m_Plan.Count;

				if (planLength == 0)
				{
					m_Text = string.Empty;
				}
				else
				{
					var context = new ProduceContext();
					m_Plan.ForEach((chunk) => chunk.Produce(context));
					m_Text = context.Result.ToString();
				}
			}
		}

		/// <summary>Creates the plan for expansion.</summary>
		private void CreatePlan()
		{
			if (m_Plan != null)
			{
				return;
			}

			lock (this) // this doesn't look good, but in this case is required
			{
				// 1. two threads could enter this method, but at this point other one may have created the plan already
				if (m_Plan != null)
				{
					return;
				}

				// 2. No plan for empty strings (easy one)
				if (string.IsNullOrEmpty(m_Text))
				{
					m_Plan = s_EmptyPlan;
				}

				var m = s_SplitRx.Match(m_Text);
				int startIndex = 0;
				int foundIndex = 0;
				int textLength = m_Text.Length;

				var plan = new LinkedList<AbstractChunk>();
				var lastNewLine = new NewLinePlanHelper(m_Text, plan);

				var multiline = false;
				var expandable = false;

				int chunkLength;
				while (true)
				{
					// 'eof' is the last implicit chunk, so even if regex failed we have one more...
					foundIndex = m.Success ? m.Index : textLength;

					// append text
					chunkLength = foundIndex - startIndex;
					if (chunkLength > 0)
					{
						if (m_Text[foundIndex - 1] == '\n')
						{
							multiline = true;
						}

						if (lastNewLine.IsActive)
						{
							lastNewLine.Expand(startIndex, chunkLength);
						}
						else
						{
							plan.AddLast(new TextChunk(m_Text, startIndex, chunkLength));
						}
					}

					// flush new line info
					lastNewLine.Flush();

					// now you can finish if it was the last chunk (eof)
					if (foundIndex == textLength)
					{
						break;
					}

					// append line indentation
					var indentGroup = m.Groups["indent"];
					if (indentGroup.Success)
					{
						lastNewLine.Reset(indentGroup.Index, indentGroup.Length);
					}

					// append macro
					var nameGroup = m.Groups["name"];
					if (nameGroup.Success)
					{
						// flush new line info
						lastNewLine.Flush();
						var formatGroup = m.Groups["format"];
						plan.AddLast(new ResolvedChunk(nameGroup.Value, formatGroup.Success ? formatGroup.Value : null));
						expandable = true;
					}

					startIndex = foundIndex + m.Length;
					m = m.NextMatch();
				}

				m_IsMultiline = multiline;
				m_IsExpandable = expandable;
				m_Plan = plan;
			}
		}
		#endregion

		#region macro resolvers
        /// <summary>
        /// Dictionary resolver.
        /// </summary>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="data"> The dictionary with data. </param>
        /// <param name="name"> The name. </param>
        ///
        /// <returns>
        /// An object.
        /// </returns>
		private static object DictionaryResolver<T>(IDictionary<string, T> data, string name)
		{
			T value;
			if (data.TryGetValue(name, out value))
			{
				return value;
			}
			return null;
		}

        /// <summary>
        /// Object resolver.
        /// </summary>
        ///
        /// <param name="data"> The dictionary with data. </param>
        /// <param name="name"> The name. </param>
        ///
        /// <returns>
        /// An object.
        /// </returns>
		private static object ObjectResolver(object data, string name)
		{
			var type = data.GetType();

			var property = type.GetProperty(name);
			if (property != null)
			{
				return property.GetValue(data, null);
			}

			var field = type.GetField(name);
			return field != null ? field.GetValue(data) : null;
		}
		#endregion

		#region utilities
		internal static bool IsExpandableString(string text)
		{
			// quick test. 
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}

			// IMP:MAK performance may be improved
			// that's questionable from performance point of view
			// with lot of nested expansions this will be slower, faster without them
			// making very acurate test also helps reduce cache load
			return s_MacroRx.Match(text).Success;
		}

        /// <summary>
        /// Determines if we can check is multiline.
        /// </summary>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
		private bool CheckIsMultiline()
		{
			if (m_Plan != null)
			{
				return m_Plan.Any((c) => c.IsMultiline);
			}
			else
			{
				return m_Text.LastIndexOf('\n') >= 0;
			}
		}

        /// <summary>
        /// Determines if we can check is expandable.
        /// </summary>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
		private bool CheckIsExpandable()
        {
            return m_Plan != null ? Plan.Any((c) => c is ResolvedChunk) : s_MacroRx.Match(m_Text).Success;
        }
		#endregion

		#region resolving

		/// <summary>Expands the template with specified resolver.</summary>
		/// <param name="resolver">The resolver.</param>
		/// <returns>Expanded Templatestring.</returns>
		public TemplateString Resolve(Func<string, object> resolver)
		{
			if (m_IsExpandable.GetValueOrDefault(true))
			{
				var context = new ResolveContext(resolver);
				Plan.ForEach(chunk => chunk.Resolve(context));
				return new TemplateString(context);
			}
			else
			{
				return this;
			}
		}

        /// <summary>
        /// Expands the template with specified dictionary. Uses keys as macro names, and values as macro
        /// values.
        /// </summary>
        ///
        /// <typeparam name="T">    Type of dictionary item. </typeparam>
        /// <param name="data"> The dictionary with data. </param>
        ///
        /// <returns>
        /// Expanded Templatestring.
        /// </returns>
        public TemplateString Resolve<T>(IDictionary<string, T> data) => Resolve((name) => DictionaryResolver<T>(data, name));

        /// <summary>
        /// Expands the template with specified data object. Uses it's properties and fields as macro
        /// names.
        /// </summary>
        ///
        /// <param name="data"> The data. </param>
        ///
        /// <returns>
        /// Expanded Templatestring.
        /// </returns>
        public TemplateString Resolve(object data) => Resolve((name) => ObjectResolver(data, name));
	    #endregion

		#region to and from string conversion
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to
        /// <see cref="Indigo.CrossCutting.Utilities.Text.TemplateString"/>. Creates
        /// new template string.
        /// </summary>
        ///
        /// <param name="template"> The template. </param>
        ///
        /// <returns>
        /// The result of the conversion.
        /// </returns>
		public static implicit operator TemplateString(string template)
		{
			return Create(template);
		}

        /// <summary>
        /// Performs an implicit conversion from
        /// <see cref="Indigo.CrossCutting.Utilities.Text.TemplateString"/>
        /// to <see cref="System.String"/>. Returns unexpanded template as string.
        /// </summary>
        ///
        /// <param name="template"> The template. </param>
        ///
        /// <returns>
        /// The result of the conversion.
        /// </returns>
		public static implicit operator string(TemplateString template)
		{
			return template.Text;
		}

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        ///
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
		public override string ToString()
		{
			return Text;
		}

        /// <summary>
        /// Implements the operator +. Adds two templates.
        /// </summary>
        ///
        /// <param name="one">  The one. </param>
        /// <param name="two">  The two. </param>
        ///
        /// <returns>
        /// The result of the operator.
        /// </returns>
		public static TemplateString operator +(TemplateString one, TemplateString two)
		{
			var result = new LinkedList<AbstractChunk>();
			one.Plan.ForEach((c) => result.AddLast(c));
			two.Plan.ForEach((c) => result.AddLast(c));
			return new TemplateString(result);
		}
        #endregion

        #region Expand
        // some performance results (this were done on slow PC, faster PC favors compiled version more)
        //
        // SimpleExpansionRaceTest:
        //   Resolve: 3729.2133ms
        //   Resolve (cached): 1500.2897ms
        //   Expand: 2486.5376ms
        //
        // ComplexExpansionRaceTest:
        //   Resolve: 627.0269ms
        //   Resolve (cached): 258.3941ms
        //   Expand: 583.6748ms

        /// <summary>
        /// Resolves the string. It uses TemplateString without compilation. It's slower when used in
        /// loops (TemplateString object can reuse compiled plan) but faster for ad hoc, one time
        /// expansions.
        /// </summary>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="input">    The input. </param>
        /// <param name="data">     The data used for expansion. </param>
        ///
        /// <returns>
        /// Expanded string.
        /// </returns>
        public static string Expand<T>(string input, IDictionary<string, T> data) => Expand(input, (name) => DictionaryResolver(data, name));

        /// <summary>
        /// Resolves the string. It uses TemplateString without compilation. It's slower when used in looks (TemplateString
        /// object can reuse compiled plan) but faster for ad hoc, one time expansions.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="data">The data used for expansion.</param>
        /// <returns>Expanded string.</returns>
        public static string Expand(string input, object data) => Expand(input, (name) => ObjectResolver(data, name));

        /// <summary>
        /// Resolves the string. It uses TemplateString without compilation. It's slower when used in looks (TemplateString
        /// object can reuse compiled plan) but faster for ad hoc, one time expansions.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns>Expanded string.</returns>
        public static string Expand(string input, Func<string, object> resolver)
		{
			string text = input;
			StringBuilder result = null;

			var m = s_QuickMacroRx.Match(text);
			var startIndex = 0;

			while (m.Success)
			{
				if (result == null)
					result = new StringBuilder();

				result.Append(input, startIndex, m.Index - startIndex);
				string name = m.Groups["name"].Value;
				object value = resolver(name);

				if (value != null)
				{
					result.Append(
						FixIndent(
							Expand(value.ToString(), resolver),
							input, m.Index, m.Length));
				}
				else
				{
					result.Append(m.Value);
				}

				startIndex = m.Index + m.Length;

				m = m.NextMatch();
			}

			if (result == null)
			{
				return input;
			}
			else
			{
				result.Append(input, startIndex, input.Length - startIndex);
				return result.ToString();
			}
		}

		/// <summary>Fixes the indent in expanded macro.</summary>
		/// <param name="text">The text.</param>
		/// <param name="input">The input.</param>
		/// <param name="inputIndex">Index of the input.</param>
		/// <param name="inputLength">Length of the input.</param>
		/// <returns>Indented expansion.</returns>
		private static string FixIndent(string text, string input, int inputIndex, int inputLength)
		{
			// this can be optimized, but for now... it just works...
			text = Indent.NormalizeNewLine(text, false, false);
			if (text.IndexOf('\n') < 0) return text;

			int headIndex = Indent.ScanForLineStart(input, inputIndex);
			// int tailIndex = Indent.ScanForLineEnd(input, inputIndex + inputLength);

			string head = input.Substring(headIndex, inputIndex - headIndex);

			// string tail = input.Substring(inputIndex + inputLength, tailIndex - inputIndex - inputLength + 1);
			bool whiteHead = Indent.IsEmptyLine(head);

			string indent =
				whiteHead
				? head
				: Indent.FindIndent(head);

			var result = new StringBuilder();

			bool first = true;

			foreach (string line in Indent.ExtractLines(text))
			{
				result.Append(first ? line : Indent.IdentLine(indent, line));
				first = false;
			}

			return result.ToString();
		}
		#endregion
	}
}
