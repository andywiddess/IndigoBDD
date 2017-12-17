using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.Logging
{
    public interface ITelemetryListener
    {
        void WriteData(string operationName,float measurement,DateTime startDateTime, Indigo.CrossCutting.Utilities.CallStatus.BusinessOperation businessOperation);
    }
}
