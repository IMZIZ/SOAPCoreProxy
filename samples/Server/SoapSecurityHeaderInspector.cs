using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using SoapCore.Extensibility;
using Microsoft.Extensions.Primitives;

namespace Server
{
    public class SoapSecurityHeaderInspector : IMessageInspector
    {
        //TO-DO In the inspector:
        //1.Auditing message; 2. Inspect/Authorize request
        public object AfterReceiveRequest(ref Message message)
        {
            Console.WriteLine("IDispatchMessageInspector.AfterReceiveRequest called.");
            //message.Headers.GetHeader<string>(0);
            Console.WriteLine("Request message received");
            int headerIndex = message.Headers.FindHeader("test", "http://tempuri.org/");
            if (headerIndex >= 0)
            {
                Console.WriteLine("Request message header",
                                      message.Headers.GetHeader<string>(headerIndex));
            }
            //var data = message.GetBody<string>();
            //Console.WriteLine("Reply content: {0}", data.ToString());
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            Console.WriteLine("IDispatchMessageInspector.BeforeSendReply called.");
        }
    }
}
