using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestStack.White.ScreenObjects;
using TestStack.White.UIItems.WindowItems;

namespace Indigo.WhiteIntegration
{
    public class MainScreen 
        : AppScreen
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainScreen"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="screenRepository">The screen repository.</param>
        public MainScreen(Window window, ScreenRepository screenRepository) 
            : base(window, screenRepository)
        {
        }
        #endregion
    }
}
