using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestStack.White;
using TestStack.White.UIItems;

namespace Indigo.WhiteIntegration.Configuration
{
    public static class ConfigurationFactory
    {
        /// <summary>
        /// Creates the specified framework.
        /// </summary>
        /// <param name="framework">The framework.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">framework</exception>
        public static IApplication Create(WindowsFramework framework)
        {
            switch (framework)
            {
                case WindowsFramework.WinForms:
                    return new WinformsConfiguration(framework);

                case WindowsFramework.Win32:
                case WindowsFramework.Swt:
                //case Constants.WinRT:
                default:
                    throw new ArgumentOutOfRangeException("framework");
            }
        }
    }
}