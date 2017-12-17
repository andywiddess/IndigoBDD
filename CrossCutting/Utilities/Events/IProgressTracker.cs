using System;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>Interface for progress tracker.</summary>
	/// <typeparam name="TStatus">The type of the status.</typeparam>
	public interface IProgressTracker<TStatus>
	{
		#region events

		/// <summary>Occurs when progress changes.</summary>
		event EventHandler ProgressChanged;

		/// <summary>Occurs when status changed.</summary>
		event EventHandler StatusChanged;

		#endregion

		#region properties

		/// <summary>Gets or sets the progress.</summary>
		/// <value>The progress.</value>
		double Progress { get; set; }

		/// <summary>Gets or sets the status.</summary>
		/// <value>The status.</value>
		TStatus Status { get; set; }

		/// <summary>Gets or sets the minimum.</summary>
		/// <value>The minimum.</value>
		double Minimum { get; set; }

		/// <summary>Gets or sets the maximum.</summary>
		/// <value>The maximum.</value>
		double Maximum { get; set; }

		/// <summary>Gets or sets the position.</summary>
		/// <value>The position.</value>
		double Position { get; set; }

		#endregion

		#region public interface

		/// <summary>Hijacks event handlers.</summary>
		/// <param name="progressChanged">The progress changed handler (can be <c>null</c>).</param>
		/// <param name="statusChanged">The status changed handler (can be <c>null</c>).</param>
		/// <returns><c>this</c></returns>
		IProgressTracker<TStatus> Hijack(Action<double> progressChanged, Action<TStatus> statusChanged);

		/// <summary>Adjusts the tracker to range (0, <paramref name="maximum"/>).</summary>
		/// <param name="maximum">The maximum.</param>
		/// <returns><c>this</c></returns>
		IProgressTracker<TStatus> Adjust(double maximum);

		/// <summary>Adjusts the tracker to range (<paramref name="minimum"/>, <paramref name="maximum"/>).</summary>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <returns><c>this</c></returns>
		IProgressTracker<TStatus> Adjust(double minimum, double maximum);

		/// <summary>Adjusts the tracker to range (<paramref name="minimum"/>, <paramref name="maximum"/>).</summary>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <param name="position">The current position.</param>
		/// <returns><c>this</c></returns>
		IProgressTracker<TStatus> Adjust(double minimum, double maximum, double position);

		/// <summary>Creates new tracker covering range (<paramref name="minimum"/>, <paramref name="maximum"/>).</summary>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <returns><c>this</c></returns>
		IProgressTracker<TStatus> Range(double minimum, double maximum);

		/// <summary>Creates new tracker covering range (<paramref name="minimum"/>, <paramref name="maximum"/>).</summary>
		/// <typeparam name="TSubStatus">The type of the sub status.</typeparam>
		/// <param name="minimum">The minimum position.</param>
		/// <param name="maximum">The maximum position.</param>
		/// <param name="statusTranslator">The status translator method.</param>
		/// <returns><c>this</c></returns>
		IProgressTracker<TSubStatus> Range<TSubStatus>(
			double minimum, double maximum, Func<TSubStatus, TStatus> statusTranslator);

		#endregion
	}
}
