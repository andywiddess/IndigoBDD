using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.Selenium.Contracts
{
    public interface IThen
    {
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
        /// Checks the content of for URL.
        /// </summary>
        /// <param name="text">The text.</param>
        void CheckForUrlContent(string text);

        /// <summary>
        /// Checks for URL content not equal to.
        /// </summary>
        /// <param name="text">The text.</param>
        void CheckForUrlContentNotEqualTo(string text);

        /// <summary>
        /// Closes the application.
        /// </summary>
        void CloseApplication();

        /// <summary>
        /// Checks the button is disabled.
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        void CheckButtonIsDisabled(string buttonName);

        /// <summary>
        /// Checks the item does not exist on the current view
        /// </summary>
        /// <param name="item">The item.</param>
        void CannotSeeItem(string item);

        /// <summary>
        /// Checks the item exists on the current view
        /// </summary>
        /// <param name="item">The item.</param>
        void CanSeeItem(string item);
    }
}
