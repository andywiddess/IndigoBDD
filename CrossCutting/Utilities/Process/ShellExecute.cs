#region Header
// --------------------------------------------------------------------------------------
// Indigo.CrossCutting.Utilities.Process.ShellExecute.cs
// --------------------------------------------------------------------------------------
//
// Execute external process.
//
// Copyright (c) 2011 Sepura Plc
//
// Sepura Confidential
//
// Created: Milosz Krajewski
//
// --------------------------------------------------------------------------------------
#endregion

using System;

namespace Indigo.CrossCutting.Utilities.Process
{
	/// <summary>
	/// Execute external process.
	/// </summary>
	public class ShellExecute
	{
		#region fields

		private string m_FileName = String.Empty;
		private string m_Arguments = String.Empty;
		private string m_WorkingDirectory = ".";
		private int m_SleepTime = 100;
		private int m_KillTime = 1000;
		private IShellExecuteMonitor m_Monitor;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		public string FileName
		{
			get { return m_FileName; }
			set { m_FileName = value; }
		}

		/// <summary>
		/// Gets or sets the arguments.
		/// </summary>
		/// <value>The arguments.</value>
		public string Arguments
		{
			get { return m_Arguments; }
			set { m_Arguments = value; }
		}

		/// <summary>
		/// Gets or sets the working directory.
		/// </summary>
		/// <value>The working directory.</value>
		public string WorkingDirectory
		{
			get { return m_WorkingDirectory; }
			set { m_WorkingDirectory = value; }
		}

		/// <summary>
		/// Gets or sets the sleep time.
		/// </summary>
		/// <value>The sleep time.</value>
		public int SleepTime
		{
			get { return m_SleepTime; }
			set { m_SleepTime = value; }
		}

		/// <summary>
		/// Gets or sets the kill time.
		/// </summary>
		/// <value>The kill time.</value>
		public int KillTime
		{
			get { return m_KillTime; }
			set { m_KillTime = value; }
		}

		/// <summary>
		/// Gets or sets the process monitor.
		/// </summary>
		/// <value>The monitor.</value>
		public IShellExecuteMonitor Monitor
		{
			get { return m_Monitor; }
			set { m_Monitor = value; }
		}

		#endregion

		#region internal fields

		private System.Diagnostics.Process m_Process;

		#endregion

		#region execute

		/// <summary>
		/// Executes the specified process.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="arguments">The arguments.</param>
		/// <param name="workingDirectory">The working directory.</param>
		/// <param name="sleepTime">The sleep time.</param>
		/// <param name="killTime">The kill time.</param>
		/// <param name="monitor">The monitor.</param>
		/// <returns>ExitCode of external process.</returns>
		public static int Execute(
				string filename, string arguments, string workingDirectory,
				int sleepTime, int killTime, IShellExecuteMonitor monitor)
		{
			ShellExecute runner = new ShellExecute();
			runner.FileName = filename;
			runner.Arguments = arguments;
			runner.WorkingDirectory = workingDirectory;
			runner.SleepTime = sleepTime;
			runner.KillTime = killTime;
			runner.Monitor = monitor;
			return runner.Execute();
		}

		/// <summary>
		/// Executes the specified process.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="arguments">The arguments.</param>
		/// <param name="workingDirectory">The working directory.</param>
		/// <param name="monitor">The monitor.</param>
		/// <returns>ExitCode of external process.</returns>
		public static int Execute(
				string filename, string arguments, string workingDirectory,
				IShellExecuteMonitor monitor)
		{
			ShellExecute runner = new ShellExecute();
			runner.FileName = filename;
			runner.Arguments = arguments;
			runner.WorkingDirectory = workingDirectory;
			runner.Monitor = monitor;
			return runner.Execute();
		}

		/// <summary>
		/// Executes the specified process.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="arguments">The arguments.</param>
		/// <param name="workingDirectory">The working directory.</param>
		/// <returns>ExitCode of external process.</returns>
		public static int Execute(
				string filename, string arguments, string workingDirectory)
		{
			ShellExecute runner = new ShellExecute();
			runner.FileName = filename;
			runner.Arguments = arguments;
			runner.WorkingDirectory = workingDirectory;
			return runner.Execute();
		}

		/// <summary>
		/// Executes external process.
		/// </summary>
		/// <returns>ExitCode of external process.</returns>
		protected int Execute()
		{
			if (m_Process != null)
			{
				throw new InvalidOperationException("Processing already started");
			}

			try
			{
				// Start a new process for the cmd
				m_Process = new System.Diagnostics.Process();
				m_Process.StartInfo.UseShellExecute = false;
				m_Process.StartInfo.RedirectStandardOutput = true;
				m_Process.StartInfo.RedirectStandardError = true;
				m_Process.StartInfo.CreateNoWindow = true;
				m_Process.StartInfo.FileName = m_FileName;
				m_Process.StartInfo.Arguments = m_Arguments;
				m_Process.StartInfo.WorkingDirectory = m_WorkingDirectory;
				m_Process.Start();

				// Invoke stdOut and stdErr readers - each
				// has its own thread to guarantee that they aren't
				// blocked by, or cause a block to, the actual
				// process running (or the GUI).
				(new Action(ReadStdOut)).BeginInvoke(null, null);
				(new Action(ReadStdErr)).BeginInvoke(null, null);

				bool naturalDeath = true;

				if (m_Monitor != null) m_Monitor.OnProcessStarted();

				// Wait for the process to end, or cancel it
				while (!m_Process.WaitForExit(m_SleepTime))
				{
					if ((m_Monitor != null) && (m_Monitor.OnTerminateQuery()))
					{
						m_Monitor.OnProcessKilling();
						m_Process.Kill();
						naturalDeath = false;
						if (!m_Process.WaitForExit(m_KillTime))
						{
							m_Monitor.OnProcessAbandoned();
						}
						else
						{
							m_Monitor.OnProcessKilled();
						}
					}
				}

				if (naturalDeath)
				{
					m_Monitor.OnProcessEnded();
				}

				return m_Process.ExitCode;
			}
			finally
			{
				m_Process = null;
			}
		}

		#endregion

		#region event handlers

		/// <summary>
		/// Handles reading of stdErr
		/// </summary>
		protected virtual void ReadStdErr()
		{
			string str;
			while ((m_Process != null) && ((str = m_Process.StandardError.ReadLine()) != null))
			{
				if (m_Monitor != null) m_Monitor.OnStandardErrorString(str);
			}
		}

		/// <summary>
		/// Handles reading of stdout and firing an event for
		/// every line read
		/// </summary>
		protected virtual void ReadStdOut()
		{
			string str;
			while ((m_Process != null) && ((str = m_Process.StandardOutput.ReadLine()) != null))
			{
				if (m_Monitor != null) m_Monitor.OnStandardOutputString(str);
			}
		}

		#endregion
	}
}
