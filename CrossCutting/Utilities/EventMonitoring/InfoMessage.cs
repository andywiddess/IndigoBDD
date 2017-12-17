using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Instrumentation;
using System.Configuration.Install;
using System.ComponentModel;

namespace Indigo.CrossCutting.Utilities.EventMonitoring
{
    /// <summary>
    /// this is a message this instrumented application publishes through WMI
    /// </summary>
    [InstrumentationClass(InstrumentationType.Instance)]
    public class InfoMessage
    {
        #region Members
        public string Message;
        public string Guid;
        public int Type;
        #endregion

        #region Static Methods
        /// <summary>
        /// Empties the message.
        /// </summary>
        /// <returns></returns>
        public static InfoMessage EmptyMessage()
        {
            return new InfoMessage();
        }
        #endregion
    }
}
