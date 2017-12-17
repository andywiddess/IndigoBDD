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

namespace Indigo.CrossCutting.Utilities
{
    [Hooks]
    public class SeleniumHooks
    {
        [NBehave.Narrator.Framework.Hooks.BeforeFeature]
        public void SetupFeature()
        {
        }

        [NBehave.Narrator.Framework.Hooks.AfterFeature]
        public void TearDownFeature()
        {
            try
            {
                //AutomationInstance.Instance.StopBrowser();
            }
            catch (Exception)
            {

            }
        }

        [NBehave.Narrator.Framework.Hooks.BeforeScenario]
        public void SetupScenario()
        {
            System.Threading.Thread.Sleep(1500);
        }

        [NBehave.Narrator.Framework.Hooks.AfterScenario]
        public void TearDownScenario()
        {

        }
    }

    public static class ContextKeys
    {
        public const string Driver = "DRIVER";
        public const string BaseUrl = "BASE_URL";
    }
}
