using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;

namespace OpenQA.Selenium.ElementTypes
{
    public class CheckBoxSe : ElementSe
    {
        public CheckBoxSe(IWebElement webElement)
            : base(webElement)
        {
        }

        public CheckBoxSe(IWebDriver webDriver, By by)
            : base(webDriver, by)
        {
        }

        public CheckBoxSe(IWebElement webElement, By by)
            : base(webElement, by)
        {
        }

        public CheckBoxSe(IWebDriver webDriver, By by, Func<ElementSe, bool> predicate)
            : base(webDriver, by, predicate)
        {
        }

        public CheckBoxSe(IWebElement webElement, By by, Func<ElementSe, bool> predicate)
            : base(webElement, by, predicate)
        {
        }

        public override string ElementTag
        {
            get { return "input"; }
        }

        public bool IsChecked
        {
            get
            {
                try
                {
                    if (WebElement.Selected)
                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public void SetChecked(bool theValue)
        {
            if (theValue && !IsChecked)
            {
                WebElement.Click();
            }
            else if (!theValue && IsChecked)
            {
                WebElement.Click();
            }
        }
    }
}
