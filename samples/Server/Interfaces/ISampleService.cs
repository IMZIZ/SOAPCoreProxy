using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceModel;
using MHPService;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Server.Interfaces
{

	[ServiceContract]
	public interface ISampleService
	{
		[OperationContract]
		[XmlSerializerFormat(SupportFaults = true)]
		//[ServiceFilter(typeof(SOAPRequestActionFilter))]
		Task<GetCityStateByZipCodeResponse> GetCityStateByZipCodeRequest(string zipCode, string envId);

	
	}
}
