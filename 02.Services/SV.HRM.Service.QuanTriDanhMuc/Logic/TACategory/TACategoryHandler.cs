using Dapper;
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
    public class TACategoryHandler : ITACategoryHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;
        public TACategoryHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler)
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
        }
        public async Task<Response<bool>> Create(TACategoryCreateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(TACategory)}.json", nameof(TACategory), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@TACategoryCode", model.TACategoryCode);
                param.Add("@TACategoryName", model.TACategoryName);
                param.Add("@ThresholdOVTBeforeShift", model.ThresholdOVTBeforeShift);
                param.Add("@ThresholdOVTAfterShift", model.ThresholdOVTAfterShift);
                param.Add("@ThresholdDaytime", model.ThresholdDaytime);
                param.Add("@ThresholdNightTime", model.ThresholdNightTime);
                param.Add("@RoundOVTType", model.RoundOVTType);
                param.Add("@RoundUnitOVT", model.RoundUnitOVT);
                param.Add("@RoundLCIECOType", model.RoundLCIECOType);
                param.Add("@RoundUnitLCIECO", model.RoundUnitLCIECO);
                param.Add("@RoundWKTType", model.RoundWKTType);
                param.Add("@RoundUnitWKT", model.RoundUnitWKT);
                param.Add("@Note", model.Note);
                param.Add("@IsDefault", model.IsDefault);
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

        public async Task<Response<TACategoryModel>> FindById(int recordID)
        {
            TACategoryModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(TACategory)}.json", nameof(TACategory), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<TACategoryModel>(sqlQuery, new { TACategoryID = recordID });
                if (result != null)
                {
                    return new Response<TACategoryModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<TACategoryModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<TACategoryModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<bool>> Update(int id, TACategoryUpdateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(TACategory)}.json", nameof(TACategory), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@TACategoryID", id);
                param.Add("@TACategoryCode", model.TACategoryCode);
                param.Add("@TACategoryName", model.TACategoryName);
                param.Add("@ThresholdOVTBeforeShift", model.ThresholdOVTBeforeShift);
                param.Add("@ThresholdOVTAfterShift", model.ThresholdOVTAfterShift);
                param.Add("@ThresholdDaytime", model.ThresholdDaytime);
                param.Add("@ThresholdNightTime", model.ThresholdNightTime);
                param.Add("@RoundOVTType", model.RoundOVTType);
                param.Add("@RoundUnitOVT", model.RoundUnitOVT);
                param.Add("@RoundLCIECOType", model.RoundLCIECOType);
                param.Add("@RoundUnitLCIECO", model.RoundUnitLCIECO);
                param.Add("@RoundWKTType", model.RoundWKTType);
                param.Add("@RoundUnitWKT", model.RoundUnitWKT);
                param.Add("@Note", model.Note);
                param.Add("@IsDefault", model.IsDefault);
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
                        var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<TACategory>(sql: queryFormat, new { @recordIDs = dataRecords });
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
                    return await _baseHandler.DeleteManyCheckUseRecord<TACategory>(data);
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
