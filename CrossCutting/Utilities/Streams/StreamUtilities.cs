using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Streams
{
	/// <summary>
	/// Some Stream utilities.
	/// </summary>
	public class StreamUtilities
	{
		private const int m_DefaultBufferLength = 0x4000;

		/// <summary>
		/// Copies the stream.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="target">The target.</param>
		/// <param name="maximumLength">The maximum length.</param>
		/// <param name="progress">The progress callback. Can be <c>null</c>.</param>
		/// <returns>Number of bytes actually copied.</returns>
		public static ulong CopyStream(Stream source, Stream target, ulong maximumLength, Action<ulong> progress)
		{
			if (source == null)
				throw new ArgumentNullException("source", "source is null.");
			if (target == null)
				throw new ArgumentNullException("target", "target is null.");

			if (progress != null) progress(0);

			if (maximumLength == 0) return 0;

			int bufferSize = (int)Math.Min((ulong)m_DefaultBufferLength, maximumLength);

			byte[] buffer = new byte[bufferSize];
			ulong copied = 0;
			ulong left = maximumLength;

			while (left > 0)
			{
				int bytes = (int)Math.Min((ulong)buffer.Length, left);
				int read = source.Read(buffer, 0, bytes);
				if (read == 0) break;
				target.Write(buffer, 0, read);
				copied += (ulong)read;
				left -= (ulong)read;
				if (progress != null) progress(copied);
			}

			return copied;
		}

		/// <summary>
		/// Copies the file. Also copies the file attributes.
		/// </summary>
		/// <param name="sourceName">Name of the source.</param>
		/// <param name="targetName">Name of the target.</param>
		/// <param name="progress">The progress callback. Can be <c>null</c>.</param>
		public static void CopyFile(string sourceName, string targetName, Action<ulong> progress)
		{
			using (FileStream source = new FileStream(sourceName, FileMode.Open, FileAccess.Read, FileShare.Read))
			using (FileStream target = new FileStream(targetName, FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				CopyStream(source, target, ulong.MaxValue, progress);
			}
			CopyFileAttributes(sourceName, targetName);
		}

		/// <summary>Copies the file attributes.</summary>
		/// <param name="sourceName">Name of the source.</param>
		/// <param name="targetName">Name of the target.</param>
		/// <param name="copyTimestamp">if set to <c>true</c> copies timestamp.</param>
		/// <param name="copyAttributes">if set to <c>true</c> copies file attributes (ReadOnly, Hidden, Archive).</param>
		public static void CopyFileAttributes(
			string sourceName, string targetName, 
			bool copyTimestamp = true, bool copyAttributes = false)
		{
			FileInfo sourceInfo = new FileInfo(sourceName);
			FileInfo targetInfo = new FileInfo(targetName);

			if (copyTimestamp)
			{
				targetInfo.CreationTimeUtc = sourceInfo.CreationTimeUtc;
				targetInfo.LastAccessTimeUtc = sourceInfo.LastAccessTimeUtc;
				targetInfo.LastWriteTimeUtc = sourceInfo.LastWriteTimeUtc;
			}
			if (copyAttributes)
			{
				var mask = 
					FileAttributes.ReadOnly | 
					FileAttributes.Hidden | 
					FileAttributes.Archive;
				targetInfo.Attributes = (targetInfo.Attributes & ~mask) | (sourceInfo.Attributes & mask);
			}
		}

		/// <summary>
		/// Gets the short name of the path.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="shortPath">The short path.</param>
		/// <param name="shortPathLength">Short length of the path.</param>
		/// <returns>Same path but using 8.3 file names.</returns>
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern int GetShortPathName(
				[MarshalAs(UnmanagedType.LPTStr)] string path,
				[MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath,
				int shortPathLength);

		/// <summary>
		/// Gets the short name of the path.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>Same path but using 8.3 file names.</returns>
		public static string GetShortPathName(string path)
		{
			int maximumLength = Math.Max(1024, path.Length * 2); // it can be estimated better, but I'm in hurry, sorry :-)
			StringBuilder result = new StringBuilder(maximumLength);
			GetShortPathName(path, result, result.Capacity);
			return result.ToString();
		}

	}
}
