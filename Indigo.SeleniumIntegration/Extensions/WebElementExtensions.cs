using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.ElementTypes;

namespace OpenQA.Selenium
{
    public static class WebElementExtensions
    {
        /// <summary>
        /// Finds the element.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IWebElement FindElement(this IWebElement driver, By by, Func<ElementSe, bool> predicate)
        {
            try
            {
                return new ElementSeCollection(driver, by).Where(predicate).First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the elements.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IEnumerable<IWebElement> FindElements(this IWebElement driver, By by, Func<ElementSe, bool> predicate)
        {
            try
            {
                return new ElementSeCollection(driver, by).Where(predicate);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the element.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IWebElement FindElement(this IWebDriver driver, By by, Func<ElementSe, bool> predicate)
        {
            try
            {
                return new ElementSeCollection(driver, by).Where(predicate).First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the elements.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IEnumerable<IWebElement> FindElements(this IWebDriver driver, By by, Func<ElementSe, bool> predicate)
        {
            try
            {
                return new ElementSeCollection(driver, by).Where(predicate);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
