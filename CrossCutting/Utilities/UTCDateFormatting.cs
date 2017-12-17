using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities
{
    public static class UTCDateFormatting
    {
        /// <summary>
        /// Returns string formatted in Zulu (UTC) time. value must be UTC DateTime.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUTCDateTimeString(DateTime value)
        {
            // Date expected to be UTC date already.
            // e.g 2003-10-26T14:33:41.1234567zzz
            return value.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// To the UTC date string with time initialiser.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToUTCDateStringWithTimeInitialiser(DateTime value)
        {
            // Date expected to be UTC date already.
            // e.g 2003-10-26T14:33:41.1234567zzz
            return value.ToString("yyyy-MM-ddT00:00:00zzz", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// To the UTC date string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToUTCDateString(DateTime value)
        {
            // Date expected to be UTC date already.
            // e.g 2003-10-26
            return value.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses the UTC formatted string, returning DateTime in UTC.
        /// Use ToLocalTime() on the result if you want to convert to local time.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime FromUTCDateTimeString(string value)
        {
            return DateTime.Parse(value, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal);
        }
    }
}
