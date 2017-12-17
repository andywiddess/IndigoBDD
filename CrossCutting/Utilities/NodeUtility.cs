using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.Utility
{
    public static class NodeUtility
    {
        /// <summary>
        /// Gets the node value.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="i">The i.</param>
        /// <param name="ident">The identity of the node.</param>
        /// <returns></returns>
        public static string GetNodeValue(IDictionary<string, object> node, int i, string ident)
        {
            string result = (((IDictionary<string, object>)(IDictionary<string, object>)((object[])node["records"])[i])[ident] != null)
                        ? ((IDictionary<string, object>)(IDictionary<string, object>)((object[])node["records"])[i])[ident].ToString()
                        : string.Empty;

            return result;
        }

        /// <summary>
        /// Gets the record type value.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="i">The i.</param>
        /// <param name="ident">The ident.</param>
        /// <returns></returns>
        public static string GetRecordTypeValue(IDictionary<string, object> node, int i, string ident)
        {
            string result = ((IDictionary<string, object>)((IDictionary<string, object>)(IDictionary<string, object>)((object[])node["records"])[i])[ident] != null)
                        ? ((IDictionary<string, object>)((IDictionary<string, object>)(IDictionary<string, object>)((object[])node["records"])[i])[ident])["Name"].ToString()
                        : string.Empty;

            return result;

        }

        /// <summary>
        /// Gets the bool value.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="i">The i.</param>
        /// <param name="ident">The ident.</param>
        /// <returns></returns>
        public static bool GetBoolValue(IDictionary<string, object> node, int i, string ident)
        {
            bool result = false;

            try
            {
                string val = GetNodeValue(node, i, ident);
                if (!string.IsNullOrEmpty(val))
                    result = bool.Parse(val);
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// Gets the double value.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="i">The i.</param>
        /// <param name="ident">The ident.</param>
        /// <returns></returns>
        public static double GetDoubleValue(IDictionary<string, object> node, int i, string ident)
        {
            double result = 0.0d;

            try
            {
                string val = GetNodeValue(node, i, ident);
                if (!string.IsNullOrEmpty(val))
                    result = double.Parse(val);
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
