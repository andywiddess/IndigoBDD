using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsvHelper.Configuration;

using Indigo.CrossCutting.Utilities.Selenium.Contracts;

namespace Indigo.Drivers.Model
{
	public sealed class DataModelMap 
		: ClassMap<IDataModel>
	{
		#region Constructors

		public DataModelMap()
		{
			AutoMap();
		}
		#endregion
	}
}
