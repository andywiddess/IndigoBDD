using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NBehave.Narrator.Framework;

using Indigo.Drivers.Configuration;
using Indigo.Drivers.Contracts;

namespace Indigo.Implementation
{
    public class FileProcessing
        : IProcessingEngine
    {
        #region Members
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessing" /> class.
        /// </summary>
        public FileProcessing()
        {

        }
        #endregion

        #region Implementation

        /// <summary>
        /// Performs an Indigo process
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        public bool Process(IConfiguration configuration)
        {
            bool response = true;

            // Iterate through the list of files in alphabetical order
            Directory.GetFiles($@"{configuration.InputDirectory}")
                .Where(x => x.Contains(".feature"))
                .OrderBy(x => x)
                .ToList()
                .ForEach(x =>
                {
                    try
                    {
                        x.ExecuteFile();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Feature {x} failed with the following exception\n{ex.ToString()}\n");
                        response = false;
                    }
                });

            return response;
        }
        #endregion
    }
}
