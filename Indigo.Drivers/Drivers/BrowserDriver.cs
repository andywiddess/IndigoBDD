using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

using OpenQA.Selenium.ElementTypes;
using OpenQA.Selenium.Support.Events;
using TestStack.White;

using Indigo.Drivers.Configuration;
using Indigo.CrossCutting.Utilities;
using Indigo.CrossCutting.Utilities.Logging;
using Indigo.CrossCutting.Utilities.Selenium.Contracts;
using Indigo.CrossCutting.Utilities.Selenium;
using Indigo.SeleniumIntegration;
using OpenQA.Selenium.Interactions;


namespace Indigo.Drivers
{
    /// <summary>
    /// Browser Driver
    /// </summary>
    /// <seealso cref="IDriver" />
    public class BrowserDriver
            : IDriver
    {
        #region Members
        /// <summary>
        /// The drivertype (default being chrome)
        /// </summary>
        private DriverType drivertype = DriverType.CHROME;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WindowsDriver"/> is initialised.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialised; otherwise, <c>false</c>.
        /// </value>
        public bool Initialised { get; private set; }

        /// <summary>
        /// Gets or sets the driver.
        /// </summary>
        /// <value>
        /// The driver.
        /// </value>
        public IWebDriver Driver { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        /// <value>
        /// The output path.
        /// </value>
        public string OutputPath { get; set; }

        /// <summary>
        /// Gets the application process.
        /// </summary>
        /// <value>
        /// The application process.
        /// </value>
        public Process ApplicationProcess { get; private set; }

        /// <summary>
        /// Gets the last window.
        /// </summary>
        /// <value>
        /// The last window.
        /// </value>
        public IWebElement LastWindow { get; private set; }

        /// <summary>
        /// The current feature
        /// </summary>
        private string currentFeature = string.Empty;

        /// <summary>
        /// The data set
        /// </summary>
        private int currentDataSet = -1;

        /// <summary>
        /// The data list
        /// </summary>
        private List<IDataModel> dataList = null;

        /// <summary>
        /// The event driver
        /// </summary>
        private EventFiringWebDriver eventDriver = null;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserDriver"/> class.
        /// </summary>
        /// <param name="typ">The typ.</param>
        public BrowserDriver()
        {
            Initialised = false;
        }
        #endregion

        #region IDriver Implementation
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
        public void GetApplication(string name, string path, string outputPath, string form)
        {
            LogUtils.LogStart(() => name, () => path, () => outputPath);

            // using the path identify the  type of selenium driver we're using
            drivertype = typeofPathUsed(path);
            OutputPath = outputPath;

            if (!isDriverRunning())
            {
                switch (drivertype)
                {
                    case DriverType.CHROME:
                        ChromeOptions options = new ChromeOptions();
                        string[] startingOptions = { "--start-maximized --disable-application-cache --aggressive-cache-discard --disable-notifications --disable-reading-from-canvas --disable-remote-playback-api --disable-shared-workers --disable-voice-input --enable-aggressive-domstorage-flushing" };
                        options.AddArguments(startingOptions.AsEnumerable());
                        Driver = new ChromeDriver(options);
                        Initialised = true;
                        break;

                    case DriverType.FIREFOX:
                        Driver = new FirefoxDriver();
                        break;

                    case DriverType.IE:
                        Driver = new InternetExplorerDriver("");
                        break;
                }

                // Assign the event driver if applicable
                if (Driver != null)
                {
                    eventDriver = new EventFiringWebDriver(Driver);
                    eventDriver.ElementClicked += (sender, args) => { };
                    eventDriver.ElementValueChanged += (sender, args) => { };
                    eventDriver.FindElementCompleted += (sender, args) => { };
                    eventDriver.NavigatedForward += (sender, args) => { };
                    eventDriver.ExceptionThrown += (sender, args) => { };
                    eventDriver.Navigated += (sender, args) => { };
                    eventDriver.NavigatedBack += (sender, args) => { };
                    eventDriver.ScriptExecuted += (sender, args) => { };
                    Driver = eventDriver; // this is important!
                }
            }
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        public void StartApplication()
        {
            LogUtils.LogStart();
            /*
                        var dc = new DesiredCapabilities();
                        dc.SetCapability("app", $"{appArguments.ApplicationLocation}\\{appArguments.ApplicationName}.exe");
                        dc.SetCapability("launchDelay", 4);
                        this.Driver = new RemoteWebDriver(new Uri(BaseUrl), dc);

                        ApplicationProcess = Process.GetProcessesByName(appArguments.ApplicationName)[0];
                        Initialised = true;
            */
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        public void CloseApplication()
        {
            LogUtils.LogStart();

            Driver?.Close();

            Stop();
        }

        /// <summary>
        /// Sets the feature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <param name="dataSet">The data set.</param>
        public void SetFeature(string feature, string dataSet)
        {
            currentFeature = feature;
            currentDataSet = int.Parse(dataSet);
            if (dataList == null ||
                !dataList.Exists(e => e.Feature.Equals(currentFeature) && e.DataSet == currentDataSet))
            {
                dataList = ExcelUtils
                    .Get(ConfigHandler.Settings.DataFile)
                    .Where(e => e.Feature.Equals(currentFeature) && e.DataSet == currentDataSet).ToList();
            }
        }

        /// <summary>
        /// Finds and Clicks a specific button by name
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        public void ClickButton(string item)
        {
            LogUtils.LogStart(() => item);

            IWebElement button = Driver.SearchForButton(translate(item));
            if (button != null)
            {
                button.Click();
            }
            else
            {
                throw new Exception($"Unable to locate the button {item}");
            }
        }

        /// <summary>
        /// Checks the button is disabled.
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        /// <returns></returns>
        public bool CheckButtonIsDisabled(string buttonName)
        {
            LogUtils.LogStart(() => buttonName);
            bool response = false;

            IWebElement button = Driver.SearchForButton(translate(buttonName));
            if (button != null)
            {
                response = !button.Enabled;
            }
            else
            {
                throw new Exception($"Unable to locate the button {buttonName}");
            }

            return response;
        }

        /// <summary>
        /// Submits the form
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        public void Submit(string buttonName)
        {
            LogUtils.LogStart(() => buttonName);


            IWebElement button = Driver.SearchForButton(translate(buttonName));
            if (button != null)
            {
                button.Click();
                waitForPeriod(2);
            }
            else
            {
                throw new Exception($"Unable to locate the button {buttonName}");
            }
        }

        /// <summary>
        /// Selects the menu option.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="option">The option.</param>
        public void SelectMenuOption(string menu, string option)
        {
            LogUtils.LogStart(() => menu, () => option);

            waitForPeriod(1);
            var field = Driver.SearchForElement(translate(menu));
            if (field != null)
            {
                field.Click();

                // Find option
                field = Driver.SearchForElement(translate(option));
                field?.Click();
            }
            else
            {
                throw new Exception($"Failed to find option {option} within the menu {menu}");
            }
        }

        /// <summary>
        /// Selects the menu option.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="option1">The option1.</param>
        /// <param name="option2">The option2.</param>
        public void SelectMenuOption(string menu, string option1, string option2)
        {
            throw new NotImplementedException("SelectMenuOption");
        }

        /// <summary>
        /// Fills the out field value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="currentValue">The current value.</param>
        /// <exception cref="Exception"></exception>
        public void FillOutFieldValue(string text, string reference, string currentValue)
        {
            throw new NotImplementedException("FillOutFieldValue");
        }

        /// <summary>
        /// Fills the out panel value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="automationId">The automation identifier.</param>
        /// <exception cref="Exception"></exception>
        public void FillOutPanelValue(string text, string automationId)
        {
            LogUtils.LogStart(() => text, () => automationId);

            var field = Driver.SearchForElement(translate(automationId));
            if (field != null)
            {
                field.Clear();
                field.SendKeys(translate(text));
            }
            else
            {
                throw new Exception($"Failed to find the field {automationId}");
            }
        }

        /// <summary>
        /// Fills the out filter value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="filter">The filter.</param>
        /// <exception cref="Exception"></exception>
        public void FillOutFilterValue(string text, int filterColumn)
        {
            LogUtils.LogStart(() => text, () => filterColumn);

            var field = Driver.SearchForGridFilterElement(filterColumn);
            if (field != null)
            {
                field.Clear();
                field.SendKeys(translate(text));
            }
            else
            {
                throw new Exception($"Failed to find the filter column {filterColumn} in the grid");
            }
        }

        /// <summary>
        /// Fills the out panel HTML value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="automationId">The automation identifier.</param>
        public void FillOutPanelHtmlValue(string text, string automationId)
        {
            LogUtils.LogStart(() => text, () => automationId);
            IWebDriver frame = null;

            try
            {
                var xpath = $"//div[contains(@class,\"{automationId}\")]//iframe";
                var elem = Driver.FindElement(By.XPath(xpath));
                if (elem == null)
                    throw new Exception($"Unable to locate the MCE editor that is linked to the form-group {automationId}. Please check the form-group contains the {automationId} within the parent div class");

                frame = Driver.SwitchTo().Frame(elem);
                IWebElement element = Driver.FindElement(By.CssSelector("body"));
                Driver.ExecuteScript($"arguments[0].innerHTML = '{translate(text)}'", element);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (frame != null)
                    Driver.SwitchTo().ParentFrame();
            }
        }

        /// <summary>
        /// Sets the RadioButton.
        /// </summary>
        /// <param name="radioButton">The radio button.</param>
        public void SetRadioButton(string radioButton)
        {
            LogUtils.LogStart(() => radioButton);

            var field = Driver.SearchForElement(translate(radioButton));
            if (field != null)
            {
                field.Click();
            }
            else
            {
                throw new Exception($"Failed to find the field {translate(radioButton)}");
            }
        }

        /// <summary>
        /// Takes a screenshot
        /// </summary>
        /// <param name="filename"></param>
        public void Screenshot(string filename)
        {
            LogUtils.LogStart(() => filename);

            var imagename = filename + ".png";
            var imagePath = Path.Combine(ConfigHandler.Settings.OutputDirectory, imagename);
            try
            {
                Driver.TakeScreenshot(imagePath);
                Trace.WriteLine($"Screenshot taken: {imagePath}");
            }
            catch (Exception)
            {
                Trace.TraceError($"Failed to save screenshot to directory: {OutputPath}, filename: {imagePath}");
            }
        }

        /// <summary>
        /// Sees the titlebar.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SeeTitlebar(string text)
        {
            throw new NotImplementedException("FillOutPanelValue");
        }

        /// <summary>
        /// Confirms the dialog displayed.
        /// </summary>
        /// <param name="text">The text.</param>
        public void ConfirmWindowDisplayed(string text)
        {
            throw new NotImplementedException("FillOutPanelValue");
        }

        /// <summary>
        /// Confirms the dialog displayed.
        /// </summary>
        /// <param name="text">The text.</param>
        public void ConfirmDialogDisplayed(string text)
        {
            throw new NotImplementedException("FillOutPanelValue");
        }

        /// <summary>
        /// Checks the field has value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="label">The label.</param>
        /// <exception cref="Exception"></exception>
        public void CheckFieldHasValue(string text, string label)
        {
            LogUtils.LogStart(() => text, () => label);

            IWebElement element = null;
            int[] sleepPeriods = new int[] { 3, 5, 10 };
            int retries = 0;

            while (retries < 3 && element == null)
            {
                element = Driver.SearchForElement(translate(label));
                if (element != null &&
                    element.Text.Contains(translate(text)))
                    continue;

                waitForPeriod(sleepPeriods[retries]);
                retries++;
            }

            if (element == null)
                throw new Exception($"Tried searching for {text} within {label} with no success after 3 retries");

        }

        /// <summary>
        /// Navigates to page.
        /// </summary>
        /// <param name="location">The location.</param>
        public void NavigateToPage(string location)
        {
            LogUtils.LogStart(() => location);

            Driver.Navigate().GoToUrl(translate(location));
            Driver.WaitForPageToLoad();
        }

        /// <summary>
        /// URL's contain some text based on location
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public bool UrlContains(string location)
        {
            LogUtils.LogStart(() => location);

            bool response = false;
            int[] sleepPeriods = new int[] { 3, 5, 10 };
            int retries = 0;

            while (retries < 3 && !response)
            {
                response = Driver.Url.Contains(translate(location));
                if (response)
                    continue;

                waitForPeriod(sleepPeriods[retries]);
                retries++;
            }

            return response;
        }

        /// <summary>
        /// URL's do not contain some text based on location
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>Returns true if the URL no longer contains the text to search for</returns>
        public bool UrlDoesNotContain(string location)
        {
            LogUtils.LogStart(() => location);

            bool response = true;
            int[] sleepPeriods = new int[] { 3, 5, 10 };
            int retries = 0;

            while (retries < 3 && response)
            {
                response = Driver.Url.Contains(translate(location));
                if (!response)
                    continue;

                waitForPeriod(sleepPeriods[retries]);
                retries++;
            }

            return !response;
        }

        /// <summary>
        /// Selects the option from DDL.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="element">The element.</param>
        /// <exception cref="NoSuchElementException"></exception>
        /// <exception cref="InvalidSelectorException"></exception>
        public void SelectOptionFromDDL(string option, string element)
        {
            var selectedElement = translate(element);
            var seelctedOption = translate(option);

            var select = new SelectListSe(Driver, Driver.SearchForElement(selectedElement));
            if (select == null)
                throw new NoSuchElementException($"{selectedElement} is not available on the page");

            if (select.Options.ToList().Exists(o => o.Text.Equals(seelctedOption)))
                select.Options.ToList().Find(o => o.Text.Equals(seelctedOption)).Click();
            else
                throw new InvalidSelectorException($"Unable to find option {seelctedOption} within the select list {selectedElement}");
        }

        /// <summary>
        /// Checks the item is not visible.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool CheckItemIsNotVisible(string item)
        {
            bool response = false;

            try
            {
                Driver.SearchForElement(item);
            }
            catch (Exception)
            {
                // Expecting exception
                response = true;
            }

            return response;
        }

        /// <summary>
        /// Changes the colourwheel colour.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="colour">The colour.</param>
        public void ChangeColourwheelColour(int x, int y, string colour)
        {
            try
            {
                var rectObject = Driver.FindElements(By.XPath("//*[name()='svg']/*[name()='rect']"));
                if (rectObject != null &&
                    rectObject.Count == 3)
                {
                    Actions builder = new Actions(Driver);
                    IAction dragAndDrop = builder.ClickAndHold(rectObject[2])
                        .MoveByOffset(x, y)
                        .Release()
                        .Build();
                    dragAndDrop.Perform();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Stops the driver.
        /// </summary>
        private void Stop()
        {
            if (Driver != null)
            {
                // Close down and clear references
                Driver.Dispose();
                Driver = null;

                try
                {
                    // Remove straggling processes
                    switch (drivertype)
                    {
                        case DriverType.CHROME:
                            foreach (Process proc in Process.GetProcessesByName(Resource.CHROME))
                            {
                                proc.Kill();
                            }
                            break;

                        case DriverType.FIREFOX:
                            foreach (Process proc in Process.GetProcessesByName(Resource.FIREFOX))
                            {
                                proc.Kill();
                            }
                            break;

                        case DriverType.IE:
                            foreach (Process proc in Process.GetProcessesByName(Resource.IE))
                            {
                                proc.Kill();
                            }
                            break;

                    }
                }
                catch (Exception)
                {
                    // Ignore Exception 
                }
            }
        }

        /// <summary>
        /// Finds the main window.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private IWebElement FindMainWindow(string name)
        {
            var mainWindowStrategy = By.XPath($"/*[@AutomationId='{name}']");
            LastWindow = this.Driver.FindElement(mainWindowStrategy);

            return LastWindow;
        }

        /// <summary>
        /// Determines whether [is driver running].
        /// </summary>
        /// <returns></returns>
        private bool isDriverRunning()
        {
            bool response = false;

            try
            {
                if (Driver != null)
                {
                    // Remove straggling processes
                    switch (drivertype)
                    {
                        case DriverType.CHROME:
                            foreach (Process proc in Process.GetProcessesByName(Resource.CHROME))
                            {
                                response = true;
                            }
                            break;

                        case DriverType.FIREFOX:
                            foreach (Process proc in Process.GetProcessesByName(Resource.FIREFOX))
                            {
                                response = true;
                            }
                            break;

                        case DriverType.IE:
                            foreach (Process proc in Process.GetProcessesByName(Resource.IE))
                            {
                                response = true;
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
                // Ignore Exception 
            }

            return response;
        }

        /// <summary>
        /// Typeofs the path used.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private DriverType typeofPathUsed(string path)
        {
            return path.Contains(Resource.CHROME)
                ? DriverType.CHROME
                : path.Contains(Resource.FIREFOX)
                    ? DriverType.FIREFOX
                    : DriverType.IE;
        }

        /// <summary>
        /// Waits for period.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        private void waitForPeriod(int seconds)
        {
            System.Threading.Thread.Sleep(seconds * 1000);
        }

        /// <summary>
        /// Translates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private string translate(string value)
        {
            string response = value.Replace("{", "").Replace("}", "");

            if (value.Contains("{") &&
                dataList != null &&
                dataList.Count > 0)
            {
                response = dataList.Exists(e => e.Key.Equals(response, StringComparison.CurrentCultureIgnoreCase))
                    ? dataList.First(e => e.Key.Equals(response, StringComparison.CurrentCultureIgnoreCase)).Value
                    : response;
            }

            return response;
        }

        /// <summary>
        /// Waits for visibility before click.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="field">The field.</param>
        /// <exception cref="Exception"></exception>
        private void waitForVisibilityBeforeClick(string name, IWebElement field)
        {
            LogUtils.LogStart(() => field);

            int[] sleepPeriods = new int[] { 1, 2, 5 };
            int retries = 0;

            while (retries < 3 && !field.Displayed)
            {
                if (field.Displayed)
                {
                    field.Click();
                    continue;
                }

                waitForPeriod(sleepPeriods[retries]);
                retries++;
            }

            if (!field.Displayed)
                throw new Exception($"Tried searching/clicking for {name} with no success after 3 retries");
        }
        #endregion
    }
}
