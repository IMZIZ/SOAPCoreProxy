using Microsoft.AspNetCore.Http;
using SoapCore.Extensibility;
using SoapCore.ServiceModel;
using Microsoft.Extensions.Primitives;
using System;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Server
{
    public class FilteringEndpointBehavior : IEndpointBehavior
    {
        private IMessageInspector _messageInspector;

        public FilteringEndpointBehavior(IMessageInspector messageInspector)
        {
            _messageInspector = messageInspector;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException();
        }

        //To authorize the request
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add((IClientMessageInspector)_messageInspector);
        }

        //To dispatch Request depends on request operation
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            throw new NotImplementedException();
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            throw new NotImplementedException();
        }
    }
}
