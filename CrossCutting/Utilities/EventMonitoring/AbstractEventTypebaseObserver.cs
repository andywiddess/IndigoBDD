using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.EventMonitoring
{
    /// <summary>
    /// Present the observer base class that process the event when Event type has been matched.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public abstract class AbstractEventTypebaseObserver<TEvent> 
        : AbstractEventObserver
    where TEvent : AbstractEvent
    {
        #region Methods
        /// <summary>
        /// Processes the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        public abstract void Process(TEvent e);

        /// <summary>
        /// Process the event.
        /// </summary>
        /// <param name="value"></param>
        protected override void ProcessEvent(AbstractEvent value)
        {
            var e = value as TEvent;
            if (e != null)
                Process(e);
        }
        #endregion
    }
}
