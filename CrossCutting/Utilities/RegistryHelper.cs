using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Utility class for reading / writing to the registry.  Currently limited to HKLM
    /// </summary>
    public class RegistryHelper
    {
        #region Members
        /// <summary>
        /// Registry Key Name
        /// </summary>
        private readonly string keyName;

        /// <summary>
        /// Create key if Missing
        /// </summary>
        private readonly bool createIfMissing;

        /// <summary>
        /// Key
        /// </summary>
        private RegistryKey key;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a RegistryKeyHelper to inspect, read and write to an existing RegistryKey
        /// </summary>
        /// <param name="keyName">Registry Key Name</param>
        public RegistryHelper(string keyName)
            : this(keyName, false)
        {
        }

        /// <summary>
        /// Create a RegistryKeyHelper with option to create the key if it is not present
        /// </summary>
        /// <param name="keyName">Registry Key Name</param>
        /// <param name="createIfMissing">flag to create the key if it is not found</param>
        public RegistryHelper(string keyName, bool createIfMissing)
        {
            this.keyName = AdjustKeyPathFor64BitProcess(keyName);
            this.createIfMissing = createIfMissing;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Determines if the RegistryKey exists.  When using createIfMissing, this is always true
        /// </summary>
        public virtual bool RegistryKeyExists
        {
            get
            {
                return RegistryKey != null;
            }
        }

        /// <summary>
        /// Gets the Registry Key Name
        /// </summary>
        public string KeyName
        {
            get
            {
                return keyName;
            }
        }

        /// <summary>
        /// Gets the registry key.
        /// </summary>
        /// <value>
        /// The registry key.
        /// </value>
        private RegistryKey RegistryKey
        {
            get
            {
                if (key == null)
                {
                    key = Registry.LocalMachine.OpenSubKey(keyName, true);
                    if (key == null &&
                        createIfMissing)
                    {
                        Registry.LocalMachine.CreateSubKey(keyName);
                        key = Registry.LocalMachine.OpenSubKey(keyName, true);
                    }
                }
                return key;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the value from the registry key
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string GetValue(string name)
        {
            if (!RegistryKeyExists) return null;

            return RegistryKey.GetValue(name) as String;
        }

        /// <summary>
        /// Sets the value in the registry
        /// </summary>
        /// <param name="name">Value Name</param>
        /// <param name="value">Value to set</param>
        /// <remarks>will throw an exception if missing and createIfMissing is false</remarks>
        public virtual void SetValue(string name, string value)
        {
            if (!RegistryKeyExists)
                throw new Exception(string.Format("Unable to write registry value \"{0}", name));

            RegistryKey.SetValue(name, value, RegistryValueKind.String);
        }

        /// <summary>
        /// Delete Registry Key
        /// </summary>
        public virtual void Delete()
        {
            Registry.LocalMachine.DeleteSubKey(keyName);
        }
        #endregion

        #region Private members

        /// <summary>
        /// Change incoming requests for Software to Software\Wow6432Node when running as a 64bit process
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        /// <remarks>This allows the toolkit to remain ANY CPU, but provide Registry Redirection for 32-bit compatibility</remarks>
        private static string AdjustKeyPathFor64BitProcess(string keyName)
        {
            int indexOfSoftware = keyName.IndexOf("software", StringComparison.OrdinalIgnoreCase);

            // process is 64bit when ptr size is 8 bits &&  our string starts with "software"
            if (IntPtr.Size == 8 &&
                indexOfSoftware == 0)
            {
                // insert our 32 compatibility node into the key
                return keyName.Insert(8, @"\Wow6432Node");
            }

            return keyName;
        }
        #endregion
    }
}
