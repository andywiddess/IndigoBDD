using System;
using System.Globalization;
using System.Xml.Schema;
using System.Xml;
using System.Runtime.Serialization;
using System.Xml.Serialization;
namespace Indigo.CrossCutting.Utilities.DataTypes
{
    /// <summary>
    /// This struct is intended to supplement the .Net standard
    /// Datatypes, providing similar functionality for
    /// a Time element.
    /// </summary>
    [DataContract(Namespace = "http://www.sepura.co.uk/IA")]
    public struct Time : IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Internal timespan store so that we can 
        /// use underlying methods and properties.
        /// </summary>
        [DataMember]
        public TimeSpan internalTime;
        private static bool isTryParseSuccessful = true;
        /// <summary>
        /// Constant specifying the default format for string representations.
        /// </summary>
        private const char DEFAULT_FORMAT = 't';
        /// <summary>
        /// Static constructor that populates the fields.
        /// </summary>
        static Time()
        {
            validDateFormats = new string[] 
            { 
            "t", "T", "hh","HH", "S", "Y", "dd/MM/yy","d/M/yy","d/M/yyyy",
            "ddd d MM yy", "ddd d MMM yy","ddd d MMM yyyy","ddd d MMMM yyyy",
            "dddd d MM yy", "dddd d MMM yy","dddd d MMM yyyy","dddd d MMMM yyyy"
            };
            defaultFormatString = DEFAULT_FORMAT.ToString();
            defaultFormatProvider = DateTimeFormatInfo.CurrentInfo;
            MinValue = new Time(0,0,0);
            MaxValue = new Time(23,59,59);
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
        /// <param name="ticks">A Timespan and time expressed in 100 nano-second units.</param>
        /// Unit Test: Pass
        public Time(long ticks)
        {
            internalTime = new TimeSpan(ticks);
        }
        /// <summary>
        /// Constructor taking a hours minutes and seconds
        /// </summary>
        /// <param name="hours">The hours</param>
        /// <param name="minutes">The minutes</param>
        /// <param name="seconds">The seconds</param>
        /// Unit Test: Pass
        public Time(int hours, int minutes, int seconds):this(hours,minutes,seconds,0)
        {
            
        }


        /// <summary>
        /// Constructor taking a date time
        /// </summary>
        /// <param name="dateTime">The dateTime</param>
        /// Unit Test: Pass
        public Time(DateTime dateTime):this(dateTime.Hour,dateTime.Minute,dateTime.Second,dateTime.Millisecond)
        {
            
        }

                /// <summary>
        /// Constructor taking a hours minutes
        /// </summary>
        /// <param name="hours">The hours</param>
        /// <param name="minutes">The minutes</param>
        /// Unit Test: Pass
        public Time(int hours, int minutes):this(hours,minutes,0,0)
        {
            
        }

        /// <summary>
        /// Constructor taking a hours minutes seconds and Milliseconds
        /// </summary>
        /// <param name="hours">The hours</param>
        /// <param name="minutes">The minutes</param>
        /// <param name="seconds">The seconds</param>
        /// <param name="milliseconds">The milliseconds.</param>
        /// Unit Test: Pass
        public Time(int hours, int minutes, int seconds, int milliseconds)
        {
            internalTime = new TimeSpan(0,hours, minutes, seconds,milliseconds);
        }



        ///<summary>
        /// Constructor taking a time in string format.
        /// Exceptions:
        /// ArgumentNullException: time is a null reference.
        /// FormatException: date does not contain a valid string representation of a date.
        /// </summary>
        /// <param name="time">sting basis for time</param>
        /// Unit Test: Pass
        public Time(string time)
        {
            if (time.Trim() == "")
            {
                internalTime = new TimeSpan(0);
            }
            else
            {
                internalTime = TimeSpan.Parse(time);
            }
        }
        #endregion

        #region Calculation operations
        /// <summary>
        /// Add the value of the specified TimeSpan to this time.
        /// will Roll around and alwasy return the time portion of any resultant date
        /// in the range 00:00:00 --> 23:59:59
        /// </summary>
        /// <param name="value">TimeSpan to add to this time.</param>
        /// <returns>The time.</returns>
        /// Unit Test: Pass
        public Time Add(TimeSpan value)
        {
            TimeSpan interim = this.internalTime.Add(value);
            Time result = FromTimeSpan(interim);
            return result;
        }
        /// <summary>
        /// Add the specified number of hours to this instance.
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <returns>
        /// The new time.
        /// </returns>
        /// Unit Test: Pass
        public Time AddHours(int hours)
        {
            //if (hours > 23 || hours < -23)
            //    throw(new System.ArgumentOutOfRangeException("hours",value,"hours must be between -23 and +23"));
            TimeSpan result;
            if (hours >= 0)
            {
               result= new TimeSpan(hours,0,0);
                return this.Add(result);
            }
            else
            {
                result = new TimeSpan(0-hours,0,0);
                return this.Subtract(result);
            }
            
        }
        /// <summary>
        /// Add the specified number of Minutes to this instance.
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns>
        /// The new date.
        /// </returns>
        public Time AddMinutes(int minutes)
        {
           //if (minutes > ((23*60)+59) || minutes < 0-((23*60)+59))
           //     throw(new System.ArgumentOutOfRangeException("minutes",minutes,"minutes must be between -23h 59m and + 23h 59m"));
           TimeSpan result;
            if (minutes >= 0)
            {
               result= new TimeSpan(0,minutes,0);
                return this.Add(result);
            }
            else
            {
                result = new TimeSpan(0,0-minutes,0);
                // add a day 
                return this.Subtract(result);
            }
        }
        /// <summary>
        /// Add the specified number of seconds to this instance.
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns>
        /// The new Time.
        /// </returns>
        /// Unit Test: Pass
        public Time AddSeconds(int seconds)
        {
           TimeSpan result;
            if (seconds >= 0)
            {
               result= new TimeSpan(0,0,seconds);
                return this.Add(result);
            }
            else
            {
                result = new TimeSpan(0,0,0-seconds);
                // add a day 
                return this.Subtract(result);
            }
        }
        /// <summary>
        /// Add the specified number of Milliseconds to this instance.
        /// 
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="milliseconds">The number of Milliseconds to add.</param>
        /// <returns>The new Time.</returns>
        /// Unit Test: Pass
        public Time AddMilliseconds(int milliseconds)
        {
            TimeSpan result;
            if (milliseconds >= 0)
            {
               result= new TimeSpan(0,0,0,0,milliseconds);
                return this.Add(result);
            }
            else
            {
                result = new TimeSpan(0,0,0,0,-milliseconds);
                // add a day 
                return this.Subtract(result);
            }
        }



        /// <summary>
        /// Subtracts the specified duration from this instance.
        /// 
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="value">Duration to subtract.</param>
        /// <returns>Times defining result.</returns>
        /// Unit Test: Pass
        public Time Subtract(Time value)
        {
            TimeSpan subtract = value.internalTime;
            return Subtract(subtract);
        }
        /// <summary>
        /// Subtracts the specified duration from this instance.
        /// 
        /// Exceptions:
        /// System.ArgumentOutOfRangeException
        /// </summary>
        /// <param name="value">Duration to subtract.</param>
        /// <returns>Time part of result.</returns>
        /// Unit Test: Pass
        public Time Subtract(DateTime value)
        {
            Time subtract = new Time(value);
            return  this.Subtract(subtract);
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
        public Time Subtract(TimeSpan value)
        {
            TimeSpan interim = new TimeSpan(0,internalTime.Hours, internalTime.Minutes, internalTime.Seconds, internalTime.Milliseconds);
            interim = interim.Subtract(value);
            Time result = FromTimeSpan(interim);
            return result;
        }
        #endregion
        
        #region Comparison operations
        /// <summary>
        /// Compare two instances of Time and returns an indication of their relative value.
        /// </summary>
        /// <param name="time1">The time1.</param>
        /// <param name="time2">The time2.</param>
        /// <returns>
        /// relative value.
        /// </returns>
        /// Unit Test: Pass
        public static int Compare(Time time1, Time time2)
        {
            TimeSpan ts1 = time1.internalTime;
            TimeSpan ts2 = time2.internalTime;
            return TimeSpan.Compare(ts1, ts2);
        }

        /// <summary>
        /// Compare this instanct to an instance of Date or DateTime and returns an indication 
        /// of the relative value. If a Time is supplied, only the Date part is considered
        /// in the comparison.
        /// </summary>
        /// <param name="value">Time to compare to this instance.</param>
        /// <returns>relative value.</returns>
        /// Unit Test: Pass
        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (!(value is Time || value is DateTime))
            {
                throw new ArgumentException("value was of type '" + value.GetType().Name + "', not Time , Timespan or DateTime.");
            }
            int comparisonResult = 0;
            if (value is Time)
            {
                comparisonResult = CompareTo((Time)value);
            }
            if (value is DateTime)
            {
                comparisonResult = CompareTo((DateTime)value);
            }

            return comparisonResult;
        }
        /// <summary>
        /// Compare this instanct to an instance of Date and returns an indication 
        /// of the relative value.
        /// </summary>
        /// <param name="value">DateTime to compare to this instance.</param>
        /// <returns>relative value.</returns>
        /// Unit Test: Pass
        public int CompareTo(DateTime value)
        {
            Time compareTo = new Time(value);
            return internalTime.CompareTo(compareTo.internalTime);
        }

        /// <summary>
        /// Compare this instanct to an instance of Time and returns an indication 
        /// of the relative value.
        /// </summary>
        /// <param name="value">Time to compare to this instance.</param>
        /// <returns>relative value.</returns>
        /// Unit Test: Pass
        public int CompareTo(Time value)
        {
            return internalTime.CompareTo(value.internalTime);
        }

 
        /// <summary>
        /// Returns a boolean indicating whether this instance is equal to the specified
        /// DateTime. If a DateTime is supplied, only the time part is considered
        /// in the comparison.
        /// </summary>
        /// <param name="value">DateTime to compare this instance to.</param>
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
                Time compare = new Time(value.ToString());
                return (compare == this);
            }
            if (!(value is Time || value is DateTime))
            {
                //throw new ArgumentException("value was of type '" + value.GetType().Name + "', not Time or DateTime.");
            }
            bool equals = false;
            if (value is Time)
            {
                equals = Equals((Time)value);
            }
            else if (value is DateTime)
            {
                equals = Equals((DateTime)value);
            }
            return equals;
        }
        /// <summary>
        /// Returns a boolean indicating whether this instance is equal to the specified
        /// DateTime. 
        /// </summary>
        /// <param name="value">DateTime to compare this instance to.</param>
        /// <returns>true if 'value' is equal to the current instance.</returns>
        /// Unit Test: Pass
        public bool Equals(DateTime value)
        {
            Time equals = new Time(value);
            return Equals(equals);
        }
        /// <summary>
        /// Returns a boolean indicating whether this instance is equal to the specified
        /// Date or DateTime.
        /// </summary>
        /// <param name="value">Date to compare this instance to.</param>
        /// <returns>true if 'value' is equal to the current instance.</returns>
        /// Unit Test: Pass
        public bool Equals(Time value)
        {
            return
            (internalTime.Hours == value.internalTime.Hours
            && internalTime.Minutes == value.internalTime.Minutes
            && internalTime.Seconds == value.internalTime.Seconds
            && internalTime.Milliseconds == value.internalTime.Milliseconds);
        }

        /// <summary>
        /// Returns a value indicating whether two times are equal.
        /// </summary>
        /// <param name="time1">The first time.</param>
        /// <param name="time2">The second time.</param>
        /// <returns>true if time1 equals time 2.</returns>
        /// Unit Test: Pass
        public static bool Equals(Time time1, Time time2)
        {
            return time1.Equals(time2);
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
                results[loop] = internalTime.ToString();//validDateFormats[loop]);
            }
            return results;
        }
        #endregion
        #region Operator overloads
        /// <summary>
        /// Add a timespan to a Time
        /// </summary>
        /// <param name="d">The original Time</param>
        /// <param name="t">The timespan to add to the time</param>
        /// <returns>The time plus the timespan</returns>
        /// Unit Test: Pass
        public static Time operator +(Time d, TimeSpan t)
        {
            return d.Add(t);
        }
        /// <summary>
        /// Compare two times
        /// </summary>
        /// <param name="t1">The first time</param>
        /// <param name="t2">The second time</param>
        /// <returns>true if the times match</returns>
        /// Unit Test: Pass
        public static bool operator ==(Time t1, Time t2)
        {
            return t1.Equals(t2);
        }


        /// <summary>
        /// Compare two times
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>
        /// true if the first time, t1, is later than t2.
        /// </returns>
        /// Unit Test: Pass
        public static bool operator >(Time t1, Time t2)
        {
            return t1.internalTime > t2.internalTime;
        }

        /// <summary>
        /// Compare two times
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>
        /// true if the first time, time1, is later than or the same day as time2.
        /// </returns>
        /// Unit Test: Pass
        public static bool operator >=(Time t1, Time t2)
        {
            return t1.internalTime >= t2.internalTime;
        }
        /// <summary>
        /// Compare two times
        /// </summary>
        /// <param name="t1">The first time</param>
        /// <param name="t2">The second time</param>
        /// <returns>true if the dates are different.</returns>
        /// Unit Test: Pass
        public static bool operator !=(Time t1, Time t2)
        {
            return !(t1.Equals(t2));
        }


        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>
        /// true if the times time1 is earlier than time2.
        /// </returns>
        /// Unit Test: Pass
        public static bool operator <(Time t1, Time t2)
        {
           return t1.internalTime < t2.internalTime;
        }

        /// <summary>
        /// Compare two dates
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>
        /// true if the times time1 is earlier or the same as time2.
        /// </returns>
        /// Unit Test: Pass
        public static bool operator <=(Time t1, Time t2)
        {
            return t1.internalTime <= t2.internalTime;
        }


        /// <summary>
        /// Subtract a timespan from a date, resulting in an earlier date.
        /// </summary>
        /// <param name="t1">The time</param>
        /// <param name="t2">The time to subtract</param>
        /// <returns>A timerepresenting the differences between the two dates</returns>
        /// Unit Test: Pass
        public static Time operator -(Time t1, Time t2)
        {
            return t1.Subtract(t2);
        }

        #endregion
        #region Parsing methods

        /// <summary>
        /// Attempt to parse a string into a Date value.
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <param name="result">The resultant date.</param>
        /// <returns>'true' if the parse succeeded, else 'false'.</returns>
        public static bool TryParse(string value, out Time result)
        {
            try
            {
                result = Time.Parse(value);
                return true;
            }
            catch (System.FormatException)
            {
                result = Time.MinValue;
                return false;
            }
            catch (System.ArgumentNullException)
            {
                result = Time.MinValue;
                return false;
            }
            catch (System.ArgumentException)
            {
                result = Time.MinValue;
                return false;
            }
        }

        /// <summary>
        /// Attempt to parse a string into a Time value.
        /// only difference of this method is that this
        /// would return 
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <param name="result">The resultant time.</param>
        /// <returns>'true' if the parse succeeded, else 'false'.</returns>
        public static bool TryParseExact(string value, out Time result)
        {
            //make this explicitly false so that we can evaluate the value and return the result
            isTryParseSuccessful = false;
            try
            {
                result = Time.Parse(value);
                //check whether we have got only number as date i.e. "10102007" for 10th october 2007
                if (new Time("") == result && isNumeric(value))
                {
                    //see whether we have got sufficient values to create time
                    double doubleVal;
                    if (double.TryParse(value, out doubleVal))
                        if (value.Length > 6)
                            result = Time.Parse(doubleVal.ToString("##/##/####"));
                        else if (value.Length == 6)
                            result = Time.Parse(doubleVal.ToString("##/##/##"));
                }
                return isTryParseSuccessful;
            }
            catch (System.FormatException)
            {
                result = Time.MinValue;
                return false;
            }
            catch (System.ArgumentNullException)
            {
                result = Time.MinValue;
                return false;
            }
            catch (System.ArgumentException)
            {
                result = Time.MinValue;
                return false;
            }
        }
        /// <summary>
        /// Converts the specified string representation of a time into its Time equvilant.
        /// </summary>
        /// <param name="s">string to convert to a time</param>
        /// <returns>The Time</returns>
        /// Unit Test: Pass
        public static Time Parse(string s)
        {
            return Time.Parse(s, defaultFormatProvider);
        }
        /// <summary>
        /// Converts the specified object representaion of a time into its Time equivalent.
        /// </summary>
        /// <param name="o">object that needs to be converted to time</param>
        /// <returns>The Time</returns>
        public static Time Parse(Object o)
        {
            return Time.Parse(o.ToString(), defaultFormatProvider);
        }
        /// <summary>
        /// Converts the specified string representation of a time into its Time equvilant.
        /// </summary>
        /// <param name="s">string to convrt to a time</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// The Time
        /// </returns>
        public static Time Parse(string s, IFormatProvider provider)
        {
            return Time.Parse(s, provider, DateTimeStyles.None);
        }
        /// <summary>
        /// Converts the specified string representation of a Time into its Time equvilant.
        /// </summary>
        /// <param name="s">string to convrt to a Time</param>
        /// <param name="provider">The provider.</param>
        /// <param name="styles">The styles.</param>
        /// <returns>
        /// The Time
        /// </returns>
        public static Time Parse(string s, IFormatProvider provider, DateTimeStyles styles)
        {
            TimeSpan result;
            isTryParseSuccessful = TimeSpan.TryParse(s, out result);
            return FromTimeSpan(result);
        }
        /// <summary>
        /// Converts the specified string representation of a time to is Time equivalent using the
        /// specified array of formats, culture-specific format information, and style. The format of the
        /// string representation must match at least one of the specified formats exactly.
        /// Exceptions:
        /// System.FormatException
        /// System.ArgumentNullException
        /// System.ArgumentException
        /// </summary>
        /// <param name="s">A string containing a time to convert.</param>
        /// <param name="format">The format.</param>
        /// <param name="provider">A System.IFormatProvider that provides culture specific information about s.</param>
        /// <returns>
        /// The converted Time.
        /// </returns>
        public static Time ParseExact(string s, string format, IFormatProvider provider)
        {
            return ParseExact(s, format, provider, DateTimeStyles.None);
        }

        /// <summary>
        /// Converts the specified string representation of a time to is Time equivalent using the
        /// specified array of formats, culture-specific format information, and style. The format of the
        /// string representation must match at least one of the specified formats exactly.
        /// Exceptions:
        /// System.FormatException
        /// System.ArgumentNullException
        /// System.ArgumentException
        /// </summary>
        /// <param name="s">A string containing a time to convert.</param>
        /// <param name="format">The format.</param>
        /// <param name="provider">A System.IFormatProvider that provides culture specific information about s.</param>
        /// <param name="style">A bitwise combination of System.Globalization.DateTimeStyles values that indicates the permitted
        /// format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
        /// <returns>
        /// The converted Time.
        /// </returns>
        public static Time ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
        {
            return ParseExact(s, new string[] { format }, provider, DateTimeStyles.None);
        }

        /// <summary>
        /// Converts the specified string representation of a time to is Time equivalent using the 
        /// specified array of formats, culture-specific format information, and style. The format of the
        /// string representation must match at least one of the specified formats exactly.
        /// 
        /// Exceptions: 
        /// System.FormatException
        /// System.ArgumentNullException
        /// System.ArgumentException
        /// </summary>
        /// <param name="s">A string containing a time to convert.</param>
        /// <param name="formats">An array of expected formats of s.</param>
        /// <param name="provider">A System.IFormatProvider that provides culture specific information about s.</param>
        /// <param name="style">A bitwise combination of System.Globalization.DateTimeStyles values that indicates the permitted
        /// format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
        /// <returns>The converted Time.</returns>
        public static Time ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
        {
            DateTime parseResult = DateTime.ParseExact(s, formats, provider, style);
            Time result = new Time(parseResult);
            return result;
        }


        #endregion
        #region From... methods
        /// <summary>
        /// Method to extract a Time from a TimeSpan.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns>
        /// The new Time.
        /// </returns>
        /// Unit Test: Pass
        public static Time FromTimeSpan(TimeSpan timeSpan)
        {
            Time newTime = new Time(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,timeSpan.Milliseconds);
            return newTime;
        }
        /// <summary>
        /// Method to extract a DateTime from a Time.
        /// </summary>
        /// <param name="time">The time to use as the basis for the new DateTime.</param>
        /// <returns>The new DateTime.</returns>
        /// <returns></returns>
        public static DateTime ToDateTime(Time time)
        {
            DateTime newDateTime = new DateTime(time.internalTime.Ticks);
            return newDateTime;
        }
        /// <summary>
        /// Returns a time equivalent to the OLE Automation Date.
        /// </summary>
        /// <param name="d">OLE Automation Date</param>
        /// <returns>Converted Time.</returns>
        /// Unit Test: Pass
        public static Time FromOADate(double d)
        {
            DateTime fromOA = DateTime.FromOADate(d);
            Time result = new Time(fromOA);
            return result;
        }
        #endregion
        #region To... methods
        /// <summary>
        /// Return a DateTime holding the same time as this instance.
        /// Date is min date.
        /// </summary>
        /// <returns>This ime as a DateTime.</returns>
        /// Unit Test: Pass
        public DateTime ToDateTime()
        {
            return new DateTime(internalTime.Ticks);
        }
        /// <summary>
        /// Convert the date to a string in long time format.
        /// </summary>
        /// <returns>string containing date shown in long date format.</returns>
        /// Unit Test: Pass
        public string ToTimeString()
        {
            return ToString("T");
        }
        /// <summary>
        /// Convert the date to a string in short time format.
        /// </summary>
        /// <returns>string containing date shown in short date format.</returns>
        /// Unit Test: Pass
        public string ToShortTimeString()
        {
            return ToString("t");
        }
        /// <summary>
        /// Convert this time to a string in the default format.
        /// </summary>
        /// <returns>string containing date expressed in standard format</returns>
        /// Unit Test: Pass
        public override string ToString()
        {
            return ToString(defaultFormatString, defaultFormatProvider);
        }
        /// <summary>
        /// Convert this time to a string in the default format.
        /// </summary>
        /// <param name="provider">Format Provider</param>
        /// <returns>string containing time expressed in standard format</returns>
        public string ToString(IFormatProvider provider)
        {
            return ToString(defaultFormatString, provider);
        }
        /// <summary>
        /// Convert this time to a string in the default format.
        /// </summary>
        /// <param name="format">string containing required format.</param>
        /// <returns>string containing time expressed in standard format</returns>
        /// Unit Test: Pass
        public string ToString(string format)
        {
            return ToString(format, defaultFormatProvider);
        }
        /// <summary>
        /// Convert this time to a string in the default format.
        /// </summary>
        /// <param name="format">string containing required format.</param>
        /// <param name="provider">Format Provider</param>
        /// <returns>string containing time expressed in standard format</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            checkFormat(ref format);
            if (provider == null)
            {
                provider = defaultFormatProvider;
            }
            string result = Time.ToDateTime(this).ToString(format, provider);
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
        /// Gets the hour of  time represented by this instance
        /// </summary>
        public int Hour
        {
            get
            {
                return internalTime.Hours;
            }
        }
        /// <summary>
        /// Gets the minute part represented by this instance
        /// </summary>
        public int Minutes
        {
            get
            {
                return internalTime.Minutes;
            }
        }

        /// <summary>
        /// Gets the secods part of the time represented by this instance
        /// </summary>
        public int Seconds
        {
            get
            {
                return internalTime.Seconds;
            }
        }
        /// <summary>
        /// Gets the milliseconds component of the time represented in this instance
        /// </summary>
        public int Milliseconds
        {
            get
            {
                return internalTime.Milliseconds;
            }
        }
       
        /// <summary>
        /// Gets the current Time
        /// </summary>
        /// Unit Test: Pass
        public static Time Now
        {
            get
            {
                return new Time(DateTime.Now);
            }
        }

       
        /// <summary>
        /// The latest Time that can be represented in the Time struct.
        /// </summary>
        /// Unit Test: Pass
        public static readonly Time MaxValue;
        /// <summary>
        /// The earliest Time that can be represented in the Time struct.
        /// </summary>
        /// Unit Test: Pass
        public static readonly Time MinValue;
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
        public TimeSpan ToTimeSpan()
        {
            return this.internalTime;
        }
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
            return new DateTime(internalTime.Ticks);
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
            if (conversionType == typeof(Time))
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
            return new DateTime(internalTime.Ticks);
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
            if (conversionType == typeof(Time))
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


        public static Time FromObject(object value)
        {
            if (value is Time)
                return (Time)value;
            if (value is DateTime)
                return new Time((DateTime)value);
            if (value is string)
                return new Time((string)value);
            if (value is long)
                return new Time((long)value);
            if (value is double)
                return FromOADate((double)value);
            if (value is TimeSpan)
                return Time.FromTimeSpan((TimeSpan)value);
            throw new System.InvalidCastException(string.Format("Unable to convert from {0} to Time", value.GetType().FullName));
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



        public static Time FromXmlString(string xmlString)
        {
            return Time.FromTimeSpan(System.Xml.XmlConvert.ToTimeSpan(xmlString));
        }

        public string ToXmlString()
        {
            return System.Xml.XmlConvert.ToString(this.internalTime);
        }
    }
}


