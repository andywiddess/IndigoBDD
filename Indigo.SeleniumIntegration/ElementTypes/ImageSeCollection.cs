using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class ImageSeCollection : BaseSeCollection<ImageSe>
    {
        public ImageSeCollection()
        {
        }

        public ImageSeCollection(IWebDriver webDriver)
            : base(webDriver)
        {
        }

        public ImageSeCollection(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public ImageSeCollection(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public ImageSeCollection(IWebDriver webDriver, By by, Func<IWebElement, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public ImageSeCollection(IWebElement webElement, By by, Func<IWebElement, bool> predicate)
            : base(webElement, by, predicate)
        {
        }
    }
}