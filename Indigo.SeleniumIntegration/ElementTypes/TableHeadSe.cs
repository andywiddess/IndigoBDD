using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class TableHeadSe : TableElements
    {
        private static string columnTag = "th";

        public TableHeadSe(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
            InitializeRowsandCells(columnTag);
        }
       
        public TableHeadSe(IWebElement webElement, By by)
            : base(webElement, by)
        {
            InitializeRowsandCells(columnTag);
        }

        public TableHeadSe(IWebDriver webDriver, By by, Func<ElementSe, bool> predicate)
            : base(webDriver, by, predicate)
        {
            InitializeRowsandCells(columnTag);
        }

        public TableHeadSe(IWebElement webElement, By by, Func<ElementSe, bool> predicate)
            : base(webElement, by, predicate)
        {
            InitializeRowsandCells(columnTag);
        }

        public TableHeadSe(IWebElement body)
            : base(body, columnTag)
        {
        }

        public override string ElementTag
        {
            get { return "thead"; }
        }
    }
}
