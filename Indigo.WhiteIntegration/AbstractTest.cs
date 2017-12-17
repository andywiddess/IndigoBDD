using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;

using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.InputDevices;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using EA.AutomatedTesting.Indigo.WhiteIntegration.Configuration;

namespace EA.AutomatedTesting.Indigo.WhiteIntegration
{
    /// <summary>
    /// Abstract Test
    /// </summary>
    public abstract class AbstractTest
    {
        #region Members
        /// <summary>
        /// Gets the framework.
        /// </summary>
        /// <value>
        /// The framework.
        /// </value>
        public WindowsFramework Framework { get; private set; }

        /// <summary>
        /// The keyboard
        /// </summary>
        internal Keyboard Keyboard;

        /// <summary>
        /// Gets the main window.
        /// </summary>
        /// <value>
        /// The main window.
        /// </value>
        public Window MainWindow { get; private set; }

        /// <summary>
        /// Gets the main screen.
        /// </summary>
        /// <value>
        /// The main screen.
        /// </value>
        public MainScreen MainScreen { get; private set; }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        public Application Application { get; private set; }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public ScreenRepository Repository { get; private set; }

        /// <summary>
        /// The windows to close
        /// </summary>
        public readonly List<Window> windowsToClose = new List<Window>();

        /// <summary>
        /// The main window
        /// </summary>
        private IDisposable mainWindow;

        /// <summary>
        /// The application path
        /// </summary>
        private string applicationPath = string.Empty;

        /// <summary>
        /// The output path
        /// </summary>
        private string outputPath = string.Empty;

        /// <summary>
        /// The initial window name
        /// </summary>
        private string initialWindowName = "MainWindow";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTest"/> class.
        /// </summary>
        /// <param name="framework">The framework.</param>
        protected AbstractTest(WindowsFramework framework)
        {
            Framework = framework;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTest" /> class.
        /// </summary>
        /// <param name="framework">The framework.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="applicationPath">The application path.</param>
        /// <param name="mainWindowName">Name of the main window.</param>
        protected AbstractTest(WindowsFramework framework, string outputPath, string applicationPath, string mainWindowName)
            : this(framework)
        {
            this.outputPath = outputPath;
            this.applicationPath = applicationPath;
            this.initialWindowName = mainWindowName;

            Directory.CreateDirectory(this.outputPath);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Bases the setup.
        /// </summary>
        public void BaseSetup()
        {
            mainWindow = SetMainWindow(Framework, applicationPath, initialWindowName);
        }

        /// <summary>
        /// Bases the teardown.
        /// </summary>
        public void BaseTeardown()
        {
            mainWindow.Dispose();
        }

        /// <summary>
        /// Takes the screenshot.
        /// </summary>
        /// <param name="screenshotName">Name of the screenshot.</param>
        /// <returns></returns>
        public string TakeScreenshot(string screenshotName)
        {
            var imagename = screenshotName + ".png";
            var imagePath = Path.Combine(outputPath, imagename);
            try
            {
                Desktop.CaptureScreenshot().Save(imagePath, ImageFormat.Png);
                Trace.WriteLine(String.Format("Screenshot taken: {0}", imagePath));
            }
            catch (Exception)
            {
                Trace.TraceError(String.Format("Failed to save screenshot to directory: {0}, filename: {1}", outputPath, imagePath));
            }
            return imagePath;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the main window.
        /// </summary>
        /// <param name="framework">The framework.</param>
        /// <returns></returns>
        protected IDisposable SetMainWindow(WindowsFramework framework)
        {
            try
            {
                Keyboard = Keyboard.Instance;
                var configuration = ConfigurationFactory.Create(framework);
                Application = configuration.LaunchApplication(applicationPath);
                Repository = new ScreenRepository(Application);
                MainWindow = configuration.GetMainWindow(Application, initialWindowName);
                MainScreen = configuration.GetMainScreen(Repository, initialWindowName);

                return new ShutdownApplicationDisposable(this);
            }
            catch (Exception e)
            {
                if (Application != null)
                    Application.Close();
                throw;
            }
        }

        /// <summary>
        /// Sets the main window.
        /// </summary>
        /// <param name="framework">The framework.</param>
        /// <param name="applicationPath">The application path.</param>
        /// <param name="mainWindowName">Name of the main window.</param>
        /// <returns></returns>
        protected IDisposable SetMainWindow(WindowsFramework framework, string applicationPath, string mainWindowName)
        {
            try
            {
                Keyboard = Keyboard.Instance;
                var configuration = ConfigurationFactory.Create(framework);
                Application = configuration.LaunchApplication(applicationPath);
                Repository = new ScreenRepository(Application);
                MainWindow = configuration.GetMainWindow(Application, mainWindowName);
                MainScreen = configuration.GetMainScreen(Repository, mainWindowName);

                return new ShutdownApplicationDisposable(this);
            }
            catch (Exception e)
            {
                if (Application != null)
                    Application.Close();
                throw;
            }
        }

        /// <summary>
        /// Starts the scenario.
        /// </summary>
        /// <param name="scenarioButtonId">The scenario button identifier.</param>
        /// <param name="windowTitle">The window title.</param>
        /// <returns></returns>
        protected Window StartScenario(string scenarioButtonId, string windowTitle)
        {
            MainWindow.Get<Button>(scenarioButtonId).Click();
            var window = MainWindow.ModalWindow(windowTitle);
            windowsToClose.Add(window);
            return window;
        }

        /// <summary>
        /// Restarts this instance.
        /// </summary>
        protected void Restart()
        {
            mainWindow.Dispose();
            mainWindow = SetMainWindow(Framework);
        }

        /// <summary>
        /// Gets the framework identifier.
        /// </summary>
        /// <value>
        /// The framework identifier.
        /// </value>
        protected string FrameworkId
        {
            get
            {
                var type = Framework.GetType();
                var memInfo = type.GetMember(Framework.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(FrameworkIdAttribute), false);
                return ((FrameworkIdAttribute)attributes[0]).FrameworkId;
            }
        }
        #endregion
    }
}
