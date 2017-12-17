using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Text
{
	/// <summary>
	/// Implementation of <see cref="IXmlSerializable"/> allowing
	/// to store string as CDATA.
	/// </summary>
	public class CDataXmlElement
        : IXmlSerializable
	{
		#region consts

		/// <summary>
		/// Characters which needs to be escaped in XML.
		/// </summary>
		private readonly static HashSet<char> EscapedChars = new HashSet<char>(new[] { '&', '<', '>', '\n', '\r', '\t' });

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="CDataXmlElement"/> class.
		/// </summary>
		public CDataXmlElement()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CDataXmlElement"/> class.
		/// Initializes with given text.
		/// </summary>
		/// <param name="text">The text.</param>
		public CDataXmlElement(string text)
		{
			Text = text;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the text of element.
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; set; }

		#endregion

		#region utilities

		/// <summary>Checks if text requires CDATA section..</summary>
		/// <param name="text">The text.</param>
		/// <returns><c>true</c> is text rquires CDATA section; <c>false</c> otherwise.</returns>
		private static bool RequiresCData(string text)
		{
			//RMT-1736 fix. Leading-trailing spaces trimmed by t-sql openxml. So if it starts or ends with a white space enclose in a CData section.
			if (text.Length > 0 && (char.IsWhiteSpace(text[0]) || char.IsWhiteSpace(text[text.Length - 1])))
			{
				return true;
			}

			for (int i = text.Length - 1; i >= 0; i--)
			{
				if (EscapedChars.Contains(text[i])) 
					return true;
			}
			return false;
		}

        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable
        /// interface, you should return null (Nothing in Visual Basic) from this method, and instead, if
        /// specifying a custom schema is required, apply the
        /// <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        ///
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the
        /// object that is produced by the
        /// <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" />
        /// method and consumed by the
        /// <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" />
        /// method.
        /// </returns>
        XmlSchema IXmlSerializable.GetSchema() => null;

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        ///
        /// <param name="reader">   The <see cref="T:System.Xml.XmlReader" /> stream from which the
        ///                         object is deserialized. </param>
        void IXmlSerializable.ReadXml(XmlReader reader)
		{
			Text = reader.ReadString();
			reader.Read();
		}

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        ///
        /// <param name="writer">   The <see cref="T:System.Xml.XmlWriter" /> stream to which the object
        ///                         is serialized. </param>
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			var text = Text;
			if (text == null)
			{
				// do nothing
			}
			else if (text.Length == 0)
			{
				writer.WriteString(string.Empty);
			}
			else if (!RequiresCData(text))
			{
				writer.WriteString(text);
			}
			else
			{
				writer.WriteCData(text);
			}
		}

		#endregion
	}
}
