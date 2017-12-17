using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;

using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.InputDevices;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using EA.AutomatedTesting.Indigo.WhiteIntegration.Configuration;

namespace EA.AutomatedTesting.Indigo.WhiteIntegration.Implementation
{
    public class GivenTest
        : AbstractTest
    {
        #region Members
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GivenTest" /> class.
        /// </summary>
        public GivenTest()
            : base(WindowsFramework.WinForms)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GivenTest"/> class.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="execPath">The execute path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="formName">Name of the form.</param>
        public GivenTest(string applicationName, string execPath, string outputPath, string formName)
            : base(WindowsFramework.WinForms, outputPath, execPath, formName)
        {

        }
        #endregion

        #region Overrides
        #endregion
    }
}
