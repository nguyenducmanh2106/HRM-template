using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SV.HRM.Caching.Interface;
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

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class StaffRefHandler : IStaffRefHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ICached _cached;
        private readonly IBaseHandler _baseHandler;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StaffRefHandler(ICached cached, IHttpContextAccessor httpContextAccessor,
            IDapperUnitOfWork dapperUnitOfWork, IBaseHandler baseHandler)
        {
            _cached = cached;
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<int>> CreateOrUpdate(StaffRefCreateRequestModel model)
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


                #region Lưu thông tin tham chiếu
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffRefID", model.StaffRefID);
                param.Add("@StaffID", model.StaffID);
                param.Add("@Manager", model.Manager);
                param.Add("@PositionName", model.PositionName);
                param.Add("@Organization", model.Organization);
                param.Add("@Telephone", model.Telephone);
                param.Add("@Email", model.Email);
                param.Add("@Note", model.Note);
                param.Add("@CreatedBy", user.UserId);
                param.Add("@LastUpdatedBy", user.UserId);
                param.Add("@OutputData", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/StaffRef.json", "StaffRef", "sp_StaffRef_CreateOrUpdate");
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
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/StaffRef.json", "StaffRef", "sp_StaffRef_DeleteMany");

                int result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, new { @StaffRefIDs = string.Join(",", ids) }, null, CommandType.StoredProcedure);
                if (result > 0)
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);

                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }

        public async Task<Response<StaffRefDetailModel>> GetById(int id)
        {
            try
            {
                StaffRefDetailModel modelDetail;
                var param = new { @StaffRefID = id };
                string sqlQuery = JSONObject.GetQueryFromJSON("SqlCommand/StaffRef.json", "StaffRef", "sp_StaffRef_GetById");
                var objReturn = _dapperUnitOfWork.GetRepository().QueryMultiple(sqlQuery, param, null, CommandType.StoredProcedure,
                        gr => gr.Read<StaffRefDetailModel>().FirstOrDefault());

                if (objReturn != null)
                {
                    modelDetail = objReturn[0] as StaffRefDetailModel ?? new StaffRefDetailModel();
                    return new Response<StaffRefDetailModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, modelDetail);
                }
                else
                {
                    return new Response<StaffRefDetailModel>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffRefDetailModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS);
            }
        }
    }
}
