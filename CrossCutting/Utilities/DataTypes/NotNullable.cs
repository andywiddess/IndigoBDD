using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities.DataTypes
{
    /// <summary>
    /// Not Nullable Structure for Nullable Types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct NotNullable<T> where T : class, new()
    {
        /// <summary>
        /// Private Field
        /// </summary>
        private T value;

        /// <summary>
        /// Gets a not nullable structure for the specified type
        /// If the value is null it creates a new instance of the same
        /// </summary>
        public T Value
        {
            get
            {
                if (this.value == null)
                {
                    this.value = new T();
                }
                return this.value;
            }
        }
    }
}
