using System;
using System.Globalization;
using System.Xml.Schema;
using System.Xml;
using System.Runtime.Serialization;
namespace Indigo.CrossCutting.Utilities.DataTypes
{
    /// <summary>
    /// This struct is intended to supplement the .Net standard
    /// DateTime struct, providing similar functionality but without
    /// a Time element.
    /// </summary>
    [DataContract( Namespace = "http://www.sepura.co.uk/IA")]
    public struct Date : IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Internal DateTime store so that we can 
        /// use underlying methods and properties.
        /// </summary>
        [DataMember]
        public DateTime internalDateTime;
        private static bool isTryParseSuccessful = true;
        /// <summary>
        /// Constant specifying the default format for string representations.
        /// </summary>
        private const char DEFAULT_FORMAT = 'd';
        /// <summary>
        /// Static constructor that populates the fields.
        /// </summary>
        static Date()
        {
            validDateFormats = new string[] 
            { 
            "d", "D", "m","M", "y", "Y", "dd/MM/yy","d/M/yy","d/M/yyyy",
            "ddd d MM yy", "ddd d MMM yy","ddd d MMM yyyy","ddd d MMMM yyyy",
            "dddd d MM yy", "dddd d MMM yy","dddd d MMM yyyy","dddd d MMMM yyyy"
            };
            defaultFormatString = DEFAULT_FORMAT.ToString();
            defaultFormatProvider = DateTimeFormatInfo.CurrentInfo;
            MinValue = FromDateTime(DateTime.MinValue);
            MaxValue = FromDateTime(DateTime.MaxValue);
        }
        /// <summary>
        /// Returns TypeCode.Object for type conversions via IConvertible
        /// </summary>
        /// <returns>TypeCode.Object</returns>
        /// Unit Test: Pass
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #region Constructors
        /// <summary>
        /// Constructor taking a number of ticks,
        /// </summary>
        /// <param name="ticks">A date and time expressed in 100 nano-second units.</param>
        /// Unit Test: Pass
        public Date(long ticks)
        {
            internalDateTime = new DateTime(ticks);
        }
        /// <summary>
        /// Constructor taking a year month and day
        /// </summary>
        /// <param name="year">The year</param>
        /// <param name="month">The month</param>
        /// <param name="day">The day</param>
        /// Unit Test: Pass
        public Date(int year, int month, int day)
        {
            internalDateTime = new DateTime(year, month, day);
        }
        /// <summary>
        /// Constructor taking a year month and day
        /// </summary>
        /// <param name="year">The year</param>
        /// <param name="month">The month</param>
        /// <param name="day">The day</param>
        /// <param name="calendar">The System.Globalization.Calendar that applies to this Date.</param>
        public Date(int year, int month, int day, Calendar calendar)
        {
            internalDateTime = new DateTime(year, month, day, calendar);
        }
        /// <summary>
        /// Constructor taking a date in string format.
        /// 
        /// Exceptions:
        /// ArgumentNullException: date is a null reference.
        /// FormatException: date does not contain a valid string representation of a date.
        /// </summary>
        /// <param name="date">sting basis for date</param>
        /// Unit Test: Pass
        public Date(string date)
        {
            if (date.Trim() == "")
            {
                internalDateTime = DateTime.MinValue;//new DateTime(0);
            }
            else
            {
                internalDateTime = DateTime.Parse(date);
            }
        }
        #endregion
        #region Calculation operations
        /// <summary>
        /// Add the value of the specified TimeSpan to this date.
        /// Adding less than a day will result in the current instance value being returned.
        /// Adding a negative amount (e.g. -1 hours) will result in an earlier date being returned.
        /// </summary>
        /// <param name="value">TimeSpan to add to this date.</param>
        /// <returns>The new date.</returns>
        /// Unit Test: Pass
        public Date Add(TimeSpan value)
        {
            DateTime add = new DateTime(internalDateTime.Year, internalDateTime.Month, internalDateTime.Day);
            add = add.Add(value);
            Date result = FromDateTime(add);
            return result;
        }
        /// <summary>
        /// Add the specified number of days to this instance.
        /// 
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="value">The number of days to add.</param>
        /// <returns>The new date.</returns>
        /// Unit Test: Pass
        public Date AddDays(double value)
        {
            DateTime add = new DateTime(internalDateTime.Year, internalDateTime.Month, internalDateTime.Day);
            add = add.AddDays(value);
            Date result = FromDateTime(add);
            return result;
        }
        /// <summary>
        /// Add the specified number of weeks to this instance.
        /// 
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="value">The number of weeks to add.</param>
        /// <returns>The new date.</returns>
        public Date AddWeeks(double value)
        {
            return AddDays(value * 7); ;
        }
        /// <summary>
        /// Add the specified number of months to this instance.
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="months">The months.</param>
        /// <returns>
        /// The new date.
        /// </returns>
        /// Unit Test: Pass
        public Date AddMonths(int months)
        {
            DateTime add = new DateTime(internalDateTime.Year, internalDateTime.Month, internalDateTime.Day);
            add = add.AddMonths(months);
            Date result = FromDateTime(add);
            return result;
        }
        /// <summary>
        /// Add the specified number of years to this instance.
        /// 
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="value">The number of years to add.</param>
        /// <returns>The new date.</returns>
        /// Unit Test: Pass
        public Date AddYears(int value)
        {
            DateTime add = new DateTime(internalDateTime.Year, internalDateTime.Month, internalDateTime.Day);
            add = add.AddYears(value);
            Date result = FromDateTime(add);
            return result;
        }

        /// <summary>
        /// Subtracts the specified duration from this instance.
        /// 
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="value">Duration to subtract.</param>
        /// <returns>Timespan defining result.</returns>
        /// Unit Test: Pass
        public TimeSpan Subtract(Date value)
        {
            DateTime subtract = value.ToDateTime();
            return Subtract(subtract);
        }
        /// <summary>
        /// Subtracts the specified duration from this instance.
        /// 
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="value">Duration to subtract.</param>
        /// <returns>Timespan defining result.</returns>
        /// Unit Test: Pass
        public TimeSpan Subtract(DateTime value)
        {
            DateTime subtract = new DateTime(internalDateTime.Year, internalDateTime.Month, internalDateTime.Day);
            TimeSpan result = subtract.Subtract(value);
            return result;
        }

        /// <summary>
        /// Subtracts the specified duration from this instance.
        /// 
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="value">Duration to subtract.</param>
        /// <returns>Timespan defining result.</returns>
        /// Unit Test: Pass
        public Date Subtract(TimeSpan value)
        {
            DateTime subtract = new DateTime(internalDateTime.Year, internalDateTime.Month, internalDateTime.Day);
            DateTime subtractResult = subtract.Subtract(value);
            Date result = FromDateTime(subtractResult);
            return result;
        }
        /// <summary>
        /// Returns the number of days in the specified month and year.
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="year">The Year</param>
        /// <param name="month">TheMonth.</param>
        /// <returns>The number of days in the specified month and year.</returns>
        /// Unit Test: Pass
        public static int DaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }
        /// <summary>
        /// Returns an indication whether the specified year is a leap-year.
        /// </summary>
        /// <param name="year">a four digit year.</param>
        /// <returns>true if 'year' is a leap year.</returns>
        /// Unit Test: Pass
        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }
        #endregion
        #region Comparison operations
        /// <summary>
        /// Compare two instances of Date and returns an indication of their relative value.
        /// </summary>
        /// <param name="date1">First date to compare.</param>
        /// <param name="date2">Second date to compare.</param>
        /// <returns>relative value.</returns>
        /// Unit Test: Pass
        public static int Compare(Date date1, Date date2)
        {
            DateTime dateTime1 = date1.ToDateTime();
            DateTime dateTime2 = date2.ToDateTime();
            return DateTime.Compare(dateTime1, dateTime2);
        }

        /// <summary>
        /// Compare this instanct to an instance of Date or DateTime and returns an indication 
        /// of the relative value. If a DateTime is supplied, only the Date part is considered
        /// in the comparison.
        /// </summary>
        /// <param name="value">Date to compare to this instance.</param>
        /// <returns>relative value.</returns>
        /// Unit Test: Pass
        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (!(value is Date || value is DateTime))
            {
                throw new ArgumentException("value was of type '" + value.GetType().Name + "', not Date or DateTime.");
            }
            int comparisonResult = 0;
            if (value is Date)
            {
                comparisonResult = CompareTo((Date)value);
            }
            else
            {
                comparisonResult = CompareTo((DateTime)value);
            }
            return comparisonResult;
        }
        /// <summary>
        /// Compare this instanct to an instance of Date and returns an indication 
        /// of the relative value.
        /// </summary>
        /// <param name="value">Date to compare to this instance.</param>
        /// <returns>relative value.</returns>
        /// Unit Test: Pass
        public int CompareTo(Date value)
        {
            DateTime compareTo = value.ToDateTime();
            return internalDateTime.CompareTo(compareTo);
        }
        /// <summary>
        /// Compare this instanct to an instance of DateTime and returns an indication 
        /// of the relative value. Only the Date part is considered
        /// in the comparison.
        /// </summary>
        /// <param name="value">Date to compare to this instance.</param>
        /// <returns>relative value.</returns>
        /// Unit Test: Pass
        public int CompareTo(DateTime value)
        {
            bool result = Equals(value);
            if (result)
            {
                return 0;
            }
            else
            {
                return internalDateTime.CompareTo(value);
            }
        }
        /// <summary>
        /// Returns a boolean indicating whether this instance is equal to the specified
        /// Date or DateTime. If a DateTime is supplied, only the Date part is considered
        /// in the comparison.
        /// </summary>
        /// <param name="value">Date or DateTime to compare this instance to.</param>
        /// <returns>true if 'value' is equal to the current instance.</returns>
        /// Unit Test: Pass
        public override bool Equals(object value)
        {
            if (value == null || value.GetType() == typeof(DBNull))
            {
                return false;
            }
            if (value is string)
            {
                Date compare = new Date(value.ToString());
                return (compare == this);
            }
            if (!(value is Date || value is DateTime))
            {
                return false; // The value passed in is not comparable to a date, so therefore cannot be equal to a date.
            }
            bool equals = false;
            if (value is Date)
            {
                equals = Equals((Date)value);
            }
            else
            {
                equals = Equals((DateTime)value);
            }
            return equals;
        }
        /// <summary>
        /// Returns a boolean indicating whether this instance is equal to the specified
        /// Date or DateTime. 
        /// </summary>
        /// <param name="value">DateTime to compare this instance to.</param>
        /// <returns>true if 'value' is equal to the current instance.</returns>
        /// Unit Test: Pass
        public bool Equals(DateTime value)
        {
            Date equals = Date.FromDateTime(value);
            return Equals(equals);
        }
        /// <summary>
        /// Returns a boolean indicating whether this instance is equal to the specified
        /// Date or DateTime.
        /// </summary>
        /// <param name="value">Date to compare this instance to.</param>
        /// <returns>true if 'value' is equal to the current instance.</returns>
        /// Unit Test: Pass
        public bool Equals(Date value)
        {
            return
            (internalDateTime.Year == value.Year
            && internalDateTime.Month == value.Month
            && internalDateTime.Day == value.Day);
        }

        /// <summary>
        /// Returns a value indicating whether two dates are equal.
        /// </summary>
        /// <param name="date1">The first Date.</param>
        /// <param name="date2">The second Date.</param>
        /// <returns>true if date1 equals date2.</returns>
        /// Unit Test: Pass
        public static bool Equals(Date date1, Date date2)
        {
            return
            (date1.Year == date2.Year
            && date1.Month == date2.Month
            && date1.Day == date2.Day);
        }
        /// <summary>
        /// Returns a value indicating whether two dates are equal.
        /// </summary>
        /// <param name="date">The first Date.</param>
        /// <param name="dateTime">The second Date.</param>
        /// <returns>true if date equals dateTime.</returns>
        /// Unit Test: Pass
        public static bool Equals(Date date, DateTime dateTime)
        {
            return
            (date.Year == dateTime.Year
            && date.Month == dateTime.Month
            && date.Day == dateTime.Day);
        }
        #endregion
        #region GetDateFormats operations
        /// <summary>
        /// Converts the value of this instance to all the single character string representations
        /// suppported by the default Date format specifier and culture-specific
        /// formatting information.
        /// </summary>
        /// <returns>instance data in all available string representations.</returns>
        /// Unit Test: Pass
        public string[] GetDateFormats()
        {
            return GetDateFormats(DEFAULT_FORMAT);
        }
        /// <summary>
        /// Converts the value of this instance to all the single character string representations
        /// suppported by the specified Date format specifier and culture-specific
        /// formatting information.
        /// </summary>
        /// <param name="format">string format to use.</param>
        /// <returns>instance data in all available string representations.</returns>
        public string[] GetDateFormats(char format)
        {
            return GetDateFormats(format, defaultFormatProvider);
        }
        /// <summary>
        /// Converts the value of this instance to all the single character string representations
        /// suppported by the specified Date format specifier and culture-specific
        /// formatting information.
        /// </summary>
        /// <param name="provider">culture-specific format provider.</param>
        /// <returns>instance data in all available string representations.</returns>
        public string[] GetDateFormats(IFormatProvider provider)
        {
            return GetDateFormats(DEFAULT_FORMAT, defaultFormatProvider);
        }
        /// <summary>
        /// Converts the value of this instance to all the single character string representations
        /// suppported by the specified Date format specifier and culture-specific
        /// formatting information.
        /// </summary>
        /// <param name="format">string format to use.</param>
        /// <param name="provider">culture-specific format provider.</param>
        /// <returns>instance data in all available string representations.</returns>
        public string[] GetDateFormats(char format, IFormatProvider provider)
        {
            string[] results = new string[validDateFormats.Length];
            for (int loop = 0; loop < validDateFormats.Length; loop++)
            {
                results[loop] = internalDateTime.ToString(validDateFormats[loop]);
            }
            return results;
        }
        #endregion
        #region Operator overloads
        /// <summary>
        /// Add a timespane to a date
        /// </summary>
        /// <param name="d">The original date</param>
        /// <param name="t">The timespan to add to the date</param>
        /// <returns>The date plus the timespan</returns>
        /// Unit Test: Pass
        public static Date operator +(Date d, TimeSpan t)
        {
            DateTime calculate = new DateTime(d.Year, d.Month, d.Day);
            calculate = calculate.Add(t);
            Date result = FromDateTime(calculate);
            return result;
        }
        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="d1">The first date</param>
        /// <param name="d2">The second date</param>
        /// <returns>true if the dates match</returns>
        /// Unit Test: Pass
        public static bool operator ==(Date d1, Date d2)
        {
            bool yearsMatch = d1.Year == d2.Year;
            bool monthsMatch = d1.Month == d2.Month;
            bool daysMatch = d1.Day == d2.Day;
            return (yearsMatch && monthsMatch && daysMatch);
        }
        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="d1">The first date</param>
        /// <param name="d2">The second date</param>
        /// <returns>true if the dates match</returns>
        public static bool operator ==(Date d1, DateTime d2)
        {
            bool yearsMatch = d1.Year == d2.Year;
            bool monthsMatch = d1.Month == d2.Month;
            bool daysMatch = d1.Day == d2.Day;
            return (yearsMatch && monthsMatch && daysMatch);
        }


        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="d1">The first date</param>
        /// <param name="d2">The second date</param>
        /// <returns>true if the dates match</returns>
        public static bool operator ==(DateTime d1, Date d2)
        {
            bool yearsMatch = d1.Year == d2.Year;
            bool monthsMatch = d1.Month == d2.Month;
            bool daysMatch = d1.Day == d2.Day;
            return (yearsMatch && monthsMatch && daysMatch);
        }
        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="d1">The first date</param>
        /// <param name="d2">The second date</param>
        /// <returns>true if the first date, d1, is later than d2.</returns>
        /// Unit Test: Pass
        public static bool operator >(Date d1, Date d2)
        {
            DateTime dt1 = new DateTime(d1.Year, d1.Month, d1.Day);
            DateTime dt2 = new DateTime(d2.Year, d2.Month, d2.Day);
            return dt1 > dt2;
        }
        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <returns>
        /// true if the first date, date1, is later than or the same day as date2.
        /// </returns>
        /// Unit Test: Pass
        public static bool operator >=(Date date1, Date date2)
        {
            DateTime dt1 = new DateTime(date1.Year, date1.Month, date1.Day);
            DateTime dt2 = new DateTime(date2.Year, date2.Month, date2.Day);
            return dt1 >= dt2;
        }
        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="d1">The first date</param>
        /// <param name="d2">The second date</param>
        /// <returns>true if the dates are different.</returns>
        /// Unit Test: Pass
        public static bool operator !=(Date d1, Date d2)
        {
            DateTime dt1 = new DateTime(d1.Year, d1.Month, d1.Day);
            DateTime dt2 = new DateTime(d2.Year, d2.Month, d2.Day);
            return dt1 != dt2;
        }

        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="d1">The first date</param>
        /// <param name="d2">The second date</param>
        /// <returns>true if the dates are different.</returns>
        public static bool operator !=(Date d1, DateTime d2)
        {
            DateTime dt1 = new DateTime(d1.Year, d1.Month, d1.Day);
            return dt1 != d2;
        }
        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="d1">The first date</param>
        /// <param name="d2">The second date</param>
        /// <returns>true if the dates are different.</returns>
        /// Unit Test: Pass
        public static bool operator !=(DateTime d1, Date d2)
        {
            DateTime dt2 = new DateTime(d2.Year, d2.Month, d2.Day);
            return d1 != dt2;
        }
        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <returns>
        /// true if the dates date1 is earlier than date2.
        /// </returns>
        /// Unit Test: Pass
        public static bool operator <(Date date1, Date date2)
        {
            DateTime dt1 = new DateTime(date1.Year, date1.Month, date1.Day);
            DateTime dt2 = new DateTime(date2.Year, date2.Month, date2.Day);
            return dt1 < dt2;
        }

        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <returns>
        /// true if the dates dt1 is earlier than or the same as date2.
        /// </returns>
        /// Unit Test: Pass
        public static bool operator <=(Date date1, Date date2)
        {
            DateTime dt1 = new DateTime(date1.Year, date1.Month, date1.Day);
            DateTime dt2 = new DateTime(date2.Year, date2.Month, date2.Day);
            return dt1 <= dt2;
        }
        /// <summary>
        /// Subtract one date from another, returning a TimeSpan
        /// </summary>
        /// <param name="d1">The first date</param>
        /// <param name="d2">The second date</param>
        /// <returns>A timespan representing the differences between the two dates</returns>
        /// Unit Test: Pass
        public static TimeSpan operator -(Date d1, Date d2)
        {
            DateTime dt1 = new DateTime(d1.Year, d1.Month, d1.Day);
            DateTime dt2 = new DateTime(d2.Year, d2.Month, d2.Day);
            return dt1 - dt2;
        }

        /// <summary>
        /// Subtract a timespan from a date, resulting in an earlier date.
        /// </summary>
        /// <param name="date">The first date</param>
        /// <param name="t">The timespan to subtract</param>
        /// <returns>A timespan representing the differences between the two dates</returns>
        /// Unit Test: Pass
        public static Date operator -(Date date, TimeSpan t)
        {
            DateTime dt1 = new DateTime(date.Year, date.Month, date.Day);
            DateTime resultDateTime = dt1 - t;
            Date result = FromDateTime(resultDateTime);
            return result;
        }
        #endregion
        #region Parsing methods

        /// <summary>
        /// Attempt to parse a string into a Date value.
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <param name="result">The resultant date.</param>
        /// <returns>'true' if the parse succeeded, else 'false'.</returns>
        public static bool TryParse(string value, out Date result)
        {
            try
            {
                result = Date.Parse(value);
                return true;
            }
            catch (System.FormatException)
            {
                result = Date.MinValue;
                return false;
            }
            catch (System.ArgumentNullException)
            {
                result = Date.MinValue;
                return false;
            }
            catch (System.ArgumentException)
            {
                result = Date.MinValue;
                return false;
            }
        }

        /// <summary>
        /// Attempt to parse a string into a Date value.
        /// only difference of this method is that this
        /// would return 
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <param name="result">The resultant date.</param>
        /// <returns>'true' if the parse succeeded, else 'false'.</returns>
        public static bool TryParseExact(string value, out Date result)
        {
            //make this explicitly false so that we can evaluate the value and return the result
            isTryParseSuccessful = false;
            try
            {
                result = Date.Parse(value);
                //check whether we have got only number as date i.e. "10102007" for 10th october 2007
                if (new Date("") == result && isNumeric(value))
                {
                    //see whether we have got sufficient values to create date
                    double doubleVal;
                    if (double.TryParse(value, out doubleVal))
                        if (value.Length > 6)
                            result = Date.Parse(doubleVal.ToString("##/##/####"));
                        else if (value.Length == 6)
                            result = Date.Parse(doubleVal.ToString("##/##/##"));
                }
                return isTryParseSuccessful;
            }
            catch (System.FormatException)
            {
                result = Date.MinValue;
                return false;
            }
            catch (System.ArgumentNullException)
            {
                result = Date.MinValue;
                return false;
            }
            catch (System.ArgumentException)
            {
                result = Date.MinValue;
                return false;
            }
        }
        /// <summary>
        /// Converts the specified string representation of a date into its Date equvilant.
        /// </summary>
        /// <param name="s">string to convrt to a Date</param>
        /// <returns>The Date</returns>
        /// Unit Test: Pass
        public static Date Parse(string s)
        {
            return Date.Parse(s, defaultFormatProvider);
        }
        /// <summary>
        /// Converts the specified object representaion of a date into its Date equivalent.
        /// </summary>
        /// <param name="o">object that needs to be converted to date</param>
        /// <returns>The Date</returns>
        public static Date Parse(Object o)
        {
            return Date.Parse(o.ToString(), defaultFormatProvider);
        }
        /// <summary>
        /// Converts the specified string representation of a date into its Date equvilant.
        /// </summary>
        /// <param name="s">string to convrt to a Date</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// The Date
        /// </returns>
        public static Date Parse(string s, IFormatProvider provider)
        {
            return Date.Parse(s, provider, DateTimeStyles.None);
        }
        /// <summary>
        /// Converts the specified string representation of a date into its Date equvilant.
        /// </summary>
        /// <param name="s">string to convrt to a Date</param>
        /// <param name="provider">The provider.</param>
        /// <param name="styles">The styles.</param>
        /// <returns>
        /// The Date
        /// </returns>
        public static Date Parse(string s, IFormatProvider provider, DateTimeStyles styles)
        {
            DateTime result;
            isTryParseSuccessful = DateTime.TryParse(s, out result);
            return FromDateTime(result);
        }
        /// <summary>
        /// Converts the specified string representation of a date to is CCS.Entity.Date equivalent using the
        /// specified array of formats, culture-specific format information, and style. The format of the
        /// string representation must match at least one of the specified formats exactly.
        /// Exceptions:
        /// System.FormatException
        /// System.ArgumentNullException
        /// System.ArgumentException
        /// </summary>
        /// <param name="s">A string containing a date to convert.</param>
        /// <param name="format">The format.</param>
        /// <param name="provider">A System.IFormatProvider that provides culture specific information about s.</param>
        /// <returns>
        /// The converted Date.
        /// </returns>
        public static Date ParseExact(string s, string format, IFormatProvider provider)
        {
            return ParseExact(s, format, provider, DateTimeStyles.None);
        }

        /// <summary>
        /// Converts the specified string representation of a date to is CCS.Entity.Date equivalent using the
        /// specified array of formats, culture-specific format information, and style. The format of the
        /// string representation must match at least one of the specified formats exactly.
        /// Exceptions:
        /// System.FormatException
        /// System.ArgumentNullException
        /// System.ArgumentException
        /// </summary>
        /// <param name="s">A string containing a date to convert.</param>
        /// <param name="format">The format.</param>
        /// <param name="provider">A System.IFormatProvider that provides culture specific information about s.</param>
        /// <param name="style">A bitwise combination of System.Globalization.DateTimeStyles values that indicates the permitted
        /// format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
        /// <returns>
        /// The converted Date.
        /// </returns>
        public static Date ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
        {
            return ParseExact(s, new string[] { format }, provider, DateTimeStyles.None);
        }

        /// <summary>
        /// Converts the specified string representation of a date to is CCS.Entity.Date equivalent using the 
        /// specified array of formats, culture-specific format information, and style. The format of the
        /// string representation must match at least one of the specified formats exactly.
        /// 
        /// Exceptions: 
        /// System.FormatException
        /// System.ArgumentNullException
        /// System.ArgumentException
        /// </summary>
        /// <param name="s">A string containing a date to convert.</param>
        /// <param name="formats">An array of expected formats of s.</param>
        /// <param name="provider">A System.IFormatProvider that provides culture specific information about s.</param>
        /// <param name="style">A bitwise combination of System.Globalization.DateTimeStyles values that indicates the permitted
        /// format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
        /// <returns>The converted Date.</returns>
        public static Date ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
        {
            DateTime parseResult = DateTime.ParseExact(s, formats, provider, style);
            Date result = FromDateTime(parseResult);
            return result;
        }


        #endregion
        #region From... methods
        /// <summary>
        /// Method to extract a Date from a DateTime.
        /// </summary>
        /// <param name="dateTime">The DateTime to use as the basis for the new Date.</param>
        /// <returns>The new Date.</returns>
        /// Unit Test: Pass
        public static Date FromDateTime(DateTime dateTime)
        {
            Date newDate = new Date(dateTime.Year, dateTime.Month, dateTime.Day);
            return newDate;
        }
        /// <summary>
        /// Method to extract a DateTime from a Date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// The new DateTime.
        /// </returns>
        public static DateTime ToDateTime(Date date)
        {
            DateTime newDateTime = new DateTime(date.Year, date.Month, date.Day);
            return newDateTime;
        }
        /// <summary>
        /// Returns a Date equivalent to the OLE Automation Date.
        /// </summary>
        /// <param name="d">OLE Automation Date</param>
        /// <returns>Converted Date.</returns>
        /// Unit Test: Pass
        public static Date FromOADate(double d)
        {
            DateTime fromOA = DateTime.FromOADate(d);
            Date result = FromDateTime(fromOA);
            return result;
        }
        #endregion
        #region To... methods
        /// <summary>
        /// Return a DateTime holding the same date as this instance.
        /// Time is set to midnight.
        /// </summary>
        /// <returns>This date as a DateTime.</returns>
        /// Unit Test: Pass
        public DateTime ToDateTime()
        {
            return new DateTime(internalDateTime.Year, internalDateTime.Month, internalDateTime.Day);
        }
        /// <summary>
        /// Convert the date to a string in long date format.
        /// </summary>
        /// <returns>string containing date shown in long date format.</returns>
        /// Unit Test: Pass
        public string ToLongDateString()
        {
            return ToString("D");
        }
        /// <summary>
        /// Convert the date to a string in short date format.
        /// </summary>
        /// <returns>string containing date shown in short date format.</returns>
        /// Unit Test: Pass
        public string ToShortDateString()
        {
            return ToString("d");
        }
        /// <summary>
        /// Convert this date to a string in the default format.
        /// </summary>
        /// <returns>string containing date expressed in standard format</returns>
        /// Unit Test: Pass
        public override string ToString()
        {
            return ToString(defaultFormatString, defaultFormatProvider);
        }
        /// <summary>
        /// Convert this date to a string in the default format.
        /// </summary>
        /// <param name="provider">Format Provider</param>
        /// <returns>string containing date expressed in standard format</returns>
        public string ToString(IFormatProvider provider)
        {
            return ToString(defaultFormatString, provider);
        }
        /// <summary>
        /// Convert this date to a string in the default format.
        /// </summary>
        /// <param name="format">string containing required format.</param>
        /// <returns>string containing date expressed in standard format</returns>
        /// Unit Test: Pass
        public string ToString(string format)
        {
            return ToString(format, defaultFormatProvider);
        }
        /// <summary>
        /// Convert this date to a string in the default format.
        /// </summary>
        /// <param name="format">string containing required format.</param>
        /// <param name="provider">Format Provider</param>
        /// <returns>string containing date expressed in standard format</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            checkFormat(ref format);
            if (provider == null)
            {
                provider = defaultFormatProvider;
            }
            string result = internalDateTime.ToString(format, provider);
            return result;
        }
        /// <summary>
        /// Check that the format parameter is valid for a ToString operation.
        /// 
        /// If the format is not valid then a FormatException will be raised.
        /// </summary>
        /// <param name="format">The format to check.</param>
        private void checkFormat(ref string format)
        {
            // If an empty string was supplied then substitute the default value.
            //Pravin - System was crashing if format is null.... if (format == string.Empty)
            if (string.IsNullOrEmpty(format))
            {
                format = defaultFormatString;
            }
            // Check that the format parameter was valid.
            if (format.Length == 1)
            {
                checkValidDateTimeFormatInfoCharacter(ref format);
            }
        }
        /// <summary>
        /// Check that the format provided is valid for a date. 
        /// If the format is not one of the following, then a FormatException will be raised.
        /// Format Character Associated Property / Description 
        /// d ShortDatePattern 
        /// D LongDatePattern 
        /// m, M MonthDayPattern 
        /// r, R RFC1123Pattern 
        /// y, Y YearMonthPattern 
        /// 
        /// If the supplied format is 'g' or 'G' - the 'general' and default option
        /// then 'd' or 'D' will be substituted.
        /// </summary>
        /// <param name="format">the format string to validate or substitute</param>
        private void checkValidDateTimeFormatInfoCharacter(ref string format)
        {
            bool valid = false;
            for (int loop = 0; loop < validDateFormats.Length; loop++)
            {
                if (validDateFormats[loop] == format)
                {
                    valid = true;
                    break;
                }
            }
            if (!valid)
            {
                if (format == "g")
                {
                    format = "d";
                    return;
                }
                if (format == "G")
                {
                    format = "D";
                    return;
                }
                // We'll only get here if format was not in the valid array and is not g or G.
                throw new FormatException(string.Format("'{0} is not a valid format for a Date.", format));
            }
        }
        /// <summary>
        /// string array of valid formats used for string representations
        /// </summary>
        private static string[] validDateFormats;
        #endregion
        #region Properties & Fields
        /// <summary>
        /// Gets the day of the month represented by this instance
        /// </summary>
        public int Day
        {
            get
            {
                return internalDateTime.Day;
            }
        }
        /// <summary>
        /// Gets the day of the week represented by this instance
        /// </summary>
        public DayOfWeek DayOfWeek
        {
            get
            {
                return internalDateTime.DayOfWeek;
            }
        }

        /// <summary>
        /// Gets the day of the year represented by this instance
        /// </summary>
        public int DayOfYear
        {
            get
            {
                return internalDateTime.DayOfYear;
            }
        }
        /// <summary>
        /// Gets the month component of the date represented in this instance
        /// </summary>
        public int Month
        {
            get
            {
                return internalDateTime.Month;
            }
        }
        /// <summary>
        /// Gets the year component of the date represented in this instance
        /// </summary>
        public int Year
        {
            get
            {
                return internalDateTime.Year;
            }
        }
        /// <summary>
        /// Gets the current date
        /// </summary>
        /// Unit Test: Pass
        public static Date Now
        {
            get
            {
                return Today;
            }
        }
        /// <summary>
        /// Gets the current date
        /// </summary>
        /// Unit Test: Pass
        public static Date Today
        {
            get
            {
                DateTime dateTimeToday = DateTime.Today;
                Date today = new Date(dateTimeToday.Year, dateTimeToday.Month, dateTimeToday.Day);
                return today;
            }
        }
        /// <summary>
        /// Get yesterday's date.
        /// </summary>
        public static Date Yesterday
        {
            get
            {
                return Today.AddDays(-1);
            }
        }
        /// <summary>
        /// Get tomorrow's date.
        /// </summary>
        public static Date Tomorrow
        {
            get
            {
                return Today.AddDays(1);
            }
        }
        /// <summary>
        /// The latest date that can be represented in the Date struct.
        /// </summary>
        /// Unit Test: Pass
        public static readonly Date MaxValue;
        /// <summary>
        /// The earliest date that can be represented in the Date struct.
        /// </summary>
        /// Unit Test: Pass
        public static readonly Date MinValue;
        /// <summary>
        /// The default format provider for ToString operations.
        /// </summary>
        private static DateTimeFormatInfo defaultFormatProvider;
        /// <summary>
        /// The default format for ToString operations.
        /// </summary>
        private static string defaultFormatString;
        #endregion
        //public static System.Xml.XmlQualifiedName GetXmlSchema(System.Xml.Schema.XmlSchemaSet xss)
        //{
        //    System.Xml.XmlQualifiedName name = SerializationUtilities.GetXmlSchema(xss, System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType);
        //    return name;
        //}
        ///// <summary>
        ///// Get the XMLSchemaType for the Date struct
        ///// </summary>
        //public static XmlSchemaType XmlSchemaType
        //{
        //    get
        //    {
        //        return XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Date);
        //    }
        //}

        #region IConvertible Members

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public byte ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public char ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public DateTime ToDateTime(IFormatProvider provider)
        {
            return new DateTime(internalDateTime.Ticks);
        }
        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public double ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public short ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public int ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public long ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public float ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(Date))
            {
                return this;
            }
            if (conversionType == typeof(DateTime))
            {
                return ToDateTime(provider);
            }
            throw new InvalidCastException();
        }
        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public uint ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        #endregion
        #region IConvertible Members
        TypeCode IConvertible.GetTypeCode()
        {
            return this.GetTypeCode();
        }
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return new DateTime(internalDateTime.Ticks);
        }
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(Date))
            {
                return this;
            }
            if (conversionType == typeof(DateTime))
            {
                return ToDateTime(provider);
            }
            if (conversionType == typeof(DateTime))
            {
                return ToString(provider);
            }
            throw new InvalidCastException();
        }
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        #endregion


        public static Date FromObject(object value)
        {
            if (value is Date)
                return (Date)value ;
            if (value is DateTime)
                return FromDateTime((DateTime)value);
            if (value is string)
                return new Date((string) value);
            if (value is long)
                return new Date((long)value);
            if (value is double)
                return FromOADate((double)value);
            throw new  System.InvalidCastException( string.Format("Unable to convert from {0} to Date", value.GetType().FullName));
        }


        #region Xml Conversion
        /// <summary>
        /// Converts the value in this instance into a piece of text in teh Xml date format.
        /// e.g. 21/08/2006 becomes 2006-08-21
        /// </summary>
        /// <returns>The date in xml format</returns>
        public string ToXmlString()
        {
            string result = string.Format("{0:D4}-{1:D2}-{2:D2}", Year, Month, Day);
            return result;
        }
        /// <summary>
        /// Pulls a date out of the Xml string given.
        /// e.g. 2006-08-21 becomes 21/08/2006
        /// 
        /// Required format is YYYY-MM-DD.
        /// </summary>
        /// <param name="xmlString">the date as a string in xml format.</param>
        public static Date FromXmlString(string xmlString)
        {
            return Date.FromDateTime(XmlConvert.ToDateTime(xmlString,XmlDateTimeSerializationMode.Utc));
        }

        /// <summary>
        /// Test to see if a value is numeric
        /// Modified to use TryParse which prevents exceptions occuring in this code.
        /// </summary>
        /// <param name="number">string to test</param>
        /// <returns>true if 'number' holds a number</returns>
        private static bool isNumeric(string number)
        {
            int result;
            return int.TryParse(number, out result);
        }



        #endregion





        //#region ISerializable Members
        ////Pravin:Support for serialisation
        ///// <summary>
        ///// Populates a serialization information object with the data needed to serialize the CCS.Entity.Date.
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="context"></param>
        //public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        //{
        //    info.AddValue("Date", internalDateTime);
        //}
        ///// <summary>
        ///// Initializes a new instance of the CCS.Entity.Date class with the System.Runtime.Serialization.SerializationInfo and the System.Runtime.Serialization.StreamingContext.
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="ctxt"></param>
        //public Date(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext ctxt)
        //{
        //    internalDateTime = Convert.ToDateTime(info.GetValue("Date", typeof(DateTime))); //can use - DateTime.Parse(info.GetValue("Date", typeof(DateTime)).ToString()); 
        //}
        //#endregion
    }
}

