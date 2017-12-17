using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class TableCellSe : ElementSe
    {
        public TableCellSe(IWebElement webElement)
            : base(webElement)
        {
        }

        public TableCellSe(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public TableCellSe(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public TableCellSe(IWebDriver webDriver, By by, Func<ElementSe, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public TableCellSe(IWebElement webElement, By by, Func<ElementSe, bool> predicate)
            : base(webElement, by, predicate)
        {
        }

        public override string ElementTag
        {
            get { return "td"; }
        }
    }
}
