using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Indigo.CrossCutting.Utilities.Utility
{
    /// <summary>
    /// XmlSerializerUtility provides the helper methods of xml serialize and xml deserialize
    /// </summary>
    public static class XmlSerializerUtility
    {
        /// <summary>
        /// Serialize the object to stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        public static void SerizlizeToStream(Stream stream, Type type, object instance)
        {
            stream.Position = 0;
            XmlSerializer ser = new XmlSerializer(type);
            ser.Serialize(stream, instance);
        }

        /// <summary>
        /// Serizlizes to stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="instance">The instance.</param>
        public static void SerizlizeToStream<T>(Stream stream,T instance)
        {
            SerizlizeToStream(stream,typeof(T),instance);
        }

        /// <summary>
        /// Serialize the object to xml file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        public static void SerializeToXmlFile(string fileName, Type type, object instance)
        {
            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
            SerizlizeToStream(stream,type,instance);
            stream.Close();
        }

        /// <summary>
        /// Serializes to XML file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="instance">The instance.</param>
        public static void SerializeToXmlFile<T>(string fileName, T instance)
        {
            SerializeToXmlFile(fileName, typeof(T), instance);
        }

        /// <summary>
        /// Serialize the object to xml text
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string SerializeToXml(Type type, object instance)
        {
            XmlSerializer ser = new XmlSerializer(type);
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
           
            writer.Formatting = Formatting.None;
            ser.Serialize(writer, instance);
            writer.Flush();
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream,System.Text.Encoding.UTF8);
            string xml = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            writer.Close();
            return xml;
        }

        /// <summary>
        /// Serializes to XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static string SerializeToXml<T>(T instance)
        {
            return SerializeToXml(instance.GetType(), instance);
        }

        /// <summary>
        /// Deserialize the object from stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeFormStream(Stream stream, Type type)
        {
            stream.Position = 0;
            XmlSerializer ser = new XmlSerializer(type);
            object obj = ser.Deserialize(stream);
            return obj;
        }

        /// <summary>
        /// Deserializes the form stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static T DeserializeFormStream<T>(Stream stream)
        {
            return (T)DeserializeFormStream(stream, typeof(T));
        }

        /// <summary>
        /// Deserialize the object from file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeFormXmlFile(string fileName,Type type)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(type);
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                object obj = ser.Deserialize(stream);
                stream.Close();
                return obj;
            }
            catch(Exception e) 
            {
                throw new XmlDeserializeException(fileName, type.ToString()+" "+e.Message);
                //return null;
            }
        }

        /// <summary>
        /// Deserializes the form XML file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static T DeserializeFormXmlFile<T>(string fileName)
        {
            return (T)DeserializeFormXmlFile(fileName, typeof(T));
        }

        /// <summary>
        /// Deserialize the object from text
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeFromXmlText(string xml, Type type)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream,System.Text.Encoding.UTF8);
            writer.Write(xml);
            writer.Flush();
            stream.Position = 0;

            XmlSerializer serializer = new XmlSerializer(type);
            object obj=serializer.Deserialize(stream);
            writer.Close();
            stream.Close();
            return obj;
        }

        /// <summary>
        /// Deserializes from XML text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static T DeserializeFromXmlText<T>(string xml)
        {
            return (T)DeserializeFromXmlText(xml, typeof(T));
        }
    }
}
