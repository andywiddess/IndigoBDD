using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Event fired to signal that the master addressbook should be opened 
	/// </summary>
	public class EditMasterAddressBookBroadcastArgs: BroadcastArgs
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="EditMasterAddressBookBroadcastArgs"/> class.
		/// </summary>
		/// <param name="locked">if set to <c>true</c> [locked].</param>
		public EditMasterAddressBookBroadcastArgs(bool locked)
		{
			Locked = locked;
		}

		/// <summary>
		/// Should the editor be locked
		/// </summary>
		public bool Locked { get; private set; }

	}
}
