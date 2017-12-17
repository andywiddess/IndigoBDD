using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.DesignPatterns
{
    /// <summary>
    /// Notifies about change to be made on value. Allows to Cancel the operation by returning
    /// <c>false</c>.
    /// </summary>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="before">   The before. </param>
    /// <param name="after">    The after. </param>
    ///
    /// <returns>
    /// A bool.
    /// </returns>
    public delegate bool PropertyChangingNotification<T>(T before, T after);

    /// <summary>
    /// Notifies about change already made on value.
    /// </summary>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="before">   The before. </param>
    /// <param name="after">    The after. </param>
    public delegate void PropertyChangedNotification<T>(T before, T after);
}
