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

using Indigo.CrossCutting.Utilities;
using Indigo.CrossCutting.Utilities.Logging;
using Indigo.CrossCutting.Utilities.Selenium.Contracts;
using Indigo.CrossCutting.Utilities.Selenium;
using Indigo.Drivers;
using Indigo.WhiteIntegration.Configuration;
using Indigo.WhiteIntegration.Implementation;

namespace Indigo
{
    [ActionSteps]
    public class GivenSteps
        : IGiven
    {
        /// <summary>
        /// Gets the browser instance
        /// </summary>
        [Given("the application \"$name\" using the browser \"$path\" output to \"$outputPath\"")]
        public void GetBrowser(string name, string path, string outputPath)
        {
            if (AutomationInstance.INSTANCE.Driver == null ||
                !AutomationInstance.INSTANCE.Driver.Initialised)
            {
                AutomationInstance.INSTANCE.Driver = new BrowserDriver();

                AutomationInstance.INSTANCE.Driver.GetApplication(name, path, outputPath, string.Empty);
            }
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
            if (AutomationInstance.INSTANCE.Driver == null ||
                !AutomationInstance.INSTANCE.Driver.Initialised)
            {
                AutomationInstance.INSTANCE.Driver = new WindowsDriver();

                AutomationInstance.INSTANCE.Driver.GetApplication(name, path, outputPath, form);
            }
        }

        /// <summary>
        /// Assigns the feature details to the scenario being performed
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <param name="dataSet">The data set.</param>
        [Given("that I'm using the application with Feature \"$feature\" and DataSet \"$dataSet\"")]
        public void MyApplication(string feature, string dataSet)
        {
            AutomationInstance.INSTANCE.Driver.SetFeature(feature, dataSet);
        }
    }
}
