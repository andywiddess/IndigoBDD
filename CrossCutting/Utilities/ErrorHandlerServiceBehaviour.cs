using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Sepura.ApplicationServer;

namespace Sepura.ApplicationServer
{
    public class ErrorHandlerServiceBehaviour : System.ServiceModel.Description.IServiceBehavior
    {

        #region IServiceBehavior Members

        public void AddBindingParameters(System.ServiceModel.Description.ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<System.ServiceModel.Description.ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            return;
        }

        public void ApplyDispatchBehavior(System.ServiceModel.Description.ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (System.ServiceModel.Dispatcher.ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                channelDispatcher.ErrorHandlers.Add(new Indigo.CrossCutting.Utilities.GlobalErrorHandler());
            }
        }

        public void Validate(System.ServiceModel.Description.ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //TODO: This validation does not work for the security server as it contains some strange interfaces
            //TODO: Enable this validation and add and exception for those interfaces
            //foreach (System.ServiceModel.Description.ServiceEndpoint endpoint in serviceDescription.Endpoints)
            //{
            //    if (endpoint.Contract.Name != System.ServiceModel.Description.ServiceMetadataBehavior.MexContractName)
            //    {
            //        foreach (System.ServiceModel.Description.OperationDescription operationDesc in endpoint.Contract.Operations)
            //        {
            //            if (operationDesc.Faults.Count == 0)
            //            {
            //                string msg = string.Format("ServiceErrorHandlerBehavior requires a FaultContract(typeof(GenericException)) on each operation contract. The method {0} of contract {1} contains no FaultContracts.", operationDesc.Name, endpoint.Contract.Name);
            //                throw new InvalidOperationException(msg);
            //            }

            //            var fcExists = from fc in operationDesc.Faults
            //                           where fc.DetailType == typeof(Indigo.CrossCutting.Utilities.GenericException)
            //                           select fc;

            //            if (fcExists.Count() == 0)
            //            {
            //                string msg = string.Format("ServiceErrorHandlerBehavior requires a FaultContract(typeof(GenericException)) on each operation contract.");
            //                throw new InvalidOperationException(msg);
            //            }
            //        }
            //    }
            //}
        }

        #endregion
    }
}
