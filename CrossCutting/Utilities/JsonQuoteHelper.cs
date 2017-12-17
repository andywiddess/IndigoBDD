using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

using Indigo.CrossCutting.Utilities.Logging;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Json serialize helper for removing quotes
    /// </summary>
    public static class JsonQuoteHelper
    {

        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Serialize<T>(T data)
            where T : class
        {
            if (data == null) { return string.Empty; }

            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(data.GetType());
                MemoryStream ms = new MemoryStream();
                ser.WriteObject(ms, data);

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error($"Error serialize {ex}");
            }

            return string.Empty;
        }

        /// <summary>
        /// Deserializes the specified json data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonData">The json data.</param>
        /// <returns></returns>
        public static T Deserialize<T>(string jsonData)
            where T : class, new()
        {
            if (string.IsNullOrEmpty(jsonData)) { return new T(); }

            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                return ser.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(jsonData))) as T;
            }
            catch (Exception ex)
            {
                Log.Error($"Error parsing {ex}");
                return new T();
            }
        }

    }
}
