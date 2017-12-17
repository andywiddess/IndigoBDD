using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EA.AutomatedTesting.Indigo.Contracts;
using EA.AutomatedTesting.Indigo.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

using EA.AutomatedTesting.Indigo.SeleniumIntegration;

namespace EA.AutomatedTesting.Indigo.Drivers
{
	/// <summary>
	/// Browser Driver
	/// </summary>
	/// <seealso cref="EA.AutomatedTesting.Indigo.Contracts.IDriver" />
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
		/// The application arguments
		/// </summary>
		private Arguments appArguments = null;

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

			if (!isDriverRunning())
			{
				switch (drivertype)
				{
					case DriverType.CHROME:
						ChromeOptions options = new ChromeOptions();
						string[] startingOptions = { "--start-maximized" };
						options.AddArguments(startingOptions.AsEnumerable());
						Driver = new ChromeDriver(options);
						Initialised = true;
						break;

					case DriverType.FIREFOX:
						Driver = new FirefoxDriver();
						break;

					case DriverType.IE:
						Driver = new InternetExplorerDriver(appArguments.DriverLocation);
						break;
				}
			}
		}

		/// <summary>
		/// Starts the application.
		/// </summary>
		public void StartApplication()
		{
			LogUtils.LogStart();

			var dc = new DesiredCapabilities();
			dc.SetCapability("app", $"{appArguments.ApplicationLocation}\\{appArguments.ApplicationName}.exe");
			dc.SetCapability("launchDelay", 4);
			this.Driver = new RemoteWebDriver(new Uri(BaseUrl), dc);

			ApplicationProcess = Process.GetProcessesByName(appArguments.ApplicationName)[0];
			Initialised = true;
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
		/// Finds and Clicks a specific button by name
		/// </summary>
		/// <param name="buttonName">Name of the button.</param>
		public void ClickButton(string buttonName)
		{
			LogUtils.LogStart(() => buttonName);

			IWebElement button = DriverExtensions.SearchForButtonWithText(Driver, buttonName);
			if (button != null)
			{
				button.Click();
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
			throw new NotImplementedException("SelectMenuOption");
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
			throw new NotImplementedException("FillOutPanelValue");
		}

		/// <summary>
		/// Takes a screenshot
		/// </summary>
		/// <param name="filename"></param>
		public void Screenshot(string filename)
		{
			throw new NotImplementedException("FillOutPanelValue");
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
			throw new NotImplementedException("FillOutPanelValue");
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
				Driver.Close();
				Driver = null;

				try
				{
					// Remove straggling processes
					switch (appArguments.Type)
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
					switch (appArguments.Type)
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
		#endregion
	}
}
