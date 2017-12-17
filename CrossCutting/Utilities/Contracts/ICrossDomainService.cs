#region Header
// ---------------------------------------------------------------------------
// Sepura - Commercially Confidential.
// 
// Indigo.CrossCutting.Utilities.ICrossDomainService
// CORS Interface
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
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// NOTE: If you change the interface name "ICrossDomainService" here, you must also update the reference to "ICrossDomainService" in App.config.
    /// </summary>
    [ServiceContract]
    public interface ICrossDomainService
    {
        /// <summary>
        /// Provides the policy file.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "ClientAccessPolicy.xml")]
        Message ProvidePolicyFile();
    }
}
