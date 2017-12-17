using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// A list of serializable SerializableDictionary which implements IXmlSerializable itself.
    /// </summary>
    [XmlInclude(typeof(Indigo.CrossCutting.Utilities.DataTypes.Date))]
    [XmlInclude(typeof(Indigo.CrossCutting.Utilities.DataTypes.Document))]
    [XmlInclude(typeof(Indigo.CrossCutting.Utilities.DataTypes.Time))]
    [XmlInclude(typeof(SerializableDictionary))]
    public class SerializableDictionaryList
        : List<SerializableDictionary>
        , IXmlSerializable
    {

        public System.Xml.Schema.XmlSchema GetSchema() { return null; }

        /// <summary>
        /// Read an XML element corresponding to a Set of T
        /// </summary>
        /// <param name="reader"></param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            // this line supposes that T is XmlSerializable 
            // (not an IDictionnary for example, or implements IXmlSerializable too ...)
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SerializableDictionary));

            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {

                SerializableDictionary item = (SerializableDictionary)serializer.Deserialize(reader);
                if (item != null)
                {
                    this.Add(item);
                }
            }

            reader.ReadEndElement();
        }

        /// <summary>
        /// Write an XML Element corresponding to a Set of T
        /// </summary>
        /// <param name="writer"></param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            // this line supposes that T is XML Serializable 
            // (not an IDictionnary for example, or implements IXmlSerializable too ...)
            System.Xml.Serialization.XmlSerializer serializer =
                  new System.Xml.Serialization.XmlSerializer(typeof(SerializableDictionary));

            foreach (SerializableDictionary item in this)
            {
                serializer.Serialize(writer, item, null);
            }
        }


    }

    /// <summary>
    /// A serializable Dictionary class. Used in Data Entity serialization.
    /// </summary>
    [XmlInclude(typeof(Indigo.CrossCutting.Utilities.DataTypes.Date))]
    [XmlInclude(typeof(Indigo.CrossCutting.Utilities.DataTypes.Document))]
    [XmlInclude(typeof(Indigo.CrossCutting.Utilities.DataTypes.Time))]
    [XmlInclude(typeof(SerializableDictionary))]
    public class SerializableDictionary : Dictionary<string, object>, IXmlSerializable
    {

        
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            System.Xml.Serialization.XmlSerializer keySerializer = new System.Xml.Serialization.XmlSerializer(typeof(string));
            System.Xml.Serialization.XmlSerializer valueSerializer = new System.Xml.Serialization.XmlSerializer(typeof(object));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
            {
                return;
            }
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                string key = (string)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                object value = valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

  
            

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            
            foreach (string key in this.Keys)
            {
                writer.WriteStartElement(key);
                
                object value = this[key];

                IXmlSerializable valueAsSerializable = value as IXmlSerializable;
                
                if (valueAsSerializable != null)
                {
                    valueAsSerializable.WriteXml(writer);
                }
                else
                {
                    writer.WriteValue(value);
                }
                
                writer.WriteEndElement();

            }
        }

    }


}
