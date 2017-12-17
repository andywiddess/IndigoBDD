#region Header
// --------------------------------------------------------------------------------------
// Indigo.CrossCutting.Utilities.Files.IImportTask.cs
// -------------------------------------------------------------------------------------
// 
// The interface implemented by operations which can support file importing
//
// Copyright (c) 2011 Sepura Plc
//
// Sepura Confidential (c)
//
// Created: March 2011 : Simon Hirst
// 
// --------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Indigo.CrossCutting.Utilities.Error;

namespace Indigo.CrossCutting.Utilities.Files
{
	/// <summary>
	/// The interface implemented by operations which can support file importing
	/// </summary>
	public interface IImportTask
	{
		/// <summary>
		/// Perform the import operation
		/// You need to pass back any error messages which will be shown to the user.
		/// </summary>
		/// <param name="filePath">The path of the file to be imported</param>
		/// <param name="messages">The returned messages, or null if no messages are performed</param>
		void PerformImport(string filePath, out IEnumerable<Messages> messages);

		/// <summary>
		/// Method called once the messages have been reported, allows the IImportTask to decided on the operation
		/// You are passed back any messages you created in the <c>PeformImport</c> step. 
		/// Any messages requiring acknowledgment will be acknowledged. 
		/// You then perform any final operation you were waiting on, whether you had messages or not.
		/// </summary>
		/// <param name="messages">The messages.</param>
		void MessagesReported(IEnumerable<Messages> messages);

		/// <summary>
		/// The operation is now finished, report the success or otherwise
		/// Also you should tidy up any resources etc
		/// </summary>
		/// <param name="messages">The messages.</param>
		/// <returns><true>Success</true> or <false>failure</false> of the import operation</returns>
		bool Finished(IEnumerable<Messages> messages);
	}
}
