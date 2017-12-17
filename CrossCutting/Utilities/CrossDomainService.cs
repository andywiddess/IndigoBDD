#region Header
// ---------------------------------------------------------------------------
// Sepura - Commercially Confidential.
// 
// Indigo.CrossCutting.Utilities.CrossDomainService
// CORS Support
//
// Copyright (c) 2016 Sepura Plc
// All Rights reserved.
//
// $Id:  $ :
// ---------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.ServiceModel.Channels;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Cross Domain Services
    /// </summary>
    public class CrossDomainService
        : ICrossDomainService
    {
        /// <summary>
        /// Provides the policy file.
        /// </summary>
        /// <returns></returns>
        public System.ServiceModel.Channels.Message ProvidePolicyFile()
        {
            FileStream filestream = File.Open(@"ClientAccessPolicy.xml", FileMode.Open);

            // Either specify ClientAcessPolicy.xml file path properly
            // or put that in \Bin folder of the console application
            XmlReader reader = XmlReader.Create(filestream);
            filestream.Close();

            System.ServiceModel.Channels.Message result = Message.CreateMessage(MessageVersion.None, "", reader);
            return result;
        }
    }
}
