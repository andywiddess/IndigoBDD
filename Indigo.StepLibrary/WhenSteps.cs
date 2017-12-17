using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

using NBehave.Narrator.Framework;

using OpenQA.Selenium;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.InputDevices;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
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
    public class WhenSteps
        : IWhen
    {
#region Implementation
        /// <summary>
        /// Starts the application.
        /// </summary>
        [When("I start the application")]
        public void StartApplication()
        {
            LogUtils.LogStart();

            GivenSteps.Test.BaseSetup();

            try
            {
                GivenSteps.Test.MainWindow.DisplayState = DisplayState.Maximized;
                Check.Equals(GivenSteps.Test.MainWindow.DisplayState, DisplayState.Maximized);

                GivenSteps.Test.MainWindow.DisplayState = DisplayState.Restored;
                Check.Equals(GivenSteps.Test.MainWindow.DisplayState, DisplayState.Restored);

                GivenSteps.Test.MainWindow.DisplayState = DisplayState.Minimized;
                Check.Equals(GivenSteps.Test.MainWindow.DisplayState, DisplayState.Minimized);
            }
            finally
            {
                GivenSteps.Test.MainWindow.DisplayState = DisplayState.Maximized;
            }
        }

        /// <summary>
        /// stops the application.
        /// </summary>
        [When("I stop the application")]
        public void StopApplication()
        {
            LogUtils.LogStart();

            GivenSteps.Test.BaseTeardown();
        }

        /// <summary>
        /// Waits the time.
        /// </summary>
        /// <param name="num">The number.</param>
        [When("I wait for $num seconds")]
        [When("I wait for $num second")]
        public void WaitTime(int num)
        {
            LogUtils.LogStart(() => num);

            Thread.Sleep(num * 1000);
        }

        /// <summary>
        /// Presses the element.
        /// </summary>
        /// <param name="elementName">Name of the element.</param>
        [When("I click the button with the name \"$buttonName\"")]
        public void PressElement(string buttonName)
        {
            LogUtils.LogStart(() => buttonName);

            GivenSteps.Test.MainWindow.Focus();
            Check.That(GivenSteps.Test.MainWindow.IsCurrentlyActive, Is.Not.Null);
            GivenSteps.Test.MainWindow.Get<Button>(Utils.FindItem(buttonName)).Click();
            Check.That(GivenSteps.Test.MainWindow.IsCurrentlyActive, Is.Not.Null);
        }
#endregion

#region Private Methods
#endregion
    }
#endif
}