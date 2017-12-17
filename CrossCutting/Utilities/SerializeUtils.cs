using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities
{
    public static class SerializeUtils<T> where T : class
    {
        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string Serialize(T obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Serializes to stream.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static Stream SerializeToStream(T obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
            serializer.WriteObject(memoryStream, obj);

            return memoryStream;
        }

        /// <summary>
        /// Serializes to byte array.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static byte[] SerializeToByteArray(T obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
            serializer.WriteObject(memoryStream, obj);

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Deserializes the specified XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static T Deserialize(string xml)
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.UTF8, new XmlDictionaryReaderQuotas(), null);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(reader);
            }
        }

        /// <summary>
        /// Deserializes the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static T Deserialize(byte[] array)
        {
            using (MemoryStream memoryStream = new MemoryStream(array))
            {
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.UTF8, new XmlDictionaryReaderQuotas(), null);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(reader);
            }
        }

        /// <summary>
        /// Deeps the clone.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
