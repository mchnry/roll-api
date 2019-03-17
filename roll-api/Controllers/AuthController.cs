using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mchnry.Core.Encryption;
using Mchnry.Core.JWT;
using Microsoft.AspNetCore.Mvc;
using roll_api.Infrastructure.Configuration;

namespace roll_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IRSAKeyProvider keyProv;
        private readonly JWTHelper jwtHelper;
        private readonly JWTConfiguration jwtConfig;

        public AuthController(IRSAKeyProvider keyProv, JWTHelper jwtHelper, JWTConfiguration jwtConfig)
        {
            this.keyProv = keyProv;
            this.jwtHelper = jwtHelper;
            this.jwtConfig = jwtConfig;
        }

        [HttpGet("{token}")]
        public ActionResult<string> Get(string token)
        {
            //validate token
            bool tokenValid = true;
            //TODO: Validate Token on Auth
            //TODO: Create Session Token

            jwt<ApiHeader, ApiToken> jwt = new jwt<ApiHeader, ApiToken>()
            {
                Header = new ApiHeader()
                {
                    Algorithm = "HS384",
                    exp = jwtHelper.DateToInt(jwtConfig.Expire)[1],
                    TokenName = "jwttoken"
                    //Header = new ApiHeader() { Algorithm = "HS384", exp = helper.DateToInt(TimeSpan.FromMinutes(1))[1], TokenName = "tkn" },
                },
                Token = new ApiToken()
                {
                    exp = jwtHelper.DateToInt(jwtConfig.Expire)[1],
                    iat = jwtHelper.DateToInt(TimeSpan.MinValue)[1],
                    Subject = "1",
                    JTI = "session"
                }
            };
            string toReturn = jwtHelper.Encode(jwt, keyProv.GetKey());

            this.HttpContext.Response.Headers.Add("Authorization", $"bearer {toReturn}");
            return new JsonResult(new { jwt = toReturn });

        }
    }
}