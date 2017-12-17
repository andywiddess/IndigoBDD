using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using Indigo.CrossCutting.Utilities;
//using SepuraInstaller;

namespace Indigo.CrossCutting.Utilities.Logging
{
    public class SepuraXMLLogListener : ILogListener
    {
        /// <summary>
        /// Write the Header tag in the XML file  if needed.
        /// This tag will be used for all entries of this instance of this run of Powershell
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
                XDocument logXMLDoc = XDocument.Load(logFile);
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
        /// Write the comment to the xml file
        /// </summary>
        /// <param name="logFile">The log file.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="sectionName">Name of the section.</param>
        public void Log(string logFile, string comment, string sectionName)
        {
            if (comment.StartsWith("error", StringComparison.CurrentCultureIgnoreCase) ||
                comment.StartsWith("fault", StringComparison.CurrentCultureIgnoreCase))
            {
                Log(logFile, "Error", comment, sectionName);
            }
            else
            {
                Log(logFile, "Information", comment, sectionName);
            }
        }




        /// <summary>
        /// this.Log a message to the log file
        /// Write with the given tag name
        /// Not all calls will include a valid logFile
        /// </summary>
        /// <param name="logInstanceName">Name of the log instance.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="sectionName">Name of the section.</param>
        public void Log(string logInstanceName, string elementName, string comment, string sectionName)
        {
            try
            {
                XElement element = getLogFileHeader(logInstanceName, sectionName);
                if (element != null)
                {
                    XElement newElement = writeElement(elementName, comment);
                    element.Add(newElement);
                    element.Document.Save(logInstanceName);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Cannot write comment to log file " + logInstanceName + " : " + e.Message);
            }
        }

        /// <summary>
        /// Implements ILogListener interface log by Level
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="logInstanceName">Name of the log instance.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Log(LogLevel level, string logInstanceName, string sectionName, string message, Exception exception)
        {
            try
            {
                XElement element = getLogFileHeader(logInstanceName, sectionName);
                if (element != null)
                {
                    XElement newElement = writeElement(Enum.GetName(typeof(LogLevel), level), message);
                    element.Add(newElement);
                    element.Document.Save(logInstanceName);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Cannot write comment to log file " + logInstanceName + " : " + e.Message);
            }
        }


        /// <summary>
        /// Writes the element.
        /// </summary>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private XElement writeElement(string elementName, string message)
        {
            XElement newElement = new XElement(elementName);
            newElement.SetValue(message);
            return newElement;
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
        /// this.log a message and the errors associated with the callStatus
        /// </summary>
        /// <param name="logFile">The log file.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="callStatus">The call status.</param>
        /// <param name="sectionName">Name of the section.</param>
        public void Log(string logFile, string comment, CallStatus callStatus, string sectionName)
        {
            Log(logFile, comment, sectionName);
            if (callStatus != null)
            {
                foreach (ValidationError error in callStatus.Messages)
                {
                    Log(logFile, "ErrorMessage", error.Message);
                }
            }
        }

    }
}