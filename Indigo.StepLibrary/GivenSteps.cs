using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;

using OpenQA.Selenium;
using NBehave.Narrator.Framework;

using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.InputDevices;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

using EA.Shared.CrossCutting.Framework.CheckExpression;
using EA.Shared.CrossCutting.Framework.Logging;

using EA.AutomatedTesting.Indigo.Contracts;
using EA.AutomatedTesting.Indigo.WhiteIntegration.Configuration;
using EA.AutomatedTesting.Indigo.WhiteIntegration.Implementation;

namespace EA.AutomatedTesting.Indigo.StepLibrary
{
#if NEW
    [ActionSteps]
    public class GivenSteps
        : IGiven
    {
        public static GivenTest Test = null;

        /// <summary>
        /// Gets the browser instance
        /// </summary>
        [Given("a browser")]
        public void GetBrowser()
        {
            LogUtils.LogStart();

            Check.That(DriverInstance.Instance.Driver, Is.Not.Null);
        }

        /// <summary>
        /// Get the radio manager instance
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="form">The form.</param>
        [Given("the application \"$name\" using executable path \"$path\" output to \"$outputPath\" loading the form \"$form\" ")]
        public void GetApplication(string name, string path, string outputPath, string form)
        {
            LogUtils.LogStart(() => name, () => path, () => outputPath, () => form);

            if (!string.IsNullOrEmpty(name) &&
                !string.IsNullOrEmpty(path) &&
                !string.IsNullOrEmpty(outputPath))
            {
                if (!File.Exists(path))
                    throw new Exception(string.Format("{0} does not exist and cannot be called", path));

                if (!Directory.Exists(outputPath))
                    Directory.CreateDirectory(outputPath);

                Test = new GivenTest(name, path, outputPath, form);
                if (Test == null)
                    throw new Exception("The Test configuration matching your given statement has failed");
            }
            else
            {
                throw new Exception(string.Format("The given statement is incorrectly formattted"));
            }
        }
    }
#endif
}
