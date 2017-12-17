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
    /// this is a event this instrumented application can fire
    /// </summary>
    public class EventDetails 
        : BaseEvent
    {
        public string Message;
        public string Guid;
        public int Type;
    }
}
