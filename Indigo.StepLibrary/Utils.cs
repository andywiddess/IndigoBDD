using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

using NBehave.Narrator.Framework;

using OpenQA.Selenium;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.InputDevices;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

using EA.Shared.CrossCutting.Framework.CheckExpression;
using EA.Shared.CrossCutting.Framework.Logging;

using EA.AutomatedTesting.Indigo.Contracts;
using EA.AutomatedTesting.Indigo.WhiteIntegration.Configuration;
using EA.AutomatedTesting.Indigo.WhiteIntegration.Implementation;

namespace EA.AutomatedTesting.Indigo.StepLibrary
{
#if NEW
    public static class Utils
    {
        /// <summary>
        /// Finds the item.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static TestStack.White.UIItems.Finders.SearchCriteria FindItem(string name)
        {
            TestStack.White.UIItems.Finders.SearchCriteria response = null;

            response = SearchCriteria.ByText(name);
            if (response == null)
            {
                response = SearchCriteria.ByAutomationId(name);
            }

            if (response == null)
            {
                response = SearchCriteria.ByText(name);
            }

            if (response == null)
            {
                response = SearchCriteria.ByClassName(name);
            }

            return response;
        }
        /// <summary>
        /// Finds the item.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static TestStack.White.UIItems.Finders.SearchCriteria FindItem(System.Windows.Automation.ControlType type)
        {
            return SearchCriteria.ByControlType(type);
        }
    }
#endif
}
