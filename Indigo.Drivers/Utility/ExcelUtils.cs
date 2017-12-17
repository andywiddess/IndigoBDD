using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using CsvHelper;

using Indigo.CrossCutting.Utilities.Selenium.Contracts;
using Indigo.Drivers.Model;

namespace Indigo.Drivers
{
	public static class ExcelUtils
	{
        #region Implementation

        /// <summary>
        /// Gets the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static IQueryable<IDataModel> Get(string file)
		{
			List<IDataModel> response = new List<IDataModel>();

			if (File.Exists(file))
			{
				using (TextReader txt = new StreamReader(file))
				using (CsvReader csv = new CsvReader(txt))
				{
					csv.Configuration.RegisterClassMap<DataModelMap>();
					csv.Configuration.IgnoreBlankLines = true;
					csv.Configuration.HasHeaderRecord = true;

					// Ignore first row - header only
					csv.Read();

					// Iterate through the list of data rows
					while (csv.Read())
					{
						response.Add(new DataModel()
						{
							Feature = csv.GetField<string>(0),
							DataSet = csv.GetField<int>(1),
							Key = csv.GetField<string>(2),
							Value = csv.GetField<string>(3)
						});
					}
				}
			}
			else
			{
				throw new FileNotFoundException($"{file} does not exist or not available");
			}

			return response.AsQueryable();
		}

		#endregion
	}
}
