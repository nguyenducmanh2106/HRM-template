using Dapper;
using Microsoft.Extensions.Logging;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using System.Linq;
using SV.HRM.Caching.Interface;
using Microsoft.AspNetCore.Http;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class DecisionHandler:IDecisionHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly ICached _cached;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseHandler _baseHandler;

        public DecisionHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler, ICached cached, IHttpContextAccessor httpContextAccessor
            )
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _cached = cached;
            _httpContextAccessor = httpContextAccessor;
            _baseHandler = baseHandler;
        }
        /// <summary>
        /// Tạo mới quyết định
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(DecisionCreateModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Decision)}.json", nameof(Decision), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@DecisionNo", model.DecisionNo);
                param.Add("@DecisionDate", model.DecisionDate);
                param.Add("@DecisionItemID", model.DecisionItemID);
                param.Add("@DeptID", model.DeptID);
                param.Add("@PositionID", model.PositionID);
                param.Add("@JobTitleID", model.JobTitleID);
                param.Add("@CurrencyID", model.CurrencyID);
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@BasicSalary", model.BasicSalary);
                param.Add("@ExtraNumber2", model.ExtraNumber2);
                param.Add("@ExtraText1", model.ExtraText1);
                param.Add("@ExtraText10", model.ExtraText10);
                param.Add("@AllowanceRate1", model.AllowanceRate1);
                param.Add("@AllowanceRate2", model.AllowanceRate2);
                param.Add("@AllowanceRate3", model.AllowanceRate3);
                param.Add("@AllowanceRate4", model.AllowanceRate4);
                param.Add("@EffectiveFromDate", model.EffectiveFromDate);
                param.Add("@EffectiveToDate", model.EffectiveToDate);
                param.Add("@StaffRequestID", model.StaffRequestID);
                param.Add("@StaffPerformID", model.StaffPerformID);
                param.Add("@StaffHandoverID", model.StaffHandoverID);
                param.Add("@StaffTransferID", model.StaffTransferID);
                param.Add("@StaffApproveID", model.StaffApproveID);
                param.Add("@ApproveDate", model.ApproveDate);
                param.Add("@Note", model.Note);

                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreationDate", DateTime.Now);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = FileType.QUYET_DINH; u.TypeId = outputResult; });
                        await _baseHandler.InsertManyFile(model.FileUpload);
                    }
                    // Write log
                    _baseHandler.CreateLog(new SystemLogModel
                    {
                        ApplicationId = userModel.ApplicationId,
                        UserId = userModel.UserId,
                        ActionByUser = LogAction.ADD,
                        Date = DateTime.Now,
                        Content = $"Tạo mới bản ghi quyết định {model.StaffID} - {model.DecisionNo}"
                    });
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else //trường hợp còn lại cụ thể là store proc trả ra = 0 là trường hợp exception lỗi của SQL
                {
                    if(outputResult == Constant.ERROR)
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
        public async Task<Response<DecisionModel>> FindById(int recordID)
        {
            DecisionModel result;
            try
            {   string queryGetStaff = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Staff)}.json", nameof(Staff), "Get_All");
                IEnumerable<StaffModel> staffs = await _dapperUnitOfWork.GetRepository().QueryAsync<StaffModel>(queryGetStaff);
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Decision)}.json", nameof(Decision), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<DecisionModel>(sqlQuery, new { DecisionID = recordID });
                if (result != null)
                {
                    //lấy về tên đơn vị
                    result.DeptName = _baseHandler.GetOrganization().SingleOrDefault(g => result.DeptID.HasValue && g.OrganizationId == result.DeptID)?.OrganizationName ?? "";
                    
                    if(staffs != null)
                    {
                        //lấy tên người yêu cầu
                        result.StaffRequestName = staffs.FirstOrDefault(g => result.StaffRequestID.HasValue && result.StaffRequestID == g.StaffID)?.FullName ?? "";

                        //lấy tên người thực hiện
                        result.StaffPerformName = staffs.FirstOrDefault(g => result.StaffPerformID.HasValue && result.StaffPerformID == g.StaffID)?.FullName ?? "";

                        //lấy tên người bàn giao
                        result.StaffHandoverName = staffs.FirstOrDefault(g => result.StaffHandoverID.HasValue && result.StaffHandoverID == g.StaffID)?.FullName ?? "";

                        //lấy tên người chuyển giao
                        result.StaffTransferName = staffs.FirstOrDefault(g => result.StaffTransferID.HasValue && result.StaffTransferID == g.StaffID)?.FullName ?? "";

                        //lấy tên người chấp thuật
                        result.StaffApproveName = staffs.FirstOrDefault(g => result.StaffApproveID.HasValue && result.StaffApproveID == g.StaffID)?.FullName ?? "";
                    }
                    //Lấy thông tin file đính kèm
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.QUYET_DINH, recordID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<DecisionModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<DecisionModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<DecisionModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }


        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, DecisionUpdateModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Decision)}.json", nameof(Decision), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@DecisionID", id);
                param.Add("@StaffID", model.StaffID);
                param.Add("@DecisionNo", model.DecisionNo);
                param.Add("@DecisionDate", model.DecisionDate);
                param.Add("@DecisionItemID", model.DecisionItemID);
                param.Add("@DeptID", model.DeptID);
                param.Add("@PositionID", model.PositionID);
                param.Add("@JobTitleID", model.JobTitleID);
                param.Add("@CurrencyID", model.CurrencyID);
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@BasicSalary", model.BasicSalary);
                param.Add("@ExtraNumber2", model.ExtraNumber2);
                param.Add("@ExtraText1", model.ExtraText1);
                param.Add("@ExtraText10", model.ExtraText10); //Tên quyết định
                param.Add("@AllowanceRate1", model.AllowanceRate1);
                param.Add("@AllowanceRate2", model.AllowanceRate2);
                param.Add("@AllowanceRate3", model.AllowanceRate3);
                param.Add("@AllowanceRate4", model.AllowanceRate4);
                param.Add("@EffectiveFromDate", model.EffectiveFromDate);
                param.Add("@EffectiveToDate", model.EffectiveToDate);
                param.Add("@StaffRequestID", model.StaffRequestID);
                param.Add("@StaffPerformID", model.StaffPerformID);
                param.Add("@StaffHandoverID", model.StaffHandoverID);
                param.Add("@StaffTransferID", model.StaffTransferID);
                param.Add("@StaffApproveID", model.StaffApproveID);
                param.Add("@ApproveDate", model.ApproveDate);
                param.Add("@Note", model.Note);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult == SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(FileType.QUYET_DINH, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = FileType.QUYET_DINH; u.TypeId = id; });
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
                                    Content = $"Cập nhật bản ghi quyết định {model.StaffID} - {model.DecisionNo}"
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
                        Content = $"Cập nhật bản ghi quyết định {model.StaffID} - {model.DecisionNo}"
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

        public async Task<DecisionModel> FindByStaffAndDecisionNo(int staffId, string decisionNo)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Decision)}.json", nameof(Decision), "FindByStaffAndDecisionNo");
                string renderQuery = String.Format(sqlQuery, staffId, decisionNo);
                var decision = _dapperUnitOfWork.GetRepository().QuerySingleOrDefault<DecisionModel>(renderQuery, null, null, CommandType.Text);
                if (decision != null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.QUYET_DINH, decision.DecisionID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        decision.FileUpload = resFile.Data;
                    }
                }
                return decision;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }

        public async Task<Response<bool>> CheckStaffDecisionInHistory(int staffId, DateTime date)
        {
            try
            {
                bool resultCheck = true;
                if (date != null)
                {
                    string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(Decision)}.json", nameof(Decision), "Check_Staff_Decision_In_History");
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
