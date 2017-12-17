using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.Net;

using log4net;

using EA.Shared.CrossCutting.Framework;
using EA.Shared.CrossCutting.Framework.CheckExpression;
using EA.Shared.CrossCutting.Framework.Logging;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Logging Utilities
    /// </summary>
    public static class LogUtils
    {
        #region Constants
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Log Method Start
        /// </summary>
        public static void LogStart()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                // Get the current method and parameter List
                var currentMethod = new StackTrace().GetFrame(1).GetMethod();
                var parameterList = (currentMethod.GetParameters() != null ? currentMethod.GetParameters().ToList() : null);

                if (currentMethod.GetParameters() != null)
                    currentMethod.GetParameters().ToList().ForEach(f => sb.AppendFormat("{0}{1}", (sb.Length > 0 ? ", " : ""), f.Name));

                Log.DebugFormat("Method = {0}.{1} ({2})",
                    currentMethod.DeclaringType.FullName,
                    currentMethod.Name,
                    sb.ToString());

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        /// <summary>
        /// Log Method Start with Expressions
        /// </summary>
        /// <param name="providedParameters">The provided parameters.</param>
        public static void LogStart(params Expression<Func<object>>[] providedParameters)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                // Get the current method and parameter List
                var currentMethod = new StackTrace().GetFrame(1).GetMethod();
                var parameterList = (currentMethod.GetParameters() != null ? currentMethod.GetParameters().ToList() : null);

                // Get provided methods parameters
                var providedParametars = new List<Tuple<string, Type, object>>();
                foreach (var aExpression in providedParameters)
                {
                    Expression bodyType = aExpression.Body;
                    if (bodyType is MemberExpression)
                    {
                        addProvidedParamaterDetail(providedParametars, (MemberExpression)aExpression.Body);
                    }
                    else if (bodyType is UnaryExpression)
                    {
                        UnaryExpression unaryExpression = (UnaryExpression)aExpression.Body;
                        addProvidedParamaterDetail(providedParametars, (MemberExpression)unaryExpression.Operand);
                    }
                    else
                    {
                        throw new Exception("Expression type unknown.");
                    }
                }

                // Construct Log Record Parameters
                if (currentMethod.GetParameters() != null)
                    currentMethod.GetParameters().ToList().ForEach(f =>
                    {
                        sb.AppendFormat("{0}{1}", (sb.Length > 0 ? ", " : ""), f.Name);
                        if (providedParametars.Exists(p => p.Item1.Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)))
                            sb.AppendFormat(" = {0}", providedParametars.Find(p => p.Item1.Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Item3.ToString());
                    });

                // Construct Log Record
                Log.DebugFormat("Method = {0}.{1} ({2})",
                    currentMethod.DeclaringType.FullName,
                    currentMethod.Name,
                    sb.ToString());
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        /// <summary>
        /// Writes to windows event log.
        /// </summary>
        /// <param name="logName">Name of the windows event log.</param>
        /// <param name="source">The source.</param>
        /// <param name="errorDetail">The error detail.</param>
        public static void WriteToEventLog(string logName, string source, string errorDetail)
        {
            System.Diagnostics.EventLog SQLEventLog = new System.Diagnostics.EventLog();

            try
            {
                if (!System.Diagnostics.EventLog.SourceExists(logName))
                    createLog(logName);

                SQLEventLog.Source = logName;
                SQLEventLog.WriteEntry(Convert.ToString(source)
                                      + Convert.ToString(errorDetail), EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                SQLEventLog.Source = logName;
                SQLEventLog.WriteEntry(Convert.ToString("INFORMATION: ")
                                      + Convert.ToString(ex.Message), EventLogEntryType.Error);

            }
            finally
            {
                SQLEventLog.Dispose();
                SQLEventLog = null;

            }
        }
        #endregion

        #region Private Static Methods
        /// <summary>
        /// Creates the windows event log.
        /// </summary>
        /// <param name="strLogName">Name of the windows event log.</param>
        /// <returns></returns>
        private static bool createLog(string strLogName)
        {
            bool Reasult = false;

            try
            {
                System.Diagnostics.EventLog.CreateEventSource(strLogName, strLogName);
                System.Diagnostics.EventLog SQLEventLog = new System.Diagnostics.EventLog();

                SQLEventLog.Source = strLogName;
                SQLEventLog.Log = strLogName;

                SQLEventLog.Source = strLogName;
                SQLEventLog.WriteEntry("The " + strLogName + " was successfully initialize component.", EventLogEntryType.Information);


                Reasult = true;

            }
            catch
            {
                Reasult = false;
            }

            return Reasult;
        }

        /// <summary>
        /// Adds the provided paramater detail.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="memberExpression">The member expression.</param>
        private static void addProvidedParamaterDetail(List<Tuple<string, Type, object>> list, MemberExpression memberExpression)
        {
            ConstantExpression constantExpression = (ConstantExpression)memberExpression.Expression;
            var name = memberExpression.Member.Name;
            if (memberExpression.Member.MemberType == MemberTypes.Property)
            {
                var value = ((PropertyInfo)memberExpression.Member).GetValue(constantExpression.Value);
                if (value != null)
                    value = Newtonsoft.Json.JsonConvert.SerializeObject(value);

                var type = value.GetType();
                list.Add(new Tuple<string, Type, object>(name, type, value));
            }
            else
            {
                var value = ((FieldInfo)memberExpression.Member).GetValue(constantExpression.Value);
                if (value != null)
                    value = Newtonsoft.Json.JsonConvert.SerializeObject(value);

                var type = value.GetType();
                list.Add(new Tuple<string, Type, object>(name, type, value));
            }
        }
        #endregion
    }
}
