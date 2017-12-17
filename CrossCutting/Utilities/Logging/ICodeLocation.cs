using System;

namespace Indigo.CrossCutting.Utilities.Logging
{
    /// <summary>
    /// Specifies a place in the source code or class,
    /// where an logging event occurred.
    /// </summary>
    public interface ICodeLocation
    {
        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the line number.
        /// </summary>
        /// <value>The line number.</value>
        int LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        string MethodName
        {
            get;
            set;
        }
    }
}
