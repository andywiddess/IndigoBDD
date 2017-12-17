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

            AutomationInstance.INSTANCE.Driver.StartApplication();
        }

        /// <summary>
        /// stops the application.
        /// </summary>
        [When("I stop the application")]
        public void StopApplication()
        {
            LogUtils.LogStart();

            AutomationInstance.INSTANCE.Driver.CloseApplication();
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
        /// Navigates to page.
        /// </summary>
        /// <param name="url">The URL.</param>
        [When("I browse to \"$url\"")]
        public void NavigateToPage(string url)
        {
            LogUtils.LogStart(() => url);

            AutomationInstance.INSTANCE.Driver.NavigateToPage(url);
        }

        /// <summary>
        /// Presses the element.
        /// </summary>
        /// <param name="buttonName"></param>
        [When("I click the button \"$buttonName\"")]
        public void PressElement(string buttonName)
        {
            LogUtils.LogStart(() => buttonName);

            AutomationInstance.INSTANCE.Driver.ClickButton(buttonName);
        }

        /// <summary>
        /// Submits the form by click the button
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        [When("I submit the page by clicking the button \"$buttonName\"")]
        public void Submit(string buttonName)
        {
            LogUtils.LogStart(() => buttonName);

            AutomationInstance.INSTANCE.Driver.Submit(buttonName);
        }

        /// <summary>
        /// Selects the menu option.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="option">The option.</param>
        [When("I select the menu \"$menu\" and choose option \"$option\"")]
        public void SelectMenuOption(string menu, string option)
        {
            LogUtils.LogStart(() => menu, () => option);

            AutomationInstance.INSTANCE.Driver.SelectMenuOption(menu, option);
        }

        /// <summary>
        /// Selects the menu option.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="option1">The option1.</param>
        /// <param name="option2">The option2.</param>
        [When("I select the menu \"$menu\" and choose option \"$option1\" then choose option \"$option2\"")]
        public void SelectMenuOption(string menu, string option1, string option2)
        {
            LogUtils.LogStart(() => menu, () => option1, () => option2);

            AutomationInstance.INSTANCE.Driver.SelectMenuOption(menu, option1, option2);
        }

        /// <summary>
        /// Fills the out panel value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="automationId">The automation identifier.</param>
        /// <exception cref="Exception"></exception>
        [When("I type the text \"$text\" into \"$automationId\"")]
        public void FillOutPanelValue(string text, string automationId)
        {
            LogUtils.LogStart(() => text, () => automationId);

            AutomationInstance.INSTANCE.Driver.FillOutPanelValue(text, automationId);
        }

        /// <summary>
        /// Fills the out panel HTML value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="automationId">The automation identifier.</param>
        [When("I type the htmltext \"$text\" into \"$automationId\"")]
        public void FillOutPanelHtmlValue(string text, string automationId)
        {
            LogUtils.LogStart(() => text, () => automationId);

            AutomationInstance.INSTANCE.Driver.FillOutPanelHtmlValue(text, automationId);
        }

        /// <summary>
        /// Adds text to the filter and select it
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="filter">The filter.</param>
        [When("Using a grid I type \"$text\" into filter column \"$filterColumn\"")]
        public void AddToFilterAndSelect(string text, string filterColumn)
        {
            LogUtils.LogStart(() => text, () => filterColumn);

            int column = -1;
            if (int.TryParse(filterColumn, out column))
            {
                AutomationInstance.INSTANCE.Driver.FillOutFilterValue(text, column);
            }
            else
            {
                throw new Exception($"The argument filterColumn '{filterColumn}' is expected to be a 1 based integer");
            }
        }

        /// <summary>
        /// Sets the RadioButton.
        /// </summary>
        /// <param name="radioButton">The radio button.</param>
        [When("I select the radio button \"$radioButton\"")]
        public void SetRadioButton(string radioButton)
        {
            LogUtils.LogStart(() => radioButton);

            AutomationInstance.INSTANCE.Driver.SetRadioButton(radioButton);
        }

        /// <summary>
        /// Selects the item from drop down list.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="element">The element.</param>
        [When("I select \"$text\" from the drop down list \"$element\"")]
        public void SelectItemFromDropDownList(string text, string element)
        {
            LogUtils.LogStart(() => text, () => element);

            AutomationInstance.INSTANCE.Driver.SelectOptionFromDDL(text, element);
        }

        /// <summary>
        /// Views the page.
        /// </summary>
        [When("I view the page")]
        public void ViewPage()
        {
            LogUtils.LogStart();

            // Do Nothing here at the moment
        }

        /// <summary>
        /// Changes the colour wheel colour.
        /// </summary>
        /// <param name="xaxis">The x.</param>
        /// <param name="yaxis">The y.</param>
        /// <param name="colour">The colour.</param>
        [When("I change the colourwheel offset to \"$xaxis\", \"$yaxis\" with colour \"$colour\"")]
        public void ChangeColourWheelColour(string xaxis, string yaxis, string colour)
        {
            LogUtils.LogStart(() => xaxis, () => yaxis, () => colour);

            int xValue = int.Parse(xaxis);
            int yValue = int.Parse(yaxis);

            AutomationInstance.INSTANCE.Driver.ChangeColourwheelColour(xValue, yValue, colour);
        }
        #endregion

        #region Private Methods
        #endregion
    }
}