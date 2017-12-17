using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// Encapsulates access to password encoded XML file. XML document is encrypted with password given as 
	/// an argument to <c>&lt;?xml-encode password="..."?&gt;</c> processing instruction.
	/// </summary>
	public class EncodedXml
	{
		#region static fields

		private const string m_EncXmlInstruction = @"xml-encode";

		#endregion

		#region load/save

		/// <summary>
		/// Loads encoded XML from file. Cryptographic exception are not handled (so, you should catch them yourself).
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="password">The password.</param>
		/// <returns>XmlDocument</returns>
		public static XmlDocument LoadXml(Stream stream, string password)
		{
			if (stream == null)
				throw new ArgumentNullException("stream", "stream is null.");
			if (password == null)
				throw new ArgumentNullException("password", "password is null.");

			using (PasswordStream encodeStream = new PasswordStream(stream, password, CryptoStreamMode.Read))
			using (GZipStream compressStream = new GZipStream(encodeStream, CompressionMode.Decompress))
			{
				XmlDocument document = new XmlDocument();
				document.Load(compressStream);
				return document;
			}
		}

		/// <summary>
		/// Loads the XML from embedded encrupter resource.
		/// </summary>
		/// <param name="hookClass">The hook class.</param>
		/// <param name="resourceName">Name of the resource.</param>
		/// <param name="password">The password.</param>
		/// <returns>Loaded XmlDocument</returns>
		public static XmlDocument LoadXml(Type hookClass, string resourceName, string password)
		{
			return LoadXml(ResourceFile.GetStream(hookClass, resourceName), password);
		}

		/// <summary>
		/// Saves XML encoded with given password.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <param name="outputStream">The output stream.</param>
		/// <param name="password">The password.</param>
		public static void SaveXml(XmlDocument document, Stream outputStream, string password)
		{
			if (document == null)
				throw new ArgumentNullException("document", "document is null.");
			if (outputStream == null)
				throw new ArgumentNullException("outputStream", "output is null.");
			if (password == null)
				throw new ArgumentNullException("password", "password is null.");

			using (PasswordStream encodeStream = new PasswordStream(outputStream, password, CryptoStreamMode.Write))
			using (GZipStream compressStream = new GZipStream(encodeStream, CompressionMode.Compress))
			using (XmlTextWriter writer = new XmlTextWriter(compressStream, Encoding.UTF8))
			{
				writer.Formatting = Formatting.None;
				document.Save(writer);
			}
		}

		/// <summary>
		/// Saves encoded XML. Encryption password is taken from <c>&lt;?xml-encode password="..."?&gt;</c>
		/// processing instruction. Note: password is removed from output stream.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <param name="outputStream">The output stream.</param>
		public static void SaveXml(XmlDocument document, Stream outputStream)
		{
			if (document == null)
				throw new ArgumentNullException("document", "document is null.");
			if (outputStream == null)
				throw new ArgumentNullException("outputStream", "output is null.");

			EncodedXmlArgs args = GetEncodeArgs(document);

			if ((args == null) || (args.Password == null))
			{
				throw new ArgumentException(
						"no xml-encode instruction has been found in document or password is not defined");
			}
			else
			{
				string backupInstructionData = args.Instruction.Data;
				args.Instruction.Data = ""; // hide encoding data
				SaveXml(document, outputStream, args.Password);
				args.Instruction.Data = backupInstructionData; // restore encoding data
			}
		}

		#endregion

		#region utilities

		/// <summary>
		/// Extracts encoding arguments from document.
		/// See <see cref="SaveXml(XmlDocument,Stream)"/> for more information.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns>Encoding args.</returns>
		private static EncodedXmlArgs GetEncodeArgs(XmlDocument document)
		{
			EncodedXmlArgs args = null;

			foreach (XmlNode node in document.ChildNodes)
			{
				XmlProcessingInstruction pi = node as XmlProcessingInstruction;
				if ((pi != null) && (pi.Name == m_EncXmlInstruction))
				{
					if (args != null)
						throw new NotSupportedException("Multiple xml-encode instructions are not supported");
					args = new EncodedXmlArgs(pi);
				}
			}

			return args;
		}

		#endregion
	}

	/// <summary>
	/// Encapsulation of <c>&lt;?xml-encode ...?&gt;</c> arguments.
	/// </summary>
	internal class EncodedXmlArgs
	{
		#region static fields

		private const string m_PasswordAttributeName = "password";

		#endregion

		#region fields

		private XmlProcessingInstruction m_Instruction;
		private string m_Password;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the instruction.
		/// </summary>
		/// <value>The instruction.</value>
		public XmlProcessingInstruction Instruction
		{
			get { return m_Instruction; }
		}

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password
		{
			get { return m_Password; }
			set { m_Password = value; }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="EncodedXmlArgs"/> class.
		/// Parses given processing instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		public EncodedXmlArgs(XmlProcessingInstruction instruction)
		{
			m_Instruction = instruction;
			ParseInstruction();
		}

		#endregion

		#region utility

		private void ParseInstruction()
		{
			XmlElement element = ConvertProcessingInstructionToElement(m_Instruction);
			XmlAttribute attribute;

			attribute = element.GetAttributeNode(m_PasswordAttributeName);
			if (attribute != null)
			{
				m_Password = attribute.Value;
			}
		}

		private static XmlElement ConvertProcessingInstructionToElement(XmlProcessingInstruction instruction)
		{
			string text = string.Format("<root><{0} {1}/></root>", instruction.Name, instruction.Data);
			XmlDocument document = new XmlDocument();
			document.LoadXml(text);
			return (XmlElement)document.ChildNodes[0].ChildNodes[0];
		}

		#endregion
	}
}
