using System;

using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Events
{
    /// <summary>
    /// Template for weak IObserver{T}.
    /// </summary>
    public class WeakObserver
	{
		#region class Proxy<T>

		private class Proxy<T>: IObserver<T>, IDisposable
		{
			private Func<IObserver<T>> _observer;
			private Func<IDisposable> _disposable;

			public Proxy(IObserver<T> observer, Func<IDisposable> disposable)
			{
				_observer = Patterns.Weak(observer);
				_disposable = disposable;
			}

			#region IObserver<T> Members

			public void OnCompleted()
			{
				var observer = _observer();
				if (observer == null)
				{
					Dispose();
				}
				else
				{
					observer.OnCompleted();
				}
			}

			public void OnError(Exception error)
			{
				var observer = _observer();
				if (observer == null)
				{
					Dispose();
				}
				else
				{
					observer.OnError(error);
				}
			}

			public void OnNext(T value)
			{
				var observer = _observer();
				if (observer == null)
				{
					Dispose();
				}
				else
				{
					observer.OnNext(value);
				}
			}

			#endregion

			#region IDisposable Members

			public void Dispose()
			{
				var disposable = _disposable();
				if (disposable != null)
					disposable.Dispose();
			}

			#endregion
		}

		#endregion

		#region public interface

		/// <summary>Weakly binds the specified observer to observable.</summary>
		/// <typeparam name="T">Type of data.</typeparam>
		/// <param name="observable">The observable.</param>
		/// <param name="observer">The observer.</param>
		/// <returns><see cref="IDisposable"/> allowing to unsubscribe prematurely.</returns>
		public static IDisposable Bind<T>(IObservable<T> observable, IObserver<T> observer)
		{
			IDisposable disposable = null;
			var proxy = new Proxy<T>(observer, () => disposable);
			disposable = observable.Subscribe(proxy);
			return proxy;
		}

		#endregion
	}
}
