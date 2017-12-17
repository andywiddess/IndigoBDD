using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Indigo.CrossCutting.Utilities

{
    /// <summary>
    /// Generic Argument validation for all KPI Services
    /// </summary>
    public class Arguments
    {
        #region Constants
        private const string IS_CHROME = "CHROME";
        private const string IS_FIREFOX = "FIREFOX";
        private const string IS_IE = "IE";
        private const string IS_WINIUM = "WINIUM";
        private const string IS_WINAPPDRIVER = "WINAPPDRIVER";
        #endregion

        #region Members
        /// <summary>
        /// The type
        /// </summary>
        public DriverType Type = DriverType.CHROME;

        /// <summary>
        /// The location
        /// </summary>
        public string Location = string.Empty;

        /// <summary>
        /// The driver location
        /// </summary>
        public string DriverLocation = string.Empty;

        /// <summary>
        /// The application location
        /// </summary>
        public string ApplicationLocation = string.Empty;

        /// <summary>
        /// The application name
        /// </summary>
        public string ApplicationName = string.Empty;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Arguments"/> class.
        /// </summary>
        public Arguments()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Arguments"/> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ArgumentException">Unsupported browser</exception>
        public Arguments(string[] args)
        {
            string currentLocation = string.Empty;

            try
            {
                var options = new ArgumentOptions();
                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    // consume Options instance properties
                    if (options.Verbose)
                        Console.WriteLine(options.Driver);

                    // Set the Log Location
                    Location = (!string.IsNullOrEmpty(options.LogLocation) ? options.LogLocation : Directory.GetCurrentDirectory());
                    DriverLocation = (!string.IsNullOrEmpty(options.DriverLocation) ? options.DriverLocation : Directory.GetCurrentDirectory());
                    ApplicationLocation = (!string.IsNullOrEmpty(options.ApplicationLocation) ? options.ApplicationLocation : string.Empty);
                    ApplicationName = (!string.IsNullOrEmpty(options.ApplicationName) ? options.ApplicationName : string.Empty);

                    // Switch type
                    if (options.Driver.ToUpper().Equals(IS_CHROME))
                        Type = DriverType.CHROME;
                    else if (options.Driver.ToUpper().Equals(IS_FIREFOX))
                        Type = DriverType.FIREFOX;
                    else if (options.Driver.ToUpper().Equals(IS_IE))
                        Type = DriverType.IE;
                    else
                    {
                        Console.WriteLine(string.Format("{0} - Unsupported browser type", options.Driver));
                        throw new ArgumentException("Unsupported browser");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception interpreting the command line arguments for " + ex.ToString());
            }
        }
        #endregion
    }
}