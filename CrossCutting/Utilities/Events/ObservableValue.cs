using System;
using System.Collections.Generic;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Events
{
	/// <summary>
	/// Helper class for <see cref="ObservableValue{T}"/>
	/// </summary>
	public static class ObservableValue
	{
		/// <summary>Creates <see cref="ObservableValue{T}"/> initialized with default value.</summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <returns><see cref="ObservableValue{T}"/></returns>
		public static ObservableValue<T> Create<T>() { return new ObservableValue<T>(); }

		/// <summary>Creates the <see cref="ObservableValue{T}"/> initialized with given value.</summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <param name="initialValue">The initial value.</param>
		/// <returns><see cref="ObservableValue{T}"/></returns>
		public static ObservableValue<T> Create<T>(T initialValue) { return new ObservableValue<T>(initialValue); }
	}

	/// <summary>
	/// Observable value implementing IObservableValue{T} interface.
	/// </summary>
	/// <typeparam name="T">Type of value stored.</typeparam>
	public class ObservableValue<T>: IObservableValue<T>, IObserver<T>
	{
		#region fields

		/// <summary>The value lock.</summary>
		private object _valueLock = new object();

		/// <summary>The value.</summary>
		private T _value;

		/// <summary>The value comparer.</summary>
		private IEqualityComparer<T> _comparer;

		/// <summary>Flag indicating if events should be raised when new listener subscribes.</summary>
		private int _raiseOnSubscribe;

		/// <summary>The observers collection.</summary>
		private ObservableEndpoint<T> _observers = new ObservableEndpoint<T>();

		#endregion

		#region IObservableValue<T> Members

		/// <summary>Gets or sets the value.</summary>
		/// <value>The value.</value>
		public T Value
		{
			get
			{
				lock (_valueLock) return _value;
			}
			set
			{
				lock (_valueLock)
				{
					var comparer = Comparer;
					if (comparer != null && comparer.Equals(_value, value))
						return; // they are equal - do not set anything
					_value = value;
				}

				// call observers outside lock as they might want to change the value again
				_observers.OnNext(value);
			}
		}

		/// <summary>Subscribes the specified observer.</summary>
		/// <param name="observer">The observer.</param>
		/// <returns>Disposable used to unsubscribe.</returns>
		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (observer != null && RaiseOnSubscribe)
				observer.OnNext(Value);
			return _observers.Subscribe(observer);
		}

		#endregion

		#region public interface

		/// <summary>Gets or sets a value indicating whether events should fired on subscribe.</summary>
		/// <value><c>true</c> if events should fired on subscribe; otherwise, <c>false</c>.</value>
		public bool RaiseOnSubscribe
		{
			get { return Interlocked.CompareExchange(ref _raiseOnSubscribe, 0, 0) != 0; }
			set { Interlocked.Exchange(ref _raiseOnSubscribe, value ? 1 : 0); }
		}

		/// <summary>Gets or sets the comparer. By default set to <see cref="EqualityComparer{T}"/>. 
		/// Set it to <c>null</c> to get events fired on every change.</summary>
		/// <value>The comparer.</value>
		public IEqualityComparer<T> Comparer
		{
			get { return Interlocked.CompareExchange(ref _comparer, null, null); }
			set { Interlocked.Exchange(ref _comparer, value); }
		}

		/// <summary>Initializes a new instance of the <see cref="ObservableValue{T}" /> class.</summary>
		/// <param name="initialValue">The initial value.</param>
		public ObservableValue(T initialValue)
		{
			_value = initialValue;
			_raiseOnSubscribe = 1;
			_comparer = EqualityComparer<T>.Default;
		}

		/// <summary>Initializes a new instance of the <see cref="ObservableValue{T}"/> class.</summary>
		public ObservableValue()
			: this(default(T))
		{
		}

		#endregion

		#region IObserver<T> Members

		/// <summary>Notifies the observer that the provider has finished sending push-based notifications.</summary>
		public void OnCompleted()
		{
			// do nothing - errors and endofs do not propagate through OBservableValue
		}

		/// <summary>Notifies the observer that the provider has experienced an error condition.</summary>
		/// <param name="error">An object that provides additional information about the error.</param>
		public void OnError(Exception error)
		{
			// do nothing - errors and endofs do not propagate through OBservableValue
		}

		/// <summary>Provides the observer with new data.</summary>
		/// <param name="value">The current notification information.</param>
		public void OnNext(T value)
		{
			Value = value;
		}

		#endregion
	}
}
