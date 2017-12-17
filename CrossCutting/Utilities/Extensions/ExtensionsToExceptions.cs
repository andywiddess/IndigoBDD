using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	/// <summary>
	/// Extension methods for the Exception class
	/// </summary>
	public static class ExtensionsToExceptions
	{
		/// <summary>
		/// Checks to see if any of the inner exceptions are a specific type.
		/// This will recursively call until it reaches the bottom of the exception inner stack.
		/// </summary>
		/// <typeparam name="T">The type of exception to find</typeparam>
		/// <param name="topException">The top exception.</param>
		/// <returns>
		///   <c>True</c> if any of the inner exceptions are of type T
		/// </returns>
		public static bool AnyInnerExceptionIs<T>(this Exception topException) where T: Exception
		{
			Exception checkEx = topException;
			bool isRequestedType = false;
			while (checkEx != null)
			{
				if (checkEx is T)
				{
					// We've found what we want, break and return
					isRequestedType = true;
					break;
				}
				checkEx = checkEx.InnerException;
			}

			return isRequestedType;
		}

		/// <summary>
		/// Finds the first inner exception of a specific type from an exception stack
		/// </summary>
		/// <typeparam name="T">The type of exception to findInnerException</typeparam>
		/// <param name="topException">The top exception in the exception stack</param>
		/// <returns>
		/// The exception of type <c>T</c> or <c>null</c> if no exceptions of
		/// type <c>T</c> exist in the exception stack.
		/// </returns>
		public static T FindInnerException<T>(this Exception topException) where T: Exception
		{
			Exception checkEx = topException;
			T exceptionOfType = null;
			while (checkEx != null)
			{
				if (checkEx is T)
				{
					// We've found what we want, break and return
					exceptionOfType = (T)checkEx;
					break;
				}
				checkEx = checkEx.InnerException;
			}

			return exceptionOfType;
		}

		/// <summary>
		/// Return the root exception for a given exception stack
		/// </summary>
		/// <param name="topException">The top exception in the stack</param>
		/// <returns>The root exception</returns>
		public static Exception RootException(this Exception topException)
		{
			Exception currEx = topException;
			while (currEx.InnerException != null)
			{
				currEx = currEx.InnerException;
			}

			return currEx;
		}

		/// <summary>
		/// Fetch all of the Inner Exceptions
		/// </summary>
		/// <param name="topException">The top exception.</param>
		/// <returns></returns>
		public static IEnumerable<Exception> InnerExceptions(this Exception topException)
		{
			yield return topException;

			Exception currEx = topException;
			while (currEx.InnerException != null)
			{
				yield return currEx;
				currEx = currEx.InnerException;
			}
		}

		/// <summary>
		/// Strips the aggregate exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns></returns>
		public static Exception StripAggregateException(this Exception exception)
		{
			if (exception == null)
				return null;
			var aggr = exception as AggregateException;
			if (aggr != null && aggr.InnerExceptions.Count == 1)
				return StripAggregateException(aggr.InnerExceptions.First());
			return exception;
		}

		#region Explain

		/// <summary>Explains the specified exception.</summary>
		/// <param name="exception">The exception.</param>
		/// <param name="stackTrace">if set to <c>true</c> stack trace will be included.</param>
		/// <param name="recursive">if set to <c>true</c> it recursively explain nested exceptions.</param>
		/// <returns>Multiline string explaining exception.</returns>
		public static string Explain(this Exception exception, bool stackTrace = true, bool recursive = true)
		{
			return Explain(exception, stackTrace, recursive, string.Empty, string.Empty);
		}

		/// <summary>Utility function trimming every single line.</summary>
		/// <param name="lines">The lines.</param>
		/// <returns>Trimmed block of text.</returns>
		private static string ExplainTrimLines(string lines)
		{
			return lines.Split('\n').Select(l => l.Trim()).Join("\n");
		}

		/// <summary>Utility function indenting every single line.</summary>
		/// <param name="lines">The lines.</param>
		/// <param name="indent">The indent.</param>
		/// <returns>Indented block of text.</returns>
		private static string ExplainIndentLines(string lines, string indent)
		{
			if (lines.IndexOf('\n') < 0)
			{
				return lines;
			}
			else
			{
				var text = lines.Split('\n')
					.Select(l => string.Format("{0}  {1}", indent, l))
					.Join("\n");
				return string.Format("\n{0}", text);
			}
		}

		/// <summary>Explains the specified exception.</summary>
		/// <param name="exception">The exception.</param>
		/// <param name="stackTrace">if set to <c>true</c> stack trace will be included.</param>
		/// <param name="recursive">if set to <c>true</c> it recursively explain nested exceptions.</param>
		/// <param name="indent">The indent.</param>
		/// <param name="prefix">The prefix (for inner exception references).</param>
		/// <returns>Multiline string explaining exception.</returns>
		private static string Explain(
			Exception exception, 
			bool stackTrace, bool recursive, 
			string indent, string prefix)
		{
			if (exception == null)
			{
				return string.Format("{0}No exception", indent);
			}

			try
			{
				Func<string, string> trimmed = (lines) => ExplainTrimLines(lines);
				Func<string, string> indented = (lines) => ExplainIndentLines(lines, indent);

				indent = indent.NotNull();
				var result = new StringBuilder();
				result.AppendFormat("{0}Exception: {1}", indent, exception.GetType().Name).AppendLine();
				result.AppendFormat("{0}Message: {1}", indent, indented(exception.Message.NotEmpty("-"))).AppendLine();

				if (stackTrace)
				{
					result.AppendFormat("{0}Trace: {1}", indent, indented(trimmed(exception.StackTrace.NotEmpty("-")))).AppendLine();
				}

				if (recursive)
				{
					IEnumerable<Exception> innerExceptions = null;
					var aggregate = exception as AggregateException;
					if (aggregate != null && aggregate.InnerExceptions.Count > 1)
					{
						innerExceptions = aggregate.InnerExceptions;
					}
					else
					{
						innerExceptions = new[] { exception.InnerException };
					}

					innerExceptions = innerExceptions.Where(e => e != null);

					int index = 1;
					foreach (var innerException in innerExceptions)
					{
						result
							.AppendFormat(
								"{0}Inner exception #{1}{2}:\n{3}",
								indent,
								prefix, index,
								Explain(
									innerException,
									stackTrace, recursive,
									indent + "  ", string.Format("{0}{1}.", prefix, index)))
							.AppendLine();
						index++;
					}
				}

				return result.ToString().Trim('\n', '\r');
			}
			catch (Exception)
			{
				return string.Format("{0}Unknown exception", indent);
			}
		}

		#endregion

		/// <summary>Rethrows the specified exception 
		/// preserving original stack trace.</summary>
		/// <param name="exception">The exception.</param>
		public static void Rethrow(this Exception exception)
		{
			if (exception == null)
				return;

			typeof(Exception).GetMethod("PrepForRemoting",
				BindingFlags.NonPublic | BindingFlags.Instance)
				.Invoke(exception, new object[0]);

			throw exception;
		}

		/// <summary>Rethrows the specified exception. 
		/// Syntactically allowed to be used as expression (although it will never 
		/// return anything).</summary>
		/// <param name="exception">The exception.</param>
		public static T Rethrow<T>(this Exception exception)
		{
			exception.Rethrow();
			return default(T);
		}
	}
}
