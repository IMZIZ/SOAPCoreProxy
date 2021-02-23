
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoapCore;
using SoapCore.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using Server.Interfaces;


namespace Server
{
    public class SOAPRequestActionFilter
    {
        public void OnSoapActionExecuting(string operationName, object[] allArgs, HttpContext httpContext, object result)
        {
            if (operationName != nameof(ISampleService.GetCityStateByZipCodeRequest) || allArgs.Length != 1)
                return;
            

            if (allArgs[0]!=null)
            {
                Console.WriteLine(allArgs[0].ToString());
            }
        }
    }
}