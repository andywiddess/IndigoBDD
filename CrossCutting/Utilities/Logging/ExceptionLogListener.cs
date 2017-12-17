using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Logging
{
    public class ExceptionLogListener:ILogListener
    {
        /// <summary>
        /// Write the comment to the xml file
        /// </summary>
        /// <param name="logFile">The log file.</param>
        /// <param name="message">The message.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <exception cref="System.Exception"></exception>
        public void Log(string logFile, string message, string sectionName)
        {
            if (message.StartsWith("error", StringComparison.CurrentCultureIgnoreCase) ||
                message.StartsWith("fault", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Logs the specified level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="logInstanceName">Name of the log instance.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <exception cref="System.Exception"></exception>
        public void Log(LogLevel level, string logInstanceName, string sectionName, string message, Exception exception)
        {
            if (exception != null)
                throw (exception);
            if(level == LogLevel.Error || level== LogLevel.Fatal)
                
                throw new Exception(message);
        }
    }
}
