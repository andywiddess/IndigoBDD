using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EA.AutomatedTesting.Indigo.Contracts;

namespace Indigo.CrossCutting.Utilities
{
	/// <summary>
	/// Singleton to support the definition of a selected driver
	/// </summary>
	public class AutomationInstance
	{
		#region Members
		/// <summary>
		/// Gets or sets the driver.
		/// </summary>
		/// <value>
		/// The driver.
		/// </value>
		public IDriver Driver { get; set; }

		/// <summary>
		/// The instance
		/// </summary>
		public static AutomationInstance INSTANCE = new AutomationInstance();
		#endregion

		#region Constructors

		/// <summary>
		/// Prevents a default instance of the <see cref="AutomationInstance" /> class from being created.
		/// </summary>
		private AutomationInstance()
		{

		}
		#endregion

		#region Properties
		#endregion
	}
}
