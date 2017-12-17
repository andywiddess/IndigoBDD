using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Indigo.CrossCutting.Utilities.Exceptions
{
    /// <summary>Exception thrown when file has been already locked by another proess.</summary>
    public class LockFileFailedException
        : Exception
    {
        /// <summary>Gets or sets the name of the file.</summary>
        /// <value>The name of the file.</value>
        public string FileName { get; protected set; }

        /// <summary>Formats the message.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Formatted message,</returns>
        public static string FormatMessage(string fileName)
        {
            return string.Format("Failed to acquired lock for '{0}'", fileName);
        }

        /// <summary>Initializes a new instance of the <see cref="LockFileFailedException"/> class.</summary>
        /// <param name="fileName">Name of the file.</param>
        public LockFileFailedException(string fileName)
            : base(FormatMessage(fileName))
        {
            FileName = fileName;
        }

        /// <summary>Initializes a new instance of the <see cref="LockFileFailedException"/> class.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="innerException">The inner exception.</param>
        public LockFileFailedException(string fileName, Exception innerException)
            : base(FormatMessage(fileName), innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LockFileFailedException"/> class.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected LockFileFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
