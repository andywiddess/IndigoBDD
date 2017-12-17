using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Indigo.Drivers.Contracts;
using NSubstitute.Routing.Handlers;

namespace Indigo.Drivers.Configuration
{
    /// <summary>
    /// Configuration Handler
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    /// <seealso cref="Indigo.Drivers.Contracts.IConfiguration" />
    public class ConfigHandler
            : ConfigurationSection,
              IConfiguration
    {
        #region Statics
        /// <summary>
        /// The settings
        /// </summary>
        private static readonly IConfiguration settings = ConfigurationManager.GetSection("ConfigHandler") as IConfiguration;

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public static IConfiguration Settings => settings;
        #endregion

        #region Implementation

        /// <summary>
        /// Gets or sets the type of configuration.
        /// </summary>
        /// <value>
        /// The type of configuration.
        /// </value>
        public ConfigType TypeOfConfiguration => _TypeOfConfiguration;

        /// <summary>
        /// Gets or sets the log file.
        /// </summary>
        /// <value>
        /// The log file.
        /// </value>
        public string LogFile => _LogFile;

        /// <summary>
        /// Gets or sets the test link API key.
        /// </summary>
        /// <value>
        /// The test link API key.
        /// </value>
        public string TestLinkApiKey => _TestLinkApiKey;


        /// <summary>
        /// Gets or sets the input directory.
        /// </summary>
        /// <value>
        /// The input directory.
        /// </value>
        public string InputDirectory => _InputDirectory;


        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>
        /// The output directory.
        /// </value>
        public string OutputDirectory
        {
            get
            {
                return !string.IsNullOrEmpty(_selectedDirectoryForResults)
                    ? _selectedDirectoryForResults
                    : translate();
            }
        }

        /// <summary>
        /// Gets or sets the data file.
        /// </summary>
        /// <value>
        /// The data file.
        /// </value>
        public string DataFile => _DataFile;

        /// <summary>
        /// The selected directory for results
        /// </summary>
        private string _selectedDirectoryForResults = string.Empty;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigHandler" /> class.
        /// </summary>
        public ConfigHandler()
        {

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets or sets the type of configuration.
        /// </summary>
        /// <value>
        /// The type of configuration.
        /// </value>
        [ConfigurationProperty("execution-type", DefaultValue = ConfigType.File, IsRequired = true)]
        private ConfigType _TypeOfConfiguration
        {
            get => (ConfigType)this["execution-type"];
        }

        /// <summary>
        /// Gets or sets the test link API key.
        /// </summary>
        /// <value>
        /// The test link API key.
        /// </value>
        [ConfigurationProperty("api-key", IsRequired = false)]
        //[StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|", MinLength = 0, MaxLength = 256)]
        private string _TestLinkApiKey
        {
            get => (string)this["api-key"];
        }

        /// <summary>
        /// Gets or sets the log file.
        /// </summary>
        /// <value>
        /// The log file.
        /// </value>
        [ConfigurationProperty("log-file", IsRequired = false)]
        //[StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|", MinLength = 0, MaxLength = 256)]
        private string _LogFile
        {
            get => (string)this["log-file"];
        }

        /// <summary>
        /// Gets or sets the input directory.
        /// </summary>
        /// <value>
        /// The input directory.
        /// </value>
        [ConfigurationProperty("input-directory", IsRequired = false)]
        //[StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|", MinLength = 0, MaxLength = 256)]
        private string _InputDirectory
        {
            get => (string)this["input-directory"];
        }


        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>
        /// The output directory.
        /// </value>
        [ConfigurationProperty("output-directory", IsRequired = false)]
        //[StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|", MinLength = 1, MaxLength = 256)]
        private string _OutputDirectory
        {
            get => (string)this["output-directory"];
        }

        /// <summary>
        /// Gets or sets the data file.
        /// </summary>
        /// <value>
        /// The data file.
        /// </value>
        [ConfigurationProperty("feature-datafile", IsRequired = false)]
        //[StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|", MinLength = 1, MaxLength = 256)]
        private string _DataFile
        {
            get => (string)this["feature-datafile"];
        }

        /// <summary>
        /// Translates the specified source.
        /// </summary>
        /// <returns></returns>
        private string translate()
        {
            DateTime myDate = DateTime.Now;
            var newString = _OutputDirectory.Replace("$$", $"{myDate.Year}{myDate.Month + 1}{myDate.Day + 1}");
            var response = newString;
            int index = 1;

            while (Directory.Exists(response))
            {
                response = $"{newString}_{index}";
                index++;
            }

            Directory.CreateDirectory(response);
            _selectedDirectoryForResults = response;
            return response;
        }
        #endregion
    }
}
