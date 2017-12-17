using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using Indigo.Drivers.Configuration;
using Indigo.Drivers.Contracts;
using Indigo.Implementation;
using log4net;
using log4net.Appender;
using NBehave.Narrator.Framework;

using OpenQA.Selenium;

namespace Indigo
{
    public static class Program
    {
        /// <summary>
        /// Main entry point for the testing of an application
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static int Main(string[] args)
        {
            bool inFailMode = false;

            try
            {
                // Read the application configuration and adjust the log4net file appender to the results location before processing any statements
                IProcessingEngine engine = null;
                IConfiguration config = ConfigHandler.Settings;

                log4net.GlobalContext.Properties["LogName"] = Path.Combine(config.OutputDirectory, config.LogFile);
                log4net.Config.XmlConfigurator.Configure();

                // Check which configuration type we're using and create the processing engine accordingly.
                if (config.TypeOfConfiguration == ConfigType.File)
                {
                    engine = new FileProcessing();
                }
                else
                {
                    engine = new TestLinkProcessing();
                }

                // Perform the execution and report any issues
                inFailMode = !engine.Process(config);
            }
            catch (Exception)
            {
                throw;
            }

            return inFailMode ? 1 : 0;
        }
    }
}
