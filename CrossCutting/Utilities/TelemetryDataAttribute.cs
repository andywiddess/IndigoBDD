using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Attribute used to mark a method as having measurment data available to give as Telemetry data.
    /// The Configuration Server will poll each WCFServer and each server will publish which items are available to get Telemetry data
    /// The Config Server will then instruct the WCFServer which Teletry Items to measure and finally get the items using GetServerStatus
    /// This is used by Telemetry.GetTelemetryItems 
    /// 
    /// TelemetryDataAttribute must be in the minimum format 
    /// AssemblyName.MethodName (this will automatically capture the TelemetryPerformanceCounter if it exists)
    /// [TelemetryData(MeasurementName = "ConfigurationIO.GetDataModel")]
    ///
    /// If you wish to write your own Telemetry Data
    /// AssemblyName.MethodName.MyItem (this will also automatically capture the TelemetryPerformanceCounter if it exists)
    /// Then use the code
    /// Telemetry.WriteData("AssemblyName.MethodName.MyItem", MyList.Count);
    /// 
    /// [TelemetryData(MeasurementName = "ConfigurationIO.RequestServerTelemetryItemsList.Size")]
    /// Telemetry.WriteData("ConfigurationIO.RequestServerTelemetryItemsList.Size", telemetryItems.Count);
    ///
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
    public class TelemetryDataAttribute : Attribute
    {
        private string measurementName = "";

        /// <summary>
        /// Each method may publish multiple items of Telemetry data
        /// eg  MethodName
        ///     MethodName.RecordSize
        /// </summary>
        public string MeasurementName
        {
            get { return measurementName; }
            set 
            { 
                measurementName = value;
                if (!measurementName.Contains("."))
                {
                    throw new ArgumentException("Measurement Name must contain a '.'", "MeasurementName");
                }
            }
        }
    }
}
