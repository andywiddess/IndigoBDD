#region Header
// --------------------------------------------------------------------------------------
// Indigo.CrossCutting.Utilities.Files.Helpers.cs
// --------------------------------------------------------------------------------------
// 
// Basic file and directory helpers.
//
// Copyright (c) 2009 Sepura Plc
//
// Sepura Confidential (c)
//
// Created: November 2009 : Simon Hirst
// 
//
// --------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;

namespace Indigo.CrossCutting.Utilities.Files
{
	/// <summary>
	/// Basic file and directory helpers.
	/// </summary>
	public class Helpers
	{
		/// <summary>
		/// Determines whether a filename is valid
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="returnIllegalCharPositions">The illegal characters, the position of the character and the character</param>
		/// <returns>
		/// 	<c>true</c> if the filename is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsFilenameValid(string fileName, out Dictionary<int, char> returnIllegalCharPositions)
		{
			bool valid = true;
			returnIllegalCharPositions = new Dictionary<int, char>();
			char[] invalidChars = Path.GetInvalidFileNameChars();

			for (int position = 0; position < fileName.Length; position++)
			{
				if (Array.IndexOf(invalidChars, fileName[position]) > 0)
				{
					valid = false;
					returnIllegalCharPositions.Add(position, fileName[position]);
				}
			}

			return valid;
		}

		/// <summary>
		/// Determines whether a filename is valid
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>
		/// 	<c>true</c> if the filename is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsFilenameValid(string fileName)
		{
			Dictionary<int, char> illegalChars;
			return IsFilenameValid(fileName, out illegalChars);
		}

		/// <summary>
		/// Return the path to a unique directory, inside the source path directory using the uniqueName (if available).
		/// If the uniqueName is used then try uniqueName1, uniqueName2 etc until one is found...
		/// </summary>
		/// <param name="sourcePath">The source path.</param>
		/// <param name="uniqueName">Name of the unique directory to try and create</param>
		/// <param name="create">
		/// if set to <c>true</c> then the new directory is created. If <c>false</c> then the directory
		/// will not be created
		/// </param>
		/// <returns>The new directory path created</returns>
		public static string UniqueDirectory(string sourcePath, string uniqueName, bool create)
		{
			string pathName = Path.Combine(sourcePath, uniqueName);
			int index = 1;
			while (Directory.Exists(pathName))
			{
				pathName = Path.Combine(sourcePath, String.Concat(uniqueName, index.ToString(CultureInfo.InvariantCulture)));
				index++;
			}

			if (create)
				Directory.CreateDirectory(pathName);

			return pathName;
		}

		/// <summary>
		/// Return the path to a unique directory, inside the source path directory using the uniqueName (if available).
		/// If the uniqueName is used then try uniqueName1, uniqueName2 etc until one is found...
		/// </summary>
		/// <param name="sourceDirectory">The source directory.</param>
		/// <param name="uniqueName">Name of the unique file to try and create, this can include an extension or not</param>
		/// <param name="create">
		/// if set to <c>true</c> then the new file is created. If <c>false</c> then the file
		/// will not be created
		/// </param>
		/// <returns>The new file path created</returns>
		public static string UniqueFileName(string sourceDirectory, string uniqueName, bool create)
		{
			string uniqueNameNoExt = Path.GetFileNameWithoutExtension(uniqueName);
			string fileExtension = Path.GetExtension(uniqueName);

			string fileName = Path.Combine(sourceDirectory, String.Concat(uniqueNameNoExt, fileExtension));

			int index = 1;
			while (File.Exists(fileName))
			{
				fileName = Path.Combine(sourceDirectory, String.Concat(uniqueNameNoExt, index.ToString(CultureInfo.InvariantCulture), fileExtension));
				index++;
			}

			if (create)
				File.Create(fileName);

			return fileName;
		}
	}
}
