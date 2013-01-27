using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace WCF.Behaviors
{
    public class CollectionIsListEndpointBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            Contract.Requires(endpoint != null);
            Contract.Requires(endpoint.Contract != null);
            Contract.Requires(endpoint.Contract.Operations != null);

            endpoint
                .Contract
                .Operations
                .ToList()
                .ForEach(
                    operation =>
                    {
                        operation.OperationBehaviors.Add(new CollectionIsListOperationBehavior());
                    });

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
