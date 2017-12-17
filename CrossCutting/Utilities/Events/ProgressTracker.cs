using System;
using Indigo.CrossCutting.Utilities;
using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Extensions;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Static class with utilities for progress tracker.
	/// </summary>
	public static class ProgressTracker
	{
		/// <summary>Creates the progress tracker.</summary>
		/// <typeparam name="TStatus">The type of the status.</typeparam>
		/// <param name="progressChanged">The progress changed.</param>
		/// <param name="statusChanged">The status changed.</param>
		/// <returns>New progress tracker.</returns>
		public static IProgressTracker<TStatus> Make<TStatus>(
			Action<double> progressChanged = null, Action<TStatus> statusChanged = null)
		{
			return new ProgressTracker<TStatus>(progressChanged, statusChanged);
		}

		/// <summary>Creates the progress tracker.</summary>
		/// <typeparam name="TStatus">The type of the status.</typeparam>
		/// <param name="maximum">The maximum.</param>
		/// <param name="progressChanged">The progress changed.</param>
		/// <param name="statusChanged">The status changed.</param>
		/// <returns>New progress tracker.</returns>
		public static IProgressTracker<TStatus> Make<TStatus>(
			double maximum,
			Action<double> progressChanged = null, Action<TStatus> statusChanged = null)
		{
			return new ProgressTracker<TStatus>(maximum, progressChanged, statusChanged);
		}

		/// <summary>Creates the progress tracker.</summary>
		/// <typeparam name="TStatus">The type of the status.</typeparam>
		/// <param name="minimum">The minimum.</param>
		/// <param name="maximum">The maximum.</param>
		/// <param name="progressChanged">The progress changed.</param>
		/// <param name="statusChanged">The status changed.</param>
		/// <returns>New progress tracker.</returns>
		public static IProgressTracker<TStatus> Make<TStatus>(
			double minimum, double maximum,
			Action<double> progressChanged = null, Action<TStatus> statusChanged = null)
		{
			return new ProgressTracker<TStatus>(minimum, maximum, progressChanged, statusChanged);
		}

		/// <summary>Returns fake progress tracker.</summary>
		/// <typeparam name="TStatus">The type of the status.</typeparam>
		/// <returns>Null progress tracker.</returns>
		public static IProgressTracker<TStatus> Null<TStatus>()
		{
			return NullProgressTracker<TStatus>.Default;
		}

		/// <summary>Ensures that progress tracker is not null. Creates fake one in such cases.</summary>
		/// <typeparam name="TStatus">The type of the status.</typeparam>
		/// <param name="tracker">The tracker.</param>
		/// <returns>Specified progress tracker or a fake one.</returns>
		public static IProgressTracker<TStatus> NotNull<TStatus>(this IProgressTracker<TStatus> tracker)
		{
			return tracker ?? NullProgressTracker<TStatus>.Default;
		}

		/// <summary>Resets tracker to 0%.</summary>
		/// <typeparam name="TStatus">The type of the status field.</typeparam>
		/// <param name="tracker">The tracker.</param>
		/// <returns>Same progress tracker.</returns>
		public static IProgressTracker<TStatus> Reset<TStatus>(this IProgressTracker<TStatus> tracker)
		{
			tracker.Position = tracker.Minimum;
			return tracker;
		}

		/// <summary>Sets progress to 100%.</summary>
		public static void Completed<TStatus>(this IProgressTracker<TStatus> tracker)
		{
			tracker.Position = tracker.Maximum;
		}

		/// <summary>Advances position to given position.</summary>
		/// <typeparam name="TStatus">The type of the status.</typeparam>
		/// <param name="tracker">The tracker.</param>
		/// <param name="position">The position.</param>
		/// <returns>Same progress tracker.</returns>
		public static IProgressTracker<TStatus> AdvanceTo<TStatus>(this IProgressTracker<TStatus> tracker, double position)
		{
			tracker.Position = position;
			return tracker;
		}

		/// <summary>Advances position by given number of units.</summary>
		/// <typeparam name="TStatus">The type of the status.</typeparam>
		/// <param name="tracker">The tracker.</param>
		/// <param name="delta">The delta.</param>
		/// <returns>Same tracker.</returns>
		public static IProgressTracker<TStatus> AdvanceBy<TStatus>(this IProgressTracker<TStatus> tracker, double delta)
		{
			tracker.Position += delta;
			return tracker;
		}
	}

	/// <summary>
	/// Progress tracker. 
	/// Exposes Progress (as a number between 0.0 and 1.0) and a Status (usually text but can be anything).
	/// </summary>
	/// <typeparam name="TStatus">The type of the status.</typeparam>
	public class ProgressTracker<TStatus>: IProgressTracker<TStatus>, IProgressTrackerUplink
	{
		#region events

		/// <summary>Occurs when progress changes.</summary>
		public event EventHandler ProgressChanged;

		/// <summary>Occurs when status changed.</summary>
		public event EventHandler StatusChanged;

		#endregion

		#region fields

		/// <summary>Parent tracker.</summary>
		private readonly IProgressTrackerUplink m_Parent;

		/// <summary>Parent's range minimum.</summary>
		private readonly double m_ParentMaximum;

		/// <summary>Parent's range maximum.</summary>
		private readonly double m_ParentMinimum;

		/// <summary>Status translator.</summary>
		private Func<TStatus, object> m_StatusTranslator;

		/// <summary>Determines if progress was ever updated.</summary>
		private bool m_ProgressUpdated; // = false;

		/// <summary>Cached progress value (0.0 to 1.0)</summary>
		private double m_Progress; // cached progress

		/// <summary>Minimum position.</summary>
		private double m_Minimum = 0.0;

		/// <summary>Maximum position.</summary>
		private double m_Maximum = 1.0;

		/// <summary>Current position.</summary>
		private double m_Position;

		/// <summary>Status value.</summary>
		private TStatus m_Status;
		private bool m_StatusUpdated;

		#endregion

		#region properties

		/// <summary>Gets or sets the progress.</summary>
		/// <value>The progress.</value>
		public double Progress
		{
			get { return m_Progress; }
			set
			{
				if (m_Progress == value && m_ProgressUpdated) return;
				m_Progress = value;
				RecalculatePosition();
				OnProgressUpdated();
			}
		}

		/// <summary>Gets or sets the status.</summary>
		/// <value>The status.</value>
		public TStatus Status
		{
			get { return m_Status; }
			set
			{
				if (object.Equals(m_Status, value) && m_StatusUpdated) return;
				m_Status = value;
				OnStatusUpdated();
			}
		}

		/// <summary>Gets or sets the minimum.</summary>
		/// <value>The minimum.</value>
		public double Minimum
		{
			get { return m_Minimum; }
			set
			{
				if (m_Minimum == value && m_ProgressUpdated) return;
				m_Minimum = value;
				RecalculateProgress();
				OnProgressUpdated();
			}
		}

		/// <summary>Gets or sets the maximum.</summary>
		/// <value>The maximum.</value>
		public double Maximum
		{
			get { return m_Maximum; }
			set
			{
				if (m_Maximum == value && m_ProgressUpdated) return;
				m_Maximum = value;
				RecalculateProgress();
				OnProgressUpdated();
			}
		}

		/// <summary>Gets or sets the position.</summary>
		/// <value>The position.</value>
		public double Position
		{
			get { return m_Position; }
			set
			{
				if (m_Position == value && m_ProgressUpdated) return;
				m_Position = value;
				RecalculateProgress();
				OnProgressUpdated();
			}
		}

		#endregion

		#region constructor

		/// <summary>Special constructor for creating uplinks.</summary>
		/// <param name="parent">The parent.</param>
		/// <param name="minimum">The minimum in parent.</param>
		/// <param name="maximum">The maximum in parent.</param>
		private ProgressTracker(IProgressTrackerUplink parent, double minimum, double maximum)
		{
			Patterns.NoOp(ref m_StatusTranslator);

			m_Parent = parent;
			m_ParentMinimum = minimum;
			m_ParentMaximum = maximum;
		}

		/// <summary>Initializes a new instance of the <see cref="ProgressTracker&lt;TStatus&gt;"/> class.
		/// Minimum and Maximum are assumed to be 0.0 and 1.0 respectively.</summary>
		/// <param name="progressChanged">The progress changed handler (can be <c>null</c>).</param>
		/// <param name="statusChanged">The status changed handler (can be <c>null</c>).</param>
		public ProgressTracker(Action<double> progressChanged = null, Action<TStatus> statusChanged = null)
		{
			Hijack(progressChanged, statusChanged);
		}

		/// <summary>Initializes a new instance of the <see cref="ProgressTracker&lt;TStatus&gt;"/> class.</summary>
		/// <param name="maximum">The maximum position.</param>
		/// <param name="progressChanged">The progress changed handler (can be <c>null</c>).</param>
		/// <param name="statusChanged">The status changed handler (can be <c>null</c>).</param>
		public ProgressTracker(double maximum, Action<double> progressChanged = null, Action<TStatus> statusChanged = null)
		{
			Adjust(0, maximum);
			Hijack(progressChanged, statusChanged);
		}

		/// <summary>Initializes a new instance of the <see cref="ProgressTracker&lt;TStatus&gt;"/> class.</summary>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <param name="progressChanged">The progress changed handler (can be <c>null</c>).</param>
		/// <param name="statusChanged">The status changed handler (can be <c>null</c>).</param>
		public ProgressTracker(double minimum, double maximum, Action<double> progressChanged = null, Action<TStatus> statusChanged = null)
		{
			Adjust(minimum, maximum);
			Hijack(progressChanged, statusChanged);
		}

		#endregion

		#region private implementation

		/// <summary>Called when progress is updated.</summary>
		private void OnProgressUpdated()
		{
			m_ProgressUpdated = true;
			ProgressChanged.SafeRaise(this, EventArgs.Empty, true);

			if (m_Parent != null)
			{
				try
				{
					m_Parent.UplinkPosition(m_Progress * (m_ParentMaximum - m_ParentMinimum) + m_ParentMinimum);
				}
				catch
				{
					Patterns.NoOp(); // I genuinely don't care...
				}
			}
		}

		/// <summary>Called when status is updated.</summary>
		private void OnStatusUpdated()
		{
			m_StatusUpdated = true;
			StatusChanged.SafeRaise(this, EventArgs.Empty, true);

			if (m_Parent != null)
			{
				try
				{
					var status = m_StatusTranslator == null ? (object)Status : m_StatusTranslator(Status);
					m_Parent.UplinkStatus(status);
				}
				catch
				{
					Patterns.NoOp(); // I genuinely don't care...
				}
			}
		}

		/// <summary>Recalculates the progress (knowing position).</summary>
		private void RecalculateProgress()
		{
			m_Progress = ((m_Position - m_Minimum) / (m_Maximum - m_Minimum)).EnforceRange(0, 1);
		}

		/// <summary>Recalculates the position (knowing progress).</summary>
		private void RecalculatePosition()
		{
			m_Position = m_Progress * (m_Maximum - m_Minimum) + m_Minimum;
		}

		#endregion

		#region public interface

		/// <summary>Hijacks event handlers.</summary>
		/// <param name="progressChanged">The progress changed handler (can be <c>null</c>).</param>
		/// <param name="statusChanged">The status changed handler (can be <c>null</c>).</param>
		/// <returns><c>this</c></returns>
		public IProgressTracker<TStatus> Hijack(Action<double> progressChanged, Action<TStatus> statusChanged)
		{
			if (progressChanged != null)
				ProgressChanged += (sender, e) => progressChanged((sender as ProgressTracker<TStatus>).Progress);
			if (statusChanged != null)
				StatusChanged += (sender, e) => statusChanged((sender as ProgressTracker<TStatus>).Status);
			return this;
		}

		/// <summary>Adjusts the tracker to range (0, <paramref name="maximum"/>).</summary>
		/// <param name="maximum">The maximum.</param>
		/// <returns><c>this</c></returns>
		public IProgressTracker<TStatus> Adjust(double maximum)
		{
			m_Maximum = maximum;
			m_Position = m_Position.EnforceRange(m_Minimum, m_Maximum);
			RecalculateProgress();
			OnProgressUpdated();
			return this;
		}

		/// <summary>Adjusts the tracker to range (<paramref name="minimum"/>, <paramref name="maximum"/>).</summary>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <returns><c>this</c></returns>
		public IProgressTracker<TStatus> Adjust(double minimum, double maximum)
		{
			m_Minimum = minimum;
			m_Maximum = maximum;
			m_Position = m_Position.EnforceRange(m_Minimum, m_Maximum);
			RecalculateProgress();
			OnProgressUpdated();
			return this;
		}

		/// <summary>Adjusts the tracker to range (<paramref name="minimum"/>, <paramref name="maximum"/>).</summary>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <param name="position">The current position.</param>
		/// <returns><c>this</c></returns>
		public IProgressTracker<TStatus> Adjust(double minimum, double maximum, double position)
		{
			m_Minimum = minimum;
			m_Maximum = maximum;
			m_Position = position.EnforceRange(m_Minimum, m_Maximum);
			RecalculateProgress();
			OnProgressUpdated();
			return this;
		}

		/// <summary>Creates new tracker covering range (<paramref name="minimum"/>, <paramref name="maximum"/>).</summary>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <returns><c>this</c></returns>
		public IProgressTracker<TStatus> Range(double minimum, double maximum)
		{
			return new ProgressTracker<TStatus>(this, minimum, maximum);
		}

		/// <summary>Creates new tracker covering range (<paramref name="minimum"/>, <paramref name="maximum"/>).</summary>
		/// <typeparam name="TSubStatus">The type of the sub status.</typeparam>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <param name="statusTranslator">The status translator method.</param>
		/// <returns><c>this</c></returns>
		public IProgressTracker<TSubStatus> Range<TSubStatus>(
			double minimum, double maximum, Func<TSubStatus, TStatus> statusTranslator)
		{
			return new ProgressTracker<TSubStatus>(this, minimum, maximum)
			{
				m_StatusTranslator = s => statusTranslator(s)
			};
		}

		#endregion

		#region IProgressTrackerUplink Members

		/// <summary>Passes position change up.</summary>
		/// <param name="position">The position.</param>
		void IProgressTrackerUplink.UplinkPosition(double position)
		{
			Position = position;
		}

		/// <summary>Passes status change up.</summary>
		/// <param name="status">The status.</param>
		void IProgressTrackerUplink.UplinkStatus(object status)
		{
			Status = (TStatus)status;
		}

		#endregion
	}
}
