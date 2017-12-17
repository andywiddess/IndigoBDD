using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Creates and instance of a performance counter for the telemetry recording system. Use in conjuncton with TelemetryDataAttribute on a method that publishes telemetry as follows
    /// 
    /// [TelemetryData(MeasurementName="MyClass.MyMethod")]
    /// public bool MyMethod()
    /// {
    ///    using (TelemetryPerformanceCounter counter = new TelemetryPerformanceCounter("MyClass.MyMethod"))
    ///    {
    ///         // Do work ...
    ///    }
    /// } 
    ///
    /// Telemetry performance counters can be turned on an off by the configuration server which instructs servers to collect telemetry through their 
    /// Sepura.ApplicationServer.Services.ITelemetryServer public interface.
    /// 
    /// Data collected is writen to the Telemetry classes static WriteData method which may choose to discard the information depending on whether its set to record this particular 
    /// instances measurement name. To ensure the Telemetry does not get recorded unneccesarily (and therefore cost precious processing time) the PerformanceCounterEnabled and 
    /// ProcessCounterEnabled flags can be statically set to globally turn on and off the telemetry recording process.
    /// 
    /// NOTE: Becuase these are created and destroyed very frequently, this has been implemented as a STRUCT which prevents heap fragmentation.
    /// </summary>
    /// 
    public struct TelemetryPerformanceCounter : IDisposable
    {
   
        /// <summary>
        /// Global setting to turn on the recording of PerformanceCounters. Defaults to TRUE. This tells the TelemetryPerformanceCounter to record the amount of elapsed time
        /// spent between construction and disposal.
        /// </summary>
        public static bool PerformanceCounterEnabled = true;

        /// <summary>
        /// Global setting to turn on the recording of PerformanceCounters. Defaults to FALSE. This tells the TelemetryPerformanceCounter to record the total numbers of class
        /// creation calls. When set to TRUE, an 
        /// </summary>
        public static bool ProcessCounterEnabled = false;

        /// <summary>
        /// If a category of performance counter is desired, set this value to an arbirary category name.
        /// </summary>
        public static string Category = "";

        /// <summary>
        /// The listener to write the data to
        /// </summary>
        private Indigo.CrossCutting.Utilities.Logging.ITelemetryListener listener;

        /// <summary>
        /// In order to save one record when the process starts
        /// and another when it stops we must remember the Start Value fot the TickCount
        /// </summary>
        private long startValue;
        private DateTime startTime;
        private string counterName;
        private Stopwatch stopWatch;
        private Indigo.CrossCutting.Utilities.CallStatus.BusinessOperation businessOperation;

        public TelemetryPerformanceCounter(string methodName, Indigo.CrossCutting.Utilities.CallStatus.BusinessOperation businessOperation, Indigo.CrossCutting.Utilities.Logging.ITelemetryListener listener)
        {
            this.startValue = 0;
            this.counterName = "";
            this.listener = listener;
            this.businessOperation = businessOperation;
            this.startTime = DateTime.Now;
            this.stopWatch = new Stopwatch();

            if (!(PerformanceCounterEnabled || ProcessCounterEnabled))
            {
                return;
            }
            initialize(methodName);
        }

        private void initialize(string counterName)
        {
            this.counterName = counterName;

            if (ProcessCounterEnabled || PerformanceCounterEnabled)
            {
                this.startValue = Environment.TickCount;
            }
            this.stopWatch.Start();

        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            this.stopWatch.Stop();
            if (!PerformanceCounterEnabled && !ProcessCounterEnabled)
            {
                return;
            }

            if (this.listener != null) listener.WriteData(this.counterName, (float)this.stopWatch.ElapsedMilliseconds, this.startTime, this.businessOperation);
        }

        #endregion
    }
}
