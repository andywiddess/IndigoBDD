using System;

namespace Indigo.CrossCutting.Utilities.Error
{
	/// <summary>
	/// Representation of an sepura application error. 
	/// Note, this is different from an exception!
	/// Errors are related to the logical failure of a process, not the failure of the code!
	/// </summary>
	public class Message: IComparable<Message>, IComparable
	{

		#region Member Variables

		private Severity m_severity;
		private string m_text;
		private object m_context = null;
		private Exception m_exception = null;
		private bool m_acknowledged = false;
		private bool m_requiredAcknowledge = false;

		#endregion

		#region Event Definitions

		/// <summary>
		/// Occurs when the acknowledged state has changed.
		/// </summary>
		public event EventHandler AcknowledgedStateChanged;

		#endregion

		#region Public Methods and Accessors

		/// <summary>
		/// Gets the message's severity.
		/// </summary>
		/// <value>The severity.</value>
		public Severity Severity
		{
			get { return m_severity; }
		}

		/// <summary>
		/// Gets the message's text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return m_text; }
		}

		/// <summary>
		/// Gets the message's context.
		/// </summary>
		/// <value>The context.</value>
		public object Context
		{
			get { return m_context; }
		}

		/// <summary>
		/// Gets the message's exception.
		/// </summary>
		/// <value>The exception.</value>
		public Exception Exception
		{
			get { return m_exception; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Message"/> is acknowledged.
		/// </summary>
		/// <value><c>true</c> if acknowledged; otherwise, <c>false</c>.</value>
		public bool Acknowledged
		{
			get { return m_acknowledged; }
			set
			{
				if (m_acknowledged != value)
				{
					m_acknowledged = value;

					if (AcknowledgedStateChanged != null)
						AcknowledgedStateChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this message requires an acknowledgement.
		/// </summary>
		/// <value><c>true</c> if the message requires an acknowledgement; otherwise, <c>false</c>.</value>
		public bool RequiresAcknowledge
		{
			get { return m_requiredAcknowledge; }
			set { m_requiredAcknowledge = value; }
		}

		#endregion

		#region Constructors and Finalisers

		/// <summary>
		/// Initializes a new instance of the <see cref="Message"/> class.
		/// </summary>
		/// <param name="severity">The severity.</param>
		/// <param name="text">The text.</param>
		public Message(Severity severity, string text)
		{
			m_severity = severity;
			m_text = text;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Message"/> class.
		/// </summary>
		/// <param name="severity">The severity.</param>
		/// <param name="text">The text.</param>
		/// <param name="requiresAcknowledge">if set to <c>true</c> it requires an acknowledgement.</param>
		public Message(Severity severity, string text, bool requiresAcknowledge)
			: this(severity, text)
		{
			m_requiredAcknowledge = requiresAcknowledge;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Message"/> class.
		/// </summary>
		/// <param name="severity">The severity.</param>
		/// <param name="text">The text.</param>
		/// <param name="exception">The exception.</param>
		public Message(Severity severity, string text, Exception exception)
			: this(severity, text)
		{
			m_exception = exception;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Message"/> class.
		/// </summary>
		/// <param name="severity">The severity.</param>
		/// <param name="text">The text.</param>
		/// <param name="exception">The exception.</param>
		/// <param name="context">The context.</param>
		public Message(Severity severity, string text, Exception exception, object context)
			: this(severity, text, exception)
		{
			m_context = context;
		}

		#endregion

		#region IComparable<Message> Members

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
		/// Value
		/// Meaning
		/// Less than zero
		/// This object is less than the <paramref name="other"/> parameter.
		/// Zero
		/// This object is equal to <paramref name="other"/>.
		/// Greater than zero
		/// This object is greater than <paramref name="other"/>.
		/// </returns>
		public int CompareTo(Message other)
		{
			int rtnVal = 0;

			if (other != null)
				rtnVal = Severity - other.Severity;

			return rtnVal;
		}

		#endregion

		#region IComparable Members

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
		/// Value
		/// Meaning
		/// Less than zero
		/// This instance is less than <paramref name="obj"/>.
		/// Zero
		/// This instance is equal to <paramref name="obj"/>.
		/// Greater than zero
		/// This instance is greater than <paramref name="obj"/>.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="obj"/> is not the same type as this instance.
		/// </exception>
		public int CompareTo(object obj)
		{
			int rtnVal = 0;

			if (obj != null && obj is Message)
				rtnVal = CompareTo((Message)obj);

			return rtnVal;
		}

		#endregion

	}
}
