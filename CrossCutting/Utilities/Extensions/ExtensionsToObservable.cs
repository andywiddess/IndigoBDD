using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace Indigo.CrossCutting.Utilities.Extensions
{
    /// <summary>
    /// Observable Extensions
    /// </summary>
    public static class ExtensionsToObservable
    {
        /// <summary>
        /// Pairwises the specified observable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observable">The observable.</param>
        /// <returns></returns>
        public static IObservable<T[]> Pairwise<T>(this IObservable<T> observable)
        {
            return Observable.Create<T[]>(observer => {
                var prev = default(T);
                var initialized = false;
                return observable.Subscribe(
                    next => {
                        if (initialized) observer.OnNext(new[] { prev, next });
                        initialized = true;
                        prev = next;
                    },
                    observer.OnError,
                    observer.OnCompleted);
            });
        }

        /// <summary>
        /// Selects the latest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="observable">The observable.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IObservable<U> SelectLatest<T, U>(
            this IObservable<T> observable,
            Func<T, CancellationToken, Task<U>> selector)
        {
            return observable
                .Select(item => Observable.FromAsync(token => selector(item, token)))
                .Switch();
        }
    }
}
