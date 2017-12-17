using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Presents the Trace object 
    /// </summary>
    public class TraceContext
    {
        #region Members
        /// <summary>
        /// 
        /// </summary>
        private List<TraceData> InnerTraceData = new List<TraceData>();

        /// <summary>
        /// Gets the TraceData collection.
        /// </summary>
        /// <value>
        /// The trace data.
        /// </value>
        public ICollection<TraceData> TraceData
        {
            get
            {
                return InnerTraceData;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Write a warnning message to trace context.
        /// </summary>
        /// <param name="message">The warnning message.</param>
        public void Warn(string message)
        {
            InnerTraceData.Add(new TraceData(message) { Category = "Warnning", IsWarn = true });
        }

        /// <summary>
        /// Write a warning message with category to trace context.
        /// </summary>
        /// <param name="category">The message category name.</param>
        /// <param name="message">The warnning message.</param>
        public void Warn(string category, string message)
        {
            InnerTraceData.Add(new TraceData(message) { Category = category, IsWarn = true });
        }

        /// <summary>
        /// Write a waning message, category and exception object to trace context.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message"></param>
        /// <param name="e"></param>
        public void Warn(string category, string message, Exception e)
        {
            InnerTraceData.Add(new TraceData(message) { Category = category, IsWarn = true, ErrorInfo = e });
        }

        public void Warn(string message, Exception e)
        {
            InnerTraceData.Add(new TraceData(message) { Category = "Warning", IsWarn = true, ErrorInfo = e });
        }

        /// <summary>
        /// Write a simple message to trace context.
        /// </summary>
        /// <param name="message">The message text.</param>
        public void Write(string message)
        {
            InnerTraceData.Add(new TraceData(message) { Category = "Information" });
        }

        /// <summary>
        /// Write a message with category to trace context.
        /// </summary>
        /// <param name="category">The category text.</param>
        /// <param name="message">The message text.</param>
        public void Write(string category, string message)
        {
            InnerTraceData.Add(new TraceData(message) { Category = category });
        }
        #endregion
    }
}
