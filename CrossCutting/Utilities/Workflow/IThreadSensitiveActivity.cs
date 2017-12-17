using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Workflow
{
    /// <summary>
    /// Interface implemented by an activity which is thread sensitive. Implementing this interface provides an Object that typically can be cast onto the user interface
    /// thread of the client workflow executer.
    /// </summary>
    public interface IThreadSensitiveActivity
    {
        object UserInterfaceThreadObject { get; set; }
    }
}
