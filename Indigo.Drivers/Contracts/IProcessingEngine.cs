using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.Drivers.Contracts
{
    public interface IProcessingEngine
    {
        /// <summary>
        /// Performs an Indigo process
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <returns>true if the process was successful, otherwise false</returns>
        bool Process(IConfiguration configuration);
    }
}
