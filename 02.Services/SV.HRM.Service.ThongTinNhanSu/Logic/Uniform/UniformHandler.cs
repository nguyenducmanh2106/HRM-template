using Dapper;
using Microsoft.AspNetCore.Http;
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
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class UniformHandler : IUniformHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ICached _cached;
        private readonly IBaseHandler _baseHandler;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public UniformHandler(ICached cached, IHttpContextAccessor httpContextAccessor, IConfiguration config,
           IDapperUnitOfWork dapperUnitOfWork, IBaseHandler baseHandler)
        {
            _cached = cached;
            _config = config;
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<List<UniformModel>>> GetFilter(EntityGeneric filter)
        {
            try
            {
                var listOrganization = GetListOrganization();
                var result = (await _baseHandler.GetFilter<UniformModel>(filter));

                //OrganizationModel org;
                //result.Data.ForEach(item =>
                //{
                //    org = listOrganization.FirstOrDefault(r => item.ExtraNumber5.HasValue && r.OrganizationId.Equals((int)item.ExtraNumber5));
                //    item.OrganizationName = org?.OrganizationName;
                //});

                return result;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<UniformModel>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<UniformDetailModel>> GetById(int id)
        {
            try
            {
                UniformDetailModel modelDetail;
                var param = new { @UniformID = id, @FileType = FileType.DONG_PHUC_NHAN_VIEN };
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Uniform.json", "Uniform", "sp_Uniform_GetById");
                var objReturn = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, param, null, CommandType.StoredProcedure,
                        gr => gr.Read<UniformDetailModel>().FirstOrDefault(),
                        gr => gr.Read<HRM_AttachmentModel>());

                if (objReturn != null)
                {
                    modelDetail = objReturn[0] as UniformDetailModel ?? new UniformDetailModel();
                    modelDetail.ListAttachment = objReturn[1] as List<HRM_AttachmentModel> ?? default;
                    return new Response<UniformDetailModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, modelDetail);
                }
                else
                {
                    return new Response<UniformDetailModel>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<UniformDetailModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<string>> GetStaffFullNameById(int id)
        {
            try
            {
                var param = new { @StaffID = id };
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Uniform.json", "Uniform", "GetStaffFullNameById");
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

        public async Task<Response<int>> CreateOrUpdate(UniformCreateRequestModel model)
        {
            try
            {
                var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
                #region Check condition
                if (model.StaffID <= 0)
                    return new Response<int>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
                #endregion

                model.ListAttachment.ForEach(item => item.Type = Constant.FileType.DONG_PHUC_NHAN_VIEN);
                string attachmentJson = JsonSerializer.Serialize(new { HRM_Attachment = model.ListAttachment });
                #region Lưu thông tin 
                DynamicParameters param = new DynamicParameters();
                param.Add("@UniformID", model.UniformID);
                param.Add("@StaffID", model.StaffID);
                param.Add("@UniformItemID", model.UniformItemID);
                param.Add("@ExtraText1", model.ExtraText1);
                param.Add("@UniformYear", model.UniformYear);
                param.Add("@ExtraNumber3", model.ExtraNumber3);
                param.Add("@Amount", model.Amount);
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@ExtraNumber2", model.ExtraNumber2);
                param.Add("@IssueDate", model.IssueDate);
                param.Add("@ReissueDate", model.ReissueDate);
                param.Add("@Note", model.Note);
                param.Add("@CreatedBy", user.UserId);
                param.Add("@LastUpdatedBy", user.UserId);
                param.Add("@FileType", Constant.FileType.DONG_PHUC_NHAN_VIEN);
                param.Add("@AttachmentJson", attachmentJson);
                param.Add("@OutputData", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/Uniform.json", "Uniform", "sp_Uniform_CreateOrUpdate");
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
                param.Add("@UniformIDs", string.Join(",", ids));
                param.Add("@FileType", Constant.FileType.DONG_PHUC_NHAN_VIEN);
                param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Uniform.json", "Uniform", "sp_Uniform_DeleteMany");
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
