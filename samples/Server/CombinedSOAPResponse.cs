using System;
using System.Collections.Generic;
using System.Linq;
using MHPService;
using System.Threading.Tasks;

namespace Server
{
    public class CombinedSOAPResponse
    {
        public GetCityStateByZipCodeResponse zipResponse { get; set; }
        public int addResponse { get; set; }
    }
}
