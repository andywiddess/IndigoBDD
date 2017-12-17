using System;
using System.Collections.Generic;
using System.Threading;

using Indigo.CrossCutting.Utilities.Collections;

namespace Indigo.CrossCutting.Utilities.IoC
{
	/// <summary>
	/// This is super-thin 20 lines of code IoC container.
	/// That was all what we needed so far, but before adding anything consider using real IoC container, let's say:
	/// Ninject, Windsor, Unity, SimpleInjector, Autofac.
	/// </summary>
	public class Container
	{
		#region static fields

		/// <summary>Default container.</summary>
		public static readonly Container Default = new Container();

		#endregion

		#region fields

		/// <summary>Configuration values.</summary>
		private readonly Dictionary<object, object> m_Configuration = 
			new Dictionary<object, object>();

		/// <summary>Factories.</summary>
		private readonly Dictionary<Type, Func<Container, object>> m_Factories = 
			new Dictionary<Type, Func<Container, object>>();

		/// <summary>Read/Write lock.</summary>
		private readonly ReaderWriterLockSlim m_Lock = new ReaderWriterLockSlim();

		#endregion

		#region factories

		/// <summary>Registers the specified factory.</summary>
		/// <typeparam name="T">Type of factory result.</typeparam>
		/// <param name="factory">The factory.</param>
		/// <param name="overwrite">if set to <c>true</c> factory can be overwritten, otherwise it will throw an 
		/// exception in such case.</param>
		public void Register<T>(Func<Container, T> factory, bool overwrite = true)
		{
			// we need this level of redirection. They are passed as Func<Container, T> but stored as Func<Container, object>
			Func<Container, object> redirected = (c) => factory(c);

			m_Lock.EnterWriteLock();
			try
			{
				if (overwrite)
				{
					m_Factories[typeof(T)] = redirected;
				}
				else
				{
					m_Factories.Add(typeof(T), redirected);
				}
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
		}

		/// <summary>Unregisters factory of given type.</summary>
		/// <typeparam name="T">Type of object.</typeparam>
		public void Unregister<T>()
		{
			m_Lock.EnterWriteLock();
			try
			{
				m_Factories.Remove(typeof(T));
			}
			finally
			{
				m_Lock.ExitWriteLock();
			}
		}

		/// <summary>Creates an instance of specified type.</summary>
		/// <typeparam name="T">Type of factory.</typeparam>
		/// <returns>Created object.</returns>
		public T Resolve<T>(T defaultValue = default(T))
			where T: class
		{
			Func<Container, object> factory;

			m_Lock.EnterReadLock();
			try
			{
				factory = m_Factories.TryGetValue(typeof(T), null);
			}
			finally
			{
				m_Lock.ExitReadLock();
			}

			if (factory != null)
			{
				return (T)factory(this);
			}
			else
			{
				return defaultValue;
			}
		}

		#endregion
	}
}
