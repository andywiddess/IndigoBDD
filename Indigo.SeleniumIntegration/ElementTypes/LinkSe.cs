using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class LinkSe : ElementSe
    {
        public LinkSe(IWebElement webElement)
            : base(webElement)
        {
        }

        public LinkSe(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public LinkSe(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public LinkSe(IWebDriver webDriver, By by, Func<ElementSe, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public LinkSe(IWebElement webElement, By by, Func<ElementSe, bool> predicate)
            : base(webElement, by, predicate)
        {
        }

        public override string ElementTag
        {
            get { return "a"; }
        }

        public string Url
        {
            get
            {
                try
                {
                    return WebElement.GetAttribute("href");
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get
            {
                try
                {
                    return WebElement.GetAttribute("title");
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}