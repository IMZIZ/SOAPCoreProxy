using Microsoft.AspNetCore.Http;
using SoapCore.Extensibility;
using SoapCore.ServiceModel;
using Microsoft.Extensions.Primitives;
using System;


namespace Server
{
    public class MyServiceOperationTuner : IServiceOperationTuner
    {
        public void Tune(HttpContext httpContext, object serviceInstance, OperationDescription operation)
        {
            
            //if (operation.Name.Equals("GetCityStateByZipCodeRequest"))
            //{
                //SampleService service = serviceInstance as SampleService;
               // string result = string.Empty;

                StringValues paramValue;
                if (httpContext.Request.Headers.TryGetValue("test", out paramValue))
                {
                    //result = paramValue[0];
                    Console.WriteLine("Param value from http header: " + paramValue[0]);
                }           
            //}
        }
       
    }
}