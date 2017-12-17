using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Class to write Text files full of XML fragments that contain telemetry information.
    /// </summary>
    public class TelemetryTextFileListener : ITelemetryListener
    {

        private string logFolder = "";
        public TelemetryTextFileListener(string folder)
        {
            this.logFolder = folder;
        }
        
        public void WriteData(
            string operationName, 
            float measurement, 
            DateTime startDateTime, 
            Indigo.CrossCutting.Utilities.CallStatus.BusinessOperation businessOperation)
        {
            if (this.logFolder == null) return;

            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            string fileName = ServerApplicationHelper.GetSafeFileName(
                ServerApplicationHelper.enumLogCategory.TEL,
                TelemetryListenerFactory.ApplicationName);

            XDocument xDocument = new XDocument();
            XElement rootElement = new XElement("Telemetry");

            rootElement.Add(new XElement("Operation",operationName));
            rootElement.Add(new XElement("Started",startDateTime));
            rootElement.Add(new XElement("Measure",measurement));
            if (businessOperation != null)
            {
                rootElement.Add(new XElement("BusinessOperation",
                    new XElement("CallID", businessOperation.CallID),
                    new XElement("OperationID", businessOperation.ID),
                    new XElement("Description", businessOperation.Description),
                    new XElement("OriginatingUser", businessOperation.OriginatingUser)));

            }
            xDocument.Add(rootElement);
            
            System.IO.Stream stream = Indigo.CrossCutting.Utilities.Logging.LoggingFilePool.GetBufferedOutputStream(System.IO.Path.Combine(this.logFolder, fileName));
            byte[] bytes = Encoding.UTF8.GetBytes(xDocument.ToString() + Environment.NewLine);
            stream.Write(bytes, 0, bytes.Length);
            return;
        }

        
    }
}
