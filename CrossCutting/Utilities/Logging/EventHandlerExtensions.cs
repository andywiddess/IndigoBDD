using System;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Event Handler Extension Methods
    /// </summary>
    public static class EventHandlerExtensions
    {
        #region Static Methods
        /// <summary>
        /// Raises the specified event handler.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The instance containing the event data.</param>
        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e) where TEventArgs : EventArgs
        {
            var tempEvent = eventHandler;
            if (tempEvent != null)
                eventHandler(sender, e);
        }
        #endregion
    }
}
