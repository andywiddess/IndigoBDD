using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.Selenium.Contracts
{
    public interface IWhen
    {
        /// <summary>
        /// Starts the application.
        /// </summary>
        void StartApplication();

        /// <summary>
        /// stops the application.
        /// </summary>
        void StopApplication();

        /// <summary>
        /// Waits the time.
        /// </summary>
        /// <param name="num">The number.</param>
        void WaitTime(int num);

        /// <summary>
        /// Presses the element.
        /// </summary>
        /// <param name="elementName">Name of the element.</param>
        void PressElement(string buttonName);

        /// <summary>
        /// Submits the form by click the button
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        void Submit(string buttonName);

        /// <summary>
        /// Selects the menu option.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="option">The option.</param>
        void SelectMenuOption(string menu, string option);

        /// <summary>
        /// Selects the menu option.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="option1">The option1.</param>
        /// <param name="option2">The option2.</param>
        void SelectMenuOption(string menu, string option1, string option2);

        /// <summary>
        /// Fills the out panel value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="automationId">The automation identifier.</param>
        /// <exception cref="Exception"></exception>
        void FillOutPanelValue(string text, string automationId);

        /// <summary>
        /// Fills the out panel HTML value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="automationId">The automation identifier.</param>
        void FillOutPanelHtmlValue(string text, string automationId);

        /// <summary>
        /// Navigates to page.
        /// </summary>
        /// <param name="location">The location.</param>
        void NavigateToPage(string location);

        /// <summary>
        /// Adds text to the filter and select it
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="filter">The filter.</param>
        void AddToFilterAndSelect(string text, string filter);

        /// <summary>
        /// Sets the RadioButton.
        /// </summary>
        /// <param name="radioButton">The radio button.</param>
        void SetRadioButton(string radioButton);

        /// <summary>
        /// Selects the item from drop down list.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="element">The element.</param>
        void SelectItemFromDropDownList(string text, string element);

        /// <summary>
        /// Views the page.
        /// </summary>
        void ViewPage();


        /// <summary>
        /// Changes the colour wheel colour.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="colour">The colour.</param>
        void ChangeColourWheelColour(string x, string y, string colour);
    }
}
