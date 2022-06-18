using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SV.HRM.Caching.Common;
using SV.HRM.Caching.Impl;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;

namespace SV.HRM.API.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _roleCode;
        private readonly string _rightCode;
        public CustomAuthorizeAttribute(string roleCode, string rightCode)
        {
            _roleCode = roleCode;
            _rightCode = rightCode;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                if (context != null)
                {
                    var cacheConfig = AppSettings.Instance.Get<CachingConfigModel>("Cache:Redis:Data");
                    var cache = new RedisCached(cacheConfig);

                    StringValues access_Token = string.Empty;
                    context.HttpContext.Request.Headers.TryGetValue(Constant.RequestHeader.AUTHORIZATION, out access_Token);
                    if (!string.IsNullOrEmpty(access_Token))
                    {
                        #region Lấy sessionID
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(access_Token);
                        var tokenS = jsonToken as JwtSecurityToken;
                        string sessionID = tokenS.Claims.First(claim => claim.Type == "sid")?.Value;

                        if (string.IsNullOrEmpty(sessionID))
                            throw new Exception("Phiên đăng nhập của bạn đã hết hạn.");
                        #endregion
                        string jsonData = cache.Get($"{Constant.USER_SESSION}:{sessionID}:{access_Token}");
                        var user = JsonConvert.DeserializeObject<UserInfoCacheModel>(jsonData);

                        if (user != null && CheckPermission(user))
                        {
                            return;
                        }
                        else
                        {
                            context.Result = new ObjectResult(HttpStatusCode.MethodNotAllowed)
                            {
                                StatusCode = (int)HttpStatusCode.MethodNotAllowed
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {
                context.Result = new ForbidResult();
            }
        }

        public bool CheckPermission(UserInfoCacheModel user)
        {
            if (user.Permissions.Any(r => r.RoleCode.Equals(_roleCode) && r.RightCodes.Contains(_rightCode)))
                return true;
            return false;
        }
    }
}
