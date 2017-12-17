using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.Identity
{
    /// <summary>
    /// UId Extensions
    /// </summary>
    public static class IUIdExtensions
    {
        #region Static Methods
        /// <summary>
        /// To the reference.
        /// </summary>
        /// <typeparam name="TTo">The type of to.</typeparam>
        /// <param name="me">Me.</param>
        /// <returns></returns>
        public static IUId<TTo> ToRef<TTo>(this IHaveUniversalIdentity me)
            where TTo : IHaveUniversalIdentity
        {
            if (me != null)
                return new Ref<TTo>(me.Id);
            else
                return null;
        }
        #endregion
    }
}
