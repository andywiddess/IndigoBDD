using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Security;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Management;
using System.Web.Security;
using System.Xml;

using log4net;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// The main application log.
    /// Use this static class to write to a defined in the current .
    /// </summary>
    public static class Log
    {
        #region Constants
        /// <summary>
        /// Log Entry Timeout Period
        /// </summary>
        static int logEntrySentTimeOut = 15000;
        #endregion

        #region Members
        /// <summary>
        /// Gets or sets the skip frame count.
        /// </summary>
        /// <value>The skip frame count.</value>
        public static int SkipFrameCount
        {
            get;
            set;
        }

        /// <summary>
        /// Log4net Logger
        /// </summary>
        private static ILog internalLogger = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the <see cref="Log"/> class.
        /// </summary>
        static Log()
        {
            SkipFrameCount = 4;

            try
            {
                log4net.Config.XmlConfigurator.Configure();

                internalLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                internalLogger.Debug($"Started Logging {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"trying to read configuration from file {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\log4net.config");
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region ListOfProperties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public static ILog Logger
        {
            get => internalLogger;
            set => internalLogger = value;
        }

        /// <summary>
        /// Gets a value indicating whether debug is enabled. If <code>true</code>
        /// calls to Debug will be attempted, otherwise they will be ignored.
        /// </summary>
        /// <value><c>true</c> if debug is enabled; otherwise, <c>false</c>.</value>
        public static bool DebugEnabled => IsLevelEnabled(LogLevelType.Debug);

        /// <summary>
        /// Gets a value indicating whether info is enabled. If <code>true</code>
        /// calls to Info will be attempted, otherwise they will be ignored.
        /// </summary>
        /// <value><c>true</c> if info is enabled; otherwise, <c>false</c>.</value>
        public static bool InfoEnabled => IsLevelEnabled(LogLevelType.Info);

        /// <summary>
        /// Gets a value indicating whether [warn enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [warn enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool WarnEnabled => IsLevelEnabled(LogLevelType.Warn);

        /// <summary>
        /// Gets a value indicating whether error is enabled. If <code>true</code>
        /// calls to Error will be attempted, otherwise they will be ignored.
        /// </summary>
        /// <value><c>true</c> if error is enabled; otherwise, <c>false</c>.</value>
        public static bool ErrorEnabled => IsLevelEnabled(LogLevelType.Error);

        /// <summary>
        /// Gets a value indicating whether fatal is enabled. If <code>true</code>
        /// calls to Fatal will be attempted, otherwise they will be ignored.
        /// </summary>
        /// <value><c>true</c> if fatal is enabled; otherwise, <c>false</c>.</value>
        public static bool FatalEnabled => IsLevelEnabled(LogLevelType.Fatal);

        #endregion

        #region Static Methods
        /// <summary>
        /// Determines whether [is permission granted] [the specified requested permission].
        /// </summary>
        /// <param name="requestedPermission">The requested permission.</param>
        /// <returns>
        /// 	<c>true</c> if [is permission granted] [the specified requested permission]; otherwise, <c>false</c>.
        /// </returns>
        static bool IsPermissionGranted(IPermission requestedPermission)
        {
            bool result = false;

            try
            {
                // Try and get this permission.
                requestedPermission.Demand();
                result = true;
            }
            catch
            {
                // Do Nothing
            }

            return result;
        }

        /// <summary>
        /// Determines whether [is level enabled] [the specified log level].
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <returns>
        /// 	<c>true</c> if [is level enabled] [the specified log level]; otherwise, <c>false</c>.
        /// </returns>
        static bool IsLevelEnabled(LogLevelType logLevel)
        {
            bool result = false;

            try
            {
                switch (logLevel)
                {
                    case LogLevelType.Debug:
                        result = internalLogger.IsDebugEnabled;
                        break;

                    case LogLevelType.Error:
                        result = internalLogger.IsErrorEnabled;
                        break;

                    case LogLevelType.Fatal:
                        result = internalLogger.IsFatalEnabled;
                        break;

                    case LogLevelType.Info:
                        result = internalLogger.IsInfoEnabled;
                        break;

                    case LogLevelType.Warn:
                        result = internalLogger.IsWarnEnabled;
                        break;

                    default:
                        result = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                internalLogger.Error(ex);
            }

            return result;
        }

        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        public static void WriteLogEntry(LogLevelType logLevel, string message)
        {
            try
            {
                WriteLogEntryAux(logLevel, message, null, null);
            }
            catch (Exception ex)
            {
                internalLogger.Error(ex);
            }
        }

        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="exception">The exception.</param>
        public static void WriteLogEntry(LogLevelType logLevel, Exception exception)
        {
            try
            {
                WriteLogEntryAux(logLevel, "Exception Raised", exception, null);
            }
            catch (Exception ex)
            {
                internalLogger.Error(ex);
            }
        }

        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void WriteLogEntry(LogLevelType logLevel, string message, Exception exception)
        {
            try
            {
                WriteLogEntryAux(logLevel, message, exception, null);
            }
            catch (Exception ex)
            {
                internalLogger.Error(ex);
            }
        }

        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="properties">The properties.</param>
        public static void WriteLogEntry(LogLevelType logLevel, string message, Exception exception, IDictionary<string, object> properties)
        {
            try
            {
                WriteLogEntryAux(logLevel, message, exception, properties);
            }
            catch (Exception ex)
            {
                internalLogger.Error(ex);
            }
        }

        /// <summary>
        /// Writes the log entry aux.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="properties">The properties.</param>
        static void WriteLogEntryAux(LogLevelType logLevel, string message, Exception exception, IDictionary<string, object> properties)
        {
            CodeLocation codeLocation = GetCallerLocation(SkipFrameCount);

            var entry = new ServerLogEntry(logLevel, message)
            {
                CodeLocation = codeLocation,
                LogName = codeLocation.ClassName,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                OccuredAt = DateTime.Now,
                ListOfProperties = properties
            };

            object eventRaisedLock = new object(); // TODO: Should use Shared Locking

            if (exception != null)
                entry.Exception = exception;

            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    try
                    {
                        ThreadPool.QueueUserWorkItem(
                            delegate
                            {
                                try
                                {
                                    Monitor.TryEnter(eventRaisedLock, logEntrySentTimeOut);
                                    try
                                    {
                                        // Log Message
                                        if (entry != null)
                                            writeMessage(entry);
                                    }
                                    finally
                                    {
                                        Monitor.Exit(eventRaisedLock);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex);
                                }
                            });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                });
        }

        /// <summary>
        /// Writes the log entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
        private static string writeLogEntry(IServerLogEntry entry)
        {
            string message = string.Empty;

            try
            {
                if (entry.Exception != null)
                {
                    string location = string.Empty;
                    if (entry.CodeLocation != null)
                        location = $"SOURCE: {entry.CodeLocation.FileName}:{entry.CodeLocation.LineNumber} \n\tMETHOD: {entry.CodeLocation.ClassName}.{entry.CodeLocation.MethodName}";

                    message = $"{nullVal(entry.Message)}\n\t{location}\n\t{entry.Exception}";
                }
                else
                {
                    message = $"{nullVal(entry.Message)}";
                }
            }
            catch
            {
            }

            return message;
        }

        /// <summary>
        /// Nulls the val.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        private static string nullVal(string val)
        {
            return !string.IsNullOrEmpty(val) ? val : "<Null>";
        }

        /// <summary>
        /// Writes the message.
        /// </summary>
        /// <param name="entry">The entry.</param>
        private static void writeMessage(IServerLogEntry entry)
        {
            switch (entry.LogLevel)
            {
                case LogLevelType.Debug:
                    if (DebugEnabled)
                    {
                        internalLogger.Debug(writeLogEntry(entry));
                    }
                    break;

                case LogLevelType.Error:
                    if (ErrorEnabled)
                    {
                        internalLogger.Error(writeLogEntry(entry));
                    }
                    break;

                case LogLevelType.Fatal:
                    if (FatalEnabled)
                    {
                        internalLogger.Fatal(writeLogEntry(entry));
                    }
                    break;

                case LogLevelType.Info:
                    if (InfoEnabled)
                    {
                        internalLogger.Info(writeLogEntry(entry));
                    }
                    break;

                case LogLevelType.Warn:
                    if (WarnEnabled)
                    {
                        internalLogger.Warn(writeLogEntry(entry));
                    }
                    break;

                default:
                    internalLogger.Info(writeLogEntry(entry));
                    break;
            }
        }

        /// <summary>
        /// Gets the caller location from the <see cref="StackTrace"/>.
        /// TODO: Make an extensibility point for GetCallerLocation.
        /// </summary>
        /// <param name="methodCallCount">The method call count.</param>
        /// <returns>
        /// The code location that the call to log originated.
        /// </returns>
        static CodeLocation GetCallerLocation(int methodCallCount)
        {
            string className;
            string methodName;
            string fileName;
            int lineNumber;

            var stackTrace = new StackTrace(methodCallCount, true);
            var stackFrame = stackTrace.GetFrame(0);

            if (stackFrame != null)
            {
                var reflectedType = stackFrame.GetMethod().ReflectedType;

                className = reflectedType?.FullName;
                methodName = stackFrame.GetMethod().Name;
                fileName = stackFrame.GetFileName();
                lineNumber = stackFrame.GetFileLineNumber();
            }
            else
            {
                className = string.Empty;
                methodName = string.Empty;
                fileName = string.Empty;
                lineNumber = -1;
            }

            CodeLocation callerLocation = new CodeLocation
            {
                ClassName = className,
                MethodName = methodName,
                FileName = fileName,
                LineNumber = lineNumber
            };

            return callerLocation;
        }
        #endregion

        #region Logging Static Methods
        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Debug"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void Debug(string message)
        {
            WriteLogEntry(LogLevelType.Debug, message, null, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Debug"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="prms">The Parameters.</param>
        public static void Debug(string message, params object[] prms)
        {
            WriteLogEntry(LogLevelType.Debug, string.Format(message, prms), null, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Debug"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="properties">Custom properties
        /// to add additional logging information.</param>
        public static void Debug(string message, IDictionary<string, object> properties)
        {
            WriteLogEntry(LogLevelType.Debug, message, null, properties);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Debug"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception that this message
        /// is regarding.</param>
        public static void Debug(string message, Exception exception)
        {
            WriteLogEntry(LogLevelType.Debug, message, exception, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Debug"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception that this message
        /// is regarding.</param>
        /// <param name="properties">Custom properties
        /// to add additional logging information.</param>
        public static void Debug(string message, Exception exception, IDictionary<string, object> properties)
        {
            WriteLogEntry(LogLevelType.Debug, message, exception, properties);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Info"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void Info(string message)
        {
            WriteLogEntry(LogLevelType.Info, message, null, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Info"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="prms">The Parameters.</param>
        public static void Info(string message, params object[] prms)
        {
            WriteLogEntry(LogLevelType.Info, string.Format(message, prms), null, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Info"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="properties">Custom properties
        /// to add additional logging information.</param>
        public static void Info(string message, IDictionary<string, object> properties)
        {
            WriteLogEntry(LogLevelType.Info, message, null, properties);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Info"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception that this message  is regarding.</param>
        public static void Info(string message, Exception exception)
        {
            WriteLogEntry(LogLevelType.Info, message, exception, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Info"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception that this message  is regarding.</param>
        /// <param name="properties">Custom properties  to add additional logging information.</param>
        public static void Info(string message, Exception exception, IDictionary<string, object> properties)
        {
            WriteLogEntry(LogLevelType.Info, message, exception, properties);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Warn"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void Warn(string message)
        {
            WriteLogEntry(LogLevelType.Warn, message, null, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Warn"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="prms">The Parameters.</param>
        public static void Warn(string message, params object[] prms)
        {
            WriteLogEntry(LogLevelType.Warn, string.Format(message, prms), null, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Warn"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="properties">Custom properties  to add additional logging information.</param>
        public static void Warn(string message, IDictionary<string, object> properties)
        {
            WriteLogEntry(LogLevelType.Warn, message, null, properties);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Warn"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception that this message  is regarding.</param>
        public static void Warn(string message, Exception exception)
        {
            WriteLogEntry(LogLevelType.Warn, message, exception, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Warn"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception that this message  is regarding.</param>
        /// <param name="properties">Custom properties  to add additional logging information.</param>
        public static void Warn(string message, Exception exception, IDictionary<string, object> properties)
        {
            WriteLogEntry(LogLevelType.Warn, message, exception, properties);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Error"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void Error(string message)
        {
            WriteLogEntry(LogLevelType.Error, message, null, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Error"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="prms">The Parameters.</param>
        public static void Error(string message, params object[] prms)
        {
            WriteLogEntry(LogLevelType.Error, string.Format(message, prms), null, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Error"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="properties">Custom properties  to add additional logging information.</param>
        public static void Error(string message, IDictionary<string, object> properties)
        {
            WriteLogEntry(LogLevelType.Error, message, null, properties);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Error"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception that this message  is regarding.</param>
        public static void Error(string message, Exception exception)
        {
            WriteLogEntry(LogLevelType.Error, message, exception, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Error"/>
        /// level to the active .
        /// </summary>
        /// <param name="exception">The exception being raised.</param>
        public static void Error(Exception exception)
        {
            WriteLogEntry(LogLevelType.Error, "Internal Error", exception, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Error"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception that this message  is regarding.</param>
        /// <param name="properties">Custom properties  to add additional logging information.</param>
        public static void Error(string message, Exception exception, IDictionary<string, object> properties)
        {
            WriteLogEntry(LogLevelType.Error, message, exception, properties);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Fatal"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void Fatal(string message)
        {
            WriteLogEntry(LogLevelType.Fatal, message, null, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Fatal"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="prms">The Parameters.</param>
        public static void Fatal(string message, params object[] prms)
        {
            WriteLogEntry(LogLevelType.Fatal, string.Format(message, prms), null, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Fatal"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="properties">Custom properties  to add additional logging information.</param>
        public static void Fatal(string message, IDictionary<string, object> properties)
        {
            WriteLogEntry(LogLevelType.Fatal, message, null, properties);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Fatal"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception that this message  is regarding.</param>
        public static void Fatal(string message, Exception exception)
        {
            WriteLogEntry(LogLevelType.Fatal, message, exception, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Fatal"/>
        /// level to the active .
        /// </summary>
        /// <param name="exception">The exception that this message  is regarding.</param>
        public static void Fatal(Exception exception)
        {
            WriteLogEntry(LogLevelType.Fatal, "Fatal Error", exception, null);
        }

        /// <summary>
        /// Writes a log entry at the <see cref="LogLevelType.Fatal"/>
        /// level to the active .
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception that this message  is regarding.</param>
        /// <param name="properties">Custom properties  to add additional logging information.</param>
        public static void Fatal(string message, Exception exception, IDictionary<string, object> properties)
        {
            WriteLogEntry(LogLevelType.Fatal, message, exception, properties);
        }
        #endregion
    }
}
