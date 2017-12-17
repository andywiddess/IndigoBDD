using System;
using System.IO;
using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// Encapsulates temporary folder. Folder is automaticaly deleted when this object is disposed.
	/// </summary>
	public class TemporaryFolder
        : IDisposable
	{
		#region fields

		private readonly string m_FolderPath;

		/// <summary>
		/// Flag indicating if object has been already disposed.
		/// </summary>
		protected bool m_Disposed;

		#endregion

		#region properties

		/// <summary>
		/// Gets the folder path.
		/// </summary>
		/// <value>The folder path.</value>
		public string FolderPath
		{
			get { return m_FolderPath; }
		}

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TemporaryFolder"/> class.
		/// Creates the folder by default.
		/// </summary>
		public TemporaryFolder()
			: this(true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TemporaryFolder"/> class.
		/// </summary>
		/// <param name="create">if set to <c>true</c> folder is physicaly created.</param>
		public TemporaryFolder(bool create)
		{
			string path = Path.GetTempFileName();
			File.Delete(path);

			m_FolderPath = path;

			if (create)
			{
				Directory.CreateDirectory(path);
			}
		}

		#endregion

		#region utilities

		/// <summary>
		/// Gets the full path of the file inside temporary folder.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>Full path to the file.</returns>
		public string GetFilePath(string fileName)
		{
			return Path.Combine(m_FolderPath, fileName);
		}

		/// <summary>
		/// Ensures that folder exists. If it doesn't creates it.
		/// </summary>
		/// <returns><c>true</c> if folder has been just created; <c>false</c> if it did exist before</returns>
		public bool EnsureFolder()
		{
			if (Directory.Exists(m_FolderPath))
			{
				return false;
			}

			Directory.CreateDirectory(m_FolderPath);
			return true;
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="TemporaryFolder"/> is reclaimed by garbage collection.
		/// </summary>
		~TemporaryFolder()
		{
			Dispose(false);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// Deletes the temporary folder, all files in it and all subfolders.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

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
		/// Disposes managed resources (if any).
		/// </summary>
		protected virtual void DisposeManaged() { /* managed resources */ }

		/// <summary>
		/// Disposes unmanaged resources (if any).
		/// </summary>
		protected virtual void DisposeUnmanaged()
		{
			Patterns.IgnoreException(() => Directory.Delete(m_FolderPath, true));
		}

		#endregion
	}
}
