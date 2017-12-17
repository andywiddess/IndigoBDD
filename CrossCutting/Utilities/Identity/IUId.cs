using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.Identity
{
    /// <summary>
    /// Unique Identity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Indigo.CrossCutting.Utilities.Identity.IHaveUniversalIdentity" />
    public interface IUId<out T> 
        : IHaveUniversalIdentity where T : IHaveUniversalIdentity
    {
    }
}
