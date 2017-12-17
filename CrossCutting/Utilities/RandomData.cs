using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Generates randomised data
    /// </summary>
    public static class RandomData
    {

        private static Random random = new Random();
        public static string[] GenerateRandomData(Type dataType, Nullable<int> length, int sampleSize)
        {
            List<string> sampleValues = new List<string>();
            for (int count = 0; count < sampleSize; count++)
            {
                sampleValues.Add(generateRandomData(dataType, length).ToString());
            }
            return sampleValues.ToArray();
        }

        /// <summary>
        /// Generates a random string filled with a ToString of data for the passed data type. Strings returned are double quote delimited, otherwise undelimited
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string generateRandomData(Type dataType, Nullable<int> length)
        {
            if (dataType == typeof(string))
            {
                return randomString(length.Value, true);
            }
            else if (dataType == typeof(DateTime))
            {
                DateTime dateTime = new DateTime(
                    random.Next(1950, 2000) + 1,
                    random.Next(12) + 1,
                    random.Next(25) + 1,
                    random.Next(23) + 1,
                    random.Next(59) + 1,
                    random.Next(59) + 1);
                return dateTime.ToString("f");
            }
            else if (dataType == typeof(int))
            {
                return random.Next(int.MinValue, int.MaxValue).ToString();
            }
            else if (dataType == typeof(Indigo.CrossCutting.Utilities.DataTypes.Date))
            {
                DateTime dateTime = new DateTime(
                    random.Next(1950, 2000) + 1,
                    random.Next(12) + 1,
                    random.Next(25) + 1);
                return dateTime.ToString("D");
            }
            else if (dataType == typeof(Indigo.CrossCutting.Utilities.DataTypes.Time))
            {
                DateTime dateTime = new DateTime(
                    random.Next(2000) + 1,
                    random.Next(12) + 1,
                    random.Next(25) + 1,
                    random.Next(23) + 1,
                    random.Next(59) + 1,
                    random.Next(59) + 1);
                return dateTime.ToString("t");
            }
            else if (dataType == typeof(long))
            {
                return ((long)random.Next(int.MaxValue) + (long)random.Next(int.MaxValue)).ToString();
            }
            else if (dataType == typeof(float))
            {
                return random.Next(1000).ToString() + "." + random.Next(1000).ToString();
            }
            else if (dataType == typeof(decimal))
            {
                return random.Next(1000).ToString() + "." + random.Next(10).ToString();
            }
            else if (dataType == typeof(bool))
            {
                return (random.Next(100) > 50).ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        private static string randomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        /// <summary>
        /// Generates a random unicode string with the given length
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        private static string randomUnicodeString(int length)
        {
            byte[] str = new byte[length * 2];

            for (int i = 0; i < length * 2; i += 2)
            {
                int chr = random.Next(0xD7FF);
                str[i + 1] = (byte)((chr & 0xFF00) >> 8);
                str[i] = (byte)(chr & 0xFF);
            }

            return Encoding.Unicode.GetString(str);
        }

        /// <summary>
        /// Gets the random.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static string GetRandom(IList<string> list)
        {
            return list[random.Next(list.Count - 1)];
        }

        /// <summary>
        /// Gets the random.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static TObject GetRandom<TObject>(IList<TObject> list)
        {
            return list[random.Next(list.Count - 1)];
        }
    }
}
