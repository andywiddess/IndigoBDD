using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.ElementTypes;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

namespace Indigo.SeleniumIntegration
{
    /// <summary>
    /// Driver extensions 
    /// </summary>
    public static class DriverExtensions
    {
        /// <summary>
        /// Searches for element.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="Exception">No element found with id, name or css selector:  + key</exception>
        public static IWebElement SearchForElement(this IWebDriver driver, string key)
        {
            IWebElement element = null;
            string xpath = string.Empty;

            driver.WaitForPageToLoad();

            // check for ID first
            element = (driver.findItemById(key) ??
                        driver.findItemByName(key) ??
                        driver.findItemByClass(key)) ??
                        driver.findItemWithText("a", key) ??
                        driver.findItemWithValue("a", key) ??
                        driver.FindElement(By.CssSelector(key));
            if (element == null)
                throw new Exception($"Button with the Id,Class or text containing {key} cannot be located.");

            return element;
        }

        /// <summary>
        /// Searches for button with text.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static IWebElement SearchForButton(this IWebDriver driver, string key)
        {
            IWebElement element = null;
            string xpath = string.Empty;

            driver.WaitForPageToLoad();

            // check for ID first
            element = (driver.findItemById(key) ??
                        driver.findItemByName(key) ??
                        driver.findItemByClass(key)) ??
                        driver.findItemWithText("button", key) ??
                        driver.findItemWithValue("button", key) ??
                        driver.findItemWithValue("input", key) ??
                        driver.findItemWithText("a", key) ??
                        driver.findItemWithValue("a", key) ??
                        driver.FindElement(By.CssSelector(key));
            if (element == null)
                throw new Exception($"Button with the Id,Class or text containing {key} cannot be located.");

            return element;
        }

        /// <summary>
        /// Sets the value of text box by title.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="title">The title.</param>
        /// <param name="value">The value.</param>
        public static void SetValueOfTextBoxByTitle(this IWebDriver driver, string title, string value)
        {
            var elem = driver.FindElement(By.CssSelector("[title='" + title + "']"));
            elem.Clear();
            elem.SendKeys(value);
        }

        /// <summary>
        /// Clicks the selector.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="selector">The selector.</param>
        public static void ClickSelector(this IWebDriver driver, string selector)
        {
            var elem = driver.FindElement(By.CssSelector(selector));
            driver.FocusOnElement(elem);
            elem.Click();
        }

        /// <summary>
        /// Saves the and close modal.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="closeCssSelector">The close CSS selector.</param>
        public static void SaveAndCloseModal(this IWebDriver driver, string closeCssSelector)
        {
            driver.FindElement(By.CssSelector(closeCssSelector)).Click();
            Thread.Sleep(2000);
            driver.SwitchTo().DefaultContent();
        }

        /// <summary>
        /// Focuses the on element.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="elem">The elem.</param>
        public static void FocusOnElement(this IWebDriver driver, IWebElement elem)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("arguments[0].scrollIntoView();", elem);
        }

        /// <summary>
        /// Resizes to full screen.
        /// </summary>
        /// <param name="driver">The driver.</param>
        public static void ResizeToFullScreen(this IWebDriver driver)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("window.resizeTo(screen.width,screen.height);window.x=0;window.y=0");
        }

        /// <summary>
        /// Takes the screenshot.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="name">The name.</param>
        public static void TakeScreenshot(this IWebDriver driver, string name)
        {
            try
            {
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(name, ScreenshotImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Determines whether this instance [can find text] the specified text.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static bool CanFindText(this IWebDriver driver, string text)
        {
            string xpathToFind = String.Format("//*[contains(text(),\"{0}\")]", text);
            try
            {
                driver.FindElement(By.XPath(xpathToFind));
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

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

        /*
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
*/
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

        /*
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
        */

        /// <summary>
        /// Gets the type of the attribute as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        public static T GetAttributeAsType<T>(this IWebElement element, string attributeName)
        {
            string value = element.GetAttribute(attributeName) ?? string.Empty;
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
        }

        /// <summary>
        /// Sets the attribute.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentException">
        /// element;Element must wrap a web driver
        /// or
        /// element;Element must wrap a web driver that supports javascript execution
        /// </exception>
        public static void SetAttribute(this IWebElement element, string attributeName, string value)
        {
            IWrapsDriver wrappedElement = element as IWrapsDriver;
            if (wrappedElement == null)
                throw new ArgumentException("element", "Element must wrap a web driver");

            IWebDriver driver = wrappedElement.WrappedDriver;
            IJavaScriptExecutor javascript = driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("element", "Element must wrap a web driver that supports javascript execution");

            javascript.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2])", element, attributeName, value);
        }

        /// <summary>
        /// Waits for page to load.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <exception cref="ArgumentException">driver;Driver must support javascript execution</exception>
        public static void WaitForPageToLoad(this IWebDriver driver)
        {
            TimeSpan timeout = new TimeSpan(0, 0, 30);
            WebDriverWait wait = new WebDriverWait(driver, timeout);

            Thread.Sleep(500);

            IJavaScriptExecutor javascript = driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("driver", "Driver must support javascript execution");

            wait.Until((d) =>
            {
                try
                {
                    string readyState = javascript.ExecuteScript(
                    "if (document.readyState) return document.readyState;").ToString();
                    return readyState.ToLower() == "complete";
                }
                catch (InvalidOperationException e)
                {
                    //Window is no longer available
                    return e.Message.ToLower().Contains("unable to get browser");
                }
                catch (WebDriverException e)
                {
                    //Browser is no longer available
                    return e.Message.ToLower().Contains("unable to connect");
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Searches for grid filter elements in column x.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public static IWebElement SearchForGridFilterElement(this IWebDriver driver, int column)
        {
            IWebElement element = null;
            string xpath = string.Empty;

            try
            {
                element = driver.FindElement(By.XPath($"(.//input[contains(@class, \"ui-grid-filter-input-0\")])[{column}]"));
            }
            catch (Exception)
            {
                // Ignore
            }

            return element;
        }

        #region Private Methods

        /// <summary>
        /// Finds the item containing text.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private static IWebElement findItemContainingText(this IWebDriver driver, string type, string text)
        {
            IWebElement element = null;
            string xpath = string.Empty;

            try
            {
                element = driver.FindElement(By.XPath($"//{type}[contains(text(),\"{text}\"]"));
            }
            catch (Exception)
            {
                // Ignore
            }

            return element;
        }

        /// <summary>
        /// Finds the item with text.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private static IWebElement findItemWithText(this IWebDriver driver, string type, string text)
        {
            IWebElement element = null;
            string xpath = string.Empty;

            try
            {
                element = driver.FindElement(By.XPath($"//{type}[text() = '{text}']"));
            }
            catch (Exception)
            {
                // Ignore
            }

            return element;
        }

        /// <summary>
        /// Finds the item with value.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private static IWebElement findItemWithValue(this IWebDriver driver, string type, string text)
        {
            IWebElement element = null;
            string xpath = string.Empty;

            try
            {
                element = driver.FindElement(By.XPath($"//{type}[@value = '{text}']"));
            }
            catch (Exception)
            {
                // Ignore
            }

            return element;
        }

        /// <summary>
        /// Finds the item by identifier.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private static IWebElement findItemById(this IWebDriver driver, string id)
        {
            IList<IWebElement> element = null;
            string xpath = string.Empty;

            try
            {
                element = driver.FindElements(By.Id(id));
            }
            catch (Exception)
            {
                // Ignore
            }

            return element != null && element.Count > 0 ? element[0] : null;
        }

        /// <summary>
        /// Finds the name of the item by.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static IWebElement findItemByName(this IWebDriver driver, string name)
        {
            IList<IWebElement> element = null;
            string xpath = string.Empty;

            try
            {
                element = driver.FindElements(By.Name(name));
            }
            catch (Exception)
            {
                // Ignore
            }

            return element != null && element.Count > 0 ? element[0] : null;
        }

        /// <summary>
        /// Finds the item by class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        private static IWebElement findItemByClass(this IWebDriver driver, string className)
        {
            IWebElement element = null;
            string xpath = string.Empty;

            try
            {
                element = driver.FindElement(By.ClassName(className));
            }
            catch (Exception)
            {
                // Ignore
            }

            return element;
        }
        #endregion
    }
}
