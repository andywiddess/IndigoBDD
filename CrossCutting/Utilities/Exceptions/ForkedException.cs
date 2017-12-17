using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Exceptions
{
    /// <summary>
    /// Exception thrown when forked action fails. As rethrowing same exception overwrites stack-trace, we need
    /// really simple exception which will contain a reference to original exception, but clearly suggest what happened.
    /// Thats why all contructors have innerException mandatory - there are no ForkedExceptions on their own they always
    /// encapsulate some other exception.
    /// </summary>
    public class ForkedException 
        : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="ForkedException"/> class.</summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        private ForkedException(Exception innerException, string message = null)
            : base(FormatMessage(message ?? "Exception in forked thread", innerException), innerException)
        {
        }

        /// <summary>Formats the message.</summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <returns>Formatted message.</returns>
        private static string FormatMessage(string message, Exception innerException)
        {
            if (innerException == null)
            {
                return $"{message} (Unknown exception)";
            }
            else
            {
                return $"{message} ({innerException.GetType().Name}: {innerException.Message})";
            }
        }

        /// <summary>Strips ForkedException from specified exception.</summary>
        /// <param name="e">The e.</param>
        /// <returns>'Real' exception.</returns>
        public static Exception Strip(Exception e)
        {
            var forked = e as ForkedException;
            if (forked != null) return Strip(forked.InnerException);
            return e;
        }

        /// <summary>Makes ForkedException from the specified exception. Does nothing if e is already a FarkedException..</summary>
        /// <param name="e">The exception to encapsulate.</param>
        /// <param name="message">The message.</param>
        /// <returns>ForkedException.</returns>
        public static ForkedException Make(Exception e, string message = null)
        {
            return (e == null) ? null : ((e as ForkedException) ?? new ForkedException(e, message));
        }

        /// <summary>Initializes a new instance of the <see cref="ForkedException"/> class.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected ForkedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
