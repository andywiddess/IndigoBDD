using System;
using System.IO;
using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Exceptions;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// LockFile privides locking machanism to provide exclusive access to files.
	/// <example><![CDATA[
	/// using (LockFile.Scope(fileName)) { ... }
	/// ]]></example>
	/// </summary>
	public class LockFile
        : IDisposable
	{
		#region fields

		private string m_FileName;
		private FileStream m_LockFile;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="LockFile"/> class.</summary>
		/// <param name="fileName">Name of the file.</param>
		protected LockFile(string fileName)
		{
			try
			{
				m_FileName = fileName;
				m_LockFile = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
			}
			catch (IOException)
			{
				throw new LockFileFailedException(fileName);
			}
		}

		/// <summary>Scopes the specified file name. Note, if lock cannot be acquired this method will 
		/// throw <see cref="LockFileFailedException"/> exception.</summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns></returns>
		public static IDisposable Scope(string fileName)
		{
			return new LockFile(fileName);
		}

		#endregion

		#region utilities

		/// <summary>Releases the lock.</summary>
		private void Cleanup()
		{
			if (m_LockFile != null)
			{
				var file = m_LockFile;
				m_LockFile = null;
				Patterns.IgnoreException(() => file.Close());
			}

			if (m_FileName != null)
			{
				var fileName = m_FileName;
				m_FileName = null;
				Patterns.IgnoreException(() => File.Delete(fileName));
			}
		}

		#endregion

		#region overrides

		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format("LockFile({0})", m_FileName);
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="TemporaryFile"/> is reclaimed by garbage collection.
		/// </summary>
		~LockFile()
		{
			Cleanup();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Cleanup();
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}