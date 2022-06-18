using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SV.HRM.Caching.Interface;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Impl;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class StaffAssetHandler : IStaffAssetHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ICached _cached;
        private readonly IBaseHandler _baseHandler;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public StaffAssetHandler(ICached cached, IHttpContextAccessor httpContextAccessor, IConfiguration config,
           IDapperUnitOfWork dapperUnitOfWork, IBaseHandler baseHandler)
        {
            _cached = cached;
            _config = config;
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<int>> CreateOrUpdate(StaffAssetCreateRequestModel model)
        {
            try
            {
                //Fix tạm
                var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached) ?? new UserModel();
                user.UserId = 1;

                #region Check condition
                if (model.StaffID <= 0)
                    return new Response<int>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
                #endregion

                model.ListAttachment.ForEach(item => item.Type = Constant.FileType.TRANG_BI_BAO_HO);
                string attachmentJson = JsonSerializer.Serialize(new { HRM_Attachment = model.ListAttachment });
                #region Lưu thông tin 
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffAssetID", model.StaffAssetID);
                param.Add("@StaffID", model.StaffID);
                param.Add("@AssetCode", model.AssetCode);
                param.Add("@AssetName", model.AssetName);
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@ExtraNumber2", model.ExtraNumber2);
                param.Add("@Unit", model.Unit);
                param.Add("@Quantity", model.Quantity);
                param.Add("@ExtraNumber3", model.ExtraNumber3);
                param.Add("@ExtraNumber4", model.ExtraNumber4);
                param.Add("@FromDate", model.FromDate);
                param.Add("@ToDate", model.ToDate);
                param.Add("@Note", model.Note);
                param.Add("@CreatedBy", user.UserId);
                param.Add("@LastUpdatedBy", user.UserId);
                param.Add("@FileType", Constant.FileType.TRANG_BI_BAO_HO);
                param.Add("@AttachmentJson", attachmentJson);
                param.Add("@OutputData", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/StaffAsset.json", "StaffAsset", "sp_StaffAsset_CreateOrUpdate");
                await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);
                #endregion

                int outputData = param.Get<int>("@OutputData");
                int outputResult = param.Get<int>("@OutputResult");
                if (outputResult > Constant.NODATA)
                {
                    return new Response<int>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, outputData);
                }
                else
                {
                    return new Response<int>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<int>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<bool>> DeleteMany(List<int> ids)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffAssetIDs", string.Join(",", ids));
                param.Add("@FileType", Constant.FileType.TRANG_BI_BAO_HO);
                param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/StaffAsset.json", "StaffAsset", "sp_StaffAsset_DeleteMany");
                await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);
                int outputResult = param.Get<int>("@OutputResult");

                if (outputResult > 0)
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);

                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<List<StaffAssetModel>>> GetFilter(EntityGeneric filter)
        {
            try
            {
                var listOrganization = GetListOrganization();
                var result = (await _baseHandler.GetFilter<StaffAssetModel>(filter));

                OrganizationModel org;
                result.Data.ForEach(item =>
                {
                    org = listOrganization.FirstOrDefault(r => item.ExtraNumber5.HasValue && r.OrganizationId.Equals((int)item.ExtraNumber5));
                    item.OrganizationName = org?.OrganizationName;
                });

                return result;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<StaffAssetModel>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<StaffAssetDetailModel>> GetById(int id)
        {
            try
            {
                StaffAssetDetailModel modelDetail;
                var param = new { @StaffAssetID = id, @FileType = FileType.TRANG_BI_BAO_HO };
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/StaffAsset.json", "StaffAsset", "sp_StaffAsset_GetById");
                var objReturn = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, param, null, CommandType.StoredProcedure,
                        gr => gr.Read<StaffAssetDetailModel>().FirstOrDefault(),
                        gr => gr.Read<HRM_AttachmentModel>());

                if (objReturn != null)
                {
                    modelDetail = objReturn[0] as StaffAssetDetailModel ?? new StaffAssetDetailModel();
                    modelDetail.ListAttachment = objReturn[1] as List<HRM_AttachmentModel> ?? default;
                    return new Response<StaffAssetDetailModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, modelDetail);
                }
                else
                {
                    return new Response<StaffAssetDetailModel>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffAssetDetailModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<string>> GetStaffFullNameById(int id)
        {
            try
            {
                var param = new { @StaffID = id };
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/StaffAsset.json", "StaffAsset", "GetStaffFullNameById");
                var result = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<string>(sqlQuery, param, null, CommandType.Text);

                if (!string.IsNullOrEmpty(result))
                {
                    return new Response<string>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                else
                {
                    return new Response<string>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<string>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public List<OrganizationModel> GetListOrganization()
        {
            IDbConnection connection = null;
            List<OrganizationModel> listData = new List<OrganizationModel>();
            try
            {
                string connectionString = _config.GetValue<string>("Databases:MSSQL:ConnectionStrings:SystemConnectionString");
                connection = new SqlConnection(connectionString);
                using (var unitOfwork = new DapperUnitOfWork(connection))
                {
                    listData = unitOfwork.GetRepository().Query<OrganizationModel>("SELECT OrganizationID,OrganizationName FROM Organizations WHERE Status = @status;", new { status = StatusRecord.ACTIVE }).ToList();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                logger.Error($"[ERROR]: {ex}");
            }

            return listData;
        }
    }
}
