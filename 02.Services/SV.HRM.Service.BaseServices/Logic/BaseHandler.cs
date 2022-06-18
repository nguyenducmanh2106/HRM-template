using AutoMapper;
using Dapper;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using SV.HRM.DataProcess.Infrastructure.Impl;
using Microsoft.AspNetCore.Http;
using SV.HRM.Caching.Common;
using SV.HRM.Caching.Interface;
using NLog;

namespace SV.HRM.Service.BaseServices
{
    public class BaseHandler : IBaseHandler
    {
        private readonly IMapper _mapper;
        private readonly ICached _cached;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public BaseHandler(IMapper mapper, ICached cached, IDapperUnitOfWork dapperUnitOfWork, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _cached = cached;
            _dapperUnitOfWork = dapperUnitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Check trùng theo trường nào đó
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public async Task<Response<bool>> CheckDuplicate<T>(string keySearch, int q)
        {
            try
            {
                string layoutCode = typeof(T).Name;
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "CheckDuplicate");
                string queryFormat = string.Format(sqlQuery, layoutCode, keySearch, q);
                int result = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, new { });
                if (result > 0)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, false);
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
        /// Check trùng theo trường nào đó
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public async Task<Response<bool>> CheckDuplicate(BaseCheckDuplicate model)
        {
            try
            {
                var param = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(model.CustomData));
                var dyParameters = new DynamicParameters();
                foreach (var kvp in param)
                {
                    dyParameters.Add(kvp.Key, kvp.Value);
                }
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "CheckDuplicate");
                string queryFormat = string.Format(sqlQuery, model.TableName, model.FieldName);
                int result = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(queryFormat, dyParameters, null, CommandType.StoredProcedure);
                if (result > 0)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, false);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layout"></param>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete<T>(string layout, int recordID)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "Delete");
                string queryFormat = string.Format(sqlQuery, layout, layout + "ID");
                int result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, new { recordID = recordID });
                if (result > 0)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, false);
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
        /// Hàm chung lấy về combobox sử dụng lazyload phân trang
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode">Tên bảng</param>
        /// <param name="keySearch">Tên trường dùng để tìm trên combobox</param>
        /// <param name="q">Giá trị của keySearch</param>
        /// <param name="page">Trang thứ mấy</param>
        /// <param name="commandType">Lấy từ mặc định hay lấy từ store procedure để custom lấy thêm dữ liệu</param>
        /// <returns></returns>
        public async Task<Response<List<T>>> GetCombobox<T>(string layoutCode, string keySearch, string q, int page, CommandType? commandType = null, bool? hasStatusColumn = false)
        {
            var conn = _dapperUnitOfWork.GetRepository().GetDbConnection();

            List<T> lstStaffModel = new List<T>();
            int totalCount = 0;

            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetCombobox");
                if (hasStatusColumn == true)
                {
                    sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetComboboxHasStatusColumn");
                }
                string renderQuery = String.Format(sqlQuery, layoutCode, keySearch, q ?? "");
                if (commandType == CommandType.StoredProcedure)
                {
                    sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", $"proc_GetCombobox");
                    renderQuery = String.Format(sqlQuery, layoutCode, keySearch);
                }
                var param = new
                {
                    @searchText = q ?? "",
                    @page = page
                };
                var objectResult = _dapperUnitOfWork.GetRepository().QueryMultiple(renderQuery, param, null, commandType,
                    gr =>
                        gr.Read<T>(),
                    gr =>
                        gr.Read<Int32>().FirstOrDefault()
                    );

                if (objectResult != null && objectResult[0] != null && objectResult[1] != null)
                {
                    lstStaffModel = (List<T>)objectResult[0];
                    totalCount = (int)objectResult[1];
                    int totalPage = (int)Math.Ceiling((decimal)totalCount / 10);
                    return new Response<List<T>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, lstStaffModel, 10, totalCount, totalPage);
                }
                return new Response<List<T>>(Constant.ErrorCode.ERRORSYSTEM_CODE, Constant.ErrorCode.ERRORSYSTEM_MESS);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<T>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Lấy combobox theo quan trường nào đó sử dụng eager load
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// /// <param name="hasStatusColumn">đối với bảng có cột trạng thái</param>
        /// <returns></returns>

        public async Task<Response<List<T>>> GetComboboxByField<T>(string layoutCode, string keySearch, int q, bool? hasStatusColumn = false)
        {
            List<T> lstStaffModel = new List<T>();
            int totalCount = 0;

            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetComboboxByField");
                if (hasStatusColumn == true)
                {
                    sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetComboboxByFieldHasStatusColumn");
                }
                string renderQuery = String.Format(sqlQuery, layoutCode, keySearch, q);
                var param = new
                {

                };
                var objectResult = _dapperUnitOfWork.GetRepository().QueryMultiple(renderQuery, param, null, CommandType.Text,
                    gr =>
                        gr.Read<T>(),
                    gr =>
                        gr.Read<Int32>().FirstOrDefault()
                    );

                if (objectResult != null && objectResult[0] != null && objectResult[1] != null)
                {
                    lstStaffModel = (List<T>)objectResult[0];
                    totalCount = (int)objectResult[1];
                    int totalPage = (int)Math.Ceiling((decimal)totalCount / 10);
                    return new Response<List<T>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, lstStaffModel, 10, totalCount, totalPage);
                }
                return new Response<List<T>>(Constant.ErrorCode.ERRORSYSTEM_CODE, Constant.ErrorCode.ERRORSYSTEM_MESS);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<T>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }


        /// <summary>
        /// Grid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityGeneric"></param>
        /// <returns></returns>
        public async Task<Response<List<T>>> GetFilter<T>(EntityGeneric entityGeneric)
        {
            List<T> listData = new List<T>();
            int totalCount = 0;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{entityGeneric.LayoutCode}.json", entityGeneric.LayoutCode, "Proc_Grid");

                var param = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(entityGeneric.CustomPagingData));
                var dyParameters = new DynamicParameters();
                foreach (var kvp in param)
                {
                    dyParameters.Add(kvp.Key, kvp.Value);
                }

                List<object> lst = null;
                List<object> objReturn = null;
                var keyCached = KeyCacheHelper.GenCacheKey(this.GetType().Namespace + ":" + this.GetType().Name + ":" + MethodBase.GetCurrentMethod().Name, param);
                if (entityGeneric.IsGetCache)
                {
                    lst = _cached.Get<List<object>>(keyCached);
                }
                // Nếu ko có thì lấy từ db rồi lại lưu vào cached
                if ((lst == null || !lst.Any()))
                {
                    objReturn = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, dyParameters, null, CommandType.StoredProcedure,
                    gr =>
                        gr.Read<T>(),
                    gr =>
                        gr.Read<Int32>().FirstOrDefault()
                    );
                    _cached.Add(keyCached, objReturn, 60);
                }
                else
                {
                    objReturn = lst;
                }
                if (objReturn != null && objReturn[0] != null && objReturn[1] != null)
                {
                    listData = (List<T>)objReturn[0];
                    totalCount = (int)objReturn[1];
                    int totalPage = (int)Math.Ceiling((decimal)totalCount / entityGeneric.PageSize);
                    return new Response<List<T>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, listData, listData.Count(), totalCount, totalPage, entityGeneric.PageIndex, entityGeneric.PageSize);
                }
                else
                {
                    return new Response<List<T>>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<T>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Lấy danh sách từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<T>>> GetComboboxFromQTHT<T>(string layoutCode, string keySearch, int q, bool? hasStatusColumn = false)
        {
            List<T> lstStaffModel = new List<T>();
            int totalCount = 0;
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (var connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetComboboxByField");
                        if (hasStatusColumn == true)
                        {
                            sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetComboboxByFieldHasStatusColumn");
                        }
                        var param = new object { };
                        string renderQuery = String.Format(sqlQuery, layoutCode, keySearch, q);
                        var objectResult = connection.QueryMultiple(renderQuery, param, null, null, CommandType.Text);


                        if (objectResult != null)
                        {
                            lstStaffModel = objectResult.Read<T>().ToList();
                            totalCount = objectResult.Read<int>().SingleOrDefault();
                            int totalPage = (int)Math.Ceiling((decimal)totalCount / 10);
                            return new Response<List<T>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, lstStaffModel, 10, totalCount, totalPage);
                        }
                        return new Response<List<T>>(Constant.ErrorCode.ERRORSYSTEM_CODE, Constant.ErrorCode.ERRORSYSTEM_MESS);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }

        public async Task<Response<T>> GetNameLocation<T>(string layoutCode, string keySearch, int q)
        {
            T lstStaffModel;
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (var connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetNameLocation");
                        var param = new object { };
                        string renderQuery = String.Format(sqlQuery, layoutCode, keySearch, q);
                        var objectResult = await connection.QueryFirstAsync<object>(renderQuery, param, null, null, CommandType.Text);


                        if (objectResult != null)
                        {
                            lstStaffModel = (T)objectResult;
                            return new Response<T>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, lstStaffModel);
                        }
                        return new Response<T>(Constant.ErrorCode.ERRORSYSTEM_CODE, Constant.ErrorCode.ERRORSYSTEM_MESS);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Hàm chung lấy về combobox nhân viên sử dụng lazyload phân trang
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode">Tên bảng</param>
        /// <param name="keySearch">Tên trường dùng để tìm trên combobox</param>
        /// <param name="q">Giá trị của keySearch</param>
        /// <param name="page">Trang thứ mấy</param>
        /// <param name="commandType">Lấy từ mặc định hay lấy từ store procedure để custom lấy thêm dữ liệu</param>
        /// <returns></returns>
        public async Task<Response<List<StaffComboboxModel>>> GetComboboxStaff(string layoutCode, string keySearch, string q, int page, CommandType? commandType = null)
        {
            var conn = _dapperUnitOfWork.GetRepository().GetDbConnection();

            List<StaffComboboxModel> lstStaffModel = new List<StaffComboboxModel>();
            int totalCount = 0;

            try
            {
                var orgs = GetPermissionAccessOrganizationId();

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetCombobox");
                string renderQuery = String.Format(sqlQuery, layoutCode, keySearch, q ?? "");
                if (commandType == CommandType.StoredProcedure)
                {
                    sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", $"proc_GetCombobox");
                    renderQuery = String.Format(sqlQuery, layoutCode, keySearch);
                }
                var param = new
                {
                    @searchText = q ?? "",
                    @DeptID = orgs?.Count() > 0 ? String.Join(',', orgs?.Select(g=>g.id)) : "",
                    @page = page
                };
                var objectResult = _dapperUnitOfWork.GetRepository().QueryMultiple(renderQuery, param, null, commandType,
                    gr =>
                        gr.Read<StaffComboboxModel>(),
                    gr =>
                        gr.Read<Int32>().FirstOrDefault()
                    );

                if (objectResult != null && objectResult[0] != null && objectResult[1] != null)
                {
                    lstStaffModel = (List<StaffComboboxModel>)objectResult[0];
                    totalCount = (int)objectResult[1];
                    int totalPage = (int)Math.Ceiling((decimal)totalCount / 10);
                    return new Response<List<StaffComboboxModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, lstStaffModel, 10, totalCount, totalPage);
                }
                return new Response<List<StaffComboboxModel>>(Constant.ErrorCode.ERRORSYSTEM_CODE, Constant.ErrorCode.ERRORSYSTEM_MESS);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<StaffComboboxModel>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// hàm lấy về danh sách id của cơ cấu tổ chức mà user có quyền truy cập
        /// </summary>
        /// <param name="organizationId">Id của cơ cấu tổ chức</param>
        /// <returns></returns>
        public IEnumerable<OrganizationModel> GetPermissionAccessOrganizationId()
        {
            var userInfo = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
            IDbConnection connection = null;
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                connection = new SqlConnection(connectionString);
                //connection.Open();
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();
                    var ret = dal.Query<OrganizationModel>("proc_recursive_organization", new { @ApplicationId = userInfo.ApplicationId, @UserId = userInfo.UserId }, null, CommandType.StoredProcedure);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                connection.Close();
                return null;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetComboboxByFieldNoCount");
                //if (hasStatusColumn == true)
                //{
                //    sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetComboboxByFieldHasStatusColumnNoCount");
                //}
                string renderQuery = String.Format(sqlQuery, "Users", "1", "1");
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var lstResult = await unitOfwork.GetRepository().QueryAsync<UserModel>(renderQuery, null, null, CommandType.Text);
                        if ((lstResult != null) && (lstResult.Count() > 0))
                        {
                            return lstResult.ToList();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách quyền của user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        public async Task<Response<List<Permissions>>> GetPermissionByUser(int userID, int? appID)
        {
            try
            {
                List<Permissions> permissions = new List<Permissions>();
                List<GroupRoleBase> groupRoles = new List<GroupRoleBase>();
                List<RoleBase> roles = new List<RoleBase>();
                List<RightBase> rights = new List<RightBase>();

                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        string sql = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "sp_Users_GetPermission");
                        unitOfwork.GetRepository().QueryMultiple(sql, new { @UserId = userID, @ApplicationId = appID }, null, CommandType.StoredProcedure,
                           gr => groupRoles = gr.Read<GroupRoleBase>().ToList(),
                           gr => roles = gr.Read<RoleBase>().GroupBy(r => r.RoleId).Select(r => r.First()).ToList(),
                           gr => rights = gr.Read<RightBase>().ToList()
                          );

                        foreach (var role in roles)
                        {
                            permissions.Add(new Permissions
                            {
                                RoleCode = role.RoleCode,
                                RightCodes = rights.Where(r => r.RoleId.Equals(role.RoleId)).Select(r => r.RightCode).Distinct().ToList()
                            });
                        }
                    }
                }

                return new Response<List<Permissions>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, permissions);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<Permissions>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<UserInfoCacheModel>> GetUserInfo(string userName)
        {
            UserInfoCacheModel user;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetUserByUserName");
                string renderQuery = String.Format(sqlQuery, userName);
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        user = (await unitOfwork.GetRepository().QueryAsync<UserInfoCacheModel>(renderQuery, null, null, CommandType.Text)).FirstOrDefault();
                    }
                }

                return new Response<UserInfoCacheModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, user);
            }
            catch
            {
                return new Response<UserInfoCacheModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteMany<T>(List<int> recordID)
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
        /// Xóa bản ghi theo bảng được chọn
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteMany<T>(List<Guid> recordID)
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
                var dataRecords = JsonConvert.DeserializeObject<List<int>>(objectDelete[0].ToString());

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
                //else
                //{
                //    if (dataRecords != null && dataRecords.Count > 0)
                //    {
                //        // query lấy tất cả các bảng có trường có tên (T + ID)
                //        string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "GetAll_Table_Related");
                //        string relatedID = layout + "ID"; // tên trường 
                //        var result = await _dapperUnitOfWork.GetRepository().QueryAsync<string>(sql: sqlQuery, new { @relatedID = relatedID });
                //        if (result.Count() > 0)
                //        {
                //            foreach (var item in dataTable)
                //            {
                //                if (!item.Equals(layout))
                //                {
                //                    // đếm số bản ghi
                //                    string sqlQuery2 = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "CountByIdRecord");
                //                    string queryFormat = string.Format(sqlQuery2, item, layout + "ID");
                //                    var result2 = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<int>(sql: queryFormat, new { @recordIDs = dataRecords });
                //                    if (result2 > 0)
                //                    {
                //                        checkUse = true;
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
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

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn check bản ghi có đang được sử dụng không
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyCheckUseRecordGuid<T>(List<object> objectDelete)
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
                //else
                //{
                //    if (dataRecords != null && dataRecords.Count > 0)
                //    {
                //        // query lấy tất cả các bảng có trường có tên (T + ID)
                //        string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "GetAll_Table_Related");
                //        string relatedID = layout + "ID"; // tên trường 
                //        var result = await _dapperUnitOfWork.GetRepository().QueryAsync<string>(sql: sqlQuery, new { @relatedID = relatedID });
                //        if (result.Count() > 0)
                //        {
                //            foreach (var item in dataTable)
                //            {
                //                if (!item.Equals(layout))
                //                {
                //                    // đếm số bản ghi
                //                    string sqlQuery2 = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "CountByIdRecord");
                //                    string queryFormat = string.Format(sqlQuery2, item, layout + "ID");
                //                    var result2 = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<int>(sql: queryFormat, new { @recordIDs = dataRecords });
                //                    if (result2 > 0)
                //                    {
                //                        checkUse = true;
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
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
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        /// <summary>
        /// Get list user
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
                    var ret = dal.Query<BaseUserModel>("SELECT UserId, UserName, FullName, Id FROM Users WHERE ApplicationId = @applicationId and Status != @status;", new { applicationId = user.ApplicationId, status = Constant.StatusRecord.DELETED });
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
    }
}
