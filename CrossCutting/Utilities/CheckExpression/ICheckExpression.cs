using System;

namespace Indigo.CrossCutting.Utilities.CheckExpression
{
    /// <summary>
    /// Check Expression Interface
    /// </summary>
    public interface ICheckExpression
    {
        /// <summary>
        /// Validates the specified object
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <returns>bool, true if the expression is valid</returns>
        bool Validate(object obj);
    }
}