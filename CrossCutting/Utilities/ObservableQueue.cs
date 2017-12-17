using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Class using reactive extensions to model an observable queue
    /// Code from here: https://social.msdn.microsoft.com/Forums/en-US/ae49e7a2-3a7c-48e1-85da-d8d1bcaca7e9/how-to-implement-a-single-worker-consumer-producer-queue-using-rx?forum=rx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Reactive.Subjects.ISubject{System.Func{T}, T}" />
    /// <seealso cref="System.IDisposable" />
    public sealed class ObservableQueue<T>
        : ISubject<Func<T>, T>,
          IDisposable
    {
        #region Members
        /// <summary>
        /// The input object - receives incoming items
        /// </summary>
        private readonly Subject<Func<T>> input = new Subject<Func<T>>();

        /// <summary>
        /// The output object
        /// </summary>
        private readonly Subject<T> output = new Subject<T>();

        /// <summary>
        /// The subscription to the input stream
        /// </summary>
        private readonly IDisposable subscription;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableQueue{T}"/> class.
        /// </summary>
        public ObservableQueue()
        {
            // Use the default scheduler to observe the input stream. When an item is available
            // (function returning type T) the function is called and the result passed to the
            // output stream.
            subscription = input
                .ObserveOn(TaskPoolScheduler.Default)
                .Select(func => func())
                .Subscribe(output);
        }
        #endregion

        #region IObserver<TIn> Members
        /// <summary>
        /// Called when the next item is available
        /// </summary>
        /// <param name="value">The value.</param>
        public void OnNext(Func<T> value)
        {
            input.OnNext(value);
        }

        /// <summary>
        /// Called when an error condition occurs
        /// </summary>
        /// <param name="error">The error.</param>
        public void OnError(Exception error)
        {
            input.OnError(error);
        }

        /// <summary>
        /// Called when completed.
        /// </summary>
        public void OnCompleted()
        {
            input.OnCompleted();
        }
        #endregion IObserver<TIn> Members

        #region IObservable<TOut> Members
        /// <summary>
        /// Subscribes the specified observer to the output stream
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns>Returns a disposable object that can be used to unsubscribe the observer from the subject</returns>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            return output.Subscribe(observer);
        }
        #endregion IObservable<TOut> Members

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            subscription.Dispose();
        }
        #endregion IDisposable Members
    }
}