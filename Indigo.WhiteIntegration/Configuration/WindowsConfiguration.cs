using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

using Indigo.WhiteIntegration;

namespace Indigo.WhiteIntegration.Configuration
{
    /// <summary>
    /// Microsoft Winforms Configuration 
    /// </summary>
    /// <seealso cref="Indigo.WhiteIntegration.AbstractApplication" />
    public class WinformsConfiguration
        : AbstractApplication
    {
        #region Members
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsConfiguration" /> class.
        /// </summary>
        /// <param name="framework">The framework.</param>
        public WinformsConfiguration(WindowsFramework framework)
                    : base(framework)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsConfiguration"/> class.
        /// </summary>
        /// <param name="framework">The framework.</param>
        /// <param name="applicationPath">The application path.</param>
        public WinformsConfiguration(WindowsFramework framework, string applicationPath)
            : base(framework)
        {
            ApplicationExePath = applicationPath;
        }
        #endregion

        #region Public Methods
        #endregion
    }
}
