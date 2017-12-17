using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;
namespace OpenQA.Selenium.ElementTypes
{
    public class TableRowSeCollection : BaseSeCollection<TableHeadSe>
    {
        public TableRowSeCollection()
        {
        }

        public TableRowSeCollection(IWebDriver webDriver)
            : base(webDriver)
        {
        }

        public TableRowSeCollection(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public TableRowSeCollection(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public TableRowSeCollection(IWebDriver webDriver, By by, Func<IWebElement, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public TableRowSeCollection(IWebElement webElement, By by, Func<IWebElement, bool> predicate)
            : base(webElement, by, predicate)
        {
        }
    }
}