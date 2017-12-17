using System;


namespace Indigo.CrossCutting.Utilities.Extensions
{
	public static class ExtensionsToAction
	{
		public static Func<TResult> Memoize<TResult>(this Func<TResult> self)
		{
			return Memoize(self, false);
		}

		public static Func<TResult> Memoize<TResult>(this Func<TResult> self, bool threadSafe)
		{
			Guard.AgainstNull(self, "self");

			TResult result = default(TResult);
			Exception exception = null;
			bool executed = false;

			Func<TResult> memoizedFunc = () =>
				{
					if (!executed)
					{
						try
						{
							result = self();
						}
						catch (Exception ex)
						{
							exception = ex;
							throw;
						}
						finally
						{
							executed = true;
						}
					}

					if (exception != null)
						throw exception;

					return result;
				};

			return threadSafe
			       	? memoizedFunc.Synchronize(() => !executed)
			       	: memoizedFunc;
		}

		public static Func<TResult> Synchronize<TResult>(this Func<TResult> self)
		{
			return self.Synchronize(() => true);
		}

		public static Func<TResult> Synchronize<TResult>(this Func<TResult> self, Func<bool> needsSynchronizationPredicate)
		{
			Guard.AgainstNull(self, "self");
			Guard.AgainstNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

			var lockObject = new object();

			return () =>
				{
					if (needsSynchronizationPredicate())
					{
						lock (lockObject)
						{
							return self();
						}
					}

					return self();
				};
		}
	}
}