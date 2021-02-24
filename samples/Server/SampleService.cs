
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MHPService;
using SecondaryService;
using Server.Interfaces;
using System.ServiceModel;
using System.ServiceModel.Channels;
using SoapCore.Extensibility;

namespace Server
{
	public class SampleService : ISampleService
	{
		 
		private MHPServicesClient _mhpClient;
		private AddServiceClient _addServiceClient;

		public SampleService()
        {
			
			IMessageInspector messageInspector = new SoapSecurityHeaderInspector();

			_mhpClient = new MHPServicesClient();
			_addServiceClient = new AddServiceClient();
			var requestInterceptor = new FilteringEndpointBehavior(messageInspector);
			//_mhpClient.Endpoint.EndpointBehaviors.Add(requestInterceptor);

		}

		public Task<GetCityStateByZipCodeResponse> GetCityStateByZipCodeRequest(string zipCode, string envId)
		{

			//Get TrackingID 
			Task<GetCityStateByZipCodeResponse> response = _mhpClient.GetCityStateByZipCodeAsync(zipCode, envId);
			//second method
			// ...
			//third method
			// ...
			//Consolidate results to finial Response class
			//and Put TrackingID/status/results into final Response class
			return response;
		}

        public async Task<CombinedSOAPResponse> MultiCall(string zipCode, string envId, int number1, int number2)
        {
			CombinedSOAPResponse myCombinedResponse = new CombinedSOAPResponse();
			myCombinedResponse.zipResponse = await _mhpClient.GetCityStateByZipCodeAsync(zipCode, envId);
			myCombinedResponse.addResponse = await _addServiceClient.SimpleAddAsync(number1, number2);

			return myCombinedResponse;
		}

        public Task<int> SimpleAdd(int number1, int number2)
		{
			Task<int> response = _addServiceClient.SimpleAddAsync(number1, number2);
			return response;
		}

    }
}
