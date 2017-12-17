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
    /// installer class which will publish the InfoMessage to the WMI schema
    /// (the assembly attribute Instrumented defines the namespace this
    /// class gets published too
    /// </summary>
    [RunInstaller(true)]
    public class InstrumentationProviderInstaller 
        : DefaultManagementProjectInstaller
    {
    }
}
