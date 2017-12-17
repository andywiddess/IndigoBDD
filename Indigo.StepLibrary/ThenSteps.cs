using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Automation;

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
    public class ThenSteps
        : IThen
    {
#region Implementation
        /// <summary>
        /// Takes a screenshot
        /// </summary>
        /// <param name="text">The text.</param>
        [Then("I take a screenshot using the file prefix \"$filename\"")]
        public void Screenshot(string filename)
        {
            LogUtils.LogStart(() => filename);

            GivenSteps.Test.TakeScreenshot(filename);
        }

        /// <summary>
        /// Sees the titlebar.
        /// </summary>
        /// <param name="text">The text.</param>
        [Then("I should see the titlebar \"$text\"")]
        public void SeeTitlebar(string text)
        {
            LogUtils.LogStart(() => text);
            TestStack.White.UIItems.Finders.SearchCriteria criteria = Utils.FindItem(text);
            if (criteria == null)
                criteria = Utils.FindItem(ControlType.TitleBar);

            TitleBar bar = GivenSteps.Test.MainWindow.Get<TitleBar>(criteria);
            Check.That(bar, Is.Not.Null);
        }
#endregion
    }
#endif
}
