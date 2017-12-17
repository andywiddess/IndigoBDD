using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// This is progress tracker which does not report to anything. 
	/// It is used to avoid checking for null every time tracker is passed and it is optional.
	/// </summary>
	/// <typeparam name="TStatus">The type of the status.</typeparam>
	internal class NullProgressTracker<TStatus>: IProgressTracker<TStatus>
	{
		#region static fields

		/// <summary>Default null tracker for give status type.</summary>
		public static readonly NullProgressTracker<TStatus> Default = new NullProgressTracker<TStatus>();

		#endregion

		#region constructor

		/// <summary>
		/// Prevents a default instance of the <see cref="NullProgressTracker&lt;TStatus&gt;"/> class from being created.
		/// </summary>
		private NullProgressTracker()
		{
		}

		#endregion

		#region IProgressTracker<TStatus> Members

		#region events

		/// <summary>Occurs when progress changes.</summary>
		public event EventHandler ProgressChanged { add { } remove { } }

		/// <summary>Occurs when status changed.</summary>
		public event EventHandler StatusChanged { add { } remove { } }

		#endregion

		#region properties

		/// <summary>Gets or sets the progress.</summary>
		/// <value>The progress.</value>
		public double Progress { get { return 0.5; } set { } }

		/// <summary>Gets or sets the status.</summary>
		/// <value>The status.</value>
		public TStatus Status { get { return default(TStatus); } set { } }

		/// <summary>Gets or sets the minimum.</summary>
		/// <value>The minimum.</value>
		public double Minimum { get { return 0.0; } set { } }

		/// <summary>Gets or sets the maximum.</summary>
		/// <value>The maximum.</value>
		public double Maximum { get { return 1.0; } set { } }

		/// <summary>Gets or sets the position.</summary>
		/// <value>The position.</value>
		public double Position { get { return 0.5; } set { } }

		#endregion

		#region public interface

		/// <summary>Hijacks event handlers.</summary>
		/// <param name="progressChanged">The progress changed handler (can be <c>null</c>).</param>
		/// <param name="statusChanged">The status changed handler (can be <c>null</c>).</param>
		/// <returns><c>this</c></returns>
		public IProgressTracker<TStatus> Hijack(Action<double> progressChanged, Action<TStatus> statusChanged)
		{
			return this;
		}

		/// <summary>Adjusts the tracker to range (0, <paramref name="maximum"/>).</summary>
		/// <param name="maximum">The maximum.</param>
		/// <returns><c>this</c></returns>
		public IProgressTracker<TStatus> Adjust(double maximum)
		{
			return this;
		}

		/// <summary>Adjusts the tracker to range (<paramref name="minimum"/>, <paramref name="maximum"/>).</summary>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <returns><c>this</c></returns>
		public IProgressTracker<TStatus> Adjust(double minimum, double maximum)
		{
			return this;
		}

		/// <summary>Adjusts the tracker to range (<paramref name="minimum"/>, <paramref name="maximum"/>).</summary>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <param name="position">The current position.</param>
		/// <returns><c>this</c></returns>
		public IProgressTracker<TStatus> Adjust(double minimum, double maximum, double position)
		{
			return this;
		}

		/// <summary>Creates new tracker covering range (<paramref name="minimum"/>, <paramref name="maximum"/>).</summary>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <returns><c>this</c></returns>
		public IProgressTracker<TStatus> Range(double minimum, double maximum)
		{
			return this;
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
			return NullProgressTracker<TSubStatus>.Default;
		}

		#endregion

		#endregion
	}
}
