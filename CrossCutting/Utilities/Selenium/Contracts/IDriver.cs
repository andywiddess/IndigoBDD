using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.Selenium.Contracts
{
    public interface IDriver
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WindowsDriver"/> is initialised.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialised; otherwise, <c>false</c>.
        /// </value>
        bool Initialised { get; }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="form">The form.</param>
        /// <exception cref="System.Exception">
        /// The Test configuration matching your given statement has failed
        /// or
        /// </exception>
        void GetApplication(string name, string path, string outputPath, string form);


        /// <summary>
        /// Starts the application.
        /// </summary>
        void StartApplication();

        /// <summary>
        /// Closes the application.
        /// </summary>
        void CloseApplication();

        /// <summary>
        /// Sets the feature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <param name="dataSet">The data set.</param>
        void SetFeature(string feature, string dataSet);

        /// <summary>
        /// Finds and Clicks a specific button by name
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        void ClickButton(string buttonName);

        /// <summary>
        /// Submits the form
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
        /// Fills the out field value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="currentValue">The current value.</param>
        /// <exception cref="Exception"></exception>
        void FillOutFieldValue(string text, string reference, string currentValue);

        /// <summary>
        /// Fills the out panel value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="automationId">The automation identifier.</param>
        /// <exception cref="Exception"></exception>
        void FillOutPanelValue(string text, string automationId);

        /// <summary>
        /// Fills the out filter value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="filterColumn">The filter column.</param>
        /// <exception cref="Exception"></exception>
        void FillOutFilterValue(string text, int filterColumn);

        /// <summary>
        /// Fills the out panel HTML value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="automationId">The automation identifier.</param>
        void FillOutPanelHtmlValue(string text, string automationId);

        /// <summary>
        /// Takes a screenshot
        /// </summary>
        /// <param name="filename"></param>
        void Screenshot(string filename);

        /// <summary>
        /// Sees the titlebar.
        /// </summary>
        /// <param name="text">The text.</param>
        void SeeTitlebar(string text);

        /// <summary>
        /// Confirms the dialog displayed.
        /// </summary>
        /// <param name="text">The text.</param>
        void ConfirmWindowDisplayed(string text);

        /// <summary>
        /// Confirms the dialog displayed.
        /// </summary>
        /// <param name="text">The text.</param>
        void ConfirmDialogDisplayed(string text);

        /// <summary>
        /// Checks the field has value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="label">The label.</param>
        /// <exception cref="Exception"></exception>
        void CheckFieldHasValue(string text, string label);

        /// <summary>
        /// Navigates to page.
        /// </summary>
        /// <param name="location">The location.</param>
        void NavigateToPage(string location);

        /// <summary>
        /// URL's contain some text based on location
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        bool UrlContains(string location);

        /// <summary>
        /// URL's do not contain some text based on location
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        bool UrlDoesNotContain(string location);

        /// <summary>
        /// Sets the RadioButton.
        /// </summary>
        /// <param name="radioButton">The radio button.</param>
        void SetRadioButton(string radioButton);

        /// <summary>
        /// Checks the button is disabled.
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        /// <returns></returns>
        bool CheckButtonIsDisabled(string buttonName);

        /// <summary>
        /// Selects the option from DDL.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="element">The element.</param>
        /// <exception cref="NoSuchElementException"></exception>
        /// <exception cref="InvalidSelectorException"></exception>
        void SelectOptionFromDDL(string option, string element);

        /// <summary>
        /// Checks the item is not visible.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool CheckItemIsNotVisible(string item);

        /// <summary>
        /// Changes the colourwheel colour.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="colour">The colour.</param>
        void ChangeColourwheelColour(int x, int y, string colour);
    }
}
