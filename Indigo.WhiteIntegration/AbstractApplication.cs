using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Indigo.WhiteIntegration
{
    /// <summary>
    /// Description of an Application
    /// </summary>
    /// <seealso cref="Indigo.WhiteIntegration.IApplication" />
    public abstract class AbstractApplication 
        : IApplication
    {
        #region Members
        /// <summary>
        /// The window framework instance
        /// </summary>
        public WindowsFramework Framework { get; set; }

        /// <summary>
        /// Gets the application executable path.
        /// </summary>
        /// <value>
        /// The application executable path.
        /// </value>
        public string ApplicationExePath { get; set; }

        /// <summary>
        /// Gets or sets the application output path.
        /// </summary>
        /// <value>
        /// The application output path.
        /// </value>
        public string ApplicationOutputPath { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractApplication"/> class.
        /// </summary>
        /// <param name="framework">The framework.</param>
        protected AbstractApplication(WindowsFramework framework)
        {
            this.Framework = framework;
        }
        #endregion

        #region IApplication Implementation
        /// <summary>
        /// Launches the application.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public Application LaunchApplication(string path)
        {
            //var app = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath), ApplicationExePath);
            var processStartInfo = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            return Application.Launch(processStartInfo);
        }

        /// <summary>
        /// Gets the main window.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="windowName">Name of the window - "MainWindow"</param>
        /// <returns></returns>
        public Window GetMainWindow(Application application, string windowName)
        {
            return application.GetWindow(Criteria(windowName), InitializeOption.NoCache);
        }

        /// <summary>
        /// Gets the main screen.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="windowName">Name of the window - "MainWindow"</param>
        /// <returns></returns>
        public MainScreen GetMainScreen(ScreenRepository repository, string windowName)
        {
            return repository.Get<MainScreen>(Criteria(windowName), InitializeOption.NoCache);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Retrieves the Window based on the criteria provided
        /// </summary>
        /// <param name="textName">Name of the text.</param>
        /// <returns></returns>
        public SearchCriteria Criteria(string textName)
        {
            return SearchCriteria.ByFramework(Framework.FrameworkId()).AndByText(textName);
        }
        #endregion
    }
}
