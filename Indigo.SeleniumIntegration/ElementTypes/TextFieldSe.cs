using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class TextFieldSe : ElementSe
    {
        public TextFieldSe(IWebElement webElement)
            : base(webElement)
        {
        }

        public TextFieldSe(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public TextFieldSe(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public TextFieldSe(IWebDriver webDriver, By by, Func<ElementSe, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public TextFieldSe(IWebElement webElement, By by, Func<ElementSe, bool> predicate)
            : base(webElement, by, predicate)
        {
        }

        public override string ElementTag
        {
            get { return "input"; }
        }
    }
}