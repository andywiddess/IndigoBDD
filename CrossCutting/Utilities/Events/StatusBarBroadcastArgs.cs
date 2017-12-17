using System;
using Indigo.CrossCutting.Utilities.Events;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Broadcast argument for updating the text in the status bar
	/// If you want to reset the status message to the default string (usually 'Ready') then
	/// create this object with the default parameterless constructor
	/// </summary>
	public class StatusBarBroadcastArgs: BroadcastArgs
	{

		#region Member Variables

		private string m_statusText = String.Empty;
		private bool m_reset;

		#endregion

		/// <summary>
		/// The disposable object used for the scope of the status bar update
		/// </summary>
		public class ResetStatusTextScope: IDisposable
		{
			private object m_sender;
			private string m_resetText;

			/// <summary>
			/// Initializes a new instance of the <see cref="ResetStatusTextScope"/> class.
			/// </summary>
			/// <param name="sender">The sender.</param>
			/// <param name="resetText">The reset text.</param>
			internal ResetStatusTextScope(object sender, string resetText)
			{
				m_resetText = resetText;
				m_sender = sender;
			}

			#region IDisposable Members

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
				if (String.IsNullOrWhiteSpace(m_resetText))
					BroadcastCenter.Default.Notify(m_sender, new StatusBarBroadcastArgs());
				else
					BroadcastCenter.Default.Notify(m_sender, new StatusBarBroadcastArgs(m_resetText));
			}

			#endregion
		}

		#region Constructors and Finalisers

		/// <summary>
		/// Initializes a new instance of the <see cref="StatusBarBroadcastArgs"/> class.
		/// </summary>
		/// <param name="statusText">The status text.</param>
		public StatusBarBroadcastArgs(string statusText)
		{
			m_reset = false;
			m_statusText = statusText;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StatusBarBroadcastArgs"/> class.
		/// Calling the default constructor will reset the text to empty.
		/// </summary>
		public StatusBarBroadcastArgs()
		{
			m_reset = true;
		}		

		#endregion

		#region Public Methods and Accessors

		/// <summary>
		/// Provides a IDisposable scope for the setting of the status bar text.
		/// This will set the text to the "message" until the scope is disposed where 
		/// it will be set to the "resetText", unless "resetText" is null or empty then it will
		/// use the default 'Ready' message
		/// <example>
		/// using (StatusBarBroadcastArgs.Scope(this, "Hello World")
		/// {
		///		// Do your stuff - while 'Hello world' is displayed....
		/// }
		/// // Now the 'Ready' text will be displayed again
		/// </example>
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="message">The message.</param>
		/// <param name="resetText">The reset text, or null/empty to use the 'Ready' message</param>
		/// <returns>The IDisposable scope</returns>
		public static IDisposable Scope(object sender, string message, string resetText = null)
		{
			BroadcastCenter.Default.Notify(sender, new StatusBarBroadcastArgs(message));
			return new ResetStatusTextScope(sender, resetText);
		}

		/// <summary>
		/// Gets the reset.
		/// </summary>
		public bool Reset
		{
			get { return m_reset; }
		}

		/// <summary>
		/// Gets the status text.
		/// </summary>
		public string StatusText
		{
			get { return m_statusText; }
		}

		#endregion

	}
}
