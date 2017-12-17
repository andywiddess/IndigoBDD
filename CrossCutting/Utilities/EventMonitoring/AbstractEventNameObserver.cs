using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.EventMonitoring
{
    /// <summary>
    /// Present the base class to process the Event which name has been matched.
    /// </summary>
    public abstract class AbstractEventNameObserver 
        : AbstractEventObserver
    {
        #region Members
        /// <summary>
        /// Gets/Sets which event should be processed.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        public virtual string EventName { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Process event.
        /// </summary>
        /// <param name="value">The value.</param>
        public abstract void Process(AbstractEvent value);

        /// <summary>
        /// Process the event.
        /// </summary>
        /// <param name="value"></param>
        protected override void ProcessEvent(AbstractEvent value)
        {
            if (value.Name.Equals(EventName, StringComparison.OrdinalIgnoreCase))
                Process(value);
        }
        #endregion
    }
}
