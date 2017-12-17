using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class TableSe : ElementSe
    {
        public TableSe(IWebElement webElement)
            : base(webElement)
        {
            InitializeHeadAndBody();
        }

        public TableSe(IWebElement webElement, By by)
            : base(webElement, by)
        {
            InitializeHeadAndBody();
        }

        public TableSe(IWebDriver webDriver, By by, Func<ElementSe, bool> predicate)
            : base(webDriver, by, predicate)
        {
            InitializeHeadAndBody();
        }

        public TableSe(IWebElement webElement, By by, Func<ElementSe, bool> predicate)
            : base(webElement, by, predicate)
        {
            InitializeHeadAndBody();
        }

        public TableSe(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
            InitializeHeadAndBody();
        }

        public override string ElementTag
        {
            get { return "table"; }
        }

        public TableHeadSe TableHead { get; set; }
        public TableBodySe TableBody { get; set; }

        private void InitializeHeadAndBody()
        {
            if (WebElement == null)
            {
                return;
            }

            if (WebElement.Text.Contains("thead"))
            {
                TableHead = new TableHeadSe(WebElement, By.TagName("thead"));
            }

            TableBody = new TableBodySe(WebElement, By.TagName("tbody"));
        }
    }
}

