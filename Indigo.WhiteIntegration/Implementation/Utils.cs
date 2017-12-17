using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

using OpenQA.Selenium;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.InputDevices;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

using Indigo.WhiteIntegration.Configuration;
using Indigo.WhiteIntegration.Implementation;

namespace Indigo.WhiteIntegration.Implementation
{
	public static class Utils
	{
		/// <summary>
		/// Finds the title.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="criteria">The criteria.</param>
		/// <returns></returns>
		public static TestStack.White.UIItems.WindowItems.TitleBar FindTitle(Window window, string criteria)
		{
			TestStack.White.UIItems.Finders.SearchCriteria query = null;
			TestStack.White.UIItems.WindowItems.TitleBar response = null;

			// Find Title bar using the control type
			if (window.TitleBar == null ||
			    !window.TitleBar.Title.Equals(criteria))
				throw new Exception($"TitleBar {criteria} cannot be found inside this window");

			return window.TitleBar;
		}

		/// <summary>
		/// Finds the button.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="criteria">The criteria.</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static TestStack.White.UIItems.Button FindButton(Window window, string criteria)
		{
			TestStack.White.UIItems.Button response = null;
			TestStack.White.UIItems.Finders.SearchCriteria query = null;

			// Find button by Automation Id
			query = FindItemByAutomation(criteria);
			if (query != null)
			{
				try
				{
					response = window.Get<Button>(query);
				}
				catch
				{
				}
			}

			// Find button by Name
			if (response == null)
			{
				query = FindItemByName(criteria);
				if (query != null)
				{
					try
					{
						response = window.Get<Button>(query);
					}
					catch
					{

					}
				}
			}

			// Find button by Class
			if (response == null)
			{
				query = FindItemByClass(criteria);
				if (query != null)
				{
					try
					{
						response = window.Get<Button>(query);
					}
					catch
					{
					}
				}
			}

			if (response == null)
				throw new Exception($"Button with the criteria {criteria} cannot be found");

			return response;
		}

		/// <summary>
		/// Finds the field and text.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="field">The field.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static TestStack.White.UIItems.Label FindLabelAndText(Window window, string field, string value)
		{
			TestStack.White.UIItems.Label response = null;
			TestStack.White.UIItems.Finders.SearchCriteria query = null;

			// Find button by Automation Id
			query = FindItemByAutomation(field);
			if (query != null)
			{
				try
				{
					response = window.Get<Label>(query);
				}
				catch
				{
				}
			}

			// Find button by Name
			if (response == null)
			{
				query = FindItemByName(field);
				if (query != null)
				{
					try
					{
						response = window.Get<Label>(query);
					}
					catch
					{

					}
				}
			}

			// Find button by Class
			if (response == null)
			{
				query = FindItemByClass(field);
				if (query != null)
				{
					try
					{
						response = window.Get<Label>(query);
					}
					catch
					{
					}
				}
			}

			if (response == null)
				throw new Exception($"TextBox with the criteria {field} cannot be found");

			return response;
		}

		/// <summary>
		/// Finds the panel and text.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="field">The field.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static TestStack.White.UIItems.Panel FindPanelAndText(Window window, string field, string value)
		{
			TestStack.White.UIItems.Panel response = null;
			TestStack.White.UIItems.Finders.SearchCriteria query = null;

			// Find button by Automation Id
			query = FindItemByAutomation(field);
			if (query != null)
			{
				try
				{
					response = window.Get<Panel>(query);
				}
				catch
				{
				}
			}

			// Find button by Name
			if (response == null)
			{
				query = FindItemByName(field);
				if (query != null)
				{
					try
					{
						response = window.Get<Panel>(query);
					}
					catch
					{

					}
				}
			}

			// Find button by Class
			if (response == null)
			{
				query = FindItemByClass(field);
				if (query != null)
				{
					try
					{
						response = window.Get<Panel>(query);
					}
					catch
					{
					}
				}
			}

			if (response == null)
				throw new Exception($"Panel with the criteria {field} cannot be found");

			return response;
		}

		/// <summary>
		/// Finds the name of the item by.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private static TestStack.White.UIItems.Finders.SearchCriteria FindItemByName(string name)
		{
			return SearchCriteria.ByText(name);
		}

		/// <summary>
		/// Finds the item by class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private static TestStack.White.UIItems.Finders.SearchCriteria FindItemByClass(string name)
		{
			return SearchCriteria.ByClassName(name);
		}

		/// <summary>
		/// Finds the item by automation.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private static TestStack.White.UIItems.Finders.SearchCriteria FindItemByAutomation(string name)
		{
			return SearchCriteria.ByAutomationId(name);
		}
	}
}
