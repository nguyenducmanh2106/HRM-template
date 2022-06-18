using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NLog;
using SV.HRM.Caching.Interface;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SV.HRM.DataProcess.Infrastructure.Impl;
using System.Linq;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SV.HRM.Service.BaseServices
{
    public class UserConfigHandler : IUserConfigHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBaseHandler _baseHandler;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly ICached _cached;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserConfigHandler(IBaseHandler baseHandler, IDapperUnitOfWork dapperUnitOfWork, IConfiguration configuration, ICached cached, IHttpContextAccessor httpContextAccessor)
        {
            _baseHandler = baseHandler;
            _dapperUnitOfWork = dapperUnitOfWork;
            _configuration = configuration;
            _cached = cached;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<List<UserConfigModel>>> GetFilter(EntityGeneric queryFilter)
        {
            try
            {
                IDbConnection connection = null;
                List<UserConfigModel> listData = new List<UserConfigModel>();
                int totalCount = 0;
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", queryFilter.LayoutCode, "Proc_Grid");
                var param = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(queryFilter.CustomPagingData));
                var dyParameters = new DynamicParameters();
                foreach (var kvp in param)
                {
                    dyParameters.Add(kvp.Key, kvp.Value);
                }

                var objReturn = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, dyParameters, null, CommandType.StoredProcedure,
                    gr =>
                        gr.Read<UserConfigModel>(),
                    gr =>
                        gr.Read<Int32>().FirstOrDefault()
                    );
                if (objReturn != null && objReturn[0] != null && objReturn[1] != null)
                {
                    listData = (List<UserConfigModel>)objReturn[0];
                    string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                    connection = new SqlConnection(connectionString);
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        foreach(var item in listData)
                        {
                            var user = unitOfwork.GetRepository().QuerySingleOrDefault<UserConfigModel>("select UserId, UserName, FullName as UserFullName from Users where UserId = @UserId and Status != @Status",
                            new
                            {
                                UserId = item.UserID,
                                Status = Constant.StatusRecord.DELETED
                            });
                            if (user != null)
                            {
                                item.UserName = user.UserName;
                                item.UserFullName = user.UserFullName;
                            }
                        }
                    }
                    totalCount = (int)objReturn[1];
                    int totalPage = (int)Math.Ceiling((decimal)totalCount / queryFilter.PageSize);
                    return new Response<List<UserConfigModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, listData, listData.Count(), totalCount, totalPage, queryFilter.PageIndex, queryFilter.PageSize);
                }
                else
                {
                    return new Response<List<UserConfigModel>>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<UserConfigModel>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Tạo mới 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(UserConfig model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(UserConfig), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID", model.UserID);
                param.Add("@StaffID", model.StaffID);
                param.Add("@WorkflowID", model.WorkflowID);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= Constant.SUCCESS) //lưu thành công
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else //trường hợp còn lại cụ thể là store proc trả ra = 0 là trường hợp exception lỗi của SQL
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<UserConfigModel>> FindById(Guid recordID)
        {
            UserConfigModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(UserConfig), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<UserConfigModel>(sqlQuery, new { UserConfigID = recordID });
                if (result != null)
                {
                    string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                    IDbConnection connection = new SqlConnection(connectionString);
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var user = unitOfwork.GetRepository().QuerySingleOrDefault<UserConfigModel>("select UserId, UserName, (FullName + ' (' + UserName + ')') as UserFullName from Users where UserId = @UserId and Status != @Status",
                        new
                        {
                            UserId = result.UserID,
                            Status = Constant.StatusRecord.DELETED
                        });
                        if (user != null)
                        {
                            result.UserName = user.UserName;
                            result.UserFullName = user.UserFullName;
                        }
                    }
                    return new Response<UserConfigModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<UserConfigModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<UserConfigModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Hàm cập nhật ngân hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(Guid id, UserConfig model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", nameof(UserConfig), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserConfigID", id);
                param.Add("@UserID", model.UserID);
                param.Add("@StaffID", model.StaffID);
                param.Add("@WorkflowID", model.WorkflowID);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult == Constant.SUCCESS) //lưu thành công
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else //trường hợp còn lại cụ thể là store proc trả ra = 0 là trường hợp exception lỗi của SQL
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        public async Task<Response<bool>> CheckRecordInUse(List<object> obj)
        {
            try
            {
                bool checkUse = false;
                var dataRecords = JsonConvert.DeserializeObject<List<int>>(obj[0].ToString());
                // convert list object to ToDictionary<string,string>
                var dataTables = JsonConvert.DeserializeObject<List<JObject>>(obj[1].ToString()).Select(x => x?.ToObject<Dictionary<string, string>>()).ToList();
                if (dataRecords.Count() > 0 && dataTables.Count() > 0)
                {
                    string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "TopRecordById");
                    foreach (var item in dataTables)
                    {
                        string queryFormat = string.Format(sqlQuery, item["TableName"], item["ColumnName"]);
                        var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<UserConfig>(sql: queryFormat, new { @recordIDs = dataRecords });
                        if (result != null)
                        {
                            checkUse = true;
                            break;
                        }
                    }
                }
                if (!checkUse)
                {
                    var data = new List<object>();
                    data.Add(JsonConvert.SerializeObject(dataRecords));
                    return await _baseHandler.DeleteManyCheckUseRecord<UserConfig>(data);
                }
                else
                {
                    return new Response<bool>(Constant.ErrorCode.DELETE_FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
            catch (Exception)
            {

                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        public async Task<Response<bool>> CheckRecordInUseGuid(List<object> obj)
        {
            try
            {
                bool checkUse = false;
                var dataRecords = JsonConvert.DeserializeObject<List<Guid>>(obj[0].ToString());
                // convert list object to ToDictionary<string,string>
                var dataTables = JsonConvert.DeserializeObject<List<JObject>>(obj[1].ToString()).Select(x => x?.ToObject<Dictionary<string, string>>()).ToList();
                if (dataRecords.Count() > 0 && dataTables.Count() > 0)
                {
                    string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "TopRecordById");
                    foreach (var item in dataTables)
                    {
                        string queryFormat = string.Format(sqlQuery, item["TableName"], item["ColumnName"]);
                        var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<UserConfig>(sql: queryFormat, new { @recordIDs = dataRecords });
                        if (result != null)
                        {
                            checkUse = true;
                            break;
                        }
                    }
                }
                if (!checkUse)
                {
                    var data = new List<object>();
                    data.Add(JsonConvert.SerializeObject(dataRecords));
                    return await _baseHandler.DeleteManyCheckUseRecordGuid<UserConfig>(data);
                }
                else
                {
                    return new Response<bool>(Constant.ErrorCode.DELETE_FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
            catch (Exception)
            {

                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        public async Task<Response<List<UserConfigComboboxStaff>>> GetComboboxStaff(string q, int page)
        {
            List<UserConfigComboboxStaff> lstModel = new List<UserConfigComboboxStaff>();
            int totalCount = 0;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", nameof(UserConfig), "GetComboboxStaff");
                if (string.IsNullOrEmpty(q))
                {
                    q = string.Empty;
                }
                var renderQuery = string.Format(sqlQuery, "", page.ToString());
                var objectResult = _dapperUnitOfWork.GetRepository().QueryMultiple(renderQuery, null, null, CommandType.Text,
                    gr =>
                        gr.Read<UserConfigComboboxStaff>(),
                    gr =>
                        gr.Read<Int32>().FirstOrDefault()
                    );

                if (objectResult != null && objectResult[0] != null && objectResult[1] != null)
                {
                    lstModel = (List<UserConfigComboboxStaff>)objectResult[0];
                    totalCount = (int)objectResult[1];
                    int totalPage = (int)Math.Ceiling((decimal)totalCount / 10);
                    return new Response<List<UserConfigComboboxStaff>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, lstModel, 10, totalCount, totalPage);
                }
                return new Response<List<UserConfigComboboxStaff>>(Constant.ErrorCode.ERRORSYSTEM_CODE, Constant.ErrorCode.ERRORSYSTEM_MESS);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<UserConfigComboboxStaff>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<List<UserConfigComboboxUser>>> GetComboboxUser(string q, int page)
        {
            List<UserConfigComboboxUser> lstModel = new List<UserConfigComboboxUser>();
            int totalCount = 0;
            try
            {
                IDbConnection connection = null;
                List<object> objectResult = null;
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                connection = new SqlConnection(connectionString);
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", nameof(UserConfig), "GetComboboxUser");
                    if (string.IsNullOrEmpty(q))
                    {
                        q = string.Empty;
                    }
                    var renderQuery = string.Format(sqlQuery, "", page.ToString());
                    objectResult = unitOfwork.GetRepository().QueryMultiple(renderQuery, null, null, CommandType.Text,
                        gr =>
                            gr.Read<UserConfigComboboxUser>(),
                        gr =>
                            gr.Read<Int32>().FirstOrDefault()
                        );
                }
                if (objectResult != null && objectResult[0] != null && objectResult[1] != null)
                {
                    lstModel = (List<UserConfigComboboxUser>)objectResult[0];
                    totalCount = (int)objectResult[1];
                    int totalPage = (int)Math.Ceiling((decimal)totalCount / 10);
                    return new Response<List<UserConfigComboboxUser>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, lstModel, 10, totalCount, totalPage);
                }
                return new Response<List<UserConfigComboboxUser>>(Constant.ErrorCode.ERRORSYSTEM_CODE, Constant.ErrorCode.ERRORSYSTEM_MESS);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<UserConfigComboboxUser>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<List<UserConfigComboboxWorkflow>>> GetComboboxWorkflow(string q, int page)
        {
            List<UserConfigComboboxWorkflow> lstModel = new List<UserConfigComboboxWorkflow>();
            int totalCount = 0;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", nameof(UserConfig), "GetComboboxWorkflow");
                if (string.IsNullOrEmpty(q))
                {
                    q = string.Empty;
                }
                var renderQuery = string.Format(sqlQuery, "", page.ToString());
                var objectResult = _dapperUnitOfWork.GetRepository().QueryMultiple(renderQuery, null, null, CommandType.Text,
                    gr =>
                        gr.Read<UserConfigComboboxWorkflow>(),
                    gr =>
                        gr.Read<Int32>().FirstOrDefault()
                    );

                if (objectResult != null && objectResult[0] != null && objectResult[1] != null)
                {
                    lstModel = (List<UserConfigComboboxWorkflow>)objectResult[0];
                    totalCount = (int)objectResult[1];
                    int totalPage = (int)Math.Ceiling((decimal)totalCount / 10);
                    return new Response<List<UserConfigComboboxWorkflow>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, lstModel, 10, totalCount, totalPage);
                }
                return new Response<List<UserConfigComboboxWorkflow>>(Constant.ErrorCode.ERRORSYSTEM_CODE, Constant.ErrorCode.ERRORSYSTEM_MESS);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<UserConfigComboboxWorkflow>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }
    }
}
