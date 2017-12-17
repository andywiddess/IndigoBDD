using System;
using System.Collections.Generic;
using Indigo.CrossCutting.Utilities.Collections;
using Indigo.CrossCutting.Utilities.Exceptions;

namespace Indigo.CrossCutting.Utilities.Error
{
	/// <summary>
	/// A structured list of messages, with the ability to add, remove, sort messages
	/// </summary>
	public class Messages: IEnumerable<Message>
	{

		#region Member Variables

		private readonly List<Message> m_Messages = new List<Message>();

		/// <summary>The default name for the messages, these cannot be translated as we have no resource translation in Core</summary>
		private string m_name = "Messages";
		private bool m_isLocked; // = false;

		#endregion

		#region Event Definitions

		/// <summary>Event fired when any of the contained messages has changed</summary>
		public event EventHandler MessagesChanged;

		/// <summary>Event fired when the message objects have been sorted</summary>
		public event EventHandler MessagesSorted;

		/// <summary>Event fired when the <c>Name</c> is changed</summary>
		public event EventHandler NameChanged;

		#endregion

		#region Constructors and Finalisers

		/// <summary>
		/// Initializes a new instance of the <see cref="Messages"/> class.
		/// </summary>
		public Messages()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Messages"/> class.
		/// </summary>
		/// <param name="name">The collection's name.</param>
		public Messages(string name)
		{
			m_name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Messages"/> class.
		/// </summary>
		/// <param name="name">The collection's name.</param>
		/// <param name="messages">The existing messages.</param>
		public Messages(string name, IEnumerable<Message> messages)
			: this(name)
		{
			m_Messages = new List<Message>(messages);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Messages"/> class.
		/// </summary>
		/// <param name="messages">The messages.</param>
		public Messages(IEnumerable<Message> messages)
			: this(String.Empty, messages)
		{ }

		#endregion

		#region IEnumerable<Message> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<Message> GetEnumerator()
		{
			return m_Messages.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return m_Messages.GetEnumerator();
		}

		#endregion

		#region Public Methods and Accessors

		/// <summary>
		/// Gets a value indicating whether any message requires acknowledgment.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [any message requires acknowledgment]; otherwise, <c>false</c>.
		/// </value>
		public bool AnyMessageRequiresAcknowledgment
		{
			get { return m_Messages.Exists(message => message.RequiresAcknowledge); }
		}

		/// <summary>
		/// Gets a value indicating whether all messages which require acknowledgement has been acknowledged.
		/// </summary>
		/// <value>
		/// 	<c>true</c> all messages which require acknowledgement has been acknowledged; otherwise, <c>false</c>.
		/// </value>
		public bool AllMessagesAcknowledged
		{
			get
			{
				return m_Messages.TrueForAll(message => !message.RequiresAcknowledge || message.Acknowledged);
			}
		}

		/// <summary>
		/// Gets the collection's name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return m_name; }
			set
			{
				if (IsLocked)
					throw new MessagesLockedException();

				if (m_name != value)
				{
					m_name = value;

					if (NameChanged != null)
						NameChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets the read only view.
		/// </summary>
		/// <value>The read only view.</value>
		public ReadOnlyList<Message> ReadOnlyView
		{
			get { return new ReadOnlyList<Message>(m_Messages); }
		}

		/// <summary>
		/// Locks this message collection, prevents you from Adding, Removing or Clearing any of the items
		/// </summary>
		public void Lock()
		{
			m_isLocked = true;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is locked.
		/// </summary>
		/// <value><c>true</c> if this instance is locked; otherwise, <c>false</c>.</value>
		public bool IsLocked
		{
			get { return m_isLocked; }
		}

		/// <summary>
		/// Adds the specified <c>Message</c>.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <exception cref="MessagesLockedException">If the <c>Messages</c> object is locked</exception>
		public void Add(Message message)
		{
			if (IsLocked)
				throw new MessagesLockedException();

			if (message != null)
			{
				m_Messages.Add(message);

				if (MessagesChanged != null)
					MessagesChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Adds the range of <c>Message</c> items
		/// </summary>
		/// <param name="messages">The messages.</param>
		/// <exception cref="MessagesLockedException">If the <c>Messages</c> object is locked</exception>
		public void AddRange(IEnumerable<Message> messages)
		{
			if (IsLocked)
				throw new MessagesLockedException();

			if (Transforms.Count<Message>(messages) > 0)
			{
				m_Messages.AddRange(messages);

				if (MessagesChanged != null)
					MessagesChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Determines whether this contains the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns>
		/// 	<c>true</c> if this contains the specified message; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(Message message)
		{
			return m_Messages.Contains(message);
		}

		/// <summary>
		/// Removes the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns>
		///   <c>true</c> if item is successfully removed; otherwise, <c>false</c>. 
		///   This method also returns <c>false</c> if item was not found in the Messages
		/// </returns>
		/// <exception cref="MessagesLockedException">If the <c>Messages</c> object is locked</exception>
		public bool Remove(Message message)
		{
			if (IsLocked)
				throw new MessagesLockedException();

			bool removed = m_Messages.Remove(message);

			if (removed && MessagesChanged != null)
				MessagesChanged(this, EventArgs.Empty);

			return removed;
		}

		/// <summary>
		/// Clears all of the messages
		/// </summary>
		/// <exception cref="MessagesLockedException">If the <c>Messages</c> object is locked</exception>
		public void Clear()
		{
			if (IsLocked)
				throw new MessagesLockedException();

			if (m_Messages.Count > 0)
			{
				m_Messages.Clear();

				if (MessagesChanged != null)
					MessagesChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Clears all of the messages of a specified severity.
		/// </summary>
		/// <param name="severity">The severity.</param>
		/// <exception cref="MessagesLockedException">If the <c>Messages</c> object is locked</exception>
		public void Clear(Severity severity)
		{
			if (IsLocked)
				throw new MessagesLockedException();

			if (m_Messages.RemoveAll(message => message.Severity == severity) > 0)
			{
				if (MessagesChanged != null)
					MessagesChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Clears all of the messages with a severity above the specific severity
		/// </summary>
		/// <param name="severity">The severity.</param>
		/// <exception cref="MessagesLockedException">If the <c>Messages</c> object is locked</exception>
		public void ClearAbove(Severity severity)
		{
			if (IsLocked)
				throw new MessagesLockedException();

			if (m_Messages.RemoveAll(message => message.Severity > severity) > 0)
			{
				if (MessagesChanged != null)
					MessagesChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Clears all of the messages with a severity below the specific severity
		/// </summary>
		/// <param name="severity">The severity.</param>
		/// <exception cref="MessagesLockedException">If the <c>Messages</c> object is locked</exception>
		public void ClearBelow(Severity severity)
		{
			if (IsLocked)
				throw new MessagesLockedException();

			if (m_Messages.RemoveAll(message => message.Severity < severity) > 0)
			{
				if (MessagesChanged != null)
					MessagesChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Provide an enumeration of all messages of a specific severity.
		/// </summary>
		/// <param name="severity">The severity.</param>
		/// <returns>The messages of a specific severity</returns>
		public IEnumerable<Message> MessagesBySeverity(Severity severity)
		{
			foreach (Message message in m_Messages)
			{
				if (message.Severity == severity)
					yield return message;
			}
		}

		/// <summary>
		/// Counts the number of messages
		/// </summary>
		/// <returns></returns>
		public int Count
		{
			get { return m_Messages.Count; }
		}

		/// <summary>
		/// Gets the count of all messages of a specific severity
		/// </summary>
		/// <param name="severity">The severity.</param>
		/// <returns>The count of message at the specified severity</returns>
		public int CountBySeverity(Severity severity)
		{
			return m_Messages.Count(message => message.Severity == severity);
		}

		/// <summary>
		/// Sorts the messages with the most severe messages first
		/// </summary>
		public void Sort()
		{
			Sort(true);
		}

		/// <summary>
		/// Sorts the messages with the option of most severe or least severe first
		/// </summary>
		/// <param name="mostSevereFirst">if set to <c>true</c> the show them from most severe to lowest severity.</param>
		public void Sort(bool mostSevereFirst)
		{
			m_Messages.Sort((left, right) =>
			{
				if (mostSevereFirst)
					return left.CompareTo(right);
				else
					return right.CompareTo(left);
			});

			if (MessagesSorted != null)
				MessagesSorted(this, EventArgs.Empty);
		}

		/// <summary>
		/// Sorts the messages
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		public void Sort(IComparer<Message> comparer)
		{
			m_Messages.Sort(comparer);

			if (MessagesSorted != null)
				MessagesSorted(this, EventArgs.Empty);
		}

		/// <summary>
		/// Sorts the messages
		/// </summary>
		/// <param name="comparison">The comparison.</param>
		public void Sort(Comparison<Message> comparison)
		{
			m_Messages.Sort(comparison);

			if (MessagesSorted != null)
				MessagesSorted(this, EventArgs.Empty);
		}

		#endregion

	}
}
