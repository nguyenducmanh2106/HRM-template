using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using SV.HRM.DataProcess.Infrastructure.Impl;
using Microsoft.AspNetCore.Http;
using SV.HRM.Caching.Interface;
using NLog;
using Newtonsoft.Json;

namespace SV.HRM.Service.LeaveManagement
{
    public class BaseHandler : IBaseHandler
    {
        private readonly ICached _cached;
        private readonly IConfiguration _configuration;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public BaseHandler(ICached cached, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IDapperUnitOfWork dapperUnitOfWork)
        {
            _cached = cached;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _dapperUnitOfWork = dapperUnitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<BaseUserModel>> GetListUser()
        {
            IDbConnection connection = null;
            try
            {
                var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
                string connectionString = _configuration.GetValue<string>(Constant.ConnectionString.SYSTEM_CONNECTION_STRING);
                connection = new SqlConnection(connectionString);
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();
                    var ret = await dal.QueryAsync<BaseUserModel>("SELECT UserId, UserName, FullName, ID FROM Users WHERE ApplicationId = @applicationId and Status != @status;", new { applicationId = user.ApplicationId, status = Constant.StatusRecord.DELETED });
                    return ret.ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                connection.Close();
                return null;
            }
        }

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteMany<T>(List<object> recordID)
        {
            try
            {
                string layout = typeof(T).Name;
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "DeleteMany");
                string queryFormat = string.Format(sqlQuery, layout, layout + "ID");
                int result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(queryFormat, new { @recordIDs = recordID });
                if (result > 0)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn check bản ghi có đang được sử dụng không
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyCheckUseRecord<T>(List<object> objectDelete)
        {
            try
            {
                bool checkUse = false; // biến check bản ghi có được sử dụng không
                // list id các bản ghi
                var dataRecords = JsonConvert.DeserializeObject<List<Guid>>(objectDelete[0].ToString());

                string sqlQueryRelated = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "GetAll_Table_Related");

                string layout = typeof(T).Name; // bảng gốc
                string relatedIDs = layout + "ID"; // tên trường 
                var resultRelated = await _dapperUnitOfWork.GetRepository().QueryAsync<string>(sql: sqlQueryRelated, new { @relatedID = relatedIDs });
                var dataTable = (List<string>)resultRelated;//list các bảng liên quan sử dụng layout+ ID

                // check bản ghi result > 0: bản ghi đang được sử dụng 
                if (dataTable != null && dataTable.Count > 0)
                {
                    foreach (var tbl in dataTable)
                    {
                        if (!tbl.Equals(layout))
                        {
                            string layoutChild = tbl; // bảng liên quan
                            string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "TopRecordById");
                            string queryFormat = string.Format(sqlQuery, layoutChild, layout + "ID");
                            var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<T>(sql: queryFormat, new { @recordIDs = dataRecords });
                            if (result != null)
                            {
                                checkUse = true;
                                break;
                            }
                        }
                    }
                }
                if (!checkUse)
                {
                    string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "DeleteMany");
                    string queryFormat = string.Format(sqlQuery, layout, layout + "ID");
                    int result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(queryFormat, new { @recordIDs = dataRecords });
                    if (result > 0)
                    {
                        return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                    }
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
                else
                {
                    return new Response<bool>(Constant.ErrorCode.DELETE_FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }
    }
}
