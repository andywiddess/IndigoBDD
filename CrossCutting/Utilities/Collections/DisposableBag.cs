using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Indigo.CrossCutting.Utilities.Collections
{
	/// <summary>Disposable bag of disposable items. Used to dispose multiple items with one call.</summary>
	public class DisposableBag: ICollection<IDisposable>, IDisposable
	{
		#region fields

		private ISet<IDisposable> _bag = new HashSet<IDisposable>();
		private bool _disposed;

		#endregion

		#region public interface

		/// <summary>Adds the specified item to bag.</summary>
		/// <param name="item">The item to be added.</param>
		/// <param name="ignore">if set to <c>true</c> item won't be added to bag. It is used for conditial adding.</param>
		public T Add<T>(T item, bool ignore = false)
			where T: IDisposable
		{
			if (!ignore && item != null)
			{
				bool added = false;

				lock (_bag)
				{
					if (!_disposed)
					{
						_bag.Add(item);
						added = true;
					}
				}

				if (!added)
				{
					item.Dispose(); // dispose outside the lock!
				}
			}
			return item;
		}

		/// <summary>Gets a value indicating whether this instance is disposed.</summary>
		/// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
		public bool IsDisposed { get { lock (_bag) return _disposed; } }

		#endregion

		#region ICollection<IDisposable> Members

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		void ICollection<IDisposable>.Add(IDisposable item)
		{
			Add(item, false);
		}

		/// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
		public void Clear()
		{
			lock (_bag) _bag.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.</returns>
		public bool Contains(IDisposable item)
		{
			lock (_bag) return _bag.Contains(item);
		}

		/// <summary>Copies items to array.</summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(IDisposable[] array, int arrayIndex)
		{
			lock (_bag) _bag.CopyTo(array, arrayIndex);
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
		public int Count
		{
			get { lock (_bag) return _bag.Count; }
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
		public bool IsReadOnly
		{
			get { return _bag.IsReadOnly; }
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns>true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
		public bool Remove(IDisposable item)
		{
			lock (_bag) return _bag.Remove(item);
		}

		#endregion

		#region IEnumerable<IDisposable> Members

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
		public IEnumerator<IDisposable> GetEnumerator()
		{
			return _bag.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _bag.GetEnumerator();
		}

		#endregion

		#region IDisposable Members

		/// <summary>Tries the dispose object.</summary>
		/// <param name="disposable">The disposable object.</param>
		/// <returns>Exception thrown or <c>null</c></returns>
		private static Exception TryDispose(IDisposable disposable)
		{
			if (disposable == null)
				return null;

			try
			{
				disposable.Dispose();
			}
			catch (Exception e)
			{
				return e;
			}

			return null;
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		/// <exception cref="System.AggregateException">Exception thrown by one of the finalizers</exception>
		public void Dispose()
		{
			IDisposable[] disposables;

			lock (_bag)
			{
				_disposed = true;
				disposables = _bag.Where(d => d != null).ToArray();
				_bag.Clear();
			}

			// actual dispose is outside the lock
			var exceptions = disposables.Select(TryDispose).Where(e => e != null).ToArray();
			if (exceptions.Length > 0)
				throw new AggregateException("Exception thrown by one of the finalizers", exceptions);
		}

		#endregion
	}
}
