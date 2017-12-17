﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class DivSe : ElementSe
    {
        public DivSe(IWebElement webElement)
            : base(webElement)
        {
        }

        public DivSe(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public DivSe(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public DivSe(IWebDriver webDriver, By by, Func<ElementSe, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public DivSe(IWebElement webElement, By by, Func<ElementSe, bool> predicate)
            : base(webElement, by, predicate)
        {
        }

        public override string ElementTag
        {
            get { return "div"; }
        }
    }
}