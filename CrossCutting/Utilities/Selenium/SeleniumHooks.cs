using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using NBehave.Narrator.Framework;
using NBehave.Narrator.Framework.Hooks;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

using Indigo.CrossCutting.Utilities.Logging;

namespace Indigo.CrossCutting.Utilities.Selenium
{
    [Hooks]
    public class SeleniumHooks
    {
        [NBehave.Narrator.Framework.Hooks.BeforeFeature]
        public void SetupFeature()
        {
            Log.Debug("SetupFeature Started");
        }

        [NBehave.Narrator.Framework.Hooks.AfterFeature]
        public void TearDownFeature()
        {
            try
            {
                Log.Debug("TearDownFeature Started");
                //AutomationInstance.Instance.StopBrowser();
            }
            catch (Exception)
            {

            }
        }

        [NBehave.Narrator.Framework.Hooks.BeforeScenario]
        public void SetupScenario()
        {
            Log.Debug("SetupScenario Started");
            System.Threading.Thread.Sleep(1500);
        }

        [NBehave.Narrator.Framework.Hooks.AfterScenario]
        public void TearDownScenario()
        {
            Log.Debug("TearDownScenario Started");
        }
    }

    public static class ContextKeys
    {
        public const string Driver = "DRIVER";
        public const string BaseUrl = "BASE_URL";
    }
}
