using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indigo.Drivers.Contracts;

namespace Indigo.Implementation
{
    public class TestLinkProcessing
        : IProcessingEngine
    {
        #region Members
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLinkProcessing" /> class.
        /// </summary>
        public TestLinkProcessing()
        {

        }
        #endregion

        #region Implementation

        /// <summary>
        /// Performs an Indigo process
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        public bool Process(IConfiguration configuration)
        {
            bool response = false;

            return response;
        }
        #endregion
    }
}
