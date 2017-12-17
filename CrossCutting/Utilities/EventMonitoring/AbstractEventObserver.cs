using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.EventMonitoring
{
    /// <summary>
    /// Present the Observer base class.
    /// </summary>
    public abstract class AbstractEventObserver 
        : IObserver<AbstractEvent>, 
          IDisposable
    {
        #region Methods
        /// <summary>
        /// Process the event.
        /// </summary>
        /// <param name="e">The e.</param>
        protected abstract void ProcessEvent(AbstractEvent e);

        /// <summary>
        /// Trigger when event process error.
        /// </summary>
        /// <param name="error">The error.</param>
        protected virtual void OnError(Exception error)
        { 
        }

        /// <summary>
        /// Release the resources.
        /// </summary>
        protected virtual void Dispose() 
        { 
        }
        #endregion

        #region Implement the IObserver interface
        /// <summary>
        /// Called when [completed].
        /// </summary>
        void IObserver<AbstractEvent>.OnCompleted()
        {
        }

        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        void IObserver<AbstractEvent>.OnError(Exception error)
        {
            this.OnError(error);
        }

        /// <summary>
        /// Called when [next].
        /// </summary>
        /// <param name="value">The value.</param>
        void IObserver<AbstractEvent>.OnNext(AbstractEvent value)
        {
            this.ProcessEvent(value);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.Dispose();
        }
        #endregion
    }

}
