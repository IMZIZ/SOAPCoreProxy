﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SoapCore;
using Server.Interfaces;
using SoapCore.Extensibility;
using MHPService;



namespace Server
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{


			services.TryAddSingleton<ISampleService, SampleService>();
			services.AddSoapMessageInspector(new SoapSecurityHeaderInspector());
			// This one performs output logging
			services.AddSoapMessageInspector(new MessageBroadcaster());
			services.AddSoapMessageFilter(new WsMessageFilter("SI", "PI"));
			services.AddSoapServiceOperationTuner(new MyServiceOperationTuner());
			//services.AddSingleton<IFaultExceptionTransformer, DefaultFaultExceptionTransformer>();
			services.AddMvc(x => x.EnableEndpointRouting = false);
			services.AddSoapCore();
		}

		public void Configure(IApplicationBuilder app)
		{
			
			app.UseSoapEndpoint<ISampleService>("/Service.svc", new BasicHttpBinding(), SoapSerializer.DataContractSerializer);
			 
			app.UseSoapEndpoint<ISampleService>("/Service.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
			app.UseMvc();
		}
	}
}
