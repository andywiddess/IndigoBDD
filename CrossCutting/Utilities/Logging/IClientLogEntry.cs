namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// A log entry originating from a remote location
    /// such as from a web client, arriving via a web service.
    /// </summary>
    public interface IClientLogEntry
        : ILogEntry
    {
        /// <summary>
        /// Gets the exception log May be null.
        /// </summary>
        /// <value>The exception log.</value>
        IExceptionLog ExceptionLog
        {
            get;
        }
    }
}
