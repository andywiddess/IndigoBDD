using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class ListItemSe : ElementSe
    {
        public ListItemSe(IWebElement webElement)
            : base(webElement)
        {
        }

        public ListItemSe(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public ListItemSe(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public ListItemSe(IWebDriver webDriver, By by, Func<ElementSe, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public ListItemSe(IWebElement webElement, By by, Func<ElementSe, bool> predicate)
            : base(webElement, by, predicate)
        {
        }

        public override string ElementTag
        {
            get { return "li"; }
        }
    }
}