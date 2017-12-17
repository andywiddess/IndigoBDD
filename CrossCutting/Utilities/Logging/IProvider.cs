using System;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Provider Interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProvider<T> where T : class
    {
        /// <summary>
        /// Gets the provided item.
        /// </summary>
        /// <value>The provided item.</value>
        T ProvidedItem
        {
            get;
        }
    }
}
