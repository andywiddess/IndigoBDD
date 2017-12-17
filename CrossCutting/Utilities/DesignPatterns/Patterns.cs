using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Indigo.CrossCutting.Utilities.Collections;
using Indigo.CrossCutting.Utilities.Exceptions;
using Indigo.CrossCutting.Utilities.Process;
using Indigo.CrossCutting.Utilities.Threading;

namespace Indigo.CrossCutting.Utilities.DesignPatterns
{
    /// <summary>
    /// Procedural patterns.
    /// </summary>
    public static class Patterns
    {
        #region retry

        /// <summary>
        /// Waits between tries.
        /// </summary>
        /// <param name="retried">The retried count.</param>
        /// <param name="elapsed">The elapsed time in seconds.</param>
        /// <param name="maxTime">The max time in seconds.</param>
        /// <param name="interval">The interval in seconds.</param>
        private static void RetryWait(int retried, double elapsed, double maxTime, double interval)
        {
            if (retried > 0)
            {
                double pause = Math.Max(Math.Min(maxTime - elapsed, interval), 0);
                Thread.Sleep(TimeSpan.FromSeconds(pause));
            }
        }

        private static readonly Exception UnknownException = new Exception("Unknown exception");

        /// <summary>
        /// Retries the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="intervalSeconds">The interval, in seconds, between retrying the operation</param>
        /// <param name="maxSeconds">The max time to keep trying the operation for, in seconds</param>
        /// <param name="maxCount">The max number of times to try the operation</param>
        public static void Retry(Action<int> action, double intervalSeconds, double maxSeconds, int maxCount)
        {
            var startTime = DateTime.Now;
            int retried = 0;
            var lastException = UnknownException;

            while (true)
            {
                double elapsed = DateTime.Now.Subtract(startTime).TotalSeconds;

                if ((retried == 0) || ((elapsed <= maxSeconds) && (retried <= maxCount)))
                {
                    try
                    {
                        RetryWait(retried, elapsed, maxSeconds, intervalSeconds);
                        action(retried);

                        return; // done, there's nothing more to be done
                    }
                    catch (Exception e)
                    {
                        lastException = e;
                    }
                }
                else
                {
                    throw lastException;
                }

                retried++;
            }
        }

        /// <summary>
        /// Retries the specified action. Action may return a result.
        /// </summary>
        /// <typeparam name="T">Type of action result.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="maxTime">The max time.</param>
        /// <param name="maxCount">The max count.</param>
        /// <returns>Value returned by successful action.</returns>
        public static T Retry<T>(Func<int, T> action, double interval, double maxTime, int maxCount)
        {
            var startTime = DateTime.Now;
            int retried = 0;
            var lastException = UnknownException;

            while (true)
            {
                double elapsed = DateTime.Now.Subtract(startTime).TotalSeconds;

                if ((retried == 0) || ((elapsed <= maxTime) && (retried <= maxCount)))
                {
                    try
                    {
                        RetryWait(retried, elapsed, maxTime, interval);
                        return action(retried);
                    }
                    catch (Exception e)
                    {
                        lastException = e;
                    }
                }
                else
                {
                    throw lastException;
                }

                retried++;
            }
        }

        /// <summary>Tries the specified actions. If any of them succeeds it returns, 
        /// throws AggregateException if all of them fail.</summary>
        /// <param name="actions">The actions.</param>
        public static void Try(params Action[] actions)
        {
            IList<Exception> exceptions = null;

            foreach (var action in actions)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception e)
                {
                    if (exceptions == null) exceptions = new List<Exception>();
                    exceptions.Add(e);
                }
            }

            if (exceptions != null)
                throw new AggregateException(exceptions);
        }

        /// <summary>Tries the specified actions. Returns result of first successful one.</summary>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="actions">The actions.</param>
        /// <returns>Result of first successful action.</returns>
        public static T Try<T>(params Func<T>[] actions)
        {
            IList<Exception> exceptions = null;

            foreach (var action in actions)
            {
                try
                {
                    return action();
                }
                catch (Exception e)
                {
                    if (exceptions == null) exceptions = new List<Exception>();
                    exceptions.Add(e);
                }
            }

            if (exceptions != null)
                throw new AggregateException(exceptions);

            return default(T);
        }

        #endregion

        #region cache value

        /// <summary>
        /// Caches the value. If given value is <c>null</c> runs the <paramref name="factory"/> method to 
        /// obtain new value.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>Cached or produced value.</returns>
        public static T CacheValue<T>(ref T variable, Func<T> factory) where T : class
        {
            if (variable == null)
            {
                variable = factory();
            }
            return variable;
        }

        /// <summary>
        /// Caches the value. If given key already exists in cache (dictionary) returns value from
        /// cache. If it doesn't runs the factory method and stores produced value in dictionary.
        /// </summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <typeparam name="V">Value type.</typeparam>
        /// <param name="cache">The cache.</param>
        /// <param name="key">The key.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>Cached or produced value.</returns>
        public static V CacheValue<K, V>(IDictionary<K, V> cache, K key, Converter<K, V> factory)
        {
            V result;
            if (cache.TryGetValue(key, out result)) return result;

            result = factory(key);
            cache[key] = result;
            return result;
        }

        #endregion

        #region change property

        /// <summary>
        /// Changes the property.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="value">The new value.</param>
        public static void ChangeProperty<T>(ref T variable, T value)
        {
            ChangeProperty(ref variable, value, null, null);
        }

        /// <summary>
        /// Changes the property.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="value">The new value.</param>
        /// <param name="afterChangeNotification">The method to call after change.</param>
        public static void ChangeProperty<T>(
                ref T variable, T value,
                PropertyChangedNotification<T> afterChangeNotification)
        {
            ChangeProperty(ref variable, value, null, afterChangeNotification);
        }

        /// <summary>
        /// Changes the property.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="variable">The variable.</param>
        /// <param name="value">The new value.</param>
        /// <param name="beforeChangeNotification">The method to call before change.</param>
        /// <param name="afterChangeNotification">The method to call after change.</param>
        /// <returns></returns>
        public static bool ChangeProperty<T>(
                ref T variable, T value,
                PropertyChangingNotification<T> beforeChangeNotification,
                PropertyChangedNotification<T> afterChangeNotification)
        {
            if (object.Equals(variable, value))
            {
                var original = variable;
                if ((beforeChangeNotification == null) || (beforeChangeNotification(original, value)))
                {
                    variable = value;
                    if (afterChangeNotification != null)
                    {
                        afterChangeNotification(original, value);
                    }
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region safe get/convert/execute

        /// <summary>
        /// Safely gets the value.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="factory">The value factory.</param>
        /// <param name="defaultValue">The default value is used when factory throws an exception.</param>
        /// <returns>Value produced by <paramref name="factory"/> or <paramref name="defaultValue"/> 
        /// is factory threw an exception.</returns>
        public static T SafeGetValue<T>(T defaultValue, Func<T> factory)
        {
            try
            {
                return factory();
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Safely gets value from multiple factories. If first one fails
        /// tries to aquire value from second one, and so on. If none
        /// succeeded rethrows last exception.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="factories">The factories.</param>
        /// <returns>First valid produced value.</returns>
        public static T SafeGetValue<T>(params Func<T>[] factories)
        {
            Exception lastException = null;

            foreach (var factory in factories)
            {
                try
                {
                    return factory();
                }
                catch (Exception e)
                {
                    lastException = e;
                }
            }

            throw lastException;
        }

        /// <summary>
        /// Function which converts one type to another by casting.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Given object cast as <typeparamref name="TOutput"/></returns>
        [Obsolete("Use Cast<I, O> instead")]
        public static TOutput CastConverter<TInput, TOutput>(TInput value)
        {
            return (TOutput)((object)value);
        }

        /// <summary>
        /// Function which returns given value. Does nothing on it's own but can be used when
        /// Converter is required as parameter.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Given value.</returns>
        [Obsolete("Use Pass<T> instead")]
        public static TType PassthroughConverter<TType>(TType value)
        {
            return value;
        }

        /// <summary>
        /// Returns given value after casting it to another type.
        /// As it may look superfluous but it is used for methods which require 
        /// <c>Converter</c> delegate and we don't want to do anything.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Same value as given.</returns>
        public static TOutput Cast<TOutput>(object value)
        {
            return (TOutput)value;
        }

        /// <summary>Returns given value after casting it to another type.
        /// As it may look superfluous but it is used for methods which require <c>Converter</c> delegate and we don't want
        /// to do anything.</summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Same value as given.</returns>
        public static TOutput Cast<TInput, TOutput>(TInput value)
        {
            return (TOutput)((object)value);
        }

        /// <summary>Default value for type 'void'. Sounds strange but there is a 
        /// symmetry between this one and <see cref="Default{T}"/></summary>
        public static void Default()
        {
        }

        /// <summary>Function returning Default value for given type.</summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Default value for type T.</returns>
        public static T Default<T>()
        {
            return default(T);
        }

        /// <summary>Determines whether the specified value is null. Used when delegate is needed.</summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is null; otherwise, <c>false</c>.</returns>
        public static bool IsNull<T>(T value)
        {
            return ReferenceEquals(value, null);
        }

        /// <summary>Determines whether the specified value is not null. Used when delegate is needed.</summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is not null; otherwise, <c>false</c>.</returns>
        public static bool IsNotNull<T>(T value)
        {
            return !ReferenceEquals(value, null);
        }

        /// <summary>Returns given value. As it may look superfluous but it is used for methods which require 
        /// <c>Converter</c> delegate and we don't want to do anything.</summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Same value as given.</returns>
        public static TType Pass<TType>(TType value)
        {
            return value;
        }

        /// <summary>Creates new object of given type using parameter-less constructor. Used when delegate s needed.</summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <returns>New object.</returns>
        public static TType New<TType>() where TType : new()
        {
            return new TType();
        }

        /// <summary>Captures the value as function.</summary>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>A function which returns captured value.</returns>
        public static Func<T> Capture<T>(T value)
        {
            return () => value;
        }

        /// <summary>
        /// Creates a new function which will lazy-evaluate given function 
        /// if needed and capture the result. It will use weak reference to 
        /// capture result if instructed to, in such case factory method may 
        /// be called multiple times.
        /// </summary>
        /// <typeparam name="T">Type of function result.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="weak">if set to <c>true</c> weak reference is used to capture result.</param>
        /// <returns>Lazy evaluation function.</returns>
        public static Func<T> Lazy<T>(Func<T> factory, bool weak = false)
        {
            if (!weak)
            {
                bool first = true;
                T result = default(T);

                return () => {
                    if (first)
                    {
                        result = factory();
                        first = false;
                    }
                    return result;
                };
            }
            else
            {
                var reference = default(WeakReference);

                return () => {
                    var result = reference == null ? null : reference.Target;
                    if (result == null)
                        reference = new WeakReference(result = factory());
                    return (T)result;
                };
            }
        }

        /// <summary>Creates a WeakReference object. 
        /// Fixes little inconvenience with WeakReference being untyped.</summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Function returning referenced value is it wasn't garbage collected.</returns>
        public static Func<T> Weak<T>(T value)
        {
            var reference = new WeakReference(value);
            return () => (T)reference.Target;
        }

        /// <summary>
        /// Safely converts the value.
        /// </summary>
        /// <typeparam name="I">Input type.</typeparam>
        /// <typeparam name="O">Output type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="converters">The converters.</param>
        /// <returns>Result of first successful conversion.</returns>
        /// <exception cref="Exception">If none of converters succeeded it rethrows exception thrown by the last one.</exception>
        [DebuggerStepThrough]
        public static O SafeConvert<I, O>(I value, params Converter<I, O>[] converters)
        {
            Exception lastException = null;

            foreach (var converter in converters)
            {
                try
                {
                    return converter(value);
                }
                catch (Exception e)
                {
                    lastException = e;
                }
            }

            throw lastException;
        }

        #endregion

        #region ignore exception

        /// <summary>
        /// Executes the code ignoring the exception.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>
        ///   <c>null</c> or exception that has been thrown.
        /// </returns>
        public static Exception IgnoreException(Action method)
        {
            try
            {
                method();
                return null;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        /// <summary>
        /// Ignores the exception.
        /// </summary>
        /// <typeparam name="T">The exception type to ignore</typeparam>
        /// <param name="method">The method to call</param>
        /// <returns>The exception ignored or null</returns>
        public static Exception IgnoreException<T>(Action method) where T : Exception
        {
            try
            {
                method();
                return null;
            }
            catch (T ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// Ignores the exception.
        /// </summary>
        /// <typeparam name="TException">The type of the exception to ignore</typeparam>
        /// <typeparam name="TResult">The type of the result to return</typeparam>
        /// <param name="factory">The factory method to call.</param>
        /// <param name="defaultValue">The default value returned when the specific exception of type TException is encountered</param>
        /// <returns>The result of the factory method or the defaultValue</returns>
        public static TResult IgnoreException<TException, TResult>(Func<TResult> factory, TResult defaultValue)
            where TException : Exception
        {
            try
            {
                return factory();
            }
            catch (TException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Executes the code ignoring the exception. Returns value returned 
        /// by <paramref name="factory"/> or <paramref name="defaultValue"/> if factory failed.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T IgnoreException<T>(Func<T> factory, T defaultValue)
        {
            try
            {
                return factory();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        #region dispose

        /// <summary>Tries to dispose the and sets to null.</summary>
        /// <typeparam name="T">Type of variable.</typeparam>
        /// <param name="variable">The variable.</param>
        /// <returns><c>true</c> is variable was disposable, <c>false</c> otherwise</returns>
        public static bool DisposeAndNull<T>(ref T variable) where T : class
        {
            if (object.ReferenceEquals(variable, null))
            {
                return false;
            }

            var disposable = variable as IDisposable;
            variable = null;

            if (disposable == null)
            {
                return false;
            }

            disposable.Dispose();

            return true;
        }

        /// <summary>Disposes many objects.</summary>
        /// <param name="collection">The collection.</param>
        public static void DisposeMany<T>(IEnumerable<T> collection)
            where T : IDisposable
        {
            if (collection != null)
                collection.Where(i => !object.ReferenceEquals(i, null)).ForEach(i => i.Dispose());
        }

        /// <summary>Disposes many objects and clear the collection.</summary>
        /// <typeparam name="T">Type of objects.</typeparam>
        /// <param name="collection">The collection.</param>
        public static void DisposeManyAnyClear<T>(ICollection<T> collection)
            where T : IDisposable
        {
            var buffer = collection.ToArray();
            collection.Clear();
            DisposeMany(buffer);
        }

        /// <summary>Disposes many object from dictionary. Note, only Values (not Keys) are disposed.</summary>
        /// <typeparam name="K">Type of key.</typeparam>
        /// <typeparam name="V">Type of values.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        public static void DisposeManyAnyClear<K, V>(IDictionary<K, V> dictionary)
            where V : IDisposable
        {
            var buffer = dictionary.Values.ToArray();
            dictionary.Clear();
            DisposeMany(buffer);
        }

        /// <summary>Creates a DisposeScope, the container for items to be disposed.</summary>
        /// <returns>The container.</returns>
        /// <example><code>
        /// using (var disposables = Patterns.DisposeScope())
        /// {
        ///    for (int i = 1000; i != 0; i--)
        ///    {
        ///        DoSomething(disposable.Add(DisposableObjectFactory());
        ///    }
        /// } // all objects created by DisposableObjectFactory will be disposed
        /// </code></example>
        public static DisposableBag DisposeScope()
        {
            return new DisposableBag();
        }

        #endregion

        #region BinaryFindFirst

        /// <summary>
        /// Uses binary search to find first index which satisfied criteria.
        /// NOTE, It works with assumption that responses from filter function is 000011111111, 
        /// it won't work with 000011110000 (ask MK for explanation).
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="stopIndex">Index of the stop.</param>
        /// <param name="leapScan">if set to <c>true</c> it jumps forward first to find a range for binary search. 
        /// Be aware that it makes sense only if <paramref name="stopIndex"/> is really large but you expect the 
        /// index you are looking for is much closer to <paramref name="startIndex"/> or getting value for high
        /// indexes is slow comparing to getting value for low indexes. Anyway, it's safer to set it to <c>false</c>.</param>
        /// <param name="filter">The filter function.</param>
        /// <returns>Found index, or <c>null</c></returns>
        public static int? BinaryFindFirst(int startIndex, int stopIndex, bool leapScan, Converter<int, bool> filter)
        {
            if (filter(startIndex)) return startIndex;
            if (!filter(stopIndex)) return null;

            long start = startIndex;
            long stop = stopIndex;

            if (leapScan)
            {
                long init = start;
                long offset = 16;
                long mid = init + offset;

                // leap forward...
                while (mid < stop)
                {
                    if (filter((int)mid))
                    {
                        stop = mid;
                        break;
                    }
                    else
                    {
                        start = mid + 1;
                        offset <<= 1; // * 2
                                      // offset += (offset >> 1); // * 1.5
                        mid = init + offset;
                    }
                }
            }

            // standard binary search
            while (stop > start)
            {
                long mid = (start + stop) >> 1; // div 2

                if (filter((int)mid))
                {
                    stop = mid;
                }
                else
                {
                    start = mid + 1;
                }
            }

            return (int)stop;
        }

        #endregion

        #region Synchronize & Fork

        #region WaitHandle

        /// <summary>
        /// Waits for all of the supplied WaitHandles to be set, calling the callback when they are all set or the timeout has occured.
        /// The callback will contain a bool param to indicate if all of the WaitHandles were signalled.
        /// </summary>
        /// <param name="handles">The handles to wait for</param>
        /// <param name="callback">The callback, with a flag to indicate if the WaitHandles were all signalled</param>
        /// <param name="timeout">The timeout.</param>
        public static void WaitAllAndCallback(IEnumerable<WaitHandle> handles, Action<bool> callback, TimeSpan timeout)
        {
            WaitHandle[] handleArr = handles.ToArray<WaitHandle>();
            ThreadPool.QueueUserWorkItem(
                Patterns.WaitOnAllAndCallbackAsync,
                new VTuple<WaitHandle[], Action<bool>, TimeSpan>(handleArr, callback, timeout));
        }

        /// <summary>
        /// Waits for all of the supplied WaitHandles to be set, calling the callback when they are all set or the timeout has occured.
        /// The callback will contain a bool param to indicate if all of the WaitHandles were signalled.
        /// </summary>
        /// <param name="handles">The handles to wait for</param>
        /// <param name="callback">The callback, with a flag to indicate if the WaitHandles were all signalled</param>
        /// <param name="timeoutMs">The timeout in MilliSeconds.</param>
        public static void WaitAllAndCallback(IEnumerable<WaitHandle> handles, Action<bool> callback, int timeoutMs)
        {
            WaitAllAndCallback(handles, callback, TimeSpan.FromMilliseconds(timeoutMs));
        }

        /// <summary>
        /// Waits for all of the supplied WaitHandles to be set, calling the callback when they are all set or the timeout has occured.
        /// The callback will contain a bool param to indicate if all of the WaitHandles were signalled.
        /// </summary>
        /// <param name="callback">The callback, with a flag to indicate if the WaitHandles were all signalled</param>
        /// <param name="timeoutMs">The timeout in MilliSeconds.</param>
        /// <param name="handles">The handles to wait for</param>
        public static void WaitAllAndCallback(Action<bool> callback, int timeoutMs, params WaitHandle[] handles)
        {
            WaitAllAndCallback(callback, TimeSpan.FromMilliseconds(timeoutMs), handles);
        }

        /// <summary>
        /// Waits for all of the supplied WaitHandles to be set, calling the callback when they are all set or the timeout has occured.
        /// The callback will contain a bool param to indicate if all of the WaitHandles were signalled.
        /// </summary>
        /// <param name="callback">The callback, with a flag to indicate if the WaitHandles were all signalled</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="handles">The handles to wait for</param>
        public static void WaitAllAndCallback(Action<bool> callback, TimeSpan timeout, params WaitHandle[] handles)
        {
            ThreadPool.QueueUserWorkItem(
                Patterns.WaitOnAllAndCallbackAsync,
                new VTuple<WaitHandle[], Action<bool>, TimeSpan>(handles, callback, timeout));
        }

        /// <summary>
        /// Waits the on all the WaitHandles and calls the CallBack
        /// This is the asynchronous method 
        /// </summary>
        /// <param name="threadContext">The thread context.</param>
        private static void WaitOnAllAndCallbackAsync(object threadContext)
        {
            var data = (VTuple<WaitHandle[], Action<bool>, TimeSpan>)threadContext;
            WaitHandle[] handles = data.Item1;
            Action<bool> callBack = data.Item2;
            TimeSpan timeOut = data.Item3;

            callBack.Invoke(WaitHandle.WaitAll(handles, timeOut));
        }

        #endregion

        #region InvokeGuard

        /// <summary>Invoke guard. Allows to specify timeout for syncing to avoid deadlock 
        /// (it will still throw the exception though).</summary>
        /// <param name="target">The target.</param>
        /// <param name="method">The method.</param>
        /// <param name="syncTimeout">The sync timeout.</param>
        /// <returns>Result returned by delegate.</returns>
        private static object InvokeGuard(ISynchronizeInvoke target, Delegate method, int syncTimeout)
        {
            object[] result = new object[1]; // one element array store result

            var abortSignal = new ManualResetEvent(false);
            var confirmSignal = new ManualResetEvent(false);
            var startSignal = new ManualResetEvent(false);

            Action receiver = () => {
                using (abortSignal) using (startSignal) using (confirmSignal)
                {
                    confirmSignal.Set(); // confirm that you got in

                    // wait for start (got in quickly)
                    int sig = WaitHandle.WaitAny(new WaitHandle[] { startSignal, abortSignal });

                    if (sig == 0) // startSignal
                    {
                        // update result (held externally)
                        result[0] = method.DynamicInvoke(null);
                    }
                    /* else // sigAbort
					{
						// returned from deadlock, but caller is no longer waiting
					} */

                    // it's done!
                }
            };

            var asyncResult = target.BeginInvoke(receiver, null);

            if (confirmSignal.WaitOne(syncTimeout)) // wait for method to start
            {
                startSignal.Set(); // yup, started - allow to proceed
                asyncResult.AsyncWaitHandle.WaitOne(); // and wait for results
            }
            else
            {
                // was waiting, but method did not started...
                abortSignal.Set();
                throw new InvalidOperationException("Synchronous invoke caused deadlock");
            }

            return result[0];
        }

        #endregion

        #region Synchronize

        /// <summary>Calls action in sync with target. Allows to call it and wait or 'fire and forget'.</summary>
        /// <param name="action">The action to call.</param>
        /// <param name="target">The synchronization target.</param>
        /// <param name="syncTimeout">The synchronization timeout. Please note that timeout is not about execution, it's about
        /// time to synchronize threads, the action itself can take forever, once method is sync'ed timeout no longer applies.
        /// Use value of <c>0</c> do not wait,
        /// <see cref="Timeout.Infinite"/> to wait forever, or any other value to make timeout active.</param>
        /// <param name="cancelToken">The cancel token.</param>
        public static void Synchronize(
            Action action,
            ISynchronizeInvoke target, int syncTimeout = Timeout.Infinite,
            CancellationToken? cancelToken = null)
        {
            if (action == null)
            {
                return;
            }

            if (cancelToken.HasValue)
            {
                var token = cancelToken.Value;
                if (token.IsCancellationRequested)
                    return; // no action is executed, it has been already canceled

                var actionOriginal = action;
                action = () => {
                    if (token.IsCancellationRequested)
                        return; // check when in-sync
                    actionOriginal();
                };
            }

            if (target == null || !target.InvokeRequired)
            {
                // just call it
                action();
            }
            else
            {
                if (syncTimeout == Timeout.Infinite)
                {
                    // call and wait (possible deadlocks)
                    target.Invoke(action, null);
                }
                else if (syncTimeout <= 0)
                {
                    // fire and forget
                    target.BeginInvoke(action, null);
                }
                else
                {
                    // call and wait for sync, throw exception if not sync'ed quick enough
                    InvokeGuard(target, action, syncTimeout);
                }
            }
        }

        /// <summary>Calls action in sync with target. Allows to call it and wait or 'fire and forget'. Returns a function
        /// which you can use (immediately or later) to collect the returned result.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="target">The synchronization target.</param>
        /// <param name="syncTimeout">The synchronization timeout. Please note that timeout is not about execution, it's about
        /// time to synchronize threads, the action itself can take forever, once method is sync'ed timeout no longer applies.
        /// Use value of <c>0</c> do not wait,
        /// <see cref="Timeout.Infinite"/> to wait forever, or any other value to make timeout active.</param>
        /// <returns></returns>
        public static Func<T> Synchronize<T>(this Func<T> factory, ISynchronizeInvoke target, int syncTimeout = Timeout.Infinite)
        {
            Func<T, Func<T>> functionize = (result) => (() => result); // fun!

            if (target == null || !target.InvokeRequired)
            {
                // just call it
                return functionize(factory());
            }
            else
            {
                if (syncTimeout == Timeout.Infinite)
                {
                    // call and wait (possible deadlocks)
                    return functionize((T)target.Invoke(factory, null));
                }
                else if (syncTimeout <= 0)
                {
                    // call and do not wait
                    // return a function which will wait for result
                    var asyncResult = target.BeginInvoke(factory, null);
                    return () => (T)target.EndInvoke(asyncResult);
                }
                else
                {
                    // call and wait for sync, throw exception if not sync'ed quick enough
                    return functionize((T)InvokeGuard(target, factory, syncTimeout));
                }
            }
        }

        #endregion

        #region Fork

        /// <summary>
        /// Resolves what exception should be thrown depending on task's result.
        /// </summary>
        /// <param name="taskCompleted">should be set to <c>true</c> if task has been completed.</param>
        /// <param name="taskCancelled">should be set to <c>true</c> if task has been cancelled.</param>
        /// <param name="taskFailure">The task failure.</param>
        /// <returns>Exception to be thrown, or <c>null</c> if no error.</returns>
        internal static Exception ResolveTaskException(bool taskCompleted, bool taskCancelled, Exception taskFailure)
        {
            if (!taskCompleted)
            {
                return new TimeoutException();
            }

            if (taskCancelled)
            {
                return new CanceledException();
            }

            var aggregateException = taskFailure as AggregateException;
            if (aggregateException != null && aggregateException.InnerExceptions.Count == 1)
            {
                taskFailure = aggregateException.InnerExceptions[0];
            }

            return taskFailure == null ? null : ForkedException.Make(taskFailure);
        }

        internal static Exception ResolveTaskException(Task task)
        {
            return ResolveTaskException(task.IsCompleted, task.IsCanceled, task.Exception);
        }

        internal static Exception ResolveTaskException<T>(Task<T> task)
        {
            return ResolveTaskException(task.IsCompleted, task.IsCanceled, task.Exception);
        }

        /// <summary>Invokes the task callback when task is finished.</summary>
        /// <param name="task">The task.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target.</param>
        private static void InvokeTaskCallback(
            Task task,
            Action<Exception> callback, ISynchronizeInvoke callbackSynchronizationTarget)
        {
            Patterns.IgnoreException(task.Wait); // note, it ignores exception but only because it's gets returned by task.Exception
            var exception = ForkedException.Strip(ResolveTaskException(task.IsCompleted, task.IsCanceled, task.Exception));
            callbackSynchronizationTarget.Synchronize(() => callback(exception), 0);
        }

        /// <summary>Invokes the task callback when task is finished.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target.</param>
        private static void InvokeTaskCallback<T>(
            Task<T> task,
            Action<T, Exception> callback, ISynchronizeInvoke callbackSynchronizationTarget)
        {
            Patterns.IgnoreException(task.Wait); // note, it ignores exception but only because it's gets returned by task.Exception
            var exception = ForkedException.Strip(ResolveTaskException(task.IsCompleted, task.IsCanceled, task.Exception));
            var result = (exception == null) ? task.Result : default(T);
            callbackSynchronizationTarget.Synchronize(() => callback(result, exception), 0);
        }

        /// <summary>
        /// Forks the execution by spawning action in separate thread.
        /// Please note, this is not a replacement for other multi threading techniques.
        /// It is meant to simplify things, when you occasionally need to spawn simple one-liner in separate
        /// thread without all the overhead.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="callback">The callback (can be <c>null</c>).</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target (can be <c>null</c>).</param>
        /// <returns><see cref="ForkedAction"/> object.</returns>
        public static ForkedAction Fork(
            Action action,
            Action<Exception> callback = null, ISynchronizeInvoke callbackSynchronizationTarget = null)
        {
            var task = Task.Factory.StartNew(action);

            if (callback != null)
            {
                Task.Factory.StartNew(() => InvokeTaskCallback(task, callback, callbackSynchronizationTarget));
            }

            return new ForkedAction(task);
        }

        /// <summary>
        /// Forks the execution by spawning factory in separate thread.
        /// </summary>
        /// <typeparam name="T">Factory result type.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target.</param>
        /// <returns><see cref="ForkedFunc{T}"/> object.</returns>
        public static ForkedFunc<T> Fork<T>(
            Func<T> factory,
            Action<T, Exception> callback = null, ISynchronizeInvoke callbackSynchronizationTarget = null)
        {
            var task = Task<T>.Factory.StartNew(factory);

            if (callback != null)
            {
                Task.Factory.StartNew(() => InvokeTaskCallback(task, callback, callbackSynchronizationTarget));
            }

            return new ForkedFunc<T>(task);
        }

        #region AbstractForkedThread adapters

        private static Action<ForkedActionThread> ForkAdapter(Action action)
        {
            return action == null
                ? (Action<ForkedActionThread>)null
                : (info) => action();
        }

        private static Func<ForkedFuncThread<T>, T> ForkAdapter<T>(Func<T> factory)
        {
            return factory == null
                ? (Func<ForkedFuncThread<T>, T>)null
                : (info) => factory();
        }

        private static Action<ForkedActionThread> ForkAdapter(Action<Exception> onFinished)
        {
            return onFinished == null
                ? (Action<ForkedActionThread>)null
                : (info) => onFinished(info.Exception);
        }

        private static Action<ForkedFuncThread<T>> ForkAdapter<T>(Action<T, Exception> onFinished)
        {
            return onFinished == null
                ? (Action<ForkedFuncThread<T>>)null
                : (info) => onFinished(info.ResultOrDefault, info.Exception);
        }

        #endregion

        /// <summary>
        /// Forks the execution by spawning action in separate thread.
        /// Please note, this is not a replacement for other multithreading techniques.
        /// It is meant to simplify things, when you occasionally need to spawn simple one-liner in separate
        /// thread without all the overhead.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="onFinished">The callback (can be <c>null</c>).</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target (can be <c>null</c>).</param>
        /// <returns><see cref="ForkedAction"/> object.</returns>
        public static ForkedActionThread ForkThread(
            Action action,
            Action<ForkedActionThread> onFinished = null,
            ISynchronizeInvoke callbackSynchronizationTarget = null)
        {
            var result = new ForkedActionThread(
                ForkAdapter(action),
                onFinished,
                callbackSynchronizationTarget);
            result.Start();
            return result;
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
        /// <returns><see cref="ForkedAction"/> object.</returns>
        public static ForkedActionThread ForkThread(
            Action<ForkedActionThread> action,
            Action<ForkedActionThread> onFinished = null,
            ISynchronizeInvoke callbackSynchronizationTarget = null)
        {
            var result = new ForkedActionThread(
                action,
                onFinished,
                callbackSynchronizationTarget);
            result.Start();
            return result;
        }

        /// <summary>
        /// Forks the execution by spawning action in separate thread.
        /// Please note, this is not a replacement for other multithreading techniques.
        /// It is meant to simplify things, when you occasionally need to spawn simple one-liner in separate
        /// thread without all the overhead.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="onFinished">The callback (can be <c>null</c>).</param>
        /// <param name="onSuspended">The OnSuspeded handler.</param>
        /// <param name="onResumed">The OnResumed handler.</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target (can be <c>null</c>).</param>
        /// <returns><see cref="ForkedAction"/> object.</returns>
        public static ForkedActionThread ForkThread(
            Action action,
            Action<ForkedActionThread> onFinished, Action onSuspended, Action onResumed,
            ISynchronizeInvoke callbackSynchronizationTarget = null)
        {
            var result = new ForkedActionThread(
                ForkAdapter(action),
                onFinished, onSuspended, onResumed, callbackSynchronizationTarget);
            result.Start();
            return result;
        }

        /// <summary>
        /// Forks the execution by spawning action in separate thread.
        /// Please note, this is not a replacement for other multithreading techniques.
        /// It is meant to simplify things, when you occasionally need to spawn simple one-liner in separate
        /// thread without all the overhead.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="onFinished">The callback (can be <c>null</c>).</param>
        /// <param name="onSuspended">The OnSuspeded handler.</param>
        /// <param name="onResumed">The OnResumed handler.</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target (can be <c>null</c>).</param>
        /// <returns><see cref="ForkedAction"/> object.</returns>
        public static ForkedActionThread ForkThread(
            Action<ForkedActionThread> action,
            Action<ForkedActionThread> onFinished, Action onSuspended, Action onResumed,
            ISynchronizeInvoke callbackSynchronizationTarget = null)
        {
            var result = new ForkedActionThread(
                action,
                onFinished, onSuspended, onResumed, callbackSynchronizationTarget);
            result.Start();
            return result;
        }

        /// <summary>
        /// Forks the execution by spawning factory in separate thread.
        /// </summary>
        /// <typeparam name="T">Factory result type.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="onFinished">The callback.</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target.</param>
        /// <returns><see cref="ForkedFunc{T}"/> object.</returns>
        public static ForkedFuncThread<T> ForkThread<T>(
            Func<T> factory,
            Action<ForkedFuncThread<T>> onFinished = null, ISynchronizeInvoke callbackSynchronizationTarget = null)
        {
            var result = new ForkedFuncThread<T>(
                ForkAdapter(factory),
                onFinished,
                callbackSynchronizationTarget);
            result.Start();
            return result;
        }

        /// <summary>
        /// Forks the execution by spawning factory in separate thread.
        /// </summary>
        /// <typeparam name="T">Factory result type.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="onFinished">The callback.</param>
        /// <param name="onSuspended">The OnSuspeded handler.</param>
        /// <param name="onResumed">The OnResumed handler.</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target.</param>
        /// <returns><see cref="ForkedFunc{T}"/> object.</returns>
        public static ForkedFuncThread<T> ForkThread<T>(
            Func<T> factory,
            Action<ForkedFuncThread<T>> onFinished, Action onSuspended, Action onResumed,
            ISynchronizeInvoke callbackSynchronizationTarget = null)
        {
            var result = new ForkedFuncThread<T>(
                ForkAdapter(factory),
                onFinished,
                onSuspended, onResumed,
                callbackSynchronizationTarget);
            result.Start();
            return result;
        }

        /// <summary>
        /// Forks the execution by spawning factory in separate thread.
        /// </summary>
        /// <typeparam name="T">Factory result type.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="onFinished">The callback.</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target.</param>
        /// <returns><see cref="ForkedFunc{T}"/> object.</returns>
        public static ForkedFuncThread<T> ForkThread<T>(
            Func<ForkedFuncThread<T>, T> factory,
            Action<ForkedFuncThread<T>> onFinished = null, ISynchronizeInvoke callbackSynchronizationTarget = null)
        {
            var result = new ForkedFuncThread<T>(
                factory,
                onFinished,
                callbackSynchronizationTarget);
            result.Start();
            return result;
        }

        /// <summary>
        /// Forks the execution by spawning factory in separate thread.
        /// </summary>
        /// <typeparam name="T">Factory result type.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="onFinished">The callback.</param>
        /// <param name="onSuspended">The OnSuspeded handler.</param>
        /// <param name="onResumed">The OnResumed handler.</param>
        /// <param name="callbackSynchronizationTarget">The callback synchronization target.</param>
        /// <returns><see cref="ForkedFunc{T}"/> object.</returns>
        public static ForkedFuncThread<T> ForkThread<T>(
            Func<ForkedFuncThread<T>, T> factory,
            Action<ForkedFuncThread<T>> onFinished, Action onSuspended, Action onResumed,
            ISynchronizeInvoke callbackSynchronizationTarget = null)
        {
            var result = new ForkedFuncThread<T>(
                factory,
                onFinished,
                onSuspended, onResumed,
                callbackSynchronizationTarget);
            result.Start();
            return result;
        }

        #endregion

        #region Defer

        private static long _timerId = 0;
        private static ConcurrentDictionary<long, Timer> _timerMap =
            new ConcurrentDictionary<long, Timer>();

        /// <summary>Defers the operation by given time.</summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="action">The action.</param>
        public static void Defer(TimeSpan timeout, Action action)
        {
            Defer(timeout, CancellationToken.None, action);
        }

        /// <summary>Defers the operation by given time.</summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="action">The action.</param>
        public static void Defer(TimeSpan timeout, CancellationToken cancellationToken, Action action)
        {
            Defer((int)timeout.TotalMilliseconds, cancellationToken, action);
        }

        /// <summary>Defers the operation by given time.</summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="action">The action.</param>
        private static void Defer(int timeout, CancellationToken cancellationToken, Action action)
        {
            var ready = 0;
            var id = Interlocked.Increment(ref _timerId);
            var timer = default(Timer);
            var handler = new TimerCallback(_ => {
                while (Interlocked.CompareExchange(ref ready, 0, 0) == 0) Thread.Yield();

                try
                {
                    Timer removed;
                    var success = _timerMap.TryRemove(id, out removed);
                    Debug.Assert(!success || object.ReferenceEquals(removed, timer));

                    if (!cancellationToken.IsCancellationRequested && success)
                    {
                        action();
                    }
                }
                finally
                {
                    timer.Dispose();
                }
            });

            timer = new Timer(handler, null, timeout, Timeout.Infinite);
            _timerMap.TryAdd(id, timer);
            Interlocked.Exchange(ref ready, 1);
        }

        #endregion

        #endregion

        #region Scope

        /// <summary>
        /// Reuses the object or creates new one. Returns a disposable object
        /// which will dispose the object if object was created or will do nothing
        /// if object was reused. Example usage, is to reuse or create a database
        /// connection. If connection has been reused it should not be disposed, if it
        /// was created it should.
        /// </summary>
        /// <typeparam name="T">Diposable reference type.</typeparam>
        /// <param name="subject">The object.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>IDisposable handling disposing the subject if it was created
        /// (it is NOT a subject itself).</returns>
        public static IDisposable UseOrCreate<T>(ref T subject, Func<T> factory)
            where T : class, IDisposable
        {
            if (subject == null)
            {
                var created = factory();
                subject = created;
                return Scope(() => Patterns.DisposeAndNull(ref created));
            }
            else
            {
                // subject does not change
                return EmptyDisposable.Default;
            }
        }

        #region class ScopeHelper

        private class ScopeHelper : IDisposable
        {
            #region fields

            private readonly Action m_Action;

            #endregion

            #region constructor

            public ScopeHelper(Action action)
            {
                m_Action = action;
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                if (m_Action != null) m_Action();
            }

            #endregion
        }

        #endregion

        /// <summary>Creates an IDisposable scope which executes given action when disposed.</summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>IDisposable scope.</returns>
        public static IDisposable Scope(Action action)
        {
            return new ScopeHelper(action);
        }

        #endregion

        #region simple

        /// <summary>Swaps two variables.</summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="a">Parameter A.</param>
        /// <param name="b">Parameter B.</param>
        public static void Swap<T>(ref T a, ref T b)
        {
            var t = a; a = b; b = t;
        }

        // "EXECUTE_PATTERNS_NOOP" should be never defined

        /// <summary>
        /// This is a filler for 'no code block'.
        /// If you want a block of code which does nothing (buy you want to emphasize this fact) call this method.
        /// It makes Refactor!Pro stop complaining. It has no effects at all. It is not even compiled 
        /// (unless you define <c>EXECUTE_PATTERNS_NOOP</c> conditional).
        /// </summary>
        [Conditional("EXECUTE_PATTERNS_NOOP")]
        public static void NoOp()
        {
            while (false) ;
        }

        /// <summary>This methods helps avoid 'variable not used' or 'variable can be read-only' messages.</summary>
        [Conditional("EXECUTE_PATTERNS_NOOP")]
        public static void NoOp<T>(ref T variable)
        {
            // I could not find anything less invasive than object.ReferenceEquals
            object.ReferenceEquals(variable, null);
        }

        /// <summary>This methods helps avoid 'variable not used' messages.</summary>
        [Conditional("EXECUTE_PATTERNS_NOOP")]
        public static void NoOp(object value)
        {
            // I could not find anything less invasive than object.ReferenceEquals
            object.ReferenceEquals(value, null);
        }

        /// <summary>This methods helps avoid 'variable not used' messages.</summary>
        [Conditional("EXECUTE_PATTERNS_NOOP")]
        public static void NoOp(params object[] values)
        {
            // I could not find anything less invasive than object.ReferenceEquals
            object.ReferenceEquals(values, null);
        }

        /// <summary>This methods DOES NOT call an action.</summary>
        [Conditional("EXECUTE_PATTERNS_NOOP")]
        public static void NoOp(Action action)
        {
            object.ReferenceEquals(action, null);
        }

        #endregion

        #region compare enumerables

        /// <summary>
        /// Difference. Note, if result is 0 both items are the same, 
        /// if result is less then 0 Item1 is less then Item2 or Item2 does not exist, 
        /// if result is greater than 0 Item1 is greated then Item2 or Item1 does not exist.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        public class DifferenceItem<T>
        {
            /// <summary>Result of comparision.</summary>
            public int Result { get; internal set; }

            /// <summary>Gets the item from list 1.</summary>
            public T Item1 { get; internal set; }

            /// <summary>Gets the item from list 2.</summary>
            public T Item2 { get; internal set; }

            /// <summary>Initializes a new instance of the <see cref="DifferenceItem&lt;T&gt;"/> class.</summary>
            /// <param name="result">The result.</param>
            /// <param name="item1">The item1.</param>
            /// <param name="item2">The item2.</param>
            public DifferenceItem(int result, T item1, T item2)
            {
                Result = result;
                Item1 = item1;
                Item2 = item2;
            }
        }

        /// <summary>Returns the differendes between items in two collection. 
        /// Note, collections need to be soreted.</summary>
        /// <typeparam name="T">Any comparable type.</typeparam>
        /// <param name="enumeratorA">The enumerator A.</param>
        /// <param name="enumeratorB">The enumerator B.</param>
        /// <returns>Collection of differences.</returns>
        public static IEnumerable<DifferenceItem<T>> Difference<T>(
            IEnumerator<T> enumeratorA, IEnumerator<T> enumeratorB)
            where T : IComparable<T>
        {
            return Difference(enumeratorA, enumeratorB, (a, b) => a.CompareTo(b));
        }

        /// <summary>Returns the differendes between items in two collection.
        /// Note, collections need to be sorted.</summary>
        /// <typeparam name="T">Any comparable type.</typeparam>
        /// <param name="enumeratorA">The enumerator A.</param>
        /// <param name="enumeratorB">The enumerator B.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>Collection of differences.</returns>
        public static IEnumerable<DifferenceItem<T>> Difference<T>(
            IEnumerator<T> enumeratorA, IEnumerator<T> enumeratorB,
            Comparison<T> comparer)
        {
            var eofA = !enumeratorA.MoveNext();
            var eofB = !enumeratorB.MoveNext();

            while (!eofA && !eofB)
            {
                var itemA = enumeratorA.Current;
                var itemB = enumeratorB.Current;
                int c = comparer(itemA, itemB);

                yield return new DifferenceItem<T>(c, itemA, itemB);

                if (c == 0)
                {
                    eofA = !enumeratorA.MoveNext();
                    eofB = !enumeratorB.MoveNext();
                }
                else if (c < 0)
                {
                    eofA = !enumeratorA.MoveNext();
                }
                else
                {
                    eofB = !enumeratorB.MoveNext();
                }
            }

            if (eofA)
            {
                while (!eofB)
                {
                    yield return new DifferenceItem<T>(1, default(T), enumeratorB.Current);
                    eofB = !enumeratorB.MoveNext();
                }
            }

            if (eofB)
            {
                while (!eofA)
                {
                    yield return new DifferenceItem<T>(-1, enumeratorA.Current, default(T));
                    eofA = !enumeratorB.MoveNext();
                }
            }
        }

        #endregion

        #region Bufferize, Debufferize

        /// <summary>Create byte buffer using BinaryWriter.</summary>
        /// <param name="writer">The writer callback.</param>
        /// <returns>Byte buffer.</returns>
        public static byte[] Bufferize(Action<BinaryWriter> writer)
        {
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                writer(binaryWriter);
                binaryWriter.Flush();
                return memoryStream.ToArray();
            }
        }

        /// <summary>Extract information from BinaryReader from byte buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="reader">The reader callback.</param>
        public static void Debufferize(byte[] buffer, Action<BinaryReader> reader)
        {
            using (var memoryStream = new MemoryStream(buffer))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                reader(binaryReader);
            }
        }

        /// <summary>Extract information from BinaryReader from byte buffer.</summary>
        /// <typeparam name="T">Returned type.</typeparam>
        /// <param name="buffer">The buffer.</param>
        /// <param name="reader">The reader callback.</param>
        /// <returns>A result of <paramref name="reader"/> callback.</returns>
        public static T Debufferize<T>(byte[] buffer, Func<BinaryReader, T> reader)
        {
            using (var memoryStream = new MemoryStream(buffer))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                return reader(binaryReader);
            }
        }

        #endregion

        #region GetPropertyInfo

        /// <summary>Gets the property info. </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="action">The action which returns property in question.</param>
        /// <returns>MemberInfo structure.</returns>
        public static MemberInfo GetPropertyInfo<T>(Expression<Func<T, object>> action)
        {
            var expression = action.Body;

            // as the action can contain boxing (action returns object) strip 
            // potential boxing expression
            var convertExpression = expression as UnaryExpression;
            var isBoxing =
                convertExpression != null &&
                convertExpression.NodeType == ExpressionType.Convert &&
                convertExpression.Type == typeof(object);
            if (isBoxing)
                expression = convertExpression.Operand;

            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
                return memberExpression.Member;

            return null;
        }

        #endregion

        #region SystemLock

        /// <summary>Creates a system-wide lock.</summary>
        /// <param name="lockId">The lock id.</param>
        /// <param name="timeout">The time to wait for resource (in seconds).</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="retry">The retry query. Should return <c>true</c> if acquiring lock should be retried.</param>
        /// <returns>
        ///   <c>true</c> if lock has been acquired and action has been executed, <c>false</c> if lock has not been acquired.
        /// </returns>
        public static bool SystemLock(
            string lockId,
            double timeout,
            Action action,
            Func<bool> retry = null)
        {
            bool created;

            using (var mutex = new Mutex(false, lockId, out created))
            {
                while (true)
                {
                    if (mutex.WaitOne(TimeSpan.FromSeconds(timeout)))
                    {
                        try
                        {
                            action();
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                        return true;
                    }
                    else
                    {
                        if (retry != null && retry())
                            continue; // if retry then retry
                        return false;
                    }
                }
            }
        }

        #endregion
    }
}
