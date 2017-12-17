using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

using Newtonsoft.Json;

using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Streams;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	public static class ExtensionsToStream
	{
		public static byte[] ReadToEnd(this Stream stream)
		{
			using (var content = new MemoryStream())
			{
				var buffer = new byte[4096];

				int read = stream.Read(buffer, 0, 4096);
				while (read > 0)
				{
					content.Write(buffer, 0, read);

					read = stream.Read(buffer, 0, 4096);
				}

				return content.ToArray();
			}
		}

		public static string ReadToEndAsText(this Stream stream)
		{
			return Encoding.UTF8.GetString(stream.ReadToEnd());
		}


		public static T ReadJson<T>(this Stream context)
		{
			using (var streamReader = new StreamReader(context))
			using (var jsonReader = new JsonTextReader(streamReader))
				return (T)new JsonSerializer().Deserialize(jsonReader, typeof(T));
		}

		public static object ReadJsonObject(this Stream context)
		{
			using (var streamReader = new StreamReader(context))
			using (var jsonReader = new JsonTextReader(streamReader))
				return new JsonSerializer().Deserialize(jsonReader);
		}

        #region TextReader

        /// <summary>Enumerates the lines in a stream.</summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Collection of lines.</returns>
        public static IEnumerable<string> EnumerateLines(this TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        #endregion

        #region Stream extensions

        /// <summary>Isolates the specified stream. Closing the stream won't close underlying stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="isolate">if set to <c>true</c> stream gets isolated.</param>
        /// <returns>Isolated stream.</returns>
        public static Stream Isolate(this Stream stream, bool isolate = true)
        {
            return
                isolate && !(stream is IsolatingStream)
                ? new IsolatingStream(stream)
                : stream;
        }

        /// <summary>Creates compression stream over specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="compress">if set to <c>true</c> compression stream is created.</param>
        /// <returns>Compression stream.</returns>
        public static Stream Compress(this Stream stream, bool compress = true)
        {
            return
                compress
                ? new DeflateStream(stream, CompressionMode.Compress)
                : stream;
        }

        /// <summary>Creates decompression stream over specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="decompress">if set to <c>true</c> decompression stream is created.</param>
        /// <returns>Decompression stream.</returns>
        public static Stream Decompress(this Stream stream, bool decompress = true)
        {
            return
                decompress
                ? new DeflateStream(stream, CompressionMode.Decompress)
                : stream;
        }

        /// <summary>Creates encryption stream over specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="password">The password.</param>
        /// <param name="encrypt">if set to <c>true</c> encryption stream is created.</param>
        /// <returns>Encryption stream.</returns>
        public static Stream Encrypt(this Stream stream, string password, bool encrypt = true)
        {
            return
                encrypt && password != null
                ? new PasswordStream(stream, password, CryptoStreamMode.Write)
                : stream;
        }

        /// <summary>Creates decryption stream over specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="password">The password.</param>
        /// <param name="decrypt">if set to <c>true</c> decryption stream is created.</param>
        /// <returns>Decryption stream.</returns>
        public static Stream Decrypt(this Stream stream, string password, bool decrypt = true)
        {
            return
                decrypt && password != null
                ? new PasswordStream(stream, password, CryptoStreamMode.Read)
                : stream;
        }

        /// <summary>Caches the specified stream starting at current position.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="length">The length.</param>
        /// <param name="cache">if set to <c>true</c> stream data gets cached.</param>
        /// <returns>Cached stream data.</returns>
        public static Stream Cache(this Stream stream, ulong length = ulong.MaxValue, bool cache = true)
        {
            if (!cache) return stream;

            var result = new MemoryStream();
            StreamUtilities.CopyStream(stream, result, length, null);
            return result;
        }

        /// <summary>Creates buffered stream for the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns>Buffered stream.</returns>
        public static BufferedStream Buffered(this Stream stream, int bufferSize = 0x4000)
        {
            return new BufferedStream(stream, bufferSize);
        }

        /// <summary>Creates memory stream over specified buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>Stream over buffer.</returns>
        public static Stream Stream(this byte[] buffer)
        {
            return new MemoryStream(buffer);
        }

        /// <summary>Create BinaryReader over specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="isolate">if set to <c>true</c> stream gets isolated.</param>
        /// <returns>BinaryReader.</returns>
        public static BinaryReader BinaryReader(this Stream stream, bool isolate = false)
        {
            return new BinaryReader(stream.Isolate(isolate));
        }

        /// <summary>Creates BinaryWriter over specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="isolate">if set to <c>true</c> stream gets isolated..</param>
        /// <returns>BinaryWriter.</returns>
        public static BinaryWriter BinaryWriter(this Stream stream, bool isolate = false)
        {
            return new BinaryWriter(stream.Isolate(isolate));
        }

        /// <summary>Create BinaryReader over specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="isolate">if set to <c>true</c> stream gets isolated.</param>
        /// <returns>BinaryReader.</returns>
        public static BinaryReader BinaryReader(this Stream stream, Encoding encoding, bool isolate = false)
        {
            return new BinaryReader(stream.Isolate(isolate), encoding);
        }

        /// <summary>Creates BinaryWriter over specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="isolate">if set to <c>true</c> stream gets isolated..</param>
        /// <returns>BinaryWriter.</returns>
        public static BinaryWriter BinaryWriter(this Stream stream, Encoding encoding, bool isolate = false)
        {
            return new BinaryWriter(stream.Isolate(isolate), encoding);
        }

        /// <summary>Creates text reader for given stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="isolate">if set to <c>true</c> stream will be isolated, so it won't be closed when reader closes.</param>
        /// <returns>TextReader.</returns>
        public static TextReader TextReader(this Stream stream, Encoding encoding, bool isolate = false)
        {
            return new StreamReader(stream.Isolate(isolate), encoding);
        }

        /// <summary>Creates text read for given stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="detectEncoding">if set to <c>true</c> detects encoding.</param>
        /// <param name="isolate">if set to <c>true</c> stream will be isolated, so it won't be closed when reader closes.</param>
        /// <returns>TextReader.</returns>
        public static TextReader TextReader(this Stream stream, bool detectEncoding = true, bool isolate = false)
        {
            return new StreamReader(stream.Isolate(isolate), detectEncoding);
        }

        /// <summary>Creates text writer over given stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="isolate">if set to <c>true</c> stream will be isolated from writer. So closing the writer will not 
        /// close the stream.</param>
        /// <returns>TextWriter.</returns>
        public static TextWriter TextWriter(this Stream stream, Encoding encoding, bool isolate = false)
        {
            return new StreamWriter(stream.Isolate(isolate), encoding);
        }

        /// <summary>Creates XML read for given stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="isolate">if set to <c>true</c> stream get isolated, so it wont be be closed when reader closes.</param>
        /// <returns>XmlReader.</returns>
        public static XmlTextReader XmlReader(this Stream stream, bool isolate = false)
        {
            return new XmlTextReader(stream.Isolate(isolate));
        }

        /// <summary>Creates XML read for given text reader.</summary>
        /// <param name="reader">The reader.</param>
        /// <returns>XmlReader.</returns>
        public static XmlTextReader XmlReader(this TextReader reader)
        {
            return new XmlTextReader(reader);
        }

        /// <summary>Creates XmlWriter for a stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="indented">if set to <c>true</c> XML will be indented (using a tab).</param>
        /// <returns>XmlWriter.</returns>
        public static XmlTextWriter XmlWriter(this Stream stream, Encoding encoding, bool indented = false)
        {
            var result = new XmlTextWriter(stream, encoding);
            if (indented)
            {
                result.Formatting = System.Xml.Formatting.Indented;
                result.Indentation = 1;
                result.IndentChar = '\t';
            }
            return result;
        }

        /// <summary>Copies data from stream to another.</summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="maximumLength">The maximum length.</param>
        /// <param name="progress">The progress.</param>
        /// <returns>The <paramref name="output"/> stream.</returns>
        public static Stream CopyTo(
            this Stream input, Stream output,
            ulong maximumLength = ulong.MaxValue, Action<ulong> progress = null)
        {
            StreamUtilities.CopyStream(input, output, maximumLength, progress);
            return output;
        }

        #endregion

        #region Serialize/Deserialize

        /// <summary>Serializes the specified object.</summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="subject">The object.</param>
        /// <param name="serialize">The serialization method, uses .NET binary serialization if <c>null</c> specified.</param>
        /// <param name="compress">if set to <c>true</c> stream will be compressed.</param>
        /// <param name="password">The password for encryption, if set to <c>null</c> stream won't be ecrypted.</param>
        /// <returns>Serialized object.</returns>
        public static byte[] Serialize<T>(this T subject, Action<Stream, T> serialize = null, bool compress = false, string password = null)
        {
            bool encrypt = !string.IsNullOrEmpty(password);
            using (var buffer = new MemoryStream())
            {
                using (var output = buffer.Isolate().Encrypt(password, encrypt).Compress(compress))
                {
                    if (serialize == null)
                    {
                        new BinaryFormatter().Serialize(output, subject);
                    }
                    else
                    {
                        serialize(output, subject);
                    }
                }
                return buffer.ToArray();
            }
        }

        /// <summary>Deserializes the specified data.</summary>
        /// <typeparam name="T">Type of expected object.</typeparam>
        /// <param name="data">The serialized object.</param>
        /// <param name="deserialize">The deserialization method, uses .NET binary deserialization if <c>null</c> specified.</param>
        /// <param name="decompress">if set to <c>true</c> stream is expected to be compressed.</param>
        /// <param name="password">The password for decryption, if set to <c>null</c> stream is expcted to be not encrypted.</param>
        /// <returns>Deserialized object.</returns>
        public static T Deserialize<T>(this byte[] data, Func<Stream, T> deserialize = null, bool decompress = false, string password = null)
        {
            bool decrypt = !string.IsNullOrEmpty(password);
            using (var input = data.Stream().Decompress(decompress).Decrypt(password, decrypt))
            {
                if (deserialize == null)
                {
                    var result = new BinaryFormatter().Deserialize(input);
                    return (T)result;
                }
                else
                {
                    return deserialize(input);
                }
            }
        }

        /// <summary>Serialization using <see cref="IBinarySerializable"/> protocol.</summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="subject">The subject.</param>
        /// <param name="writer">The writer.</param>
        /// <returns>Given object.</returns>
        public static T BinarySave<T>(this T subject, BinaryWriter writer)
            where T : IBinarySerializable
        {
            subject.BinarySerialize(writer);
            return subject;
        }

        /// <summary>Serialization using <see cref="IBinarySerializable"/> protocol.</summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="subject">The subject.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="isolate">if set to <c>true</c> stream is isolated (does not close).</param>
        /// <returns>Given object.</returns>
        public static T BinarySave<T>(this T subject, Stream stream, bool isolate = true)
            where T : IBinarySerializable
        {
            using (var writer = stream.BinaryWriter(isolate))
                return BinarySave(subject, writer);
        }

        /// <summary>Serialization using <see cref="IBinarySerializable"/> protocol.</summary>
        /// <param name="subject">The subject.</param>
        /// <returns>Serialized object.</returns>
        public static byte[] BinarySave(this IBinarySerializable subject)
        {
            return Patterns.Bufferize(w => BinarySave(subject, w));
        }

        /// <summary>Deserialization using <see cref="IBinarySerializable"/> protocol.</summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="subject">The subject.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>Given object (but populated with serialized values).</returns>
        public static T BinaryLoad<T>(this T subject, BinaryReader reader)
            where T : IBinarySerializable
        {
            subject.BinaryDeserialize(reader);
            return subject;
        }

        /// <summary>Deserialization using <see cref="IBinarySerializable"/> protocol.</summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="subject">The subject.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="isolate">if set to <c>true</c> stream gets "isolated" (does not close on exit).</param>
        /// <returns>Given object (but populated with serialized values).</returns>
        public static T BinaryLoad<T>(this T subject, Stream stream, bool isolate = true)
            where T : IBinarySerializable
        {
            using (var reader = stream.BinaryReader(isolate))
                return BinaryLoad(subject, reader);
        }

        /// <summary>Deserialization using <see cref="IBinarySerializable"/> protocol.</summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="subject">The subject.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>Given object (but populated with serialized values).</returns>
        public static T BinaryLoad<T>(this T subject, byte[] buffer)
            where T : IBinarySerializable
        {
            Patterns.Debufferize(buffer, r => subject.BinaryDeserialize(r));
            return subject;
        }

        #endregion

        #region BinaryReader/BinaryWriter

        /// <summary>Writes the unsigned long int packed. Uses 7-bits per byte, but stores only non-zero byte.</summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        public static void PackedWrite(this BinaryWriter writer, ulong value)
        {
            while (value >= 0x80)
            {
                writer.Write((byte)(value | 0x80));
                value >>= 7;
            }
            writer.Write((byte)value);
        }

        /// <summary>Reads the packed unsigned long int.</summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Unpacked unsigned long int.</returns>
        public static ulong PackedRead(this BinaryReader reader)
        {
            ulong result = 0;
            int shift = 0;

            while (true)
            {
                byte b = reader.ReadByte();
                result = result | (((ulong)(b & 0x7F)) << shift);
                if ((b & 0x80) == 0) break; // if this byte doesn't have bit 7 set, it means it was the last one
                shift += 7;
            }

            return result;
        }

        /// <summary>Writes zigzag encoded interger value.</summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        public static void ZigZagWrite(this BinaryWriter writer, long value)
        {
            PackedWrite(writer, ((ulong)(value << 1) ^ (ulong)(value >> 63)));
        }

        /// <summary>Reads zigzag encoded integer.</summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Integer value.</returns>
        public static long ZigZagRead(this BinaryReader reader)
        {
            ulong value = PackedRead(reader);
            return (long)(value >> 1) ^ (-(long)(value & 1));
        }

        /// <summary>Writes the string using UTF-8 regardelss of stream encoding.</summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        public static void WriteUtf8(this BinaryWriter writer, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                writer.PackedWrite(0);
            }
            else
            {
                var buffer = Encoding.UTF8.GetBytes(value);
                var length = buffer.Length;
                writer.PackedWrite((uint)length);
                if (length > 0) writer.Write(buffer);
            }
        }

        /// <summary>Reads the string using UTF-8 regardless of default stream encoding.</summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The string.</returns>
        public static string ReadUtf8(this BinaryReader reader)
        {
            var length = (int)reader.PackedRead();
            if (length == 0)
            {
                return string.Empty;
            }
            else
            {
                return Encoding.UTF8.GetString(reader.ReadBytes(length));
            }
        }

        /// <summary>Writes the packet. The different between this and WriteBytes is that packet includes length.</summary>
        /// <param name="writer">The writer.</param>
        /// <param name="bytes">The byte buffer.</param>
        public static void WritePacket(this BinaryWriter writer, byte[] bytes)
        {
            writer.PackedWrite((ulong)bytes.Length);
            writer.Write(bytes);
        }

        /// <summary>Reads the packet written by WritePacket.</summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Byte buffer.</returns>
        public static byte[] ReadPacket(this BinaryReader reader)
        {
            var length = reader.PackedRead();
            return reader.ReadBytes((int)length);
        }

        #endregion
    }
}