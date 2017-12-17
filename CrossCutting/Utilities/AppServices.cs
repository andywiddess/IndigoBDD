using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Indigo.CrossCutting.Utilities.IoC;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Simple service locator.
    /// </summary>
    public static class AppServices
    {
        #region factories

        /// <summary>Registers the specified factory.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        public static void RegisterFactory<T>(Func<T> factory, bool overwrite = true)
        {
            Container.Default.Register<T>((c) => factory(), overwrite);
        }

        /// <summary>Registers the singleton.</summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="subject">The subject.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        public static void RegisterInstance<T>(T subject, bool overwrite = true)
        {
            RegisterFactory<T>(() => subject, overwrite);
        }

        /// <summary>Unregisters factory for given type.</summary>
        /// <typeparam name="T">Type of object.</typeparam>
        public static void Unregister<T>()
        {
            Container.Default.Unregister<T>();
        }

        /// <summary>
        /// Resolves instance of specified type. Please note, it actually executes the delegate so if you are
        /// creating a new one or accessing a singleton depends on handler implementation.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <returns>New instance (or singleton) of given type.</returns>
        public static T Resolve<T>()
            where T : class
        {
            return Container.Default.Resolve<T>();
        }

        /// <summary>
        /// Resolves instance of specified type. Please note, it actually executes the 
        /// delegate so if you are creating a new one or accessing a singleton depends 
        /// on handler implementation.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>New instance (or singleton) of given type.</returns>
        public static T Resolve<T>(T defaultValue)
            where T : class
        {
            return Container.Default.Resolve<T>(defaultValue);
        }

        #endregion
    }
}
