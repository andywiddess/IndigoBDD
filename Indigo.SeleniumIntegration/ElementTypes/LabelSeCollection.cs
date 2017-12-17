using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class LabelSeCollection : BaseSeCollection<LabelSe>
    {
        public LabelSeCollection()
        {
        }

        public LabelSeCollection(IWebDriver webDriver)
            : base(webDriver)
        {
        }

        public LabelSeCollection(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public LabelSeCollection(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public LabelSeCollection(IWebDriver webDriver, By by, Func<IWebElement, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public LabelSeCollection(IWebElement webElement, By by, Func<IWebElement, bool> predicate)
            : base(webElement, by, predicate)
        {
        }
    }
}