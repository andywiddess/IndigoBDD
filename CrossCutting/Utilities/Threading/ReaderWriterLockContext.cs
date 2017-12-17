using System;
using System.Threading;

namespace Indigo.CrossCutting.Utilities.Threading
{
	public class ReaderWriterLockContext :
		IDisposable
	{
		private volatile bool _disposed;
		private ReaderWriterLockSlim _lock;

		public ReaderWriterLockContext()
		{
			_lock = new ReaderWriterLockSlim();
		}

		public ReaderWriterLockContext(LockRecursionPolicy policy)
		{
			_lock = new ReaderWriterLockSlim(policy);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void ReadUnlocked(Action<ReaderWriterLockContext> action)
		{
			action(this);
		}

		public void ReadLock(Action<ReaderWriterLockContext> action)
		{
			if (_disposed) throw new ObjectDisposedException("ReaderWriterLockContext");

			_lock.EnterReadLock();
			try
			{
				action(this);
			}
			finally
			{
				_lock.ExitReadLock();
			}
		}

		public V ReadLock<V>(Func<ReaderWriterLockContext, V> action)
		{
			if (_disposed) throw new ObjectDisposedException("ReaderWriterLockContext");

			_lock.EnterReadLock();
			try
			{
				return action(this);
			}
			finally
			{
				_lock.ExitReadLock();
			}
		}

		public bool ReadLock(TimeSpan timeout, Action<ReaderWriterLockContext> action)
		{
			if (_disposed) throw new ObjectDisposedException("ReaderWriterLockContext");

			if (_lock.TryEnterReadLock(timeout) == false)
				return false;

			try
			{
				action(this);
			}
			finally
			{
				_lock.ExitReadLock();
			}

			return true;
		}

		public void UpgradeableReadLock(Action<ReaderWriterLockContext> action)
		{
			if (_disposed) throw new ObjectDisposedException("ReaderWriterLockContext");

			_lock.EnterUpgradeableReadLock();
			try
			{
				action(this);
			}
			finally
			{
				_lock.ExitUpgradeableReadLock();
			}
		}

		public V UpgradeableReadLock<V>(Func<ReaderWriterLockContext, V> action)
		{
			if (_disposed) throw new ObjectDisposedException("ReaderWriterLockContext");

			_lock.EnterUpgradeableReadLock();
			try
			{
				return action(this);
			}
			finally
			{
				_lock.ExitUpgradeableReadLock();
			}
		}

		public bool UpgradeableReadLock(TimeSpan timeout, Action<ReaderWriterLockContext> action)
		{
			if (_disposed) throw new ObjectDisposedException("ReaderWriterLockContext");

			if (_lock.TryEnterUpgradeableReadLock(timeout) == false)
				return false;

			try
			{
				action(this);
			}
			finally
			{
				_lock.ExitUpgradeableReadLock();
			}

			return true;
		}

		public void WriteLock(Action<ReaderWriterLockContext> action)
		{
			if (_disposed) throw new ObjectDisposedException("ReaderWriterLockContext");

			_lock.EnterWriteLock();
			try
			{
				action(this);
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		public V WriteLock<V>(Func<ReaderWriterLockContext, V> action)
		{
			if (_disposed) throw new ObjectDisposedException("ReaderWriterLockContext");

			_lock.EnterWriteLock();
			try
			{
				return action(this);
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		public bool WriteLock(TimeSpan timeout, Action<ReaderWriterLockContext> action)
		{
			if (_disposed) throw new ObjectDisposedException("ReaderWriterLockContext");

			if (_lock.TryEnterWriteLock(timeout) == false)
				return false;

			try
			{
				action(this);
			}
			finally
			{
				_lock.ExitWriteLock();
			}

			return true;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing || _disposed) return;

			if (_lock != null)
				_lock.Dispose();

			_lock = null;
			_disposed = true;
		}

		~ReaderWriterLockContext()
		{
			Dispose(false);
		}
	}
}