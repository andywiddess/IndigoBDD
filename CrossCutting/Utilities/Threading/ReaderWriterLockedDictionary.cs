using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigo.CrossCutting.Utilities.Threading
{
	public class ReaderWriterLockedDictionary<TKey, TValue> :
		IEnumerable<KeyValuePair<TKey, TValue>>,
		IDisposable
	{
		private ReaderWriterLockedObject<Dictionary<TKey, TValue>> _collection;
		private volatile bool _disposed;

		public ReaderWriterLockedDictionary()
		{
			_collection = new ReaderWriterLockedObject<Dictionary<TKey, TValue>>(new Dictionary<TKey, TValue>());
		}

		public void Add(TKey key, TValue value)
		{
			_collection.WriteLock(x => x.Add(key, value));
		}

		public ReaderWriterLockedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
		{
			var dictionary = new Dictionary<TKey, TValue>();
			foreach (KeyValuePair<TKey, TValue> pair in keyValuePairs)
			{
				dictionary.Add(pair.Key, pair.Value);
			}

			_collection = new ReaderWriterLockedObject<Dictionary<TKey, TValue>>(dictionary);
		}

		public IEnumerable<TKey> Keys
		{
			get { return _collection.ReadLock(x => x.Keys); }
		}

		public IEnumerable<TValue> Values
		{
			get { return _collection.ReadLock(x => x.Values); }
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _collection.ReadLock(x => x.GetEnumerator());
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public TValue Retrieve(TKey key, Func<TValue> valueProvider)
		{
			TValue result = default(TValue);

			return _collection.ReadLock(x => x.TryGetValue(key, out result)) ? result : _collection.WriteLock(x =>
				{
					if (x.TryGetValue(key, out result))
						return result;

					result = valueProvider();

					x[key] = result;

					return result;
				});
		}

		public void Clear()
		{
			_collection.WriteLock(x => x.Clear());
		}

		public void Store(TKey key, TValue value)
		{
			_collection.WriteLock(x => { x[key] = value; });
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			TValue output = default(TValue);
			bool result = _collection.ReadLock(x => x.TryGetValue(key, out output));

			value = output;
			return result;
		}

		~ReaderWriterLockedDictionary()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;
			if (disposing)
			{
				_collection.Dispose();
				_collection = null;
			}
			_disposed = true;
		}
	}
}