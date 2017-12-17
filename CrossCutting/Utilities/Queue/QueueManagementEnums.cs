using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Queue
{
    /// <summary>
    /// Enum of the types of queue supported. There are only two - an initiator, and and a pending request queue
    /// </summary>
    public enum SepuraQueue          { InitiatorQueue, RequestQueue };
    /// <summary>
    /// Enum of the types of queue service supported. 
    /// There's one for every queue type - an initiator, and and a pending request processor
    /// </summary>
    public enum SepuraQueueServices { Initiator, Processor };
}
