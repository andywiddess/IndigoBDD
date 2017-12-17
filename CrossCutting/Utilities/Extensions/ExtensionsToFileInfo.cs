using System.IO;
using Indigo.CrossCutting.Utilities.Files;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	/// <summary>
	/// Extension class for <see cref="FileInfo"/>.
	/// </summary>
	public static class ExtensionsToFileInfo
    {
		/// <summary>
		/// Writes data to the specified file and subsequently verifying the data has been correctly written.
		///	Will also create backups of existing files to guard against any possible data loss.
		/// </summary>
		/// <param name="fileInfo">The <see cref="FileInfo"/> for the destination file.</param>
		/// <param name="data">The data to be written.</param>
		/// <returns>Returns <c>true</c> if the file is successfully written, otherwise <c>false</c>.</returns>
		public static bool Write(this FileInfo fileInfo, byte[] data)
		{
			return SafeFileAccess.Write(fileInfo.FullName, data);
		}

		/// <summary>
		/// Reads the contents of the specified file.
		/// </summary>
		/// <param name="fileInfo">The <see cref="FileInfo"/> for the destination file.</param>
		/// <param name="includeBackups">
		/// If set to <c>true</c> include backup files if the specified file cannot be read from.
		/// </param>
		/// <returns>Returns the contents of the file, or <c>null</c> if the file could not be read.</returns>
		public static byte[] Read(this FileInfo fileInfo, bool includeBackups = true)
		{
			return SafeFileAccess.Read(fileInfo.FullName, includeBackups);
		}
	}
}