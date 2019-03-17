using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mchnry.Core.JWT;
using Mchnry.Core.Encryption;
using roll_api.Infrastructure.Configuration;
using Microsoft.AspNetCore.Http;

namespace roll_api.Infrastructure
{
    public class ClientHelper
    {
        public jwt<ApiHeader, ApiToken> token { get; set; }

    }



}
