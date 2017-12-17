using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Factory for creating instances of the telemetry files, setting their location and flushing their contents
    /// </summary>
    public static class TelemetryListenerFactory
    {
        
        private static string applicationName = "";

        /// <summary>
        /// The Name of the Application which will form part of the Log File name
        /// </summary>
        public static string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }



        /// <summary>
        /// The list of items that will have the data stored - set by the Configuration Server
        /// If an item is not in this dictionary the system does not store data to telemetryData
        /// </summary>
        private static List<string> telemetryItemsRecorded = new List<string>();

        public static List<string> GetTelemetryItems()
        {
            return telemetryItemsRecorded;
        }

        /// <summary>
        /// Set up the items to be included in the Telemetry
        /// If an item is not in this dictionary the system does not store data to telemetryData
        /// This will also clear the current Telemetry Data
        /// </summary>
        /// <param name="items"></param>
        public static void SetCurrentTelemetryItems(List<string> items)
        {
            setCurrentTelemetryItems(items);
        }

        /// <summary>
        /// Set up the items to be included in the Telemetry
        /// Called from each ServerApplication using TelemetryItems in the Setting File
        /// If the list contains "*" then all Telemetry items will be set
        /// If the list contains "none" or "None" then all Telemetry items will be cleared ("*") takes precedence
        /// This will also clear the current Telemetry Data
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="assembly">The assembly.</param>
        #if !SILVERLIGHT
        public static void SetCurrentTelemetryItems(System.Collections.Specialized.StringCollection items, System.Reflection.Assembly assembly)
        {
            if (items.Contains("*"))
            {
                IEnumerable<string> itemsNew = getServerTelemetryItems(assembly);
                setCurrentTelemetryItems(itemsNew);
            }
            else if (items.Contains("none") || items.Contains("None"))
            {
                setCurrentTelemetryItems(new List<string>());
            }
            else
            {
                List<string> itemsList = new List<string>();
                foreach (string item in items)
                    itemsList.Add(item);
                setCurrentTelemetryItems(itemsList);
            }
        }
        #endif

        private static object lockObject = new object();

        /// <summary>
        /// Avtually set the Current Telemetry items
        /// </summary>
        /// <param name="items"></param>
        private static void setCurrentTelemetryItems(IEnumerable<string> items)
        {
            lock (lockObject)
            {
                telemetryItemsRecorded.Clear();
                foreach (string item in items)
                {
                    if (!telemetryItemsRecorded.Contains(item))
                    {
                        telemetryItemsRecorded.Add(item);
                    }
                }
            }
        }

#if !SILVERLIGHT
        /// <summary>
        /// Called by each WCFServer passing the System.Reflection.Assembly.GetExecutingAssembly()
        /// It will only parse assemblies the start with Sepura (not case sensitive) in order to avoid huge system assemblies
        /// Returns a Dictionary of items with "MeasurementName", isBeingCollected. 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<string> getServerTelemetryItems(System.Reflection.Assembly assembly)
        {
            Dictionary<string, bool> telemetryItems = new Dictionary<string, bool>();

            foreach (string telemetryItem in getAttributes(assembly))
            {
                yield return telemetryItem;
            }
            System.Reflection.AssemblyName[] assemblyNames = assembly.GetReferencedAssemblies();

            foreach (System.Reflection.AssemblyName assemblyName in assemblyNames)
            {
                if (assemblyName.Name.StartsWith("ia", StringComparison.CurrentCultureIgnoreCase))
                {
#pragma warning disable 0618 // Obsolete method still required by us
                    System.Reflection.Assembly referencedAssembly = System.Reflection.Assembly.LoadWithPartialName(assemblyName.Name);
#pragma warning restore 0618 // Obsolete method still required by us
                    foreach (string item in getAttributes(referencedAssembly))
                    {
                        yield return item;
                    }
                }
            }
        }
#endif
        /// <summary>
        /// For an assembly, get all the Public and Non Public Methods and search for TelemetryDataAttribute custom attribute
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static IEnumerable<string> getAttributes(System.Reflection.Assembly assembly)
        {
            List<string> vals = new List<string>();
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                foreach (string item in getServerTelemetryItems(type))
                {
                    yield return item;
                }
            }
        }


        /// <summary>
        /// Add items with the TelemetryDataItem attribute to the passed dictionary.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IEnumerable<string> getServerTelemetryItems(System.Type type)
        {
            System.Reflection.MemberInfo[] members = type.GetMethods(System.Reflection.BindingFlags.Public |
                                                                         System.Reflection.BindingFlags.NonPublic |
                                                                         System.Reflection.BindingFlags.Instance |
                                                                         System.Reflection.BindingFlags.Static);
            //System.Reflection.MemberInfo[] members = type.GetMethods();

            foreach (System.Reflection.MemberInfo memInfo in members)
            {
                object[] memberObject = memInfo.GetCustomAttributes(typeof(TelemetryDataAttribute), true);
                if (memberObject.Length > 0)
                {
                    for (int count = 0; count < memberObject.Length; count++)
                    {
                        TelemetryDataAttribute telemetryData = (TelemetryDataAttribute)memberObject[count];
                        if (telemetryData != null)
                        {
                            //// TelemetryData.MeasurementName must be unique
                            //if (telemetryItems.ContainsKey(telemetryData.MeasurementName))
                            //{
                            //    throw new Exception("Duplicate TelemetryData.MeasurementName: " + telemetryData.MeasurementName);
                            //}
                            // Add whether this items is already being collected.
                            bool beingCollected = telemetryItemsRecorded.Contains(telemetryData.MeasurementName);
                            if (!beingCollected)
                            {
                                yield return telemetryData.MeasurementName;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Write the passed data to the telemetry log file writer
        /// </summary>
        /// <param name="telemetryName">Name of the telemetry.</param>
        /// <param name="measure">The measure.</param>
        /// <param name="businessOperation">The business operation.</param>
        /// <param name="telemetryListener">The telemetry listener.</param>
        public static void WriteData(string telemetryName, float measure, CallStatus.BusinessOperation businessOperation, Indigo.CrossCutting.Utilities.Logging.ITelemetryListener telemetryListener)
        {
            if (telemetryListener != null) telemetryListener.WriteData(telemetryName, measure, DateTime.Now, businessOperation);
        }



    }
}
