using System;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// Encapsulates and simplifies access to embedded files.
	/// It handles both plain-text files and already compressed (gzipped) files with .z extension.
	/// Such files can be properated with GZipTool custom environment tool.
	/// </summary>
	public class ResourceFile
	{
		#region Members

		private Stream m_Stream;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceFile"/> class.
		/// </summary>
		/// <param name="hookType">The 'hook' type.</param>
		/// <param name="fileName">Name of the file.</param>
		public ResourceFile(Type hookType, string fileName)
		{
			m_Stream = GetStream(hookType, fileName);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceFile"/> class.
		/// </summary>
		/// <param name="hookType">The hook type.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="favorGzipStream">if set to <c>true</c>, checks first for gzipped stream (with .z extension). 
		/// Such files can be prepared with GZipTool.</param>
		public ResourceFile(Type hookType, string fileName, bool favorGzipStream)
		{
			m_Stream = GetStream(hookType, fileName, favorGzipStream);
		}

		#endregion

		#region GetStream

		/// <summary>
		/// Gets the stream.
		/// </summary>
		/// <returns>Embedded stream.</returns>
		public Stream GetStream()
		{
			return m_Stream;
		}

		/// <summary>
		/// Gets the stream.
		/// </summary>
		/// <param name="hookType">The hook type.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>Embedded stream.</returns>
		public static Stream GetStream(Type hookType, string fileName)
		{
			return hookType.Module.Assembly.GetManifestResourceStream(hookType, fileName);
		}

		/// <summary>
		/// Gets the stream.
		/// </summary>
		/// <param name="hookType">The hook type.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>Decompressed embedded stream.</returns>
		public static Stream GetGzipStream(Type hookType, string fileName)
		{
			Stream innerStream = GetStream(hookType, fileName);
			if (innerStream == null)
				return null;
			return new GZipStream(innerStream, CompressionMode.Decompress);
		}

		/// <summary>
		/// Gets the stream.
		/// </summary>
		/// <param name="hookType">The hook type.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="favorGzipStream">if set to <c>true</c>, checks first for gzipped stream (with .z extension). 
		/// Such files can be prepared with GZipTool.</param>
		/// <returns>Decompressed (if needed) embedded stream.</returns>
		public static Stream GetStream(Type hookType, string fileName, bool favorGzipStream)
		{
			Stream result = null;

			if (favorGzipStream)
			{
				try
				{
					result = GetGzipStream(hookType, fileName + ".z");
				}
				catch
				{
					// do nothing, try load not-gzipped file
				}
			}

			if (result == null)
			{
				result = GetStream(hookType, fileName);
			}

			return result;
		}

		#endregion

		#region SaveToFile

		/// <summary>
		/// Saves the specified resource to file.
		/// </summary>
		/// <param name="hookType">Type of the hook.</param>
		/// <param name="resourceFile">The resource file.</param>
		/// <param name="progress">The progress.</param>
		public static void SaveToFile(Type hookType, string resourceFile, Action<ulong> progress)
		{
			using (Stream resourceStream = ResourceFile.GetStream(hookType, resourceFile))
			using (FileStream targetStream = new FileStream(resourceFile, FileMode.Create, FileAccess.ReadWrite))
			{
				if (resourceStream.Length > long.MaxValue)
					throw new InvalidOperationException("source resource too long for this fn to copy (supports max long.Length)");

				StreamUtilities.CopyStream(resourceStream, targetStream, long.MaxValue, progress);
			}
		}
		#endregion

		#region GetXmlDocument

		/// <summary>
		/// Loads the XML document from embedded stream.
		/// </summary>
		/// <returns>Xml document.</returns>
		public XmlDocument LoadXmlDocument()
		{
			return LoadXmlDocument(m_Stream);
		}

		/// <summary>
		/// Loads the XML document.
		/// </summary>
		/// <param name="hookType">The hook type.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>Xml document.</returns>
		public static XmlDocument LoadXmlDocument(Type hookType, string fileName)
		{
			return LoadXmlDocument(GetStream(hookType, fileName));
		}

		/// <summary>
		/// Loads the XML document.
		/// </summary>
		/// <param name="hookType">The hook type.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="favorGzipStream">if set to <c>true</c>, checks first for gzipped stream (with .z extension). 
		/// Such files can be prepared with GZipTool.</param>
		/// <returns>Xml document.</returns>
		public static XmlDocument LoadXmlDocument(Type hookType, string fileName, bool favorGzipStream)
		{
			return LoadXmlDocument(GetStream(hookType, fileName, favorGzipStream));
		}

		/// <summary>
		/// Loads the XML document.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>Xml document.</returns>
		public static XmlDocument LoadXmlDocument(Stream stream)
		{
			XmlDocument document = new XmlDocument();
			document.Load(stream);
			return document;
		}
		#endregion
	}
}