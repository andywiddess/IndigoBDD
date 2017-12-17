using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Broadcast event that notifies listeners to run the specified report
	/// </summary>
	public class RunReportBroadcastArgs : BroadcastArgs
	{
	
		#region Member Variables

		private readonly object m_ReportFactory;

		#endregion

		#region Constructors and Finalizers

		/// <summary>Initializes a new instance of the <see cref="RunReportBroadcastArgs"/> class.</summary>
		/// <param name="reportFactory">The report factory.</param>
		public RunReportBroadcastArgs(object reportFactory)
		{
			m_ReportFactory = reportFactory;
		}

		#endregion

		#region Public Methods and Properties

		/// <summary>
		/// Gets the report factory
		/// </summary>
		public object ReportFactory
		{
			get { return m_ReportFactory; }
		}

		#endregion
	}
}
