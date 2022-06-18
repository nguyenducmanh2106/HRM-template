using Dapper;
using Microsoft.Extensions.Logging;
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
    public class ShiftHandler : IShiftHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;

        public ShiftHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler)
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
        }
        /// <summary>
        /// thêm mới công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(ShiftCreateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Shift)}.json", nameof(Shift), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@ShiftCode", model.ShiftCode);
                param.Add("@ShiftName", model.ShiftName);
                param.Add("@ShiftType", model.ShiftType);
                param.Add("@CI", model.CI);
                param.Add("@CO", model.CO);
                param.Add("@RestFrom", model.RestFrom);
                param.Add("@RestTo", model.RestTo);
                param.Add("@Note", model.Note);
                param.Add("@WorkingTime", model.WorkingTime);
                param.Add("@RestTime", model.RestTime);
                param.Add("@ThresholdCheckTimeBeforeShift", model.ThresholdCheckTimeBeforeShift);
                param.Add("@ThresholdCheckTimeAfterShift", model.ThresholdCheckTimeAfterShift);
                param.Add("@InactiveDate", model.InactiveDate);
                param.Add("@ShiftGroup", model.ShiftGroup);
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreationDate", DateTime.Now);
                param.Add("@LastUpdatedBy", model.CreatedBy);
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
        /// tìm kiếm công theo id
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<ShiftModel>> FindById(int recordID)
        {
            ShiftModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Shift)}.json", nameof(Shift), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<ShiftModel>(sqlQuery, new { ShiftID = recordID });
                if (result != null)
                {
                    return new Response<ShiftModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<ShiftModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<ShiftModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }
        /// <summary>
        /// cập nhật công theo id và model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<Response<bool>> Update(int id, ShiftUpdateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Shift)}.json", nameof(Shift), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@ShiftID", id);
                param.Add("@ShiftCode", model.ShiftCode);
                param.Add("@ShiftName", model.ShiftName);
                param.Add("@ShiftType", model.ShiftType);
                param.Add("@CI", model.CI);
                param.Add("@CO", model.CO);
                param.Add("@RestFrom", model.RestFrom);
                param.Add("@RestTo", model.RestTo);
                param.Add("@Note", model.Note);
                param.Add("@WorkingTime", model.WorkingTime);
                param.Add("@RestTime", model.RestTime);
                param.Add("@ThresholdCheckTimeBeforeShift", model.ThresholdCheckTimeBeforeShift);
                param.Add("@ThresholdCheckTimeAfterShift", model.ThresholdCheckTimeAfterShift);
                param.Add("@InactiveDate", model.InactiveDate);
                param.Add("@ShiftGroup", model.ShiftGroup);
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
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            try
            {
                bool checkUse = false; // biến check bản ghi có được sử dụng không

                var dataRecords = JsonConvert.DeserializeObject<int>(recordID[0].ToString());
                var dataTable = JsonConvert.DeserializeObject<List<string>>(recordID[1].ToString());
                if (dataRecords > 0 && dataTable.Count > 0)
                {
                    foreach (var tbl in dataTable)
                    {
                        string layoutChild = tbl; // bảng liên quan
                        string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Shift)}.json", nameof(Shift), "CountByIdRecord");
                        string queryFormat = string.Format(sqlQuery, layoutChild, nameof(Shift) + "ID");
                        var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<int>(sql: queryFormat, new { @ShiftID = dataRecords });
                        if (result > 0)
                        {
                            checkUse = true;
                            break;
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
                        var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<Shift>(sql: queryFormat, new { @recordIDs = dataRecords });
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
                    return await _baseHandler.DeleteManyCheckUseRecord<Shift>(data);
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
