using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class ImageSe : ElementSe
    {
        public ImageSe(IWebElement webElement)
            : base(webElement)
        {
        }

        public ImageSe(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public ImageSe(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public ImageSe(IWebDriver webDriver, By by, Func<ElementSe, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public ImageSe(IWebElement webElement, By by, Func<ElementSe, bool> predicate)
            : base(webElement, by, predicate)
        {
        }

        public override string ElementTag
        {
            get { return "img"; }
        }

        public string Alt
        {
            get
            {
                try
                {
                    return WebElement.GetAttribute("alt");
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public string Source
        {
            get
            {
                try
                {
                    return WebElement.GetAttribute("src");
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}