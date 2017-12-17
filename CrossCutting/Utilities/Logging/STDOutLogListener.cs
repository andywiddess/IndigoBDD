using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Logging
{
    public class STDOutLogListener: ILogListener
    {

        public void Log(string logFile, string message, string sectionName)
        {
            Console.WriteLine(sectionName);
            Console.WriteLine(message);
        }

        public void Log(LogLevel level, string logInstanceName, string sectionName, string message, Exception exception)
        {
            Console.WriteLine(string.Format("*** {0} ***", Enum.GetName(typeof(LogLevel),level).ToUpper()));
            Console.WriteLine(sectionName);
            Console.WriteLine(message);
            if (exception != null)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }
    }
}
