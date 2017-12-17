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

using Indigo.CrossCutting.Utilities;
using Indigo.CrossCutting.Utilities.CheckExpression;
using Indigo.CrossCutting.Utilities.Logging;
using Indigo.CrossCutting.Utilities.Selenium.Contracts;
using Indigo.CrossCutting.Utilities.Selenium;

using Indigo.WhiteIntegration.Configuration;
using Indigo.WhiteIntegration.Implementation;

namespace Indigo
{
    [ActionSteps]
    public class ThenSteps
        : IThen
    {
        #region Implementation
        /// <summary>
        /// Takes a screenshot
        /// </summary>
        /// <param name="filename"></param>
        [Then("I take a screenshot using the file prefix \"$filename\"")]
        public void Screenshot(string filename)
        {
            AutomationInstance.INSTANCE.Driver.Screenshot(filename);
        }

        /// <summary>
        /// Sees the titlebar.
        /// </summary>
        /// <param name="text">The text.</param>
        [Then("I should see the titlebar \"$text\"")]
        public void SeeTitlebar(string text)
        {
            AutomationInstance.INSTANCE.Driver.SeeTitlebar(text);
        }

        /// <summary>
        /// Confirms the dialog displayed.
        /// </summary>
        /// <param name="text">The text.</param>
        [Then("I should see the window \"$text\"")]
        public void ConfirmWindowDisplayed(string text)
        {
            AutomationInstance.INSTANCE.Driver.ConfirmWindowDisplayed(text);
        }

        /// <summary>
        /// Confirms the dialog displayed.
        /// </summary>
        /// <param name="text">The text.</param>
        [Then("I should see the dialog \"$text\"")]
        public void ConfirmDialogDisplayed(string text)
        {
            AutomationInstance.INSTANCE.Driver.ConfirmDialogDisplayed(text);
        }

        /// <summary>
        /// Checks the field has value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="label">The label.</param>
        /// <exception cref="Exception"></exception>
        [Then("I should see the text \"$text\" within \"$label\"")]
        public void CheckFieldHasValue(string text, string label)
        {
            AutomationInstance.INSTANCE.Driver.CheckFieldHasValue(text, label);
        }

        /// <summary>
        /// Checks the content of for URL.
        /// </summary>
        /// <param name="text">The text.</param>
        [Then("I should see \"$text\" in the contents of the url")]
        public void CheckForUrlContent(string text)
        {
            LogUtils.LogStart(() => text);

            Check.Ensure(AutomationInstance.INSTANCE.Driver.UrlContains(text), "Missing Text");
        }

        /// <summary>
        /// Checks for URL content not equal to.
        /// </summary>
        /// <param name="text">The text.</param>
        [Then("I should not see \"$text\" in the contents of the url")]
        public void CheckForUrlContentNotEqualTo(string text)
        {
            LogUtils.LogStart(() => text);

            Check.Ensure(AutomationInstance.INSTANCE.Driver.UrlDoesNotContain(text), "Missing Text");
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        [Then("I can close the application")]
        public void CloseApplication()
        {
            LogUtils.LogStart();

            AutomationInstance.INSTANCE.Driver.CloseApplication();
        }

        /// <summary>
        /// Checks the button is disabled.
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        [Then("I should not be able to click the \"$buttonName\" button")]
        public void CheckButtonIsDisabled(string buttonName)
        {
            LogUtils.LogStart(() => buttonName);

            Check.Ensure(AutomationInstance.INSTANCE.Driver.CheckButtonIsDisabled(buttonName), "Button should be disabled");
        }

        /// <summary>
        /// Checks the item does not exist on the current view
        /// </summary>
        /// <param name="item">The item.</param>
        [Then("I should not be able to see the item \"$item\"")]
        public void CannotSeeItem(string item)
        {
            LogUtils.LogStart(() => item);

            Check.Ensure(AutomationInstance.INSTANCE.Driver.CheckItemIsNotVisible(item), "item should be hidden");
        }

        /// <summary>
        /// Checks the item exists on the current view
        /// </summary>
        /// <param name="item">The item.</param>
        [Then("I should be able to see the item \"$item\"")]
        public void CanSeeItem(string item)
        {
            LogUtils.LogStart(() => item);

            Check.Ensure(!AutomationInstance.INSTANCE.Driver.CheckItemIsNotVisible(item), "item should be visible");
        }
        #endregion
    }
}
