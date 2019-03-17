using Mchnry.Core.Encryption;
using Mchnry.Core.JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using roll_api.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace roll_api.Infrastructure.Security
{
    public class JWTMiddleWare
    {
        private readonly RequestDelegate _next;


        public JWTMiddleWare(RequestDelegate next)
        {
            _next = next;
      
        }

        public async Task InvokeAsync(HttpContext context, JWTConfiguration jwtConfig, IRSAKeyProvider keyProv, ClientHelper clientHelper, JWTHelper jwtHelper)
        {
            //pull jwt from header.... validate
            string bearerToken = context.Request.Headers.FirstOrDefault(g => g.Key.StartsWith("authorization", StringComparison.OrdinalIgnoreCase)).Value;
            string jwtToken = string.Empty;
            bool expired = false, goodToken = false, noToken = true;

            if (!string.IsNullOrEmpty(bearerToken))
            {
                jwtToken = bearerToken.Replace("bearer", "", StringComparison.OrdinalIgnoreCase).TrimStart(' ');
                noToken = false;

                try
                {
                    var rsaKey = keyProv.GetKey();
                    var jwt = jwtHelper.Decode<ApiHeader, ApiToken>(jwtToken, rsaKey, out expired);

                    if (!expired)
                    {
                        goodToken = true;
                        //create a new jwt
                        jwt.Token.iat = jwtHelper.DateToInt(TimeSpan.MinValue)[0];
                        jwt.Header.exp = jwt.Token.exp = jwtHelper.DateToInt(jwtConfig.Expire)[1];

                        clientHelper.token = jwt;

                        jwtToken = jwtHelper.Encode<ApiHeader, ApiToken>(jwt, rsaKey);

                        context.Response.Headers.Add("Authorization", $"bearer {jwtToken}");
                    }

                }
                catch { }
                
                

            }

            if (goodToken)
            {
                await _next(context);
            } else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }
            
           
        }
    }

    public static class JWTMiddleWareExtension
    {
        public static IApplicationBuilder UseJWT(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JWTMiddleWare>();
        }
    }
}
