using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class TableCellSeCollection : BaseSeCollection<TableCellSe>
    {
        public TableCellSeCollection()
        {
        }

        public TableCellSeCollection(IWebDriver webDriver)
            : base(webDriver)
        {
        }

        public TableCellSeCollection(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public TableCellSeCollection(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public TableCellSeCollection(IWebDriver webDriver, By by, Func<IWebElement, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public TableCellSeCollection(IWebElement webElement, By by, Func<IWebElement, bool> predicate)
            : base(webElement, by, predicate)
        {
        }
    }
}