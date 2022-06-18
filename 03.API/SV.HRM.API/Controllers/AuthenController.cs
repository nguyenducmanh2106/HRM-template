using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using SV.HRM.API.HttpServices;
using SV.HRM.Caching.Interface;
using SV.HRM.Core.Utils;
using SV.HRM.Models;

namespace SV.HRM.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IAuthenHttpService _authenService;
        private readonly IBaseHttpService _baseService;
        private readonly IStaffHttpService _staffService;
        private readonly ICached _cached;

        public AuthenController(IAuthenHttpService authenService, IBaseHttpService baseService, IStaffHttpService staffService, ICached cached)
        {
            _authenService = authenService;
            _cached = cached;
            _baseService = baseService;
            _staffService = staffService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ResponseModel> Login(string code, bool isSystem)
        {
            var response = new ResponseModel();
            try
            {
                var tokenInfo = await _authenService.GetTokenInfo(code);
                if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.access_token) && !string.IsNullOrEmpty(tokenInfo.id_token))
                {
                    response.AccessToken = tokenInfo.access_token;
                    response.RefreshToken = tokenInfo.refresh_token;
                    response.IdToken = tokenInfo.id_token;

                    string userName = await _authenService.GetUserInfo(response.AccessToken);
                    if (!string.IsNullOrEmpty(userName))
                    {
                        var userInfo = (await _baseService.GetUserInfo(userName))?.Data;

                        if (userInfo.Status != Constant.StatusEnable)
                            throw new Exception("Tài khoản không hoạt động.Vui lòng liên hệ quản trị viên để được xử lý.");

                        if (userInfo.IsLockedOut)
                            throw new Exception("Tài khoản đang bị khóa.");

                        var res = await _baseService.GetPermissionByUser((int)userInfo?.UserId, userInfo?.ApplicationId);
                        int? staffID = _staffService.GetStaffIDByAccountID((int)userInfo?.UserId).Result.Data; //Lấy ID hồ sơ tương ứng
                        if (userInfo != null && res != null && res.Status == Constant.SUCCESS)
                        {
                            userInfo.AccessToken = response.AccessToken;
                            userInfo.Permissions = res.Data;
                            userInfo.IsSystem = isSystem;
                            userInfo.StaffID = staffID;

                            #region Lấy sessionID
                            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                            var jsonToken = handler.ReadToken(tokenInfo.access_token);
                            var tokenS = jsonToken as JwtSecurityToken;
                            string sessionID = tokenS.Claims.First(claim => claim.Type == "sid")?.Value;

                            if (string.IsNullOrEmpty(sessionID))
                                throw new Exception("Phiên đăng nhập của bạn đã hết hạn.");
                            #endregion

                            string jsonData = JsonConvert.SerializeObject(userInfo);
                            _cached.Add($"{Constant.USER_SESSION}:{sessionID}:{ response.AccessToken}", jsonData, StaticVariable.CacheDataExpireOneDay);

                            response.Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonData));
                            response.AccessToken = response.AccessToken;
                            return response;
                        }
                        else
                            throw new Exception("Không tìm thấy tài khoản trong hệ thống.");
                    }
                    else
                        throw new Exception("Phiên đăng nhập của bạn đã hết hạn hoặc token của bạn không đúng.");
                }
                else
                    throw new Exception("Phiên đăng nhập của bạn đã hết hạn.");
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                response.Title = ex.Message;
                response.Error = true;
                return response;
            }
        }
    }
}
