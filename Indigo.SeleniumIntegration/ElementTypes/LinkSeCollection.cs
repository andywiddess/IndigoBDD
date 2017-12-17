using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class LinkSeCollection : BaseSeCollection<LinkSe>
    {
        public LinkSeCollection()
        {
        }

        public LinkSeCollection(IWebDriver webDriver)
            : base(webDriver)
        {
        }

        public LinkSeCollection(IWebDriver webDriver, By by)
            :base(webDriver, by)
        { 
        }

        public LinkSeCollection(IWebElement webElement, By by)
            :base(webElement, by)
        {
        }

        public LinkSeCollection(IWebDriver webDriver, By by, Func<IWebElement, bool> predicate)
            :base(webDriver, by, predicate)
        {
        }

        public LinkSeCollection(IWebElement webElement, By by, Func<IWebElement, bool> predicate)
            :base(webElement, by, predicate)
        {
        }
    }
}