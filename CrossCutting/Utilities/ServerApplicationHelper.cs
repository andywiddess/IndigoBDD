using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Helper class with all the helper functions used by the ServerApplication
    /// </summary>
    public static class ServerApplicationHelper
    {
        /// <summary>
        /// Enumerator for Log Category
        /// </summary>
        public enum enumLogCategory
        { 
            /// <summary>
            /// Log
            /// </summary>
            LOG = 0,
            /// <summary>
            /// Trace
            /// </summary>
            TRC = 1,
            /// <summary>
            /// Telemetry
            /// </summary>
            TEL = 2,
            /// <summary>
            /// Audit
            /// </summary>
            AUD = 3,
            /// <summary>
            /// Global Exception
            /// </summary>
            GEX = 4,
            /// <summary>
            /// SQL Trace
            /// </summary>
            SQL = 5,
            /// <summary>
            /// WCF Trace
            /// </summary>
            WCF = 6,
        }

        /// <summary>
        /// Deployment Type
        /// </summary>
        public enum enumDeploymentType
        { 
            /// <summary>
            /// Deployed of IIS / Web Server
            /// </summary>
            IIS = 0,
            /// <summary>
            /// Deployed as a Service
            /// </summary>
            SERVICE = 1
        }

        /// <summary>
        /// Get the File extensions for the specified category
        /// </summary>
        /// <param name="logCategory"></param>
        /// <returns></returns>
        private static string getFileExtension(enumLogCategory logCategory) 
        {
            switch(logCategory)
            {
                case enumLogCategory.LOG:
                case enumLogCategory.TRC:
                case enumLogCategory.TEL:
                case enumLogCategory.AUD:
                case enumLogCategory.SQL:
                    return "txt";
                case enumLogCategory.GEX:
                    return "xml";
                case enumLogCategory.WCF:
                    return "svclog";
            }
            return null;
        }

        /// <summary>
        /// Get Safe File Name
        /// </summary>
        /// <param name="logCategory"></param>
        /// <param name="applicationName"></param>
        /// <param name="portNo"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetSafeFileName(enumLogCategory logCategory, string applicationName, int portNo = 0, string extension = null)
        {
            //<LogCategory><ApplicationName><Date><Port><ProcessId><ThreadId>.<Extension>
            #if !SILVERLIGHT
            if (portNo == 0 && System.ServiceModel.OperationContext.Current != null)
            {
                portNo = System.ServiceModel.OperationContext.Current.Channel.LocalAddress.Uri.Port;
            }
            #endif
            return string.Format("{0}{1}{2}{3}{4}{5}.{6}",
                Enum.GetName(typeof(enumLogCategory), logCategory),
                applicationName,
                DateTime.Now.ToString("yyyyMMdd"),
                portNo.ToString(),
                System.Diagnostics.Process.GetCurrentProcess().Id.ToString(),
                System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(),
                (extension != null) ? extension : getFileExtension(logCategory)
            );
        }

        /// <summary>
        /// Get Safe File Name for Global Exception
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="ticketNumber">The ticket number.</param>
        /// <returns></returns>
        public static string GetSafeFileNameForGEX(string applicationName, Guid ticketNumber)
        {
            //<LogCategory><ApplicationName><Date><TicketNumber>.<Extension>
            return string.Format("{0}{1}{2}{3}.{4}",
                Enum.GetName(typeof(enumLogCategory), enumLogCategory.GEX),
                applicationName,
                DateTime.Now.ToString("yyyyMMdd"),
                ticketNumber.ToString("N"),
                getFileExtension(enumLogCategory.GEX)
            );
        }

        /// <summary>
        /// Get the Default Root Directory for Logs
        /// </summary>
        /// <param name="deploymentType">Type of the deployment.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        public static string GetLogsDirectory(enumDeploymentType deploymentType, string folderName)
        {
            string logsDirectory = null;
            if (string.IsNullOrEmpty(folderName))
            {
                folderName = "Logs";
            }

            if (System.IO.Path.IsPathRooted(folderName))
            {
                logsDirectory = folderName;
            }
            else
            {
                if (deploymentType == enumDeploymentType.IIS)
                {
                    //IIS Server
                    string currentPhysicalDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                    logsDirectory = currentPhysicalDirectory.Remove(0, "file:///".Length - 2);
                    logsDirectory = logsDirectory + "\\..\\App_Data";
                    logsDirectory = System.IO.Path.Combine(logsDirectory, folderName);
                }
                else if (deploymentType == enumDeploymentType.SERVICE)
                {
                    logsDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "IA"); //TODO: Remove hardcoding
                    logsDirectory = System.IO.Path.Combine(logsDirectory, folderName);
                }
            }

            if (!System.IO.Directory.Exists(logsDirectory))
            {
                System.IO.Directory.CreateDirectory(logsDirectory);
            }

            return logsDirectory;
        }
    }
}
