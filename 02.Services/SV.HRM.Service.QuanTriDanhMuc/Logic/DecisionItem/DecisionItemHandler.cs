using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public class DecisionItemHandler : IDecisionItemHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;

        public DecisionItemHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler)
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
        }
        /// <summary>
        /// thêm mới loại quyết định
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(DecisionItemCreateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(DecisionItem)}.json", nameof(DecisionItem), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@DecisionItemCode", model.DecisionItemCode);
                param.Add("@DecisionItemName", model.DecisionItemName);
                param.Add("@TemplateFile", model.TemplateFile);
                param.Add("@InactiveDate", model.InactiveDate);
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreationDate", DateTime.Now);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

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
        /// tìm kiếm loại quyết định
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<DecisionItemModel>> FindById(int recordID)
        {
            DecisionItemModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(DecisionItem)}.json", nameof(DecisionItem), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<DecisionItemModel>(sqlQuery, new { DecisionItemID = recordID });
                if (result != null)
                {
                    return new Response<DecisionItemModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<DecisionItemModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<DecisionItemModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }
        /// <summary>
        /// cập nhật loại quyết định
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, DecisionItemUpdateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(DecisionItem)}.json", nameof(DecisionItem), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@DecisionItemID", id);
                param.Add("@DecisionItemCode", model.DecisionItemCode);
                param.Add("@DecisionItemName", model.DecisionItemName);
                param.Add("@TemplateFile", model.TemplateFile);
                param.Add("@InactiveDate", model.InactiveDate);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

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
        // tìm kiếm bản có đang được sử dụng hay không
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            try
            {
                bool checkUse = false; // biến check bản ghi có được sử dụng không
                                       // query lấy tất cả các bảng có trường có tên (T + ID)
                string layout = typeof(DecisionItem).Name;
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "GetAll_Table_Related");
                string relatedID = layout + "ID"; // tên trường
                var dataRecords = JsonConvert.DeserializeObject<int>(recordID[0].ToString());
                var dataTable = JsonConvert.DeserializeObject<List<string>>(recordID[1].ToString());
                //var dataTable = await _dapperUnitOfWork.GetRepository().QueryAsync<string>(sql: sqlQuery, new { @relatedID = relatedID });
                if(dataTable.Count() <= 0)
                {
                    dataTable = (List<string>)await _dapperUnitOfWork.GetRepository().QueryAsync<string>(sql: sqlQuery, new { @relatedID = relatedID });
                }
                if (dataRecords > 0 && dataTable.Count() > 0)
                {
                    foreach (var tbl in dataTable)
                    {
                        if (!layout.Equals(tbl))
                        {
                            string layoutChild = tbl; // bảng liên quan
                                                      // đếm số bản ghi > 0 (có bảng đang sử dụng bản ghi)
                            string sqlQuery2 = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(DecisionItem)}.json", nameof(DecisionItem), "CountByIdRecord");
                            string queryFormat = string.Format(sqlQuery2, layoutChild, nameof(DecisionItem) + "ID");
                            var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<int>(sql: queryFormat, new { @DecisionItemID = dataRecords });
                            if (result > 0)
                            {
                                checkUse = true;
                                break;
                            }
                        }
                    }
                }
                if (!checkUse)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
            catch (Exception)
            {
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
                        var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<DecisionItem>(sql: queryFormat, new { @recordIDs = dataRecords });
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
                    return await _baseHandler.DeleteManyCheckUseRecord<DecisionItem>(data);
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
    }
}
