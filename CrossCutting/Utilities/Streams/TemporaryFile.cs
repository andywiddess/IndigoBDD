using System;
using System.IO;
using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// Encapsulates temporary file. File is automatically deleted when this object is disposed.
	/// </summary>
	public class TemporaryFile
        : IDisposable
	{
		#region properties

		/// <summary>
		/// Gets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		public string FileName { get; private set; }

		/// <summary>
		/// Gets or sets the alternative name of the file.
		/// This name is not used anywhere in this class code, but it can be helpful
		/// when returning <see cref="TemporaryFile"/>s from function. It may be used
		/// to indicate what the real name should be. It's usage is totally
		/// developer responsibility, it may be left blank if not needed.
		/// </summary>
		/// <value>The alternative name of the file.</value>
		public string AlternativeName { get; set; }

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TemporaryFile"/> class.
		/// Note: empty file will be created at this point.
		/// </summary>
		public TemporaryFile()
			: this(true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TemporaryFile"/> class.
		/// </summary>
		/// <param name="create">if set to <c>true</c> empty file is created.</param>
		public TemporaryFile(bool create)
			: this(create, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TemporaryFile"/> class.
		/// </summary>
		/// <param name="create">if set to <c>true</c> empty file is created.</param>
		/// <param name="extension">The extension. Used to force different than default extension. 
		/// Extension is expected to not include leading dot</param>
		public TemporaryFile(bool create, string extension)
		{
			if (extension == null) extension = "tmp";

			while (true)
			{
				FileName = Path.Combine(Path.GetTempPath(), string.Format("{0}.{1}", Guid.NewGuid().ToString("N"), extension));
				if (!File.Exists(FileName)) break;
			}

			if (create)
			{
				File.Create(FileName).Close();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TemporaryFile"/> class.
		/// Creates a temporary file and copies data from given stream into it.
		/// </summary>
		/// <param name="copyFrom">The data which should be copied to temporary file.</param>
		public TemporaryFile(Stream copyFrom)
			: this(copyFrom, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TemporaryFile"/> class.
		/// Creates a temporary file and copies data from given stream into it.
		/// </summary>
		/// <param name="copyFrom">The data which should be copied to temporary file.</param>
		/// <param name="extension">The extension. Extension is expected to not include leading dot.</param>
		public TemporaryFile(Stream copyFrom, string extension)
			: this(true, extension)
		{
			using (Stream outStream = GetStream())
			{
				StreamUtilities.CopyStream(copyFrom, outStream, ulong.MaxValue, null);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TemporaryFile"/> class.
		/// Creates a temporary file and copies data from other file.
		/// </summary>
		/// <param name="fileName">Name of the file to copy.</param>
		/// <param name="extension">The extension. Extension is expected to not include leading dot.</param>
		public TemporaryFile(string fileName, string extension)
			: this(true, extension)
		{
			using (FileStream inStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (Stream outStream = GetStream())
			{
				StreamUtilities.CopyStream(inStream, outStream, ulong.MaxValue, null);
				outStream.Flush();
			}
		}

		#endregion

		#region utilities

		/// <summary>
		/// Gets the stream. File is created if necessary.
		/// </summary>
		/// <returns>Stream.</returns>
		public Stream GetStream()
		{
			return new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete);
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Indicates if object has been already disposed.
		/// </summary>
		protected bool m_Disposed;

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="TemporaryFile"/> is reclaimed by garbage collection.
		/// </summary>
		~TemporaryFile()
		{
			Dispose(false);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; 
		/// <c>false</c> to release only unmanaged resources.</param>
		private void Dispose(bool disposing)
		{
			if (!m_Disposed)
			{
				if (disposing)
					DisposeManaged();
				DisposeUnmanaged();
			}
			m_Disposed = true;
		}

		/// <summary>
		/// Disposes the managed resources.
		/// </summary>
		protected virtual void DisposeManaged()
		{
			/* managed resources */
		}

		/// <summary>
		/// Disposes the unmanaged resources.
		/// </summary>
		protected virtual void DisposeUnmanaged()
		{
			Patterns.IgnoreException(() => File.Delete(FileName));
		}

		#endregion
	}
}
