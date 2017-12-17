using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Indigo.CrossCutting.Utilities.EventMonitoring
{
    /// <summary>
    /// Present the Event base class.
    /// </summary>
    public abstract class AbstractEvent
    {
        #region Members
        /// <summary>
        /// Gets/Sets the current http context.
        /// </summary>
        /// <value>
        /// The HTTP context.
        /// </value>
        public HttpContextBase HttpContext { get; set; }

        /// <summary>
        /// Gets the event name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name { get; protected set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Raises this instance.
        /// </summary>
        public virtual void Raise()
        {
            if (string.IsNullOrEmpty(Name))
                Name = this.GetType().ToString();

            EventDispatcher.Process(this);
        }

        /// <summary>
        /// Raise this event and notify the observer(s) to process it.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public virtual void Raise(HttpContextBase httpContext)
        {
            HttpContext = httpContext;
            Raise();
        }
        #endregion
    }
}
