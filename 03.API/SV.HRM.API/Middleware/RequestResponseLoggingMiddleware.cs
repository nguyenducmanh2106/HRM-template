using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using NLog;
using SV.HRM.API.Common;
using SV.HRM.Caching.Interface;
using SV.HRM.Core.Utils;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SV.HRM.API.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly RequestDelegate _next;
        private readonly ICached _cached;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, ICached cached)
        {
            _next = next;
            _cached = cached;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context != null && (context.Request.Path.Value.Contains("GenerateAccount") || (context.Request.Path.Value.Contains("/authen/"))))
            {
                await _next(context).ConfigureAwait(true);
            }
            else
            {
                StringValues access_Token;
                context.Request.Headers.TryGetValue(Constant.RequestHeader.AUTHORIZATION, out access_Token);
                try
                {
                    var pubKey = AppSettings.Instance.GetString("KeyCloak:PublicKey", default);
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(pubKey), out _);

                    SecurityToken securityToken;
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    TokenValidationParameters validationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = AppSettings.Instance.GetString("KeyCloak:Audience", default),
                        ValidIssuer = AppSettings.Instance.GetString("KeyCloak:Issuer", default),
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        //LifetimeValidator = this.LifetimeValidator,
                        IssuerSigningKey = new RsaSecurityKey(rsa)
                    };
                    context.User = handler.ValidateToken(access_Token, validationParameters, out securityToken);
                    if (context.User.Identity.IsAuthenticated && !CurrentUser.CheckSession(_cached, access_Token))
                        await _next(context).ConfigureAwait(true);
                    else
                        context.Response.StatusCode = Constant.HttpStatusCode.UN_AUTHORIZED;
                }
                catch (Exception ex)
                {
                    logger.Error($"[ERROR]: {ex}");
                    context.Response.StatusCode = Constant.HttpStatusCode.UN_AUTHORIZED;
                }
            }
        }
    }
}
