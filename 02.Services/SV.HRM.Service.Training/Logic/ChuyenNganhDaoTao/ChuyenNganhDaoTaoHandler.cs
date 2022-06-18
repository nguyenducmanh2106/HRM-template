﻿using Dapper;
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

namespace SV.HRM.Service.Training
{
    public class ChuyenNganhDaoTaoHandler : IChuyenNganhDaoTaoHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;
        public ChuyenNganhDaoTaoHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler)
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
        }
        public async Task<Response<bool>> Create(ChuyenNganhDaoTaoCreateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(ChuyenNganhDaoTao)}.json", nameof(ChuyenNganhDaoTao), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@ChuyenNganhDaoTaoCode", model.ChuyenNganhDaoTaoCode);
                param.Add("@ChuyenNganhDaoTaoName", model.ChuyenNganhDaoTaoName);
                param.Add("@GhiChu", model.GhiChu);
                param.Add("@Status", model.Status);
                param.Add("@SoThuTu", model.SoThuTu);
                param.Add("@CreatedByUserId", model.CreatedByUserId);
                param.Add("@CreatedOnDate", model.CreatedOnDate);
                param.Add("@LastModifiedByUserId", model.LastModifiedByUserId);
                param.Add("@LastModifiedOnDate", DateTime.Now);

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

        public async Task<Response<ChuyenNganhDaoTaoModel>> FindById(int recordID)
        {
            ChuyenNganhDaoTaoModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(ChuyenNganhDaoTao)}.json", nameof(ChuyenNganhDaoTao), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<ChuyenNganhDaoTaoModel>(sqlQuery, new { ChuyenNganhID = recordID });
                if (result != null)
                {
                    return new Response<ChuyenNganhDaoTaoModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<ChuyenNganhDaoTaoModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<ChuyenNganhDaoTaoModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<bool>> Update(int id, ChuyenNganhDaoTaoUpdateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(ChuyenNganhDaoTao)}.json", nameof(ChuyenNganhDaoTao), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@ChuyenNganhDaoTaoID", id);
                param.Add("@ChuyenNganhDaoTaoCode", model.ChuyenNganhDaoTaoCode);
                param.Add("@ChuyenNganhDaoTaoName", model.ChuyenNganhDaoTaoName);
                param.Add("@GhiChu", model.GhiChu);
                param.Add("@Status", model.Status);
                param.Add("@SoThuTu", model.SoThuTu);
                param.Add("@LastModifiedByUserId", model.LastModifiedByUserId);
                param.Add("@LastModifiedOnDate", DateTime.Now);

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
                        var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<ChuyenNganh>(sql: queryFormat, new { @recordIDs = dataRecords });
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
                    return await _baseHandler.DeleteManyCheckUseRecord<ChuyenNganh>(data);
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