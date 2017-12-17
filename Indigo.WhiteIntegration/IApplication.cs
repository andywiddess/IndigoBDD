using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

using TestStack.White.ScreenObjects;

namespace Indigo.WhiteIntegration
{
    public interface IApplication
    {
        /// <summary>
        /// The window framework instance
        /// </summary>
        WindowsFramework Framework { get; set; }

        /// <summary>
        /// Gets the application executable path.
        /// </summary>
        /// <value>
        /// The application executable path.
        /// </value>
        string ApplicationExePath { get; set; }

        /// <summary>
        /// Gets or sets the application output path.
        /// </summary>
        /// <value>
        /// The application output path.
        /// </value>
        string ApplicationOutputPath { get; set; }

        /// <summary>
        /// Launches the application.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        Application LaunchApplication(string path);

        /// <summary>
        /// Gets the main window.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="windowName">Name of the window.</param>
        /// <returns></returns>
        Window GetMainWindow(Application application, string windowName);

        /// <summary>
        /// Gets the main screen.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="windowName">Name of the window.</param>
        /// <returns></returns>
        MainScreen GetMainScreen(ScreenRepository repository, string windowName);

        /// <summary>
        /// Retrieves the Window based on the criteria provided
        /// </summary>
        /// <param name="textName">Name of the text.</param>
        /// <returns></returns>
        SearchCriteria Criteria(string textName);
    }
}
