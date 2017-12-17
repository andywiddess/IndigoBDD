using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Logging
{
    public enum LogLevel {Off, Fatal, Error, Warn, Info, Debug, All} 

    
    public interface ILogListener
    {
        void Log(string logInstanceName, string message, string SectionName);

        void Log(LogLevel level, string logInstanceName, string sectionName, string message, Exception exception);
    }
}
