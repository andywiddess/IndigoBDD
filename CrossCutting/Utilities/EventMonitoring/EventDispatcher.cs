using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.EventMonitoring
{
    /// <summary>
    /// Present the event host,used to dispatch events to observers.
    /// </summary>
    /// <remarks>
    /// Developers should not use the class directly.In order to raise events please inherit the <see cref="AbstractEvent"/>  and <see cref="AbstractEventObserver"/>  to raise and handling events.
    /// </remarks>
    public sealed class EventDispatcher 
        : IObservable<AbstractEvent>,
          IDisposable
    {
        #region Memebers
        /// <summary>
        /// Gets the subscribed observers.
        /// </summary>
        public ICollection<AbstractEventObserver> Subscribers { get; private set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Notify observer(s) to process the event.
        /// </summary>
        /// <param name="e">The e.</param>
        public static void Process(AbstractEvent e)
        {
            var monitor = new EventDispatcher();
            monitor.ProcessEvent(e);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventDispatcher" /> class.
        /// </summary>
        public EventDispatcher()
        {
            this.Subscribers = new List<AbstractEventObserver>();
            var observers = GetObservers();
            if (observers != null)
            {
                foreach (AbstractEventObserver observer in observers)
                {
                    this.Subscribe(observer);
                }
            }
        }

        /// <summary>
        /// Notify the observer(s) to process the event.
        /// </summary>
        /// <param name="e">The e.</param>
        public void ProcessEvent(AbstractEvent e)
        {
            foreach (var o in Subscribers)
            {
                try
                {
                    ((IObserver<AbstractEvent>)o).OnNext(e);
                }
                catch (Exception ex)
                {
                    ((IObserver<AbstractEvent>)o).OnError(ex);
                }
            }
        }

        /// <summary>
        /// Add an observer to subscribes list.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <exception cref="System.Exception"></exception>
        public void Subscribe(AbstractEventObserver observer)
        {
            if (Subscribers.Contains(observer))
                throw new Exception("Observer has registered.");

            Subscribers.Add(observer);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Subscribers.Clear();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the observers.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<AbstractEventObserver> GetObservers()
        {
            return null; // TODO: Required Mvc - DependencyResolver.Current.GetServices<AbstractEventObserver>();
        }

        /// <summary>
        /// Subscribes the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        IDisposable IObservable<AbstractEvent>.Subscribe(IObserver<AbstractEvent> observer)
        {
            var eventObserver = observer as AbstractEventObserver;
            if (eventObserver != null)
            {
                this.Subscribe(eventObserver);
                return eventObserver;
            }

            throw new ArgumentNullException("observer");
        }
        #endregion
    }
}
