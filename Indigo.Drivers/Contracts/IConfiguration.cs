using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Indigo.Drivers.Configuration;

namespace Indigo.Drivers.Contracts
{
    public interface IConfiguration
    {
        /// <summary>
        /// Gets or sets the type of configuration.
        /// </summary>
        /// <value>
        /// The type of configuration.
        /// </value>
        ConfigType TypeOfConfiguration { get; }

        /// <summary>
        /// Gets or sets the log file.
        /// </summary>
        /// <value>
        /// The log file.
        /// </value>
        string LogFile { get; }

        /// <summary>
        /// Gets or sets the test link API key.
        /// </summary>
        /// <value>
        /// The test link API key.
        /// </value>
        string TestLinkApiKey { get; }


        /// <summary>
        /// Gets or sets the input directory.
        /// </summary>
        /// <value>
        /// The input directory.
        /// </value>
        string InputDirectory { get; }


        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>
        /// The output directory.
        /// </value>
        string OutputDirectory { get; }

        /// <summary>
        /// Gets or sets the data file.
        /// </summary>
        /// <value>
        /// The data file.
        /// </value>
        string DataFile { get; }
    }
}
