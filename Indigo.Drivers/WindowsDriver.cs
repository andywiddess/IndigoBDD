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
using TestStack.White.UIItems.Finders;

using EA.AutomatedTesting.Indigo.Contracts;
using EA.AutomatedTesting.Indigo.Utilities;
using EA.AutomatedTesting.Indigo.WhiteIntegration;
using EA.AutomatedTesting.Indigo.WhiteIntegration.Configuration;
using EA.AutomatedTesting.Indigo.WhiteIntegration.Implementation;
using Sepura.Apps.ElectronicCPS.CrossCutting.Utilities.CheckExpression;

namespace EA.AutomatedTesting.Indigo.Drivers
{
    public class WindowsDriver
	    : IDriver
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
		/// Gets or sets a value indicating whether this <see cref="WindowsDriver"/> is initialised.
		/// </summary>
		/// <value>
		///   <c>true</c> if initialised; otherwise, <c>false</c>.
		/// </value>
		public bool Initialised { get; private set; }

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
		/// Gets the configuration.
		/// </summary>
		/// <value>
		/// The configuration.
		/// </value>
		public IApplication Configuration { get; private set; }

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
		/// Initializes a new instance of the <see cref="WindowsDriver"/> class.
		/// </summary>
		/// <param name="typ">The typ.</param>
		public WindowsDriver()
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
			LogUtils.LogStart(() => name, () => path, () => outputPath, () => form);

			if (!string.IsNullOrEmpty(name) &&
			    !string.IsNullOrEmpty(path) &&
			    !string.IsNullOrEmpty(outputPath))
			{
				if (!File.Exists(path))
					throw new Exception($"{path} does not exist and cannot be called");

				if (!Directory.Exists(outputPath))
					Directory.CreateDirectory(outputPath);

				this.Framework = WindowsFramework.WinForms;
				this.outputPath = outputPath;
				this.applicationPath = path;
				this.initialWindowName = form;

				Directory.CreateDirectory(this.outputPath);
			}
			else
			{
				throw new Exception(string.Format("The given statement is incorrectly formattted"));
			}
		}

		/// <summary>
		/// Starts the application.
		/// </summary>
		public void StartApplication()
		{
			mainWindow = SetMainWindow(Framework, applicationPath, initialWindowName);
			Initialised = (mainWindow != null);
		}

		/// <summary>
		/// Closes the application.
		/// </summary>
		public void CloseApplication()
		{
			windowsToClose.ForEach(w => w.Close());

			//mainWindow?.Dispose();
			Application?.Dispose();
		}

		/// <summary>
		/// Finds and Clicks a specific button by name
		/// </summary>
		/// <param name="buttonName">Name of the button.</param>
		public void ClickButton(string buttonName)
		{
			LogUtils.LogStart(() => buttonName);

			MainWindow.Focus();
			Check.That(MainWindow.IsCurrentlyActive, Is.Not.Null);

			Utils.FindButton(MainWindow, buttonName).Click();

			Check.That(MainWindow.IsCurrentlyActive, Is.Not.Null);
		}

		/// <summary>
		/// Selects the menu option.
		/// </summary>
		/// <param name="menu">The menu.</param>
		/// <param name="option">The option.</param>
		public void SelectMenuOption(string menu, string option)
		{
			LogUtils.LogStart(() => menu, () => option);

			var bar = MainWindow.GetMultiple();

			var menuOption = MainWindow.MenuBar.MenuItem(menu, option);
			Check.That(menuOption, Is.Not.Null);

			menuOption.Click();
		}

		/// <summary>
		/// Selects the menu option.
		/// </summary>
		/// <param name="menu">The menu.</param>
		/// <param name="option1">The option1.</param>
		/// <param name="option2">The option2.</param>
		public void SelectMenuOption(string menu, string option1, string option2)
		{
			LogUtils.LogStart(() => menu, () => option1, () => option2);

			var selectedMenu = MainWindow.MenuBar.MenuItem(menu);
			Check.That(selectedMenu, Is.Not.Null);
			var selectedOption = selectedMenu.SubMenu(SearchCriteria.ByText(option1)).SubMenu(option2);
			Check.That(selectedOption, Is.Not.Null);

			selectedOption.Click();
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
			LogUtils.LogStart(() => reference, () => text);

			var panelField = Utils.FindPanelAndText(MainWindow, reference, text);
			if (panelField != null)
			{
				if (panelField.Items != null &&
				    panelField.Items.Count > 0)
				{
					panelField.Items.ForEach(item =>
					{
						if (item.Name.Equals(currentValue))
						{
							item.SetValue(text);
						}
					});
				}

				//panelField = Utils.FindPanelAndText(GivenSteps.Test.MainWindow, reference, text);
				//if (!panelField.Name.Equals(text))
				//	throw new Exception(string.Format("Unable to find the value {0} in panel {1}", text, reference));
			}
			else
			{
				throw new Exception($"Unable to find panel {reference}");
			}
		}

		/// <summary>
		/// Fills the out panel value.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="automationId">The automation identifier.</param>
		/// <exception cref="Exception"></exception>
		public void FillOutPanelValue(string text, string automationId)
		{
			LogUtils.LogStart(() => automationId, () => text);

			var panelField = Utils.FindPanelAndText(MainWindow, automationId, text);
			if (panelField != null)
			{
				if (panelField.Items != null &&
				    panelField.Items.Count == 1)
				{
					panelField.Items[0].SetValue(text);
				}

				//panelField = Utils.FindPanelAndText(GivenSteps.Test.MainWindow, reference, text);
				//if (!panelField.Name.Equals(text))
				//	throw new Exception(string.Format("Unable to find the value {0} in panel {1}", text, reference));
			}
			else
			{
				throw new Exception($"Unable to find panel {automationId}");
			}
		}

		/// <summary>
		/// Takes a screenshot
		/// </summary>
		/// <param name="filename"></param>
		public void Screenshot(string filename)
		{
			LogUtils.LogStart(() => filename);

			TakeScreenshot(filename);
		}

		/// <summary>
		/// Sees the titlebar.
		/// </summary>
		/// <param name="text">The text.</param>
		public void SeeTitlebar(string text)
		{
			LogUtils.LogStart(() => text);

			TitleBar bar = Utils.FindTitle(MainWindow, text);
			Check.That(bar, Is.Not.Null);
		}

		/// <summary>
		/// Confirms the dialog displayed.
		/// </summary>
		/// <param name="text">The text.</param>
		public void ConfirmWindowDisplayed(string text)
		{
			LogUtils.LogStart(() => text);

			SetCurrentWindow(text);

			//TestStack.White.UIItems.Finders.SearchCriteria criteria = Utils.FindItem(ControlType.TitleBar); // Utils.FindItem(text);

			//Window modal = GivenSteps.Test.MainWindow.ModalWindow(Utils.FindItem(text));
			//Check.That(modal, Is.Not.Null);
		}

		/// <summary>
		/// Confirms the dialog displayed.
		/// </summary>
		/// <param name="text">The text.</param>
		public void ConfirmDialogDisplayed(string text)
		{
			LogUtils.LogStart(() => text);

			SetCurrentWindow(text);

			//TestStack.White.UIItems.Finders.SearchCriteria criteria = Utils.FindItem(ControlType.TitleBar); // Utils.FindItem(text);

			//Window modal = GivenSteps.Test.MainWindow.ModalWindow(Utils.FindItem(text));
			//Check.That(modal, Is.Not.Null);
		}

		/// <summary>
		/// Checks the field has value.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="label">The label.</param>
		/// <exception cref="Exception"></exception>
		public void CheckFieldHasValue(string text, string label)
		{
			LogUtils.LogStart(() => label, () => text);

			var labelField = Utils.FindLabelAndText(MainWindow, label, text);
			if (!labelField.Name.Equals(text))
				throw new Exception(string.Format("Unable to find the value {0} in label {1}", text, label));
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Sets the current window.
		/// </summary>
		/// <param name="windowName">Name of the window.</param>
		private void SetCurrentWindow(string windowName)
		{
			MainWindow = Configuration.GetMainWindow(Application, windowName);
			MainScreen = Configuration.GetMainScreen(Repository, windowName);
			//MainWindow = Application.GetWindow(windowName);
		}

		/// <summary>
		/// Bases the teardown.
		/// </summary>
		private void BaseTeardown()
		{
			mainWindow.Dispose();
		}

		/// <summary>
		/// Takes the screenshot.
		/// </summary>
		/// <param name="screenshotName">Name of the screenshot.</param>
		/// <returns></returns>
		private string TakeScreenshot(string screenshotName)
		{
			var imagename = screenshotName + ".png";
			var imagePath = Path.Combine(outputPath, imagename);
			try
			{
				Desktop.CaptureScreenshot().Save(imagePath, ImageFormat.Png);
				Trace.WriteLine($"Screenshot taken: {imagePath}");
			}
			catch (Exception)
			{
				Trace.TraceError($"Failed to save screenshot to directory: {outputPath}, filename: {imagePath}");
			}
			return imagePath;
		}

		/// <summary>
		/// Sets the main window.
		/// </summary>
		/// <param name="framework">The framework.</param>
		/// <returns></returns>
		private IDisposable SetMainWindow(WindowsFramework framework)
		{
			try
			{
				Keyboard = Keyboard.Instance;
				Configuration = ConfigurationFactory.Create(framework);
				Application = Configuration.LaunchApplication(applicationPath);
				Repository = new ScreenRepository(Application);
				MainWindow = Configuration.GetMainWindow(Application, initialWindowName);
				MainScreen = Configuration.GetMainScreen(Repository, initialWindowName);

				return new ShutdownApplicationDisposable(this);
			}
			catch (Exception e)
			{
				Application?.Close();
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
		private IDisposable SetMainWindow(WindowsFramework framework, string applicationPath, string mainWindowName)
		{
			try
			{
				Keyboard = Keyboard.Instance;
				Configuration = ConfigurationFactory.Create(framework);
				Application = Configuration.LaunchApplication(applicationPath);
				Repository = new ScreenRepository(Application);
				MainWindow = Configuration.GetMainWindow(Application, mainWindowName);
				MainScreen = Configuration.GetMainScreen(Repository, mainWindowName);

				return new ShutdownApplicationDisposable(this);
			}
			catch (Exception e)
			{
				Application?.Close();
				throw;
			}
		}

		/// <summary>
		/// Starts the scenario.
		/// </summary>
		/// <param name="scenarioButtonId">The scenario button identifier.</param>
		/// <param name="windowTitle">The window title.</param>
		/// <returns></returns>
		private Window StartScenario(string scenarioButtonId, string windowTitle)
		{
			MainWindow.Get<Button>(scenarioButtonId).Click();
			var window = MainWindow.ModalWindow(windowTitle);
			windowsToClose.Add(window);
			return window;
		}

		/// <summary>
		/// Restarts this instance.
		/// </summary>
		private void Restart()
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
		private string FrameworkId
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
