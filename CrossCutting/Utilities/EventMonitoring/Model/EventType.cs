using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.EventMonitoring
{
    /// <summary>
    /// the types of event messages
    /// </summary>
    public enum EventType
    {
        ApplicationStart,
        ApplicationStop,
        ApplicationError,
        APIRangeReached,
        UserInitiated
    }
}
