#region Header
// ---------------------------------------------------------------------------
// Sepura - Commercially Confidential.
// 
// Indigo.CrossCutting.Utilities.IPolicyRetriever
// CORS Polivcy Retriever Interface
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
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Policy Retriever Interface
    /// </summary>
    [ServiceContract]
    public interface IPolicyRetriever
    {
        /// <summary>
        /// Gets the silverlight policy.
        /// </summary>
        /// <returns></returns>
        [OperationContract, WebGet(UriTemplate = "/clientaccesspolicy.xml")]
        Stream GetSilverlightPolicy();

        /// <summary>
        /// Gets the flash policy.
        /// </summary>
        /// <returns></returns>
        [OperationContract, WebGet(UriTemplate = "/crossdomain.xml")]
        Stream GetFlashPolicy();
    }

}
