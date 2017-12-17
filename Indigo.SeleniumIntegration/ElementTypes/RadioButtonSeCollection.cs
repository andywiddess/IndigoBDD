using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class RadioButtonSeCollection : BaseSeCollection<RadioButtonSe>
    {
        public RadioButtonSeCollection()
        {
        }

        public RadioButtonSeCollection(IWebDriver webDriver)
            : base(webDriver)
        {
        }

        public RadioButtonSeCollection(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public RadioButtonSeCollection(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public RadioButtonSeCollection(IWebDriver webDriver, By by, Func<IWebElement, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public RadioButtonSeCollection(IWebElement webElement, By by, Func<IWebElement, bool> predicate)
            : base(webElement, by, predicate)
        {
        }
    }
}