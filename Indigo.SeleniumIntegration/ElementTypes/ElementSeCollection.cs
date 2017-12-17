using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class ElementSeCollection : BaseSeCollection<ElementSe>
    {
        public ElementSeCollection()
        {
        }

        public ElementSeCollection(IWebDriver webDriver)
            : base(webDriver)
        {
        }

        public ElementSeCollection(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public ElementSeCollection(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public ElementSeCollection(IWebDriver webDriver, By by, Func<IWebElement, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public ElementSeCollection(IWebElement webElement, By by, Func<IWebElement, bool> predicate)
            : base(webElement, by, predicate)
        {
        }
    }
}