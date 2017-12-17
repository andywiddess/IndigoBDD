using System;

using Indigo.CrossCutting.Utilities.Text;

namespace Indigo.CrossCutting.Utilities.Text.Support
{
	internal class ResolvedChunk: AbstractChunk
	{
		private readonly string m_Name;
		private readonly string m_Format;

        /// <summary>
        /// Gets a value indicating whether this chunk is multiline.
        /// </summary>
        ///
        /// <value>
        /// <c>true</c> if this instance is multiline; otherwise, <c>false</c>.
        /// </value>
		public override bool IsMultiline
		{
			get { return false; }
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="name">     The name. </param>
        /// <param name="format">   Describes the format to use. </param>
		public ResolvedChunk(string name, string format)
		{
			m_Name = name;
			m_Format = format;
		}

        /// <summary>
        /// Gets formatted to string.
        /// </summary>
        ///
        /// <param name="value">    The value. </param>
        /// <param name="format">   Describes the format to use. </param>
        ///
        /// <returns>
        /// The formatted to string.
        /// </returns>
		private static string GetFormattedToString(object value, string format) => value.ToString(); // IMP:MAK implement me better

        /// <summary>
        /// Resolves (expands) the chunk to specified context.
        /// </summary>
        ///
        /// <param name="context">  The context. </param>
        public override void Resolve(ResolveContext context)
		{
			object value;
			var resolver = context.Resolver;

			try
			{
				value = resolver(m_Name);
			}
			catch (Exception e)
			{
				value = e; // it should be a TemplateStringCommand encapsulating exceptions
			}

			if (value == null)
			{
				// add macro back literally, may be resolved later
				context.Add(this);
				// ...and mark it as 'still expandable'
				context.Expandable = true;
				// Ok people. There's nothing to see here. Everybody move along.
				return;
			}

			if (value is TemplateString)
			{
				var inner = (TemplateString)value;
				InsertTemplate(context, inner);
				return;
			}

			string text = value as string;

			if (text == null)
			{
				Command command;
				Func<string> simpleGenerator;
				Func<string, string, string> advancedGenerator;

				if ((command = value as Command) != null)
				{
					text = command.Execute(context, m_Name, m_Format);
				}
				else if ((simpleGenerator = value as Func<string>) != null)
				{
					text = simpleGenerator();
				}
				else if ((advancedGenerator = value as Func<string, string, string>) != null)
				{
					text = advancedGenerator(m_Name, m_Format);
				}
				else
				{
					// if .ToString() throws an exception we don't want to handle it anymore, sorry
					// just let the exception crash the application (or be cached outside) 
					// simple generic string template cannot deal with all problems in the world
					text = m_Format == null ? value.ToString() : GetFormattedToString(value, m_Format);
				}
			}

			if (text != null)
			{
				if (TemplateString.IsExpandableString(text))
				{
					InsertTemplate(context, TemplateString.Create(text));
				}
				else
				{
					InsertText(context, text);
				}
			}
		}

        /// <summary>
        /// Inserts a template.
        /// </summary>
        ///
        /// <param name="context">  The context. </param>
        /// <param name="inner">    The inner. </param>
		private static void InsertTemplate(ResolveContext context, TemplateString inner)
		{
			var plan = inner.Plan; // in both cases plan is needed
			var expandable = inner.IsExpandable;

			if (expandable)
			{
				// this needs to be resolved first to get real (not level 1) value of 'IsMultiline'
				inner = inner.Resolve(context.Resolver);

				// get new plan after expansion
				plan = inner.Plan;
			}

			// ...so this values will be evaluated
			var multiline = inner.IsMultiline;

			if (multiline)
			{
				context.Add(IndentChunk.Default);
			}

			if (expandable)
			{
				context.Expandable = true;
			}

			context.AddMany(plan);

			if (multiline)
			{
				context.Add(UnindentChunk.Default);
			}
		}

        /// <summary>
        /// Inserts a text.
        /// </summary>
        ///
        /// <param name="context">  The context. </param>
        /// <param name="text">     The text. </param>
		private static void InsertText(ResolveContext context, string text)
		{
			int start = 0;
			int length = text.Length;
			var multiline = false;
			var first = true;

			while (start < length)
			{
				int index = text.IndexOf('\n', start);

				if (index < 0)
				{
					index = length - 1;
				}
				else if (first)
				{
					context.Add(IndentChunk.Default);
					multiline = true;
					first = false;
				}

				context.Add(new TextChunk(text, start, index - start + 1));

				start = index + 1;
			}

			if (multiline)
			{
				context.Add(UnindentChunk.Default);
			}
		}

        /// <summary>
        /// Produces (converts chunk to string) the chunk to specified context.
        /// </summary>
        ///
        /// <param name="context">  The context. </param>
		public override void Produce(ProduceContext context)
		{
			var result = context.Result;

			result.Append("$(").Append(m_Name);
			if (m_Format != null)
			{
				result.Append(':').Append(m_Format);
			}
			result.Append(")");
		}

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        ///
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
		public override string ToString()
		{
			return string.Format(
				"{0}($({1}{2}{3}))",
				GetType().Name,
				m_Name,
				m_Format == null ? string.Empty : ":",
				m_Format ?? string.Empty);
		}
	}
}
