using System;
using Indigo.CrossCutting.Utilities.Events;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	public static class ExtensionsToEventHandler
	{
		/// <summary>
		/// thanks, http://kohari.org/2009/02/07/eventhandler-extension-method/
		/// </summary>
		public static void Raise<T>(this EventHandler<T> handler, object sender, T args)
			where T : EventArgs
		{
			EventHandler<T> evt = handler;
			if (evt != null) evt(sender, args);
		}

        /// <summary>Safely raises an event.</summary>
        /// <param name="subject">The subject.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public static void SafeRaise(this EventHandler subject, object sender, EventArgs arguments)
        {
            if (subject != null)
            {
                subject(sender, arguments);
            }
        }

        /// <summary>Safely raises an event.</summary>
        /// <param name="subject">The subject.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public static void SafeRaise<T>(this EventHandler<T> subject, object sender, T arguments) where T : EventArgs
        {
            if (subject != null)
            {
                subject(sender, arguments);
            }
        }

        /// <summary>Safely raises an event.</summary>
        /// <param name="subject">The subject.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <param name="ignoreException">if set to <c>true</c> exception thrown by handler are ignored.</param>
        /// <returns>Exception thrown by handler or <c>null</c></returns>
        public static Exception SafeRaise(this EventHandler subject, object sender, EventArgs arguments, bool ignoreException)
        {
            if (subject != null)
            {
                try
                {
                    subject(sender, arguments);
                }
                catch (Exception e)
                {
                    if (ignoreException) return e;
                    throw;
                }
            }
            return null;
        }

        /// <summary>Safely raises an event.</summary>
        /// <param name="subject">The subject.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <param name="ignoreException">if set to <c>true</c> exception thrown by handler are ignored.</param>
        /// <returns>Exception thrown by handler or <c>null</c></returns>
        public static Exception SafeRaise<T>(this EventHandler<T> subject, object sender, T arguments, bool ignoreException)
            where T : EventArgs
        {
            if (subject != null)
            {
                try
                {
                    subject(sender, arguments);
                }
                catch (Exception e)
                {
                    if (ignoreException) return e;
                    throw;
                }
            }
            return null;
        }

        /// <summary>Locks events on <see cref="ISuspendableEvents"/> objects.</summary>
        /// <param name="suspendable">The object with suspendable events.</param>
        /// <returns><see cref="IDisposable"/></returns>
        public static IDisposable EventLock(this ISuspendableEvents suspendable)
        {
            return suspendable != null ? new EventLock(suspendable) : null;
        }
    }
}