using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Indigo.CrossCutting.Utilities.Extensions;
using Indigo.CrossCutting.Utilities.Text;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// Utilities to handle Xml files.
	/// </summary>
	public class XmlUtilities
	{
		/// <summary>
		/// Serializes object to specified stream.
		/// </summary>
		/// <typeparam name="T">Any type (have to me XmlSerializable).</typeparam>
		/// <param name="stream">The stream.</param>
		/// <param name="subject">The subject.</param>
		public static void Save<T>(Stream stream, T subject)
		{
			var serializer = new XmlSerializer(typeof(T));
			var writer = new XmlTextWriter(stream.Isolate(), Encoding.UTF8) 
			{
				Formatting = Formatting.Indented, 
				Indentation = 1, 
				IndentChar = '\t' 
			};
			using (writer)
			{
				serializer.Serialize(writer, subject);
			}
		}

		/// <summary>
		/// Serializes object to specified file.
		/// </summary>
		/// <typeparam name="T">Any type (have to me XmlSerializable).</typeparam>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="subject">The subject.</param>
		public static void Save<T>(string fileName, T subject)
		{
			using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				Save<T>(stream, subject);
			}
		}

		/// <summary>
		/// Serializes object to specified file. It does indent CDATA sections (to make it look better). 
		/// Use it with care, it requires indent-insensitive data in CDATA 
		/// sections (like SQL queries, for example).
		/// </summary>
		/// <typeparam name="T">Any type (have to me XmlSerializable).</typeparam>
		/// <param name="stream">The stream.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="indent">The indentation of CDATA fields.</param>
		public static void Save<T>(Stream stream, T subject, string indent)
		{
			using (MemoryStream buffer = new MemoryStream())
			{
				Save<T>(buffer, subject);
				buffer.Position = 0;

				using (StreamReader reader = new StreamReader(buffer))
				using (StreamWriter writer = new StreamWriter(stream, reader.CurrentEncoding))
				{
					IndentCData(reader, writer, indent);
				}
			}
		}

		/// <summary>
		/// Serializes object to specified file. It does indent CDATA sections (to make it look better).
		/// Use it with care, it requires indent-insensitive data in CDATA
		/// sections (like SQL queries, for example).
		/// </summary>
		/// <typeparam name="T">Any type (have to me XmlSerializable).</typeparam>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="cdataIndent">The indentation of CDATA fields.</param>
		public static void Save<T>(string fileName, T subject, string cdataIndent)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				Save<T>(stream, subject, cdataIndent);
			}
		}

		/// <summary>
		/// Deserializes object from specified reader.
		/// </summary>
		/// <typeparam name="T">Any type (have to me XmlSerializable).</typeparam>
		/// <param name="reader">The reader.</param>
		/// <returns>Deserialized object.</returns>
		public static T Load<T>(XmlReader reader)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			return (T)serializer.Deserialize(reader);
		}

		/// <summary>
		/// Deserializes object from specified stream.
		/// </summary>
		/// <typeparam name="T">Any type (have to me XmlSerializable).</typeparam>
		/// <param name="stream">The stream.</param>
		/// <returns>Deserialized object.</returns>
		public static T Load<T>(Stream stream)
		{
			using (XmlTextReader reader = new XmlTextReader(stream))
			{
				return Load<T>(reader);
			}
		}

		/// <summary>
		/// Deserializes object from specified document.
		/// </summary>
		/// <typeparam name="T">Any type (have to me XmlSerializable).</typeparam>
		/// <param name="document">The document.</param>
		/// <returns>Deserialized object.</returns>
		public static T Load<T>(XmlDocument document)
		{
			return Load<T>(new XmlNodeReader(document));
		}

		/// <summary>
		/// Deserializes object from specified file.
		/// </summary>
		/// <typeparam name="T">Any type (have to me XmlSerializable).</typeparam>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>Deserialized object.</returns>
		public static T Load<T>(string fileName)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				return Load<T>(stream);
			}
		}

		/// <summary>Loads object from XML or creates new.</summary>
		/// <typeparam name="T">Object to be loaded.</typeparam>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>Loaded or created object.</returns>
		public static T LoadOrCreate<T>(string fileName)
			where T: new()
		{
			try
			{
				return Load<T>(fileName);
			}
			catch
			{
				return new T();
			}
		}

		/// <summary>Loads object from XML or creates new.</summary>
		/// <typeparam name="T">Object to be loaded.</typeparam>
		/// <param name="stream">The stream.</param>
		/// <returns>Loaded or created object.</returns>
		public static T LoadOrCreate<T>(Stream stream)
			where T: new()
		{
			try
			{
				return Load<T>(stream);
			}
			catch
			{
				return new T();
			}
		}

		#region IndentCData

		private static readonly Regex m_CDataHead = new Regex(
				@"^(?<indent>\s*)(?<head>.*\<\!\[CDATA\[)$",
				RegexOptions.ExplicitCapture | RegexOptions.Compiled);
		private static readonly Regex m_CDataTail = new Regex(
				@"^(?<indent>\s*)(?<tail>\]\]\>.*)$",
				RegexOptions.ExplicitCapture | RegexOptions.Compiled);

		/// <summary>
		/// Indents the CDATA section in file.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="writer">The writer.</param>
		/// <param name="cdataIndent">The CDATA indent.</param>
		public static void IndentCData(StreamReader reader, StreamWriter writer, string cdataIndent)
		{
			bool cdata = false;
			string indent = string.Empty;
			StringBuilder buffer = new StringBuilder();

			while (true)
			{
				string line = reader.ReadLine();

				if (line == null)
				{
					if (cdata)
					{
						throw new ArgumentException("CDATA sections does not match");
					}
					break;
				}

				if (!cdata)
				{
					Match m = m_CDataHead.Match(line);
					if (m.Success)
					{
						indent = m.Groups["indent"].Value;
						cdata = true;
						buffer.Length = 0;
					}
					writer.WriteLine(line);
				}
				else
				{
					Match m = m_CDataTail.Match(line);
					if (m.Success)
					{
						writer.Write(Indent.ReindentBlock(indent + cdataIndent, buffer.ToString(), false));
						writer.WriteLine(Indent.IndentBlock(indent, m.Groups["tail"].Value));
						cdata = false;
					}
					else
					{
						buffer.AppendLine(line);
					}
				}
			}
		}

		#endregion
	}
}
