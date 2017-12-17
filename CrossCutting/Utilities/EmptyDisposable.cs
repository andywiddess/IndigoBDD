using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// A IDisposable object which does nothing.
    /// </summary>
    public class EmptyDisposable 
        : IDisposable
    {
        #region consts

        /// <summary>
        /// Default instance.
        /// </summary>
        public static readonly EmptyDisposable Default = new EmptyDisposable();

        #endregion

        #region constructor

        /// <summary>
        /// Prevents a default instance of the <see cref="EmptyDisposable"/> class from being created.
        /// </summary>
        private EmptyDisposable()
        {
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
    }
}
