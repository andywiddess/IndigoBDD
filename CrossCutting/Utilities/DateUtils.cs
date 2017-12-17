#region Header
// ---------------------------------------------------------------------------
// Sepura - Commercially Confidential.
// 
// Indigo.CrossCutting.Utilities.DateUtils
// Date Extension Methods
//
// Copyright (c) 2016 Sepura Plc
// All Rights reserved.
//
// $Id:  $ :
// ---------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Date specific extension methods
    /// </summary>
    public static class DateUtils
    {
        /// <summary>
        /// Gets the Start of a week based on the current date
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="startOfWeek">The start of week.</param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
                diff += 7;

            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Gets the epoch date from a reporting period.
        /// </summary>
        /// <param name="dt">The reporting period.</param>
        /// <returns>epoch time</returns>
        public static long GetEpoch(DateTime dt)
        {
            TimeSpan t = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1));
            return (long)t.TotalMilliseconds;
        }
    }
}
