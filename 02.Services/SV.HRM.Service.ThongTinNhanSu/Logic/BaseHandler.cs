using AutoMapper;
using Dapper;
using SV.HRM.Caching.Common;
using SV.HRM.Caching.Interface;
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
using static SV.HRM.Core.Utils.Constant;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using SV.HRM.DataProcess.Infrastructure.Impl;
using Microsoft.AspNetCore.Http;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
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
            IHttpContextAccessor httpContextAccessor
            )
        {
            _mapper = mapper;
            _cached = cached;
            _dapperUnitOfWork = dapperUnitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<bool>> CreateObject<T>(string layout, T entityGeneric)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(entityGeneric);
                var objSave = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{layout}.json", layout, "Proc_Create");
                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, objSave, null, CommandType.StoredProcedure);


                if (outputResult == SUCCESS) //lưu thành công
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
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<T>> FindById<T>(int recordID)
        {
            T result;
            try
            {
                string layout = typeof(T).Name;
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "FindById");
                string queryFormat = string.Format(sqlQuery, layout, layout + "ID");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<T>(queryFormat, new { @recordIDs = recordID });
                if (result != null)
                {
                    return new Response<T>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<T>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                throw ex;
                //return new Response<T>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);
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

                dyParameters.Add("PageSize", entityGeneric.PageSize);
                dyParameters.Add("PageIndex", entityGeneric.PageIndex);
                string[] paramFilters = entityGeneric.Columns?.Split(',');
                List<string> valueDeptID = new List<string>();
                foreach (var filter in paramFilters)
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        var valueParam = entityGeneric.Filters?.SingleOrDefault(g => g.name == filter)?.value ?? null;
                        if (filter == "DeptID" && valueParam != null)
                        {
                            valueDeptID.Add(valueParam);
                        }
                        if (filter == "DeptIDCurrent" && valueParam != null)
                        {
                            string org = Convert.ToString(valueParam);
                            var organization = GetListChildOrganizationAndOrganizationId(org)?.Select(g => g.OrganizationId);
                            valueParam = string.Join(',', organization);
                        }

                        dyParameters.Add(filter, valueParam);
                    }
                }
                if (param != null && param.ContainsKey("DeptID"))
                {
                    if (param["DeptID"] == null)
                    {
                        var orgs = GetPermissionAccessOrganizationId();
                        foreach (var item in orgs)
                        {
                            valueDeptID.Add(item.id.ToString());
                        }
                    }
                    valueDeptID.Add(param["DeptID"]?.ToString());

                    var organization = GetListChildOrganizationAndOrganizationId(String.Join(',', valueDeptID)).Select(g => g.OrganizationId);
                    param["DeptID"] = string.Join(',', organization);
                }

                foreach (var kvp in param)
                {
                    dyParameters.Add(kvp.Key, kvp.Value);
                }

                List<object> lst = null;
                List<object> objReturn = null;
                var keyCached = KeyCacheHelper.GenCacheKey(this.GetType().Namespace + ":" + this.GetType().Name + ":" + MethodBase.GetCurrentMethod().Name, param);
                //if (entityGeneric.IsGetCache)
                //{
                //    lst = _cached.Get<List<object>>(keyCached);
                //}
                // Nếu ko có thì lấy từ db rồi lại lưu vào cached
                if ((lst == null || !lst.Any()))
                {
                    objReturn = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, dyParameters, null, CommandType.StoredProcedure,
                    gr =>
                        gr.Read<T>(),
                    gr =>
                        gr.Read<Int32>().FirstOrDefault()
                    );
                    //_cached.Add(keyCached, objReturn, 60);
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

        public async Task<Response<bool>> UpdateObject<T>(string layout, int id, T entityGeneric)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(entityGeneric);
                var objSave = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{layout}.json", layout, "Proc_Update");
                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, objSave, null, CommandType.StoredProcedure);


                if (outputResult == SUCCESS) //lưu thành công
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else if (outputResult == DATA_IS_EXIST) // trường hợp quá trình công tác cũ có ngày kết thúc lớn hơn hoặc bằng ngày bắt đầu của quá trình công tác chuẩn bị tạo
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE_HISTORY, Constant.ErrorCode.FAIL_MESS_HISTORY, false);
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
        /// Render chuỗi tring câu lệnh sql insert nhiều file
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private IList<string> GetSqlsInBatches(List<HRM_AttachmentModel> records)
        {
            var insertSql = $"INSERT INTO [HRM_Attachment] ({nameof(HRM_AttachmentModel.Id)},{nameof(HRM_AttachmentModel.NodeId)}" +
                $",{nameof(HRM_AttachmentModel.Type)},{nameof(HRM_AttachmentModel.TypeId)},{nameof(HRM_AttachmentModel.Url)}" +
                $",{nameof(HRM_AttachmentModel.Name)},{nameof(HRM_AttachmentModel.PhysicalName)},{nameof(HRM_AttachmentModel.Size)}" +
                $",{nameof(HRM_AttachmentModel.Extension)}) VALUES ";
            var valuesSql = "('{0}','{1}',{2},{3},'{4}','{5}','{6}',{7},'{8}')";
            var batchSize = 1000;

            var sqlsToExecute = new List<string>();
            var numberOfBatches = (int)Math.Ceiling((double)records.Count / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var recordToInsert = records.Skip(i * batchSize).Take(batchSize);
                var valuesToInsert = recordToInsert.Select(u => string.Format(valuesSql, u.Id, u.Id, u.Type, u.TypeId, u.PhysicalPath, u.Name, u.PhysicalName, u.Size, u.Extension));
                sqlsToExecute.Add(insertSql + string.Join(',', valuesToInsert));
            }

            return sqlsToExecute;
        }

        /// <summary>
        /// Insert File đính kèm
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<Response<bool>> InsertManyFile(List<HRM_AttachmentModel> records)
        {
            int result = 0;
            IList<string> sqls = GetSqlsInBatches(records);
            foreach (var sql in sqls)
            {
                result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sql);
            }
            if (result == SUCCESS) //lưu thành công
            {
                return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
            }
            else //trường hợp còn lại cụ thể là store proc trả ra = 0 là trường hợp exception lỗi của SQL
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        /// <summary>
        /// Xóa danh sách file đính kèm 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyFile(int Type, List<int> TypeId)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "DeleteManyFile");
                int result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, new { @Type = Type, @TypeId = TypeId });
                return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        /// <summary>
        /// Lấy thông tin file đính kèm theo Type và TypeId
        /// </summary>
        /// <param name="Type">Loại danh mục</param>
        /// <param name="TypeId">Id của bản ghi thuộc loại danh mục</param>
        /// <returns></returns>

        public async Task<Response<List<HRM_AttachmentModel>>> GetAttachmentByTypeAndTypeId(int Type, int TypeId)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "GetAttachmentByTypeAndTypeId");
                var result = await _dapperUnitOfWork.GetRepository().QueryAsync<HRM_AttachmentModel>(sqlQuery, new { @Type = Type, @TypeId = TypeId });
                if (result != null)
                {
                    return new Response<List<HRM_AttachmentModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result.ToList());
                }
                return new Response<List<HRM_AttachmentModel>>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, null);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<HRM_AttachmentModel>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Lấy danh sách organization từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        public List<OrganizationModel> GetOrganization()
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var dal = unitOfwork.GetRepository();
                        var ret = dal.Query<OrganizationModel>("SELECT OrganizationID,OrganizationName FROM Organizations WHERE Status = @status;", new { status = StatusRecord.ACTIVE });
                        return ret.ToList();
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
        /// Check trùng theo trường nào đó
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layoutCode"></param>
        /// <param name="keySearch"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public async Task<Response<bool>> CheckDuplicate<T>(string whereClauses)
        {
            try
            {
                string layoutCode = typeof(T).Name;
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "CheckDuplicate");
                string queryFormat = string.Format(sqlQuery, layoutCode, whereClauses);
                int result = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(queryFormat, new { });
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
        /// Lấy danh sách dân tộc từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EthnicBase> GetEthnics()
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var dal = unitOfwork.GetRepository();
                        var ret = dal.Query<EthnicBase>("SELECT EthnicId,EthnicName FROM Ethnics WHERE Status = @status;", new { status = StatusRecord.ACTIVE });
                        return ret;
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
        /// Lấy danh sách quốc tịch từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CountryBase> GetCountries()
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var dal = unitOfwork.GetRepository();
                        var ret = dal.Query<CountryBase>("SELECT CountryId,CountryName FROM Countries WHERE Status = @status;", new { status = StatusRecord.ACTIVE });
                        return ret;
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
        /// Lấy danh sách tỉnh/thành từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LocationBaseModel> GetLocations()
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var dal = unitOfwork.GetRepository();
                        var ret = dal.Query<LocationBaseModel>("SELECT LocationID,LocationName FROM Locations WHERE Status = @status;", new { status = StatusRecord.ACTIVE });
                        return ret;
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
        /// Lấy danh sách quận/huyện từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DistrictBase> GetDistricts()
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var dal = unitOfwork.GetRepository();
                        var ret = dal.Query<DistrictBase>("SELECT DistrictId,DistrictName FROM Districts WHERE Status = @status;", new { status = StatusRecord.ACTIVE });
                        return ret;
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
        /// Lấy danh sách xã/phường từ phần mềm QTHT
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WardBase> GetWards()
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var dal = unitOfwork.GetRepository();
                        var ret = dal.Query<WardBase>("SELECT WardId,WardName FROM Wards WHERE Status = @status;", new { status = StatusRecord.ACTIVE });
                        return ret;
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
        /// hàm lấy về danh sách id của cơ cấu tổ chức con và chính nó
        /// </summary>
        /// <param name="organizationId">Id của cơ cấu tổ chức</param>
        /// <returns></returns>
        public IEnumerable<OrganizationModel> GetListChildOrganizationAndOrganizationId(string organizationIds)
        {
            IDbConnection connection = null;
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                connection = new SqlConnection(connectionString);
                //connection.Open();
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    var dal = unitOfwork.GetRepository();
                    var ret = dal.Query<OrganizationModel>("sp_GetListChildOrganizationAndOrganizationId", new { OrganizationId = organizationIds }, null, CommandType.StoredProcedure);
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

        /// <summary>
        /// Tìm bản ghi quá trình công tác gần nhất có trạng thái khác điều động tăng cường
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<HistoryModel> FindRecordLastest(int staffID)
        {
            HistoryModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(History)}.json", nameof(History), "FindRecordLastest");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<HistoryModel>(sqlQuery, new { StaffID = staffID });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Lấy dân tộc theo id từ phần mềm QTHT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EthnicBase GetEthnicsById(int id)
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var dal = unitOfwork.GetRepository();
                        var res = dal.QuerySingleOrDefault<EthnicBase>("SELECT EthnicId,EthnicName FROM Ethnics WHERE EthnicId = @EthnicId AND Status = @status;", new { EthnicId = id, status = StatusRecord.ACTIVE });
                        return res;
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
        /// Lấy quốc tịch theo id từ phần mềm QTHT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CountryBase GetCountriesById(int id)
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var dal = unitOfwork.GetRepository();
                        var res = dal.QuerySingleOrDefault<CountryBase>("SELECT CountryId,CountryName FROM Countries WHERE CountryId=@CountryId AND Status = @status;", new { CountryId = id, status = StatusRecord.ACTIVE });
                        return res;
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
        /// Lấy tỉnh/thành theo id từ phần mềm QTHT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LocationBaseModel GetLocationsById(int id)
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var dal = unitOfwork.GetRepository();
                        var res = dal.QuerySingleOrDefault<LocationBaseModel>("SELECT LocationID,LocationName FROM Locations WHERE LocationID=@LocationID AND Status = @status;", new { LocationID = id, status = StatusRecord.ACTIVE });
                        return res;
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
        /// Lấy quận/huyện theo id từ phần mềm QTHT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DistrictBase GetDistrictsById(int id)
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var dal = unitOfwork.GetRepository();
                        var res = dal.QuerySingleOrDefault<DistrictBase>("SELECT DistrictId,DistrictName FROM Districts WHERE DistrictId=@DistrictId AND Status = @status;", new { DistrictId = id, status = StatusRecord.ACTIVE });
                        return res;
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
        /// Lấy xã/phường theo id từ phần mềm QTHT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WardBase GetWardsById(int id)
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var dal = unitOfwork.GetRepository();
                        var res = dal.QuerySingleOrDefault<WardBase>("SELECT WardId,WardName FROM Wards WHERE WardId=@WardId AND Status = @status;", new { WardId = id, status = StatusRecord.ACTIVE });
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }

        public List<T> GetListData<T>(string tableName, string keySearch, int value, bool? hasStatusColumn = false)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetComboboxByField");
                if (hasStatusColumn == true)
                {
                    sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetComboboxByFieldHasStatusColumn");
                }
                string renderQuery = String.Format(sqlQuery, tableName, keySearch, value);
                var lstResult = _dapperUnitOfWork.GetRepository().Query<T>(renderQuery, null, null, CommandType.Text).ToList();

                return lstResult;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new List<T>();
            }
        }

        public List<T> GetSystemListData<T>(string tableName, string keySearch, int value, bool? hasStatusColumn = false)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetComboboxByField");
                if (hasStatusColumn == true)
                {
                    sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Base.json", "Base", "GetComboboxByFieldHasStatusColumn");
                }
                string renderQuery = String.Format(sqlQuery, tableName, keySearch, value);
                string connectionString = _configuration.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    using (var unitOfwork = new DapperUnitOfWork(connection))
                    {
                        var lstResult = unitOfwork.GetRepository().Query<T>(renderQuery, null, null, CommandType.Text).ToList();
                        return lstResult;
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
        /// Tạo log các thao tác
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Response<bool> CreateLog(SystemLogModel model)
        {
            var currenUser = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
            string apiUrl = StaticVariable.QTHT_API_URL + "SystemLog/CreateLog";
            var header = new List<KeyValuePair<string, string>>
            {
                    new KeyValuePair<string, string>(Constant.RequestHeader.AUTHORIZATION, currenUser.AccessToken)
            };

            return RestsharpUtils.Post<Response<bool>>(apiUrl, header, model);
        }
    }
}
