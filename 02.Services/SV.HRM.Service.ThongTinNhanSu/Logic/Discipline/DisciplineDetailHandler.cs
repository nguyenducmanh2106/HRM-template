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
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using NLog;
namespace SV.HRM.Service.ThongTinNhanSu
{
    public class DisciplineDetailHandler:IDisciplineDetailHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly ICached _cached;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseHandler _baseHandler;

        public DisciplineDetailHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler, ICached cached, IHttpContextAccessor httpContextAccessor
            )
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _cached = cached;
            _httpContextAccessor = httpContextAccessor;
            _baseHandler = baseHandler;
        }
        /// <summary>
        /// Tạo mới kỷ luật
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(DisciplineDetailCreateModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(DisciplineDetail)}.json", nameof(DisciplineDetail), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@DecisionNo", model.DecisionNo);
                param.Add("@DisciplineDate", model.DisciplineDate);
                param.Add("@DisciplineTypeID", model.DisciplineTypeID);
                param.Add("@PeriodID", model.PeriodID);
                param.Add("@EffectiveFrom", model.EffectiveFrom);
                param.Add("@EffectiveTo", model.EffectiveTo);
                param.Add("@Note", model.Note);
                param.Add("@CompensationLevel", model.CompensationLevel);
                param.Add("@CompensationMode", model.CompensationMode);

                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreationDate", DateTime.Now);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = FileType.KY_LUAT; u.TypeId = outputResult; });
                        await _baseHandler.InsertManyFile(model.FileUpload);
                    }
                    // Write log
                    _baseHandler.CreateLog(new SystemLogModel
                    {
                        ApplicationId = userModel.ApplicationId,
                        UserId = userModel.UserId,
                        ActionByUser = LogAction.UPDATE,
                        Date = DateTime.Now,
                        Content = $"Tạo mới bản ghi kỷ luật {model.StaffID} - {model.DecisionNo}"
                    });
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else //trường hợp còn lại cụ thể là store proc trả ra = 0 là trường hợp exception lỗi của SQL
                {
                    if (outputResult == Constant.ERROR)
                    {
                        return new Response<bool>(Constant.ErrorCode.FAIL_CODE, String.Format(Constant.ErrorCode.CUSTOM_ERROR_MESSAGE, "Thời gian của ngày hiệu lực không thuộc quá trình công tác nào"), false);
                    }
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
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<DisciplineDetailModel>> FindById(int recordID)
        {
            DisciplineDetailModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(DisciplineDetail)}.json", nameof(DisciplineDetail), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<DisciplineDetailModel>(sqlQuery, new { DisciplineDetailID = recordID });
                if (result != null)
                {
                    //Lấy thông tin file đính kèm
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.KY_LUAT, recordID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<DisciplineDetailModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<DisciplineDetailModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<DisciplineDetailModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }


        /// <summary>
        /// Hàm cập nhật kỷ luật
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, DisciplineDetailUpdateModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(DisciplineDetail)}.json", nameof(DisciplineDetail), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@DisciplineDetailID", id);
                param.Add("@StaffID", model.StaffID);
                param.Add("@DecisionNo", model.DecisionNo);
                param.Add("@DisciplineDate", model.DisciplineDate);
                param.Add("@DisciplineTypeID", model.DisciplineTypeID);
                param.Add("@PeriodID", model.PeriodID);
                param.Add("@EffectiveFrom", model.EffectiveFrom);
                param.Add("@EffectiveTo", model.EffectiveTo);
                param.Add("@Note", model.Note);
                param.Add("@CompensationLevel", model.CompensationLevel);
                param.Add("@CompensationMode", model.CompensationMode);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult == SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(FileType.KY_LUAT, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = FileType.KY_LUAT; u.TypeId = id; });
                            var resUpdateFile = await _baseHandler.InsertManyFile(model.FileUpload);
                            if (resUpdateFile != null && resUpdateFile.Status == SUCCESS && resUpdateFile.Data)
                            {
                                // Write log
                                _baseHandler.CreateLog(new SystemLogModel
                                {
                                    ApplicationId = userModel.ApplicationId,
                                    UserId = userModel.UserId,
                                    ActionByUser = LogAction.UPDATE,
                                    Date = DateTime.Now,
                                    Content = $"Cập nhật bản ghi kỷ luật {model.StaffID} - {model.DecisionNo}"
                                });
                                return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                            }
                        }
                    }
                    // Write log
                    _baseHandler.CreateLog(new SystemLogModel
                    {
                        ApplicationId = userModel.ApplicationId,
                        UserId = userModel.UserId,
                        ActionByUser = LogAction.UPDATE,
                        Date = DateTime.Now,
                        Content = $"Cập nhật bản ghi ky luật {model.StaffID} - {model.DecisionNo}"
                    });
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else //trường hợp còn lại cụ thể là store proc trả ra = 0 là trường hợp exception lỗi của SQL
                {
                    if (outputResult == Constant.ERROR)
                    {
                        return new Response<bool>(Constant.ErrorCode.FAIL_CODE, String.Format(Constant.ErrorCode.CUSTOM_ERROR_MESSAGE, "Thời gian của ngày hiệu lực không thuộc quá trình công tác nào"), false);
                    }
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        public async Task<DisciplineDetailModel> FindByStaffAndDecisionNo(int staffId, string decisionNo)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(DisciplineDetail)}.json", nameof(DisciplineDetail), "FindByStaffAndDecisionNo");
                string renderQuery = String.Format(sqlQuery, staffId, decisionNo);
                var discipline = _dapperUnitOfWork.GetRepository().QuerySingleOrDefault<DisciplineDetailModel>(renderQuery, null, null, CommandType.Text);
                if (discipline != null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.KY_LUAT, discipline.DisciplineDetailID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        discipline.FileUpload = resFile.Data;
                    }
                }
                return discipline;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }

        public async Task<Response<bool>> CheckStaffDisciplineInHistory(int staffId, DateTime date)
        {
            try
            {
                bool resultCheck = true;
                if (date != null)
                {
                    string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(DisciplineDetail)}.json", nameof(DisciplineDetail), "Check_Staff_Discipline_In_History");
                    string queryRender = String.Format(sqlQuery, staffId, date.ToString("yyyy-MM-dd"));
                    var resultQuery = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<object>(queryRender, null, null, CommandType.Text);
                    if (resultQuery != null)
                    {
                        resultCheck = false;
                    }
                }

                if (resultCheck)
                {
                    return new Response<bool>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, true);
                }
                else
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, false);
                }
            }
            catch (Exception ex)
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, ex.Message, false);
            }
        }
    }
}
