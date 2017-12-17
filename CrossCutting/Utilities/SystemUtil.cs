using System;

namespace Indigo.CrossCutting.Utilities
{
	public static class SystemUtil
	{
		private static Func<DateTime> _nowProvider;
		private static Func<DateTime> _utcNowProvider;

		static SystemUtil()
		{
			Reset();
		}

		public static DateTime Now
		{
			get { return _nowProvider(); }
		}

		public static void SetNow(Func<DateTime> provider)
		{
			_nowProvider = provider;
		}

		public static DateTime UtcNow
		{
			get { return _utcNowProvider(); }
		}

		public static void SetUtcNow(Func<DateTime> provider)
		{
			_nowProvider = provider;
		}

		public static void Reset()
		{
			_nowProvider = () => DateTime.Now;
			_utcNowProvider = () => DateTime.UtcNow;
		}
	}
}