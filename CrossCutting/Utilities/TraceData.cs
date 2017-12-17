using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Presents the trace data object.
    /// </summary>
    public class TraceData
    {
        #region Members
        /// <summary>
        /// Gets the trace time
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public virtual DateTime Time { get; set; }

        /// <summary>
        /// Gets the category
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual string Category { get; set; }

        /// <summary>
        /// Gets the trace detail message text.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public virtual string Message { get; set; }

        /// <summary>
        /// Gets whether this is a warning message.
        /// </summary>
        public virtual bool IsWarn { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is error; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsError { get { return ErrorInfo != null; } }

        /// <summary>
        /// Gets the ErrorInfo object.
        /// </summary>
        public virtual Exception ErrorInfo { get; set; }
        #endregion

        #region Constructors
        public TraceData(string message)
        {
            Category = "Info";
            Time = DateTime.Now;
            Message = message;
        }
        #endregion
    }
}
