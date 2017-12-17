using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Start.xml contains data about the upgrade and other startup information for the configuration server 
    /// It must persist over an upgrade and therefore it is not part of the solution.  
    /// It is created and updated when it is required and resides in the App_Data directory
    /// It's structure will change and this is where you define that structure in checkXMLStructure()
    /// </summary>
    public class StartupXML
    {
        private XDocument startupXMLDoc = null;

        public XDocument StartupXMLDoc
        {
            get { return startupXMLDoc; }
            set { startupXMLDoc = value; }
        }

        private const string xmlFileName = "Startup.xml";
        private string fileName = "";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileDirectory"></param>
        public StartupXML(string fileDirectory)
        {
            this.fileName = Path.Combine(fileDirectory, xmlFileName);
        }

        #region Helper Functions

        public bool SetUpgradeStatus(UpgradeStatus upgradeStatus)
        {
            if (!this.LoadFile()) return false;
            if (!this.UpdateElement("Upgrade", "UpgradeStatus", upgradeStatus.ToString())) return false;
            if (!this.UpdateElement("Upgrade", "LastUpdate", string.Format("{0}", DateTime.Now.ToString() ))) return false;
            this.SaveFile();
            return true;
        }

        #endregion

        #region Generic Read/Update
        /// <summary>
        /// Read an element
        /// </summary>
        /// <param name="SectionName"></param>
        /// <param name="ElementName"></param>
        /// <returns></returns>
        public string ReadElement(string SectionName, string ElementName)
        {
            return this.startupXMLDoc.Element("Startup").Element(SectionName).Descendants().FirstOrDefault(elem => elem.Name == ElementName).Value;
        }

        /// <summary>
        /// Update an element
        /// </summary>
        /// <param name="SectionName"></param>
        /// <param name="ElementName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool UpdateElement(string SectionName, string ElementName, string Value)
        {
            if (startupXMLDoc.Root.Elements(SectionName).Elements().FirstOrDefault(elem => elem.Name == ElementName) != null)
            {
                startupXMLDoc.Element("Startup").Element(SectionName).SetElementValue(ElementName, Value);
                this.SaveFile();
            }
            else
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Load, Save and Check/Define Structure

        /// <summary>
        /// Load the file if it exists
        /// </summary>
        public bool LoadFile()
        {
            try
            {
                if (File.Exists(this.fileName))
                {
                    startupXMLDoc = XDocument.Load(this.fileName);
                    this.checkXMLStructure();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("StartupXML LoadFile " + ex.Message);
            }
            return false;
        }
    
        /// <summary>
        /// Save the file
        /// </summary>
        public void SaveFile()
        {
            try
            {
                this.startupXMLDoc.Save(this.fileName);
            }
            catch (Exception ex)
            {
                throw new Exception("StartupXML SaveFile " + ex.Message);
            }
        }

        /// <summary>
        /// Create the file if it does not exist
        /// </summary>
        /// <returns></returns>
        public bool CreateFile()
        {
            if (!File.Exists(this.fileName))
            {
                startupXMLDoc = new XDocument(new XElement("Startup"));
                this.checkXMLStructure();
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Called whenever the file is loaded or created.  
        /// Modify this script to alter the structure
        /// </summary>
        private void checkXMLStructure()
        {

            // Check ConfigurationSet section exists
            if (startupXMLDoc.Root.Elements().FirstOrDefault(elem => elem.Name == "ConfigurationSet") == null)
            {
                startupXMLDoc.Root.Add(new XElement("ConfigurationSet"));
            }
            // Check ConfigurationSet/ConfigurationSetCurrent section exists
            if (startupXMLDoc.Root.Elements("ConfigurationSet").Elements().FirstOrDefault(elem => elem.Name == "ConfigurationSetCurrent") == null)
            {
                startupXMLDoc.Root.Element("ConfigurationSet").Add(new XElement("ConfigurationSetCurrent"));
            }
            // Check ConfigurationSet/ConfigurationSetForUpgrade section exists
            if (startupXMLDoc.Root.Elements("ConfigurationSet").Elements().FirstOrDefault(elem => elem.Name == "ConfigurationSetForUpgrade") == null)
            {
                startupXMLDoc.Root.Element("ConfigurationSet").Add(new XElement("ConfigurationSetForUpgrade"));
            }

            // Check Upgrade section exists
            if (startupXMLDoc.Root.Elements().FirstOrDefault(elem => elem.Name == "Upgrade") == null)
            {
                startupXMLDoc.Root.Add(new XElement("Upgrade"));
            }
            // Check Upgrade/UpgradeStatus section exists
            if (startupXMLDoc.Root.Elements("Upgrade").Elements().FirstOrDefault(elem => elem.Name == "UpgradeStatus") == null)
            {
                startupXMLDoc.Root.Element("Upgrade").Add(new XElement("UpgradeStatus"));
            }
            // Check Upgrade/LastUpdate section exists
            if (startupXMLDoc.Root.Elements("Upgrade").Elements().FirstOrDefault(elem => elem.Name == "LastUpdate") == null)
            {
                startupXMLDoc.Root.Element("Upgrade").Add(new XElement("LastUpdate"));
            }

            this.SaveFile();

        }

        #endregion

        #region enum UpgradeStatus
        /// <summary>
        /// UpgradeStatus is recorded in Startup.xml
        /// </summary>
        public enum UpgradeStatus
        {
            AwaitingUpgrade,
            UpgradeInProgress,
            UpgradeError,
            UpgradeComplete
        }
        #endregion

    }
}
