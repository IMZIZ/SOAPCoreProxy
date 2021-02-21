
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MHPService;
using Server.Interfaces;
using System.ServiceModel;
using System.ServiceModel.Channels;
using SoapCore.Extensibility;

namespace Server
{
	public class SampleService : ISampleService
	{
		 
		private MHPServicesClient _mhpClient;
		//private VUEServicesClient _vueClient;

		public void SampleSerice()
        {
			
			IMessageInspector messageInspector = new SoapSecurityHeaderInspector();

			_mhpClient = new MHPServicesClient();
			var requestInterceptor = new FilteringEndpointBehavior(messageInspector);
			_mhpClient.Endpoint.EndpointBehaviors.Add(requestInterceptor);

		}

		public Task<GetCityStateByZipCodeResponse> GetCityStateByZipCodeRequest(string zipCode, string envId)
		{
			Task<GetCityStateByZipCodeResponse> response = _mhpClient.GetCityStateByZipCodeAsync(zipCode, envId);
			return response;
		}

		 

    }
}
