using System;
using System.ComponentModel;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Process
{
    /// <summary>
    /// Allows to synchronize UI calls from threads. It should be used only in simple cases.
    /// Handles any argement-less and result-less delegates. Can be used effectively with anonymous delegates.
    /// </summary>
    /// <example><code>
    /// SyncInvoker invoker = new SyncInvoker(MyForm);
    /// ...
    /// invoker.Invoke(delegate
    /// {
    ///   MyForm.Label1.Text = "Assigning label text is sychronized to MyForm";
    /// });
    /// </code></example>
    // [Obsolete("This class can be replaced by Patterns.Synchronize(...)")]
    public class SyncInvoker 
        : ISynchronizeInvoke
    {
        #region fields

        private ISynchronizeInvoke m_InvokeTarget;

        #endregion

        #region properties

        /// <summary>
        /// Invoke target.
        /// </summary>
        /// <value>The invoke target.</value>
        public ISynchronizeInvoke InvokeTarget
        {
            get { return m_InvokeTarget; }
        }

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncInvoker"/> class. 
        /// All call will be synchronized to given target.
        /// </summary>
        /// <param name="invokeTarget">The invoke target.</param>
        public SyncInvoker(ISynchronizeInvoke invokeTarget)
        {
            m_InvokeTarget = invokeTarget;
        }

        #endregion

        #region invoke

        /// <summary>
        /// Invoke guard. Allows to specify timeout for syncing to avoid deadlock (it will still throw the exception though).
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="syncTimeout">The sync timeout.</param>
        /// <returns>Result returned by delegate.</returns>
        private object InvokeGuard(Delegate method, int syncTimeout)
        {
            object[] result = new object[1]; // one element array store result

            ManualResetEvent sigReceived = new ManualResetEvent(false);
            ManualResetEvent sigStart = new ManualResetEvent(false);
            ManualResetEvent sigAbort = new ManualResetEvent(false);

            Action receiver = () =>
            {
                sigReceived.Set(); // confirm that you got in

                // wait for start (got in quickly)
                int sig = WaitHandle.WaitAny(new WaitHandle[] { sigStart, sigAbort });

                if (sig == 0 /* sigStart */)
                {
                    result[0] = method.DynamicInvoke(null); // update result (held externally)
                }
                else /* sigAbort */
                {
                    // returned from deadlock, but caller is no longer waiting
                }

                // it's done!
            };

            IAsyncResult async = m_InvokeTarget.BeginInvoke(receiver, null);
            if (sigReceived.WaitOne(syncTimeout))
            {
                sigStart.Set();
                async.AsyncWaitHandle.WaitOne();
            }
            else
            {
                sigAbort.Set();
                throw new InvalidOperationException("Synchronous invoke caused deadlock");
            }

            return result[0];
        }

        /// <summary>
        /// Invokes the specified method in sync with target. 
        /// If no synchronization required method is just executed in current thread.
        /// </summary>
        /// <param name="method">The method.</param>
        public void Invoke(Action method)
        {
            Invoke(method, Timeout.Infinite);
        }

        /// <summary>
        /// Invokes the specified method in sync with target.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="syncTimeout">The deadlock timeout.</param>
        public void Invoke(Action method, TimeSpan syncTimeout)
        {
            Invoke(method, (int)syncTimeout.TotalMilliseconds);
        }

        /// <summary>
        /// Invokes the specified method in sync with target.
        /// If no synchronization required method is just executed in current thread.
        /// Tries to avoid deadlock.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="syncTimeout">The deadlock timeout.</param>
        public void Invoke(Action method, int syncTimeout)
        {
            if ((m_InvokeTarget != null) && (m_InvokeTarget.InvokeRequired))
            {
                if (syncTimeout != Timeout.Infinite)
                {
                    InvokeGuard(method, syncTimeout);
                }
                else
                {
                    m_InvokeTarget.Invoke(method, null);
                }
            }
            else
            {
                method();
            }
        }

        /// <summary>
        /// Invokes the specified factory in sync with target.
        /// </summary>
        /// <typeparam name="T">Type of factory.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <returns>Value returned by factory.</returns>
        public T Invoke<T>(Func<T> factory)
        {
            return Invoke<T>(factory, Timeout.Infinite);
        }

        /// <summary>
        /// Invokes the specified factory in sync with target.
        /// </summary>
        /// <typeparam name="T">Type of factory.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="syncTimeout">The deadlock timeout.</param>
        /// <returns>Value returned by factory.</returns>
        public T Invoke<T>(Func<T> factory, TimeSpan syncTimeout)
        {
            return Invoke<T>(factory, (int)syncTimeout.TotalMilliseconds);
        }

        /// <summary>
        /// Invokes the specified factory in sync with target.
        /// </summary>
        /// <typeparam name="T">Type of factory.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="syncTimeout">The deadlock timeout.</param>
        /// <returns>Value returned by factory.</returns>
        public T Invoke<T>(Func<T> factory, int syncTimeout)
        {
            if (m_InvokeTarget != null && m_InvokeTarget.InvokeRequired)
            {
                if (syncTimeout != Timeout.Infinite)
                {
                    return (T)InvokeGuard(factory, syncTimeout);
                }
                else
                {
                    return (T)m_InvokeTarget.Invoke(factory, null);
                }
            }
            else
            {
                return factory();
            }
        }

        /// <summary>
        /// Invokes the specified method in sync with target but asynchronously to current thread.
        /// If no synchronization required method is just executed in current thread, so it's not asynchronus call.
        /// </summary>
        /// <param name="method">The method.</param>
        public IAsyncResult InvokeAsync(Action method)
        {
            if ((m_InvokeTarget != null) && (m_InvokeTarget.InvokeRequired))
            {
                return m_InvokeTarget.BeginInvoke(method, null);
            }
            else
            {
                method();
                return null;
            }
        }

        /// <summary>
        /// Ends the async invoke. Expects no result.
        /// </summary>
        /// <param name="result">The result.</param>
        public void EndInvokeAsync(IAsyncResult result)
        {
            if (result == null) return;
            m_InvokeTarget.EndInvoke(result);
        }

        /// <summary>
        /// Ends the async invoke. Expects a result.
        /// </summary>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="result">The result.</param>
        /// <returns>Value returned by async routine.</returns>
        public T EndInvokeAsync<T>(IAsyncResult result)
        {
            if (result == null)
                throw new ArgumentNullException("result", "result is null.");
            return (T)m_InvokeTarget.EndInvoke(result);
        }

        #endregion

        #region ISynchronizeInvoke Members

        IAsyncResult ISynchronizeInvoke.BeginInvoke(Delegate method, object[] args)
        {
            return m_InvokeTarget.BeginInvoke(method, args);
        }

        object ISynchronizeInvoke.EndInvoke(IAsyncResult result)
        {
            return m_InvokeTarget.EndInvoke(result);
        }

        object ISynchronizeInvoke.Invoke(Delegate method, object[] args)
        {
            return m_InvokeTarget.Invoke(method, args);
        }

        bool ISynchronizeInvoke.InvokeRequired
        {
            get { return m_InvokeTarget.InvokeRequired; }
        }

        #endregion
    }
}
