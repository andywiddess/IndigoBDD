using System;
namespace Indigo.CrossCutting.Utilities.Threading
{

	/// <summary>
	/// Interface to a locked object of type T
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ILockedObject<T> :
		IDisposable
	{
		/// <summary>
		/// Access the contained object without any locking
		/// </summary>
		/// <param name="action"></param>
		void ReadUnlocked(Action<T> action);

		/// <summary>
		/// Access the contained object within a read lock
		/// </summary>
		/// <param name="action"></param>
		void ReadLock(Action<T> action);

		V ReadLock<V>(Func<T, V> action);

		/// <summary>
		/// Access the contained object within a read lock if possible before the timeout expires
		/// </summary>
		/// <param name="timeout">The time to wait for a lock before returning false</param>
		/// <param name="action"></param>
		/// <returns>True if the lock was obtained and the action called, otherwise false</returns>
		bool ReadLock(TimeSpan timeout, Action<T> action);

		/// <summary>
		/// Access the contained object within an upgradeable read lock
		/// </summary>
		/// <param name="action"></param>
		void UpgradeableReadLock(Action<T> action);

		V UpgradeableReadLock<V>(Func<T, V> action);
		
		bool UpgradeableReadLock(TimeSpan timeout, Action<T> action);

		/// <summary>
		/// Access the contained object within a write lock
		/// </summary>
		/// <param name="action"></param>
		void WriteLock(Func<T, T> action);
	
		void WriteLock(Action<T> action);

		V WriteLock<V>(Func<T, V> action);
		
		bool WriteLock(TimeSpan timeout, Func<T, T> action);
		void WriteLock(TimeSpan timeout, Action<T> action);
	}
}