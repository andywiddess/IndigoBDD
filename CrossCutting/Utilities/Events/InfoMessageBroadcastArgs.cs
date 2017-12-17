using System;
using Indigo.CrossCutting.Utilities.Events;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Broadcast argument for updating the text in the info message control
	/// </summary>
	public class InfoMessageBroadcastArgs: BroadcastArgs
	{

		#region Member Variables

		private readonly string m_message = String.Empty;

		#endregion

		#region Constructors and Finalisers

		/// <summary>
		/// Initializes a new instance of the <see cref="InfoMessageBroadcastArgs"/> class.
		/// </summary>
		/// <param name="message">The info message.</param>
		public InfoMessageBroadcastArgs(string message)
		{
			m_message = message;
		}

		#endregion

		#region Public Methods and Accessors

		/// <summary>
		/// Gets the message.
		/// </summary>
		public string Message
		{
			get { return m_message; }
		}

		#endregion

	}
}
