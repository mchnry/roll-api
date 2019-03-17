using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace roll_api.Infrastructure.Configuration
{
    public class JWTConfiguration
    {



        public string ThumbPrint { get; set; } = string.Empty;
        public TimeSpan Expire { get; set; } = TimeSpan.FromMinutes(1);
    }
}
