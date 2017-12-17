using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indigo.CrossCutting.Utilities.Selenium.Contracts;

namespace Indigo.WhiteIntegration
{
    /// <summary>
    /// ShutdownApplicationDisposable
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class ShutdownApplicationDisposable 
        : IDisposable
    {
        #region Members
        private readonly IDriver driver;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownApplicationDisposable"/> class.
        /// </summary>
        /// <param name="driver">The test base.</param>
        public ShutdownApplicationDisposable(IDriver driver)
        {
            this.driver = driver;
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            driver.CloseApplication();
        }
        #endregion
    }
}
