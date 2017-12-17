using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities.Selenium.Contracts
{
    public interface IGiven
    {
        /// <summary>
        /// Gets the browser.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        /// <param name="outputPath">The output path.</param>
        void GetBrowser(string name, string path, string outputPath);

        /// <summary>
        /// Get the radio manager instance
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="form">The form.</param>
        void GetApplication(string name, string path, string outputPath, string form);

        /// <summary>
        /// Assigns the feature details to the scenario being performed
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <param name="dataSet">The data set.</param>
        void MyApplication(string feature, string dataSet);
    }
}
