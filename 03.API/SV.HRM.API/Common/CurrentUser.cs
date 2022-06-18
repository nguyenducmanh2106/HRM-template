using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SV.HRM.Caching.Interface;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SV.HRM.API.Common
{
    public static class CurrentUser
    {
        public static UserModel GetCurrentUserInfo(IHttpContextAccessor httpContextAccessor, ICached cached)
        {
            if (httpContextAccessor.HttpContext.User.Identity != null)
            {
                var accessToken = TryRetrieveToken(httpContextAccessor);
                string sessionID = GetSessionID(accessToken);
                string cacheKey = $"{Constant.USER_SESSION}:{sessionID}:{accessToken}";
                string jsonData = cached.Get(cacheKey);
                var userInfo = JsonConvert.DeserializeObject<UserModel>(jsonData);
                if (userInfo != null && !string.IsNullOrEmpty(userInfo.AccessToken) && userInfo.AccessToken.Equals(accessToken))
                {
                    return userInfo;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private static string TryRetrieveToken(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                StringValues authHeader = string.Empty;
                httpContextAccessor.HttpContext.Request.Headers.TryGetValue(Constant.RequestHeader.AUTHORIZATION, out authHeader);
                var bearerToken = authHeader.ElementAt(0);
                return (bearerToken.StartsWith(Constant.RequestHeader.BEARER) ? bearerToken.Substring(7) : bearerToken);
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public static string GetSessionID(string accessToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken);
            var tokenS = jsonToken as JwtSecurityToken;
            return tokenS.Claims.First(claim => claim.Type == "sid").Value;
        }

        public static bool CheckSession(ICached cached, string accessToken)
        {
            string sessionID = GetSessionID(accessToken);
            return cached.Get($"{Constant.USER_SESSION}:{sessionID}:{accessToken}") is null;
        }
    }
}
