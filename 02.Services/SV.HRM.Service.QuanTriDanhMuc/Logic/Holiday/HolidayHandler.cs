using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public class HolidayHandler : IHolidayHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;

        public HolidayHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler)
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
        }

        public async Task<Response<bool>> Create(HolidayCreateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Holiday)}.json", nameof(Holiday), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@HolidayName", model.HolidayName);
                param.Add("@HldDate", model.HldDate);
                param.Add("@IsAnnual", 0);
                param.Add("@FullDay", 1);
                param.Add("@OvertimePct", 0);
                param.Add("@NightAllowancePct", 0);
                param.Add("@HolidayType", model.HolidayType);
                param.Add("@LeaveWithSalary", 0);
                param.Add("@OVTType", model.OVTType);
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

        public async Task<Response<HolidayModel>> FindById(int recordID)
        {
            HolidayModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Holiday)}.json", nameof(Holiday), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<HolidayModel>(sqlQuery, new { HolidayID = recordID });
                if (result != null)
                {
                    return new Response<HolidayModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<HolidayModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<HolidayModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<bool>> Update(int id, HolidayUpdateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Holiday)}.json", nameof(Holiday), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@HolidayID", id);
                param.Add("@HolidayName", model.HolidayName);
                param.Add("@HldDate", model.HldDate);
                param.Add("@IsAnnual", 0);
                param.Add("@FullDay", 1);
                param.Add("@OvertimePct", 0);
                param.Add("@NightAllowancePct", 0);
                param.Add("@HolidayType", model.HolidayType);
                param.Add("@LeaveWithSalary", 0);
                param.Add("@OVTType", model.OVTType);
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
        // tìm xem bản ghi có đang được sử dụng hay không
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
                        string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Holiday)}.json", nameof(Holiday), "CountByIdRecord");
                        string queryFormat = string.Format(sqlQuery, layoutChild, nameof(Holiday) + "ID");
                        var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<int>(sql: queryFormat, new { @HolidayID = dataRecords });
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
                        var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<Holiday>(sql: queryFormat, new { @recordIDs = dataRecords });
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
                    return await _baseHandler.DeleteManyCheckUseRecord<Holiday>(data);
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
