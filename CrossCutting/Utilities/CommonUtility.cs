using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Indigo.CrossCutting.Utilities.Utility
{
    public static class CommonUtility
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static double GetValue(Nullable<double> val)
        {
            return (val != null && val.HasValue ? val.Value : 0.0d);
        }

        /// <summary>
        /// Determines whether the specified val has value.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns>
        ///   <c>true</c> if the specified val has value; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValue(Nullable<double> val)
        {
            return (val != null) && (val.HasValue) && (val.Value > 0.0d);
        }

        /// <summary>
        /// Determines whether the specified val has value.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns>
        ///   <c>true</c> if the specified val has value; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValue(string val)
        {
            return (!string.IsNullOrEmpty(val) && val.Equals("Yes", StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
