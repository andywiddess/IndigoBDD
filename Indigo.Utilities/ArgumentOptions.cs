using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;

namespace Indigo.CrossCutting.Utilities

{
    /// <summary>
    /// Command Line Options for the Database service to denote the location of the shared folder
    /// </summary>
    public class ArgumentOptions
    {
        /// <summary>
        /// Gets or sets the driver.
        /// </summary>
        /// <value>
        /// The driver.
        /// </value>
        [Option('d', "driver", Required = true, HelpText = "The web driver required")]
        public string Driver { get; set; }

        /// <summary>
        /// Gets or sets the log location.
        /// </summary>
        /// <value>
        /// The log location.
        /// </value>
        [Option('i', "Information", Required = false, HelpText = "The Log file location")]
        public string LogLocation { get; set; }

        /// <summary>
        /// Gets or sets the driver location.
        /// </summary>
        /// <value>
        /// The driver location.
        /// </value>
        [Option('l', "DriverLocation", Required = true, HelpText = "The Driver Location directory")]
        public string DriverLocation { get; set; }

        /// <summary>
        /// Gets or sets the application location.
        /// </summary>
        /// <value>
        /// The application location.
        /// </value>
        [Option('a', "ApplicationLocation", Required = true, HelpText = "The Applications Location directory")]
        public string ApplicationLocation { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        [Option('n', "ApplicationName", Required = true, HelpText = "The Applications Name")]
        public string ApplicationName{ get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ArgumentOptions"/> is verbose.
        /// </summary>
        /// <value>
        ///   <c>true</c> if verbose; otherwise, <c>false</c>.
        /// </value>
        [Option('v', null, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

        /// <summary>
        /// Gets the usage.
        /// </summary>
        /// <returns></returns>
        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            //  or using HelpText.AutoBuild
            var usage = new StringBuilder();
            usage.AppendLine("Indigo 1.1");
            usage.AppendLine("Read user manual for usage instructions...");
            return usage.ToString();
        }
    }
}
