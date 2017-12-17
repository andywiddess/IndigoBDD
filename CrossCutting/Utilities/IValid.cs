using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Interface implemented by classes which can be validated
    /// </summary>
    public interface IValid
    {
        /// <summary>
        /// Return TRUE if this class is valid, and add errors to the error collection where it isnt.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        bool IsValid(ValidationErrorCollection errors);
    }
}
