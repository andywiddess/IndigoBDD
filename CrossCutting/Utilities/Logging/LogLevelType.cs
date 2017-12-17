namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Signifies the level at which to write a <see cref="ILogEntry"/>.
    /// Are used to exclude log entries that have log levels
    /// less than the current  threshold level.
    /// </summary>
    public enum LogLevelType
    {
        /// <summary>
        /// The least restrictive level.
        /// </summary>
        All = 0,

        /// <summary>
        /// Report on Method Calls
        /// </summary>
        Method = 2,

        /// <summary>
        /// For debugging purposes.
        /// </summary>
        Debug = 4,

        /// <summary>
        /// Signifies verbose information.
        /// </summary>
        Info = 8,

        /// <summary>
        /// Signifies a warning from e.g. an unexpected event.
        /// </summary>
        Warn = 16,

        /// <summary>
        /// When an application error occurs.
        /// </summary>
        Error = 32,

        /// <summary>
        /// When the application is no longer
        /// able to function or is in an unreliable state.
        /// Highly restrive logging.
        /// </summary>
        Fatal = 64,

        /// <summary>
        /// Logging is disabled.
        /// </summary>
        None = 128
    }
}
