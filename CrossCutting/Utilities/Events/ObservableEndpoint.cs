using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Indigo.CrossCutting.Utilities.Collections;
using Indigo.CrossCutting.Utilities.DesignPatterns;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Base implementation of <see cref="IObservable{T}"/>. 
	/// Equvalent of Rx's Subject but not dependant on Rx.
	/// Please note, that it does not guarantee order of execution.
	/// </summary>
	/// <typeparam name="T">Type of value stored.</typeparam>
	public class ObservableEndpoint<T>: IObservable<T>, IObserver<T>
	{
		#region fields

		/// <summary>The subscriber counter.</summary>
		private int _subscriberId = 0;

		/// <summary>The subscriber collection.</summary>
		private ConcurrentDictionary<int, IObserver<T>> _subscriberMap =
			new ConcurrentDictionary<int, IObserver<T>>();

		#endregion

		#region IObservable<T> Members

		/// <summary>Subscribes the specified observer.</summary>
		/// <param name="observer">The observer.</param>
		/// <returns>Disposable used to unsubscribe.</returns>
		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (observer == null)
				return EmptyDisposable.Default;

			var id = Interlocked.Increment(ref _subscriberId);
			Debug.Assert(id >= 0);
			var added = _subscriberMap.TryAdd(id, observer);
			Debug.Assert(added);

			// we do not want non-disposed subscriber to hold reference
			// to this object, so subscriber map is passed as weak referece.
			var weakSubscriberMap = Patterns.Weak(_subscriberMap);
			return Patterns.Scope(() => {
				var subscriberMap = weakSubscriberMap();
				if (subscriberMap == null) return; // if it has been disposed already
				IObserver<T> found;
				subscriberMap.TryRemove(id, out found);
				Debug.Assert(object.ReferenceEquals(found, observer));
			});
		}

		#endregion

		#region private implementation

		/// <summary>executes same action for each of observers.</summary>
		/// <param name="action">The action.</param>
		private void ForEach(Action<IObserver<T>> action)
		{
			_subscriberMap.Values.Where(o => o != null).ForEach(action);
		}

		#endregion

		#region IObserver<T> Members

		/// <summary>Notifies the observer that the provider has finished sending push-based notifications.</summary>
		public void OnCompleted()
		{
			ForEach(o => o.OnCompleted());
		}

		/// <summary>Notifies the observer that the provider has experienced an error condition.</summary>
		/// <param name="error">An object that provides additional information about the error.</param>
		public void OnError(Exception error)
		{
			ForEach(o => o.OnError(error));
		}

		/// <summary>Provides the observer with new data.</summary>
		/// <param name="value">The current notification information.</param>
		public void OnNext(T value)
		{
			ForEach(o => o.OnNext(value));
		}

		#endregion
	}
}
