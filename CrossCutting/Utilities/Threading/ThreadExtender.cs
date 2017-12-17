using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Indigo.CrossCutting.Utilities.DesignPatterns;
using Indigo.CrossCutting.Utilities.Exceptions;
using Indigo.CrossCutting.Utilities.Process;

namespace Indigo.CrossCutting.Utilities.Threading
{
    /// <summary>
    /// Class dedicated to extend thread-related objects.
    /// </summary>
    public static class ThreadExtender
    {
        #region Synchronize

        /// <summary>
        /// Calls action in sync with target. Allows to call it and wait or 'fire and forget'.
        /// The default is to call this Synchronously. If you need an asynchronous call 
        /// use the overload to supply an int 0 (zero), AKA "don't wait" or use the 
        /// boolean <c>async</c> call.
        /// </summary>
        /// <param name="target">The synchronization target.</param>
        /// <param name="action">The action to call.</param>
        /// <param name="syncTimeout">The synchronization timeout. Please note that 
        /// timeout is not about execution, it's about time to synchronize threads, 
        /// the action itself can take forever, once method is sync'ed timeout no longer 
        /// applies. Use value of <c>0</c> do not wait,
        /// <see cref="Timeout.Infinite"/> to wait forever, or any other value to make 
        /// timeout active.</param>
        /// <param name="cancelToken">The cancel token.</param>
        public static void Synchronize(
            this ISynchronizeInvoke target,
            Action action, int syncTimeout = Timeout.Infinite,
            CancellationToken? cancelToken = null)
        {
            Patterns.Synchronize(action, target, syncTimeout, cancelToken);
        }

        /// <summary>
        /// Calls function in sync with target. Allows to call it and wait or 'fire and forget'.
        /// The default is to call this Synchronously. If you need an asynchronous call 
        /// use the overload to supply an int 0 (zero), AKA "don't wait" or use the 
        /// boolean <c>async</c> call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The synchronization target.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="syncTimeout">The synchronization timeout. Please note that 
        /// timeout is not about execution, it's about time to synchronize threads, 
        /// the action itself can take forever, once method is sync'ed timeout no longer 
        /// applies. Use value of <c>0</c> to do not wait, <see cref="Timeout.Infinite"/> 
        /// to wait forever, or any other value to make timeout active.</param>
        /// <returns></returns>
        public static Func<T> Synchronize<T>(this ISynchronizeInvoke target, Func<T> factory, int syncTimeout = Timeout.Infinite)
        {
            return Patterns.Synchronize(factory, target, syncTimeout);
        }

        /// <summary>Calls action in sync with target. Allows to call it and wait or 'fire and forget'.</summary>
        /// <param name="target">The synchronization target.</param>
        /// <param name="action">The action to call.</param>
        /// <param name="async">if set to <c>true</c> then call the asynchronously, otherwise synchronously.</param>
        /// <param name="cancelToken">The cancel token.</param>
        public static void Synchronize(
            this ISynchronizeInvoke target,
            Action action, bool async,
            CancellationToken? cancelToken = null)
        {
            Patterns.Synchronize(action, target, async ? 0 : Timeout.Infinite, cancelToken);
        }

        /// <summary>
        /// Calls action in sync with target. Allows to call it and wait or 'fire and forget'. Returns a function
        /// which you can use (immediately or later) to collect the returned result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The synchronization target.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="async">if set to <c>true</c> then call the asynchronously, otherwise synchronously.</param>
        /// <returns></returns>
        public static Func<T> Synchronize<T>(this ISynchronizeInvoke target, Func<T> factory, bool async)
        {
            return Patterns.Synchronize(factory, target, async ? 0 : Timeout.Infinite);
        }

        #endregion

        #region Forking

        /// <summary>
        /// Forks the execution by spawning action in separate thread.
        /// Please note, this is not a replacement for other multithreading techniques.
        /// It is meant to simplify things, when you occasionally need to spawn simple one-liner in separate
        /// thread without all the overhead.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="callbackTarget">The callback synchronization target.</param>
        /// <returns>The ForkedAction object</returns>
        public static ForkedAction Fork(this Action action, Action<Exception> callback = null, ISynchronizeInvoke callbackTarget = null)
        {
            return Patterns.Fork(action, callback, callbackTarget);
        }

        /// <summary>
        /// Forks the execution by spawning factory in separate thread.
        /// Please note, this is not a replacement for other multithreading techniques.
        /// It is meant to simplify things, when you occasionally need to spawn simple one-liner in separate
        /// thread without all the overhead.
        /// </summary>
        /// <typeparam name="T">The Factory result type</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="callbackTarget">The callback synchronization target.</param>
        /// <returns>The ForkedFunc object</returns>
        public static ForkedFunc<T> Fork<T>(this Func<T> factory, Action<T, Exception> callback = null, ISynchronizeInvoke callbackTarget = null)
        {
            return Patterns.Fork(factory, callback, callbackTarget);
        }

        /// <summary>
        /// Forks the execution by spawning action in separate thread.
        /// Please note, this is not a replacement for other multithreading techniques.
        /// It is meant to simplify things, when you occasionally need to spawn simple one-liner in separate
        /// thread without all the overhead.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="onFinished">The callback (can be <c>null</c>).</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target (can be <c>null</c>).</param>
        /// <returns>
        ///   <see cref="ForkedAction"/> object.
        /// </returns>
        public static ForkedActionThread ForkThread(
            this Action action,
            Action<ForkedActionThread> onFinished = null, ISynchronizeInvoke callbackSynchronizationTarget = null)
        {
            return Patterns.ForkThread(action, onFinished, callbackSynchronizationTarget);
        }

        #endregion

        #region Delay

        /// <summary>Delays the synchronization. Used to delay the execution to next message processing loop.</summary>
        /// <param name="target">The synchronisation target.</param>
        /// <param name="action">The action.</param>
        /// <param name="minimumDelay">The minimum delay.</param>
        public static void DelaySynchronize(this ISynchronizeInvoke target, Action action, int minimumDelay = 0)
        {
            Patterns.Fork(() =>
            {
                if (minimumDelay > 0) Thread.Sleep(minimumDelay);
                target.Synchronize(action, true);
            });
        }

        #endregion

        #region Wrap

        /// <summary>Wraps the uncancellable action in abortable thread, so it looks like
        /// it is cancellable. Note, all the disadvantages of Thread.Abort are still there
        /// they are just hidden.</summary>
        /// <param name="token">The cancellation token.</param>
        /// <param name="action">The action.</param>
        /// <param name="interval">The time interval between checks on cancellation token.</param>
        public static void Wrap(this CancellationToken token, Action action, int interval = 100)
        {
            token.ThrowIfCancellationRequested();

            if (interval <= 0) interval = 1;

            using (var startSignal = new ManualResetEvent(false))
            using (var stopSignal = new ManualResetEvent(false))
            {
                var main = ForkThread(() =>
                {
                    startSignal.WaitOne(); // don't run immediately
                    try
                    {
                        action();
                    }
                    finally
                    {
                        stopSignal.Set();
                    }
                });

                startSignal.Set(); // allow main to run

                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        main.Abort(false);
                        break;
                    }
                    if (stopSignal.WaitOne(interval))
                    {
                        break;
                    }
                }

                try
                {
                    main.Wait();
                }
                catch (CanceledException e)
                {
                    // translate CanceledException to OperationCanceledException
                    // see CanceledException for details 
                    throw new OperationCanceledException(e.Message, e);
                }
            }

            token.ThrowIfCancellationRequested();
        }

        /// <summary>Wraps the uncancellable action in abortable thread, so it looks like
        /// it is cancellable. Note, all the disadvantages of Thread.Abort are still there
        /// they are just hidden.</summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="token">The cancellation token.</param>
        /// <param name="func">The function to execute.</param>
        /// <param name="interval">The time interval between cancellation token checks.</param>
        /// <returns>Result of the function.</returns>
        public static T Wrap<T>(this CancellationToken token, Func<T> func, int interval = 100)
        {
            T result = default(T);
            Wrap(token, () => result = func(), interval);
            return result;
        }

        #endregion

        /// <summary>Ignores the potential exception thrown by Task.</summary>
        /// <param name="task">The task.</param>
        public static Task IgnoreException(this Task task)
        {
            // It is about reading Exception property
            return task.ContinueWith(t => t.Exception);
        }

        /// <summary>Ignores the potential exception thrown by Task.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static Task<T> IgnoreException<T>(this Task<T> task, T defaultValue = default(T))
        {
            return task.ContinueWith(t => t.Exception != null ? defaultValue : t.Result);
        }

        /// <summary>Returns not-null cancellation token.</summary>
        /// <param name="token">The token.</param>
        /// <returns>Given or not-null cancellation token.</returns>
        public static CancellationToken NotNull(this CancellationToken? token)
        {
            return token ?? CancellationToken.None;
        }

        /// <summary>Pulses on specified token.</summary>
        /// <param name="token">The token.</param>
        /// <param name="sleepMilliseconds">The sleep milliseconds.</param>
        public static void Pulse(this CancellationToken token, int sleepMilliseconds = -1)
        {
            if (sleepMilliseconds >= 0)
                token.WaitHandle.WaitOne(sleepMilliseconds);
            token.ThrowIfCancellationRequested();
        }

        /// <summary>Pulses on specified token.</summary>
        /// <param name="token">The token.</param>
        /// <param name="sleepTimespan">The sleep timespan.</param>
        public static void Pulse(this CancellationToken token, TimeSpan sleepTimespan)
        {
            token.WaitHandle.WaitOne(sleepTimespan);
            token.ThrowIfCancellationRequested();
        }

        /// <summary>Schedules the call when wait handle is set.</summary>
        /// <param name="handle">The handle.</param>
        /// <param name="action">The action.</param>
        public static void OnSet(this WaitHandle handle, Action action)
        {
            int sync = 0;
            var callback = default(RegisteredWaitHandle);

            Interlocked.Exchange(ref sync, 0); // full fence
            callback = ThreadPool.RegisterWaitForSingleObject(
                handle,
                (s, t) => {
                    // this synchronisation is usually not needed
                    // it might be needed only in one case: 
                    // when handle is set from very beginning so this method is called
                    // before 'callback' local variable is assigned
                    while (Interlocked.CompareExchange(ref sync, 0, 0) == 0) Thread.Yield();

                    try
                    {
                        action();
                    }
                    finally
                    {
                        callback.Unregister(null);
                    }
                },
                null,
                Timeout.Infinite,
                true);
            Interlocked.Exchange(ref sync, 1); // full fence
        }

        /// <summary>Schedules the call when wait handle is set.</summary>
        /// <param name="handle">The handle.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="action">The action called when handle is set or operations times out. 
        /// Note, action's argument is set to <c>true</c> if waiting timed out, and to <c>false</c>
        /// if handle has been set.</param>
        public static void OnSet(this WaitHandle handle, TimeSpan timeout, Action<bool> action)
        {
            int sync = 0;
            var callback = default(RegisteredWaitHandle);

            Interlocked.Exchange(ref sync, 0); // full fence
            callback = ThreadPool.RegisterWaitForSingleObject(
                handle,
                (s, t) => {
                    // this synchronisation is usually not needed
                    // it might be needed only in one case: 
                    // when handle is set from very beginning so this method is called
                    // before 'callback' local variable is assigned
                    while (Interlocked.CompareExchange(ref sync, 0, 0) == 0) Thread.Yield();

                    try
                    {
                        action(t);
                    }
                    finally
                    {
                        callback.Unregister(null);
                    }
                },
                null,
                timeout,
                true);
            Interlocked.Exchange(ref sync, 1); // full fence
        }

        /// <summary>Schedules the call when action is going to be cancelled.</summary>
        /// <param name="token">The token.</param>
        /// <param name="action">The action.</param>
        public static void OnAbort(this CancellationToken token, Action action)
        {
            OnSet(token.WaitHandle, action);
        }

        /// <summary>Converts WaitHandle to Task.</summary>
        /// <param name="token">The token.</param>
        /// <returns>A task to wait for.</returns>
        public static Task ToTask(this CancellationToken token)
        {
            return ToTask(token.WaitHandle);
        }

        /// <summary>Converts WaitHandle to Task.</summary>
        /// <param name="handle">The handle.</param>
        /// <returns>A task to wait for.</returns>
        public static Task ToTask(this WaitHandle handle)
        {
            return ToTask(handle, Timeout.Infinite);
        }

        /// <summary>Converts WaitHandle to Task.</summary>
        /// <param name="handle">The handle.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A task to wait for.</returns>
        public static Task ToTask(this WaitHandle handle, TimeSpan timeout)
        {
            return ToTask(handle, (int)timeout.TotalMilliseconds);
        }

        /// <summary>Converts WaitHandle to Task. If waiting times out task is considered 'Cancelled'.</summary>
        /// <param name="handle">The wait handle.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>A task to wait for.</returns>
        public static Task ToTask(this WaitHandle handle, int timeout)
        {
            var tcs = new TaskCompletionSource<object>();
            var registration = ThreadPool.RegisterWaitForSingleObject(
                handle,
                (_, t) => {
                    if (t) // if timeout
                    {
                        tcs.TrySetCanceled();
                    }
                    else
                    {
                        tcs.TrySetResult(null);
                    }
                },
                null,
                timeout,
                true);
            tcs.Task.ContinueWith(_ => registration.Unregister(null));
            return tcs.Task;
        }

        /// <summary>
        /// Converts WaitHandle to IEnumerable (which can be converted to IObservable).
        /// Please note that this might not be recommended in heavy duty applications as this
        /// enumerable blocks thread.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Enumerable providing <c>true</c> if handle has been set, or <c>false</c> if operation timed out.</returns>
        public static IEnumerable<bool> ToEnumerable(this WaitHandle handle, TimeSpan? timeout = null)
        {
            yield return timeout.HasValue ? handle.WaitOne(timeout.Value) : handle.WaitOne();
        }

        /// <summary>
        /// Converts WaitHandle to IEnumerable (which can be converted to IObservable).
        /// Please note that this might not be recommended in heavy duty applications as this
        /// enumerable blocks thread.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="abort">The abort signal.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Enumerable providing <c>true</c> if handle has been set, or <c>false</c> if operation timed out, 
        /// or no value at all if about has been signaled.</returns>
        public static IEnumerable<bool> ToEnumerable(this WaitHandle handle, CancellationToken abort, TimeSpan? timeout = null)
        {
            var handles = new[] { handle, abort.WaitHandle };
            var signaled =
                timeout.HasValue
                ? WaitHandle.WaitAny(handles, timeout.Value)
                : WaitHandle.WaitAny(handles);
            if (signaled == WaitHandle.WaitTimeout)
                yield return false;
            else if (signaled == 0 /* handle */)
                yield return true;
            else if (signaled == 1 /* abort */)
                yield break;
        }

        /// <summary>
        /// Creates a task running on the provided scheduler with no cancellation token and no specific task options.
        /// </summary>
        /// <param name="scheduler">The task scheduler.</param>
        /// <param name="taskAction">Action to run.</param>
        /// <returns>A new task.</returns>
        public static Task Defer(this TaskScheduler scheduler, Action taskAction)
        {
            return Task.Factory.StartNew(taskAction, CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

    }
}
