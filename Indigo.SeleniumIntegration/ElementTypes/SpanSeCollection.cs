using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class SpanSeCollection : BaseSeCollection<SpanSe>
    {
       public SpanSeCollection()
        {
        }

        public SpanSeCollection(IWebDriver webDriver)
            : base(webDriver)
        {
        }

        public SpanSeCollection(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public SpanSeCollection(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public SpanSeCollection(IWebDriver webDriver, By by, Func<IWebElement, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public SpanSeCollection(IWebElement webElement, By by, Func<IWebElement, bool> predicate)
            : base(webElement, by, predicate)
        {
        }
    }
}