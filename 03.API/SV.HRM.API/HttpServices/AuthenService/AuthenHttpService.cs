using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SV.HRM.Caching.Interface;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class AuthenHttpService : IAuthenHttpService
    {
        private Dictionary<string, string> keyCloakConfig = AppSettings.Instance.Get<Dictionary<string, string>>("KeyCloak");
        private readonly ICached _cached;
        private IHttpContextAccessor _httpContextAccessor;
        private ILogger<AuthenHttpService> _logger;
        public AuthenHttpService(ICached cached, IHttpContextAccessor httpContextAccessor, ILogger<AuthenHttpService> logger)
        {
            _logger = logger;
            _cached = cached;
            _httpContextAccessor = httpContextAccessor;
            ServicePointManager.ServerCertificateValidationCallback =
            delegate (
                object s,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors
            )
            {
                return true;
            };
        }
        public async Task<Ws02IS_ResponseTokenModel> GetTokenInfo(string code)
        {
            try
            {
                string url = keyCloakConfig["Url"] + "/auth/realms/hrm/protocol/openid-connect/token";
                var header = new List<KeyValuePair<string, string>>();
                header.Add(new KeyValuePair<string, string>("Content-Type", "application/x-www-form-urlencoded"));
                var param = new List<KeyValuePair<string, string>>();
                param.Add(new KeyValuePair<string, string>("client_id", keyCloakConfig["Client_ID"]));
                param.Add(new KeyValuePair<string, string>("client_secret", keyCloakConfig["Client_Secret"]));
                param.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
                param.Add(new KeyValuePair<string, string>("code", code));
                param.Add(new KeyValuePair<string, string>("redirect_uri", keyCloakConfig["Redirect_Uri"]));

                return await RestsharpUtils.PostAsync<Ws02IS_ResponseTokenModel>(url, header, null, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
                return null;
            }
        }

        public async Task<string> GetUserInfo(string access_token)
        {
            string url = keyCloakConfig["Url"] + "/auth/realms/hrm/protocol/openid-connect/userinfo?schema=openid";
            var header = new List<KeyValuePair<string, string>>();
            header.Add(new KeyValuePair<string, string>(Constant.RequestHeader.AUTHORIZATION, Constant.RequestHeader.BEARER + access_token));
            var res = await RestsharpUtils.PostAsync<Ws02IS_ResponseSubModel>(url, header);
            if (res != null && res.preferred_username != null)
            {
                return res.preferred_username;
            }
            else
            {
                return null;
            }
        }

        public Task<Ws02IS_ResponseTokenModel> GetTokenFromRefreshToken(string refresh_token)
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticated()
        {
            StringValues access_Token = string.Empty;
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(Constant.RequestHeader.AUTHORIZATION, out access_Token);
            if (string.IsNullOrEmpty(access_Token))
            {
                return false;
            }
            #region Lấy sessionID
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(access_Token);
            var tokenS = jsonToken as JwtSecurityToken;
            string sessionID = tokenS.Claims.First(claim => claim.Type == "sid")?.Value;

            if (string.IsNullOrEmpty(sessionID))
                throw new Exception("Phiên đăng nhập của bạn đã hết hạn.");
            #endregion
            var uInfo = _cached.Get<UserModel>(Constant.USER_SESSION + ":" + sessionID + ":" + access_Token);
            if (uInfo != null && !string.IsNullOrEmpty(uInfo.AccessToken) && uInfo.AccessToken.Equals(access_Token))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
