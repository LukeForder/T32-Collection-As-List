using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace WCF.Behaviors
{
    public class CollectionIsListOperationBehavior : IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
            Contract.Requires(clientOperation != null);
            Contract.Requires(clientOperation.Formatter != null);

            clientOperation.Formatter = new CollectionIsListMessageFormatter(clientOperation.Formatter);
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
    }
}
