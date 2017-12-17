using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	#region interface ISuspendableEvents

	/// <summary>
	/// Interface providing an easy way to suspend and resume events for an object.
	/// </summary>
	public interface ISuspendableEvents
	{
		/// <summary>
		/// Suspends events.
		/// </summary>
		void SuspendEvents();

		/// <summary>
		/// Resumes events.
		/// </summary>
		void ResumeEvents();
	}

	#endregion

	#region class EventLock

	/// <summary>
	/// Allows to use <c>using</c> keyword to suspend and resume events on <see cref="ISuspendableEvents"/>.
	/// </summary>
	public class EventLock: IDisposable
	{
		#region fields

		/// <summary>
		/// The component.
		/// </summary>
		private readonly ISuspendableEvents m_Component;

		#endregion

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="EventLock"/> class.
		/// Suspends events for given <paramref name="component"/>.
		/// </summary>
		/// <param name="component">The component.</param>
		public EventLock(ISuspendableEvents component)
		{
			m_Component = component;
			if (m_Component != null)
				m_Component.SuspendEvents();
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// Resumes events on given object.
		/// </summary>
		public void Dispose()
		{
			if (m_Component != null)
				m_Component.ResumeEvents();
		}

		#endregion
	}

	#endregion
}
