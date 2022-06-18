using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using NLog;
using SV.HRM.Caching.Common;
using SV.HRM.Caching.Impl;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using static SV.HRM.Core.Utils.Constant;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class CustomAuthorize
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
        {
            private readonly string[] _roleCodes;
            private readonly string _rightCodes;
            //Danh sách quyền mặc định mỗi cá nhân trên hrm
            private readonly List<Permissions> _permissionDefault = new List<Permissions>
            {
                new Permissions
                {
                    RoleCode = Role.HSNV_MANAGER,
                    RightCodes = new List<string>
                    {
                        Right.VIEW_TTC, Right.VIEW_TTCV, Right.VIEW_TTK, Right.VIEW_TTLH,
                        Right.UPDATE,Right.UPDATE_TTC,Right.UPDATE_TTCV,Right.UPDATE_TTK,Right.UPDATE_TTLH
                    }
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_QTCT_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_QTL_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_BCCC_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_HDLD_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_GD_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_SK_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_TTD_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_QD_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_KT_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_KL_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_HS_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_GTCTH_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                },
                new Permissions
                {
                    RoleCode = Role.HSNV_DG_MANAGER,
                    RightCodes = new List<string>{ Right.VIEW}
                }
            };

            public CustomAuthorizeAttribute(string[] roleCode, string rightCode)
            {
                _roleCodes = roleCode;
                _rightCodes = rightCode;
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

                            Permissions permissions;
                            //push mặc định quyền cá nhân 
                            foreach (var item in _permissionDefault)
                            {
                                permissions = user.Permissions.FirstOrDefault(r => r.RoleCode.Equals(item.RoleCode));
                                if (permissions is null)
                                {
                                    user.Permissions.Add(item);
                                }
                                else
                                {
                                    permissions.RightCodes.AddRange(item.RightCodes);
                                    permissions.RightCodes.Distinct();
                                }
                            }

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
                catch (Exception ex)
                {
                    logger.Error($"[ERROR]: {ex}");
                    context.Result = new ForbidResult();
                }
            }

            public bool CheckPermission(UserInfoCacheModel user)
            {
                if (user.Permissions.Any(r => _roleCodes.Contains(r.RoleCode) && r.RightCodes.Contains(_rightCodes)))
                    return true;
                return false;
            }
        }
    }
}
