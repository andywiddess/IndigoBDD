#region Header

//  ---------------------------------------------------------------------------
//  Copyright Sepura Plc
//  All Rights reserved. Reproduction in whole or part is prohibited without
//  written consent of the copyright owner.
// 
//  SafeFileAccess.cs
//   Provides methods of safely reading from and writing to files.
//	 A backup mechanism is used to guard against data loss.
// 
//   Original author: MooreG
// 
//  $Id:$
//  ---------------------------------------------------------------------------

#endregion

using System;
using System.IO;
using System.Linq;

namespace Indigo.CrossCutting.Utilities.Files
{
	/// <summary>
	///     Provides methods of safely reading from and writing to files.
	/// </summary>
	public static class SafeFileAccess
	{
		/// <summary>
		/// The maximum number of backup files allowed.
		/// </summary>
		public const int MaximumBackupFiles = 10;

		/// <summary>
		/// The file extension format for backup file extensions.
		/// </summary>
		public const string BackupFileExtensionFormat = ".{0:D3}";

		/// <summary>
		/// The file extension for temporary files.
		/// </summary>
		public const string TemporaryFileExtension = ".tmp";

		/// <summary>
		/// The block size for atomic read and write.
		/// 4096 bytes is usual cluster size on NTFS.
		/// </summary>
		private const int BlockSize = 4096;

		/// <summary>
		/// Writes data to the specified file and subsequently verifying the data has been correctly written.
		///	Will also create backups of existing files to guard against any possible data loss.
		/// </summary>
		/// <param name="filePath">The full path to the destination file.</param>
		/// <param name="data">The data to be written.</param>
		/// <returns>Returns <c>true</c> if the file is successfully written, otherwise <c>false</c>.</returns>
		public static bool Write(string filePath, byte[] data)
		{
			try
			{
				if (string.IsNullOrEmpty(filePath) || data == null) return false;

				// Write to temporary file first
				var temporaryFilename = GetTemporaryFilename(filePath);
				if (string.IsNullOrEmpty(temporaryFilename)) return false;

				WriteData(temporaryFilename, data);

				// Verify writing was successful by doing a file content comparison
				if (!File.Exists(temporaryFilename)) return false;
				if (!VerifyFileContents(temporaryFilename, data))
				{
					// File cannot be verified, so delete the temporary 
					// file we created and return false.
					File.Delete(temporaryFilename);
					return false;
				}

				// Backup existing file
				if (File.Exists(filePath))
				{
					var backupPath = GetBackupFilePath(filePath);
					if (string.IsNullOrEmpty(backupPath))
					{
						File.Delete(temporaryFilename);
						return false;
					}

					File.Move(filePath, backupPath);
				}

				// Move newly written temporary file to final location
				File.Move(temporaryFilename, filePath);

				return true;
			}
			catch (Exception)
			{
				// Catching all exceptions here rather than individual IO exceptions, 
				// there are too many and we handle then all the same way.
				return false;
			}
		}

		/// <summary>
		/// Reads the contents of the specified file.
		/// </summary>
		/// <param name="filePath">The full path to the file.</param>
		/// <param name="includeBackups">
		/// If set to <c>true</c> include backup files if the specified file cannot be read from.
		/// </param>
		/// <returns>Returns the contents of the file, or <c>null</c> if the file could not be read.</returns>
		public static byte[] Read(string filePath, bool includeBackups = true)
		{
			// Check the most usual case first, the file exists and is valid
			if (File.Exists(filePath) &&
			    GetFileSize(filePath) > 0)
					return ReadData(filePath);

			// Check whether we want to include backups
			if (!includeBackups) return null;

			// Return the contents of the most recent backup
			return ReadData(GetPathToMostRecentBackup(filePath));
		}

		/// <summary>
		/// Gets the full path to a backup file that can be written to.
		/// Backups are numbered 000...009, with 000 being the most recent and 009
		/// the oldest.  When a new backup file path is requested, files are moved
		/// sequentially to the next available number.  If we have already reached
		/// the maximum number of backups, the oldest one is deleted.
		/// </summary>
		/// <param name="originalFilePath">The original file path.</param>
		/// <returns></returns>
		private static string GetBackupFilePath(string originalFilePath)
		{
			if (string.IsNullOrEmpty(originalFilePath)) return string.Empty;

			var folderPath = Path.GetDirectoryName(originalFilePath);
			var filename = Path.GetFileNameWithoutExtension(originalFilePath);

			if (string.IsNullOrEmpty(folderPath) ||
			    string.IsNullOrEmpty(filename))
					return string.Empty;

			var targetPath = string.Empty;
			for (var index = MaximumBackupFiles; index > 0; index--)
			{
				targetPath = GenerateBackupFilename(folderPath, filename, index);

				if (File.Exists(targetPath))
				{
					if (index == MaximumBackupFiles)
					{
						// We already have a maximum number of backups so we can delete the oldest one
						File.Delete(targetPath);
					}
					else
					{
						// Move the current backup file to an older file extension
						// For example, move file.004 to file.005
						var newBackupFilename = GenerateBackupFilename(folderPath, filename, index + 1);
						File.Move(targetPath, newBackupFilename);
					}
				}
			}

			return targetPath;
		}

		/// <summary>
		/// Verifies the contents of a file match the specified byte array.
		/// </summary>
		/// <param name="filePath">The full path to the file.</param>
		/// <param name="expectedContents">The expected file contents.</param>
		/// <returns>Returns <c>true</c> if the contents match, otherwise <c>false</c>.</returns>
		private static bool VerifyFileContents(string filePath, byte[] expectedContents)
		{
			var actualContents = File.ReadAllBytes(filePath);

			return actualContents.SequenceEqual(expectedContents);
		}

		/// <summary>
		/// Gets the full path to most recent valid backup file.
		/// </summary>
		/// <param name="filePath">The original file path..</param>
		/// <returns>Returns the path to a valid backup file.</returns>
		private static string GetPathToMostRecentBackup(string filePath)
		{
			if (File.Exists(filePath) &&
				GetFileSize(filePath) > 0)
			{
				return filePath;
			}

			var folder = Path.GetDirectoryName(filePath);
			var filename = Path.GetFileNameWithoutExtension(filePath);

			if (string.IsNullOrEmpty(folder) ||
			    string.IsNullOrEmpty(filename))
				return string.Empty;

			for (var index = 0; index < MaximumBackupFiles; index++)
			{
				var backupFilename = GenerateBackupFilename(folder, filename, index);
				if (File.Exists(backupFilename) &&
				    GetFileSize(backupFilename) > 0)
						return backupFilename;
			}

			return string.Empty;
		}

		/// <summary>
		/// Gets the size of the specified file.
		/// </summary>
		/// <param name="filePath">The path to the file.</param>
		/// <returns>Returns the length of the file.</returns>
		private static long GetFileSize(string filePath)
		{
			return new FileInfo(filePath).Length;
		}

		/// <summary>
		/// Gets a temporary filename based on the specified filename, changing the extension.
		/// Note: if the temporary file already exists, it will be deleted.
		/// </summary>
		/// <param name="filePath">The full path to the original file.</param>
		/// <returns>Returns the temporary filename.</returns>
		private static string GetTemporaryFilename(string filePath)
		{
			var folder = Path.GetDirectoryName(filePath);
			var filename = Path.GetFileNameWithoutExtension(filePath);

			if (string.IsNullOrEmpty(folder) ||
			    string.IsNullOrEmpty(filename))
				return string.Empty;

			var path = Path.Combine(folder, filename + TemporaryFileExtension);
			if (File.Exists(path))
				File.Delete(path);

			return path;
		}

		/// <summary>
		/// Generates a filename for a backup with the specified index.
		/// </summary>
		/// <param name="folder">The full folder path.</param>
		/// <param name="filename">The filename.</param>
		/// <param name="index">The index of the backup.</param>
		/// <returns>The new backup filename.</returns>
		private static string GenerateBackupFilename(string folder, string filename, int index)
		{
			return Path.Combine(folder, filename + string.Format(BackupFileExtensionFormat, index));
		}

		/// <summary>
		/// Read the contents of the specified file.
		/// </summary>
		/// <param name="filePath">The full path to the file.</param>
		/// <returns>The contents of the file, or null if the file does not exist.</returns>
		private static byte[] ReadData(string filePath)
		{
			if (string.IsNullOrEmpty(filePath)) return null;

			try
			{
				using (var file = new FileStream(
					filePath,
					FileMode.Open, FileAccess.Read, FileShare.Read,
					BlockSize))
				{
					var length = (int)Math.Min(int.MaxValue, file.Length);
					var buffer = new byte[length];
					file.Read(buffer, 0, length);
					return buffer;
				}
			}
			catch (FileNotFoundException)
			{
				return null;
			}
		}

		/// <summary>
		/// Writes a block of bytes to the specified file.
		/// </summary>
		/// <param name="filePath">The full path to the file.</param>
		/// <param name="data">The data to write to the file.</param>
		private static void WriteData(string filePath, byte[] data)
		{
			using (var file = new FileStream(
				filePath,
				FileMode.Create, FileAccess.Write, FileShare.None,
				BlockSize,
				FileOptions.WriteThrough))
			{
				var length = data.Length;
				file.Write(data, 0, length);
			}
		}
	}
}