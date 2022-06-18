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
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using SV.HRM.DataProcess.Infrastructure.Impl;
using NLog;

namespace SV.HRM.Service.QuanTriDanhMuc
{

    public class BaseHandler : IBaseHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly ICached _cached;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IConfiguration _configuration;

        public BaseHandler(IMapper mapper, ICached cached, IDapperUnitOfWork dapperUnitOfWork, IConfiguration configuration)
        {
            _mapper = mapper;
            _cached = cached;
            _dapperUnitOfWork = dapperUnitOfWork;
            _configuration = configuration;
        }

        public async Task<Response<bool>> CreateObject<T>(string layout, T entityGeneric)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(entityGeneric);
                var objSave = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{layout}.json", layout, "Proc_Create");
                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, objSave, null, CommandType.StoredProcedure);

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
                var dataRecords = JsonConvert.DeserializeObject<List<string>>(objectDelete[0].ToString());

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
                logger.Error($"[ERROR]: {ex}");
                throw ex;
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

                if (param != null && param.ContainsKey("DeptID"))
                {
                    var organization = GetListChildOrganizationAndOrganizationId(Convert.ToInt32(param["DeptID"])).Select(g => g.OrganizationId);
                    param["DeptID"] = string.Join(',', organization);
                }
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

        public async Task<Response<bool>> UpdateObject<T>(string layout, int id, T entityGeneric)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(entityGeneric);
                var objSave = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{layout}.json", layout, "Proc_Update");
                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, objSave, null, CommandType.StoredProcedure);


                if (outputResult == Constant.SUCCESS) //lưu thành công
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else if (outputResult == Constant.DATA_IS_EXIST) // trường hợp quá trình công tác cũ có ngày kết thúc lớn hơn hoặc bằng ngày bắt đầu của quá trình công tác chuẩn bị tạo
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
            if (result == Constant.SUCCESS) //lưu thành công
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
                        var ret = dal.Query<OrganizationModel>("SELECT OrganizationID,OrganizationName FROM Organizations WHERE Status = @status;", new { status = Constant.StatusRecord.ACTIVE });
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
                        var ret = dal.Query<EthnicBase>("SELECT EthnicId,EthnicName FROM Ethnics WHERE Status = @status;", new { status = Constant.StatusRecord.ACTIVE });
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
                        var ret = dal.Query<CountryBase>("SELECT CountryId,CountryName FROM Countries WHERE Status = @status;", new { status = Constant.StatusRecord.ACTIVE });
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
                        var ret = dal.Query<LocationBaseModel>("SELECT LocationID,LocationName FROM Locations WHERE Status = @status;", new { status = Constant.StatusRecord.ACTIVE });
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
                        var ret = dal.Query<DistrictBase>("SELECT DistrictId,DistrictName FROM Districts WHERE Status = @status;", new { status = Constant.StatusRecord.ACTIVE });
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
                        var ret = dal.Query<WardBase>("SELECT WardId,WardName FROM Wards WHERE Status = @status;", new { status = Constant.StatusRecord.ACTIVE });
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
        public IEnumerable<OrganizationModel> GetListChildOrganizationAndOrganizationId(int organizationId)
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
                    var ret = dal.Query<OrganizationModel>("sp_GetListChildOrganizationAndOrganizationId", new { OrganizationId = organizationId }, null, CommandType.StoredProcedure);
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
                        var res = dal.QuerySingleOrDefault<EthnicBase>("SELECT EthnicId,EthnicName FROM Ethnics WHERE EthnicId = @EthnicId AND Status = @status;", new { EthnicId = id, status = Constant.StatusRecord.ACTIVE });
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
                        var res = dal.QuerySingleOrDefault<CountryBase>("SELECT CountryId,CountryName FROM Countries WHERE CountryId=@CountryId AND Status = @status;", new { CountryId = id, status = Constant.StatusRecord.ACTIVE });
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
                        var res = dal.QuerySingleOrDefault<LocationBaseModel>("SELECT LocationID,LocationName FROM Locations WHERE LocationID=@LocationID AND Status = @status;", new { LocationID = id, status = Constant.StatusRecord.ACTIVE });
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
                        var res = dal.QuerySingleOrDefault<DistrictBase>("SELECT DistrictId,DistrictName FROM Districts WHERE DistrictId=@DistrictId AND Status = @status;", new { DistrictId = id, status = Constant.StatusRecord.ACTIVE });
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
                        var res = dal.QuerySingleOrDefault<WardBase>("SELECT WardId,WardName FROM Wards WHERE WardId=@WardId AND Status = @status;", new { WardId = id, status = Constant.StatusRecord.ACTIVE });
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
    }
}
