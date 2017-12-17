using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Class which is used to specify the files involved in packaged jobs
    /// </summary>
    public class JobPackageSpecification
    {
        public JobPackageSpecification(string jobPackageFileName, string jobConfigurationFileName, string packageFriendlyName)
        {
            this.PackageFileRelativeName = jobPackageFileName;
            this.PackageConfigurationFileRelativeName = jobConfigurationFileName;
            this.PackageFriendlyName = packageFriendlyName;

        }

        /// <summary>
        /// The friendly name of the package
        /// </summary>
        public string PackageFriendlyName = "";
        /// <summary>
        /// The relative name of the import file (typically a dtsConfig file) .. i.e. "UKCBDS\BaseDataFiles\Gender Promotion.dtsConfig"
        /// </summary>
        public string PackageConfigurationFileRelativeName;
        /// <summary>
        /// The relatiev name of the package file linked to the Import file ... i.e. "UKCBDS\BaseDataFiles\Gender Promotion.config"
        /// </summary>
        public string PackageFileRelativeName;
    }
}
