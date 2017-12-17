#region Header
// --------------------------------------------------------------------------------------
// Indigo.CrossCutting.Utilities.Process.IShellExecuteMonitor.cs
// --------------------------------------------------------------------------------------
//
// ShellExecute monitor.
//
// Copyright (c) 2011 Sepura Plc
//
// Sepura Confidential
//
// Created: Milosz Krajewski
//
// --------------------------------------------------------------------------------------
#endregion

namespace Indigo.CrossCutting.Utilities.Process
{
	/// <summary>
	/// ShellExecute monitor.
	/// </summary>
	public interface IShellExecuteMonitor
	{
		/// <summary>
		/// Called when process has ended.
		/// </summary>
		void OnProcessStarted();

		/// <summary>
		/// Called when process sends text to its standard output stream.
		/// Note: This method is called in separate thread, different than job execution.
		/// </summary>
		/// <param name="text">The text.</param>
		void OnStandardOutputString(string text);

		/// <summary>
		/// Called when process sends text to its standard error stream.
		/// Note: This method is called in separate thread, different than job execution.
		/// </summary>
		/// <param name="text">The text.</param>
		void OnStandardErrorString(string text);

		/// <summary>
		/// Decides if external process needs termination.
		/// </summary>
		/// <returns></returns>
		bool OnTerminateQuery();

		/// <summary>
		/// Called when process due to be killed.
		/// </summary>
		void OnProcessKilling();

		/// <summary>
		/// Called when process should be killed but time expired leaving it abandoned.
		/// </summary>
		void OnProcessAbandoned();

		/// <summary>
		/// Called when process is killed.
		/// </summary>
		void OnProcessKilled();

		/// <summary>
		/// Called when process has ended.
		/// </summary>
		void OnProcessEnded();
	}
}
