using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Indigo.CrossCutting.Utilities.Logging
{
    public class TextFileLogListener : ILogListener
    {
        /// <summary>
        /// Write the Header tag in the Txt Based XML file  if needed.
        /// Not all calls will include a valid logFile
        /// </summary>
        /// <param name="logFile">The log file.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        private XElement getLogFileHeader(string logFile, string sectionName)
        {
            XElement element = null;
            if (!string.IsNullOrEmpty(logFile))
            {
                if (!File.Exists(logFile))
                {
                    createLogFile(logFile);
                }
                XDocument logXMLDoc = new XDocument(new XElement("SepuraLogging")); //Root Element
                element = logXMLDoc.Root.Elements().LastOrDefault(elem => elem.Name == sectionName);
                if (element == null)
                {
                    logXMLDoc.Root.Add(new XElement(sectionName));
                    element = logXMLDoc.Root.Elements().Last(elem => elem.Name == sectionName);
                    XElement newElement = new XElement("StartTime");
                    newElement.SetValue(DateTime.Now.ToString());
                    element.Add(newElement);
                }
            }
            return element;
        }

        /// <summary>
        /// Create the XML file if it does not exist
        /// </summary>
        /// <param name="logFile"></param>
        private void createLogFile(string logFile)
        {
            if (!string.IsNullOrEmpty(logFile))
            {
                XDocument logXMLdoc = new XDocument(new XElement("SepuraLogging"));
                logXMLdoc.Save(logFile);
            }
        }

        /// <summary>
        /// Write the comment to the file
        /// </summary>
        /// <param name="logFilePath"></param>
        /// <param name="message"></param>
        /// <param name="sectionName"></param>
        public void Log(string logFilePath, string message, string sectionName)
        {
            if (message.StartsWith("error", StringComparison.CurrentCultureIgnoreCase) ||
                message.StartsWith("fault", StringComparison.CurrentCultureIgnoreCase))
            {
                Log(logFilePath, "Error", message, sectionName);
            }
            else
            {
                Log(logFilePath, "Information", message, sectionName);
            }
        }

        /// <summary>
        /// this.Log a message to the log file
        /// Write with the given tag name
        /// Not all calls will include a valid logFile
        /// </summary>
        /// <param name="logFilePath">The log file path.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="sectionName">Name of the section.</param>
        public void Log(string logFilePath, string elementName, string comment, string sectionName)
        {
            try
            {
                XElement element = getLogFileHeader(logFilePath, sectionName);
                if (element != null)
                {
                    XElement newElement = writeElement(elementName, comment);
                    element.Add(newElement);
                    System.IO.File.AppendAllText(logFilePath, element.Document.ToString() + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Cannot write comment to log file " + logFilePath + " : " + e.Message);
            }
        }

        /// <summary>
        /// Implements ILogListener interface log by Level
        /// </summary>
        /// <param name="level"></param>
        /// <param name="logFilePath"></param>
        /// <param name="sectionName"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Log(LogLevel level, string logFilePath, string sectionName, string message, Exception exception)
        {
            XElement element = null;
            try
            {
                element = getLogFileHeader(logFilePath, sectionName);
                if (element != null)
                {
                    XElement newElement = writeElement(Enum.GetName(typeof(LogLevel), level), message);
                    element.Add(newElement);
                }
                System.IO.File.AppendAllText(logFilePath, element.Document.ToString() + Environment.NewLine, Encoding.UTF8);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Cannot write comment to log file " + logFilePath + " : " + e.Message);
            }


            
        }

        private XElement writeElement(string elementName, string message)
        {
            XElement newElement = new XElement(elementName);
            newElement.SetValue(message);
            return newElement;
        }
    }
}
