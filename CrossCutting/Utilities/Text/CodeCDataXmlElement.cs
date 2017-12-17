using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Indigo.CrossCutting.Utilities.Text
{
	/// <summary>
	/// Implementation of <see cref="IXmlSerializable"/> allowing
	/// to store code string as CDATA. It automatically indents / unindents code
	/// to make it look better in XML file. No magic, just useful to handle
	/// user (developer) editable XML.
	/// </summary>
	public class CodeCDataXmlElement
        : IXmlSerializable
	{
		#region fields

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="CDataXmlElement"/> class.
		/// </summary>
		public CodeCDataXmlElement()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CDataXmlElement"/> class.
		/// Initializes with given text.
		/// </summary>
		/// <param name="text">The text.</param>
		public CodeCDataXmlElement(string text)
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

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema() => null;

        void IXmlSerializable.ReadXml(XmlReader reader)
		{
			Text = Indent.NormalizeText(reader.ReadString());
			reader.Read();
		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (Text == null)
			{
				// do nothing
			}
			else if (Text.Length == 0)
			{
				writer.WriteString(string.Empty);
			}
			else
			{
				writer.WriteCData(Indent.NormalizeNewLine(Text, true, true));
			}
		}

		#endregion
	}
}
