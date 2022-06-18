using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
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

namespace SV.HRM.Service.LeaveManagement
{
    public class LeaveManagementHandler : ILeaveManagementHandler
    {
        private readonly IWorkflowHandler _workflowHandler;
        private readonly IBaseHandler _baseHandler;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly ICached _cached;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public LeaveManagementHandler(IWorkflowHandler workflowHandler, IBaseHandler baseHandler, IDapperUnitOfWork dapperUnitOfWork, ICached cached, IHttpContextAccessor httpContextAccessor)
        {
            _workflowHandler = workflowHandler;
            _baseHandler = baseHandler;
            _dapperUnitOfWork = dapperUnitOfWork;
            _cached = cached;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<List<LeaveManagementModel>>> GetFilter(LeaveManagementQueryFilter filter)
        {
            try
            {
                if (filter.PageSize > 0)
                {
                    var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
                    var lstUser = await _baseHandler.GetListUser();
                    List<LeaveManagementModel> lstData = new List<LeaveManagementModel>();
                    int totalCount = 0;
                    var strQuery = JSONObject.GetQueryFromJSON($"SqlCommand/LeaveManagement.json", "LeaveManagement", "Proc_Grid");
                    var param = new DynamicParameters();
                    param.Add("PageSize", filter.PageSize);
                    param.Add("PageIndex", filter.PageNumber);
                    param.Add("WorkflowScreen", filter.WorkflowScreen);
                    param.Add("TextSearch", filter.TextSearch);
                    param.Add("UserId", user.ID.ToString());
                    List<object> objReturn = _dapperUnitOfWork.GetRepository().QueryMultiple(strQuery, param, null, CommandType.StoredProcedure,
                    gr =>
                        gr.Read<LeaveManagementModel>(),
                    gr =>
                        gr.Read<Int32>().FirstOrDefault()
                    );
                    if (objReturn != null && objReturn[0] != null && objReturn[1] != null)
                    {
                        lstData = (List<LeaveManagementModel>)objReturn[0];
                        var index = 0;
                        foreach (var item in lstData)
                        {
                            index++;
                            item.STT = (filter.PageNumber - 1) * filter.PageSize + index;
                            if (item.UserID != null)
                            {
                                item.CreatedUserName = lstUser.Find(x => x.ID == item.UserID)?.FullName;
                            }
                            if (item.CurrentUserID != null)
                            {
                                item.CurrentUserName = lstUser.Find(x => x.ID == item.CurrentUserID)?.FullName;
                            }
                            if (item.PreUserID != null)
                            {
                                item.PreUserName = lstUser.Find(x => x.ID == item.PreUserID)?.FullName;
                            }
                        }
                        totalCount = (int)objReturn[1];
                        int totalPage = (int)Math.Ceiling((decimal)totalCount / filter.PageSize);
                        return new Response<List<LeaveManagementModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, lstData, lstData.Count(), totalCount, totalPage, filter.PageNumber, filter.PageSize);
                    }
                    else
                    {
                        return new Response<List<LeaveManagementModel>>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, null);
                    }
                }
                else
                {
                    return new Response<List<LeaveManagementModel>>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<LeaveManagementModel>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Tạo mới ngân hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(LeaveManagementCreateModel model)
        {
            try
            {
                var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
                var userConfig = await GetUserConfigByUserId(user.UserId);
                var leaveManagementId = Guid.NewGuid();

                if (model.LeaveGroup == Constant.LeaveGroup.NGHI_THEO_GIO)
                {
                    model.ToDate = model.FromDate;
                }

                model.CountDay = await GetRemainDayOfByUser(user.UserId, model.FromDate.Value, model.ToDate.Value, model.LeaveGroup.Value, model.LeaveType.Value);
                model.CountHour = 8 * model.CountDay;

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/LeaveManagement.json", "LeaveManagement", "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@LeaveManagementID", leaveManagementId);
                param.Add("@LeaveGroup", model.LeaveGroup);
                param.Add("@LeaveType", model.LeaveType);
                param.Add("@LeaveID", model.LeaveID);
                param.Add("@Description", model.Description);
                param.Add("@FromDate", model.FromDate);
                param.Add("@ToDate", model.ToDate);
                param.Add("@CountHour", model.CountHour);
                param.Add("@CountDay", model.CountDay);
                param.Add("@UserID", user.ID);
                param.Add("@WorkflowId", userConfig.Data.WorkflowID);

                param.Add("@CreatedByUserID", user.ID);
                param.Add("@CreatedOnDate", DateTime.Now);
                param.Add("@LastModifiedByUserID", user.ID);
                param.Add("@LastModifiedOnDate", DateTime.Now);

                var outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= Constant.SUCCESS) //lưu thành công
                {
                    #region Save to workflow

                    Document doc = new Document();
                    doc.AuthorId = user.ID.Value;
                    doc.Comment = "Khởi tạo quy trình";
                    doc.Id = leaveManagementId;
                    doc.ManagerId = user.ID.Value;
                    doc.Name = model.Description;
                    doc.SchemeName = userConfig.Data.WorkflowCode;
                    var cdrs = _workflowHandler.CreateDocument(doc);

                    if (cdrs.Status != -1)
                    {
                        var lstAllowIds = new List<Guid>();
                        lstAllowIds.Add(user.ID.Value);
                        var allowIdentityIds = JsonConvert.SerializeObject(lstAllowIds);

                        sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/LeaveManagement.json", "LeaveManagement", "Proc_Update_Workflow");
                        param = new DynamicParameters();
                        param.Add("@LeaveManagementID", leaveManagementId);
                        param.Add("@AllowIdentityIds", allowIdentityIds);
                        param.Add("@IsDraft", true);
                        param.Add("@IsInProcess", false);
                        param.Add("@CurrentUserId", user.ID);
                        param.Add("@PreUserId", user.ID);
                        param.Add("@LastModifiedByUserID", user.ID);
                        param.Add("@LastModifiedOnDate", DateTime.Now);
                        var dsrs = _workflowHandler.GetStateNameOfDocuments(leaveManagementId);
                        if (dsrs.Status == 1)
                        {
                            param.Add("@IsFinished", dsrs.Data.IsFinished);
                            var workflowState = await GetWorkflowStateByName(dsrs.Data.StateName);
                            if (workflowState != null)
                            {
                                param.Add("@WorkflowStateId", workflowState.Data.WorkflowStateID);
                            }
                        }
                        else
                        {
                            param.Add("@IsFinished", false);
                        }

                        outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);

                        if (outputResult >= Constant.SUCCESS)
                        {
                            return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                        }
                        else
                        {
                            return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                        }
                    }
                    else
                    {
                        return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                    }

                    #endregion
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
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<LeaveManagementModel>> FindById(Guid recordID)
        {
            LeaveManagementModel result;
            try
            {
                var lstUser = await _baseHandler.GetListUser();
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/LeaveManagement.json", "LeaveManagement", "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<LeaveManagementModel>(sqlQuery, new { LeaveManagementID = recordID });
                if (result != null)
                {
                    result.CurrentUserName = lstUser.Find(x => x.ID == result.CurrentUserID)?.FullName;
                    var leaveDate = string.Empty;
                    if (result.FromDate.HasValue)
                    {
                        leaveDate = result.FromDate.Value.ToString(Constant.DateTimeFormat.DDMMYYYY);
                    }
                    leaveDate = leaveDate + " - ";
                    if (result.ToDate.HasValue)
                    {
                        leaveDate = leaveDate + result.ToDate.Value.ToString(Constant.DateTimeFormat.DDMMYYYY);
                    }
                    result.LeaveDate = leaveDate;
                    return new Response<LeaveManagementModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<LeaveManagementModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<LeaveManagementModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Hàm cập nhật ngân hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(Guid id, LeaveManagementUpdateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/LeaveManagement.json", "LeaveManagement", "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@LeaveManagementID", id);
                param.Add("@LeaveGroup", model.LeaveGroup);
                param.Add("@LeaveType", model.LeaveType);
                param.Add("@LeaveID", model.LeaveID);
                param.Add("@Description", model.Description);
                param.Add("@FromDate", model.FromDate);
                param.Add("@ToDate", model.ToDate);
                param.Add("@CountHour", model.CountHour);
                param.Add("@CountDay", model.CountDay);
                param.Add("@LastModifiedByUserID", model.LastModifiedByUserID);
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
                var dataRecords = JsonConvert.DeserializeObject<List<Guid>>(obj[0].ToString());
                // convert list object to ToDictionary<string,string>
                var dataTables = JsonConvert.DeserializeObject<List<JObject>>(obj[1].ToString()).Select(x => x?.ToObject<Dictionary<string, string>>()).ToList();
                if (dataRecords.Count() > 0 && dataTables.Count() > 0)
                {
                    string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/Base.json", "Base", "TopRecordById");
                    foreach (var item in dataTables)
                    {
                        string queryFormat = string.Format(sqlQuery, item["TableName"], item["ColumnName"]);
                        var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<Models.LeaveManagement>(sql: queryFormat, new { @recordIDs = dataRecords });
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
                    return await _baseHandler.DeleteManyCheckUseRecord<Models.LeaveManagement>(data);
                }
                else
                {
                    return new Response<bool>(Constant.ErrorCode.DELETE_FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        public async Task<Response<UserConfigModel>> GetUserConfigByUserId(int userId)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/LeaveManagement.json", "LeaveManagement", "GetUserConfigByUserId");
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserId", userId);

                var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<UserConfigModel>(sqlQuery, param, null, CommandType.Text);

                if (result != null)
                {
                    return new Response<UserConfigModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                else
                {
                    return new Response<UserConfigModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<UserConfigModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<WorkflowCommandModel>> GetWorkflowCommandByName(string workflowCommandName)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/LeaveManagement.json", "LeaveManagement", "GetWorkflowCommandByName");
                DynamicParameters param = new DynamicParameters();
                param.Add("@WorkflowCommandName", workflowCommandName);

                var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<WorkflowCommandModel>(sqlQuery, param, null, CommandType.Text);

                if (result != null)
                {
                    return new Response<WorkflowCommandModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                else
                {
                    return new Response<WorkflowCommandModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<WorkflowCommandModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<WorkflowStateModel>> GetWorkflowStateByName(string workflowStateName)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/LeaveManagement.json", "LeaveManagement", "GetWorkflowStateByName");
                DynamicParameters param = new DynamicParameters();
                param.Add("@WorkflowStateName", workflowStateName);

                var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<WorkflowStateModel>(sqlQuery, param, null, CommandType.Text);

                if (result != null)
                {
                    return new Response<WorkflowStateModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                else
                {
                    return new Response<WorkflowStateModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<WorkflowStateModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }


        public async Task<Response<WorkflowCommandModel>> GetWorkflowCommandById(Guid workflowCommandId)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/LeaveManagement.json", "LeaveManagement", "GetWorkflowCommandById");
                DynamicParameters param = new DynamicParameters();
                param.Add("@WorkflowCommandId", workflowCommandId);

                var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<WorkflowCommandModel>(sqlQuery, param, null, CommandType.Text);

                if (result != null)
                {
                    return new Response<WorkflowCommandModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                else
                {
                    return new Response<WorkflowCommandModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<WorkflowCommandModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Get Remain day off by user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="leaveGroup"></param>
        /// <param name="leaveType"></param>
        /// <returns></returns>
        public async Task<double> GetRemainDayOfByUser(int userId, DateTime fromDate, DateTime toDate, int leaveGroup, int leaveType)
        {
            try
            {
                double countDay = 0;
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/LeaveManagement.json", "LeaveManagement", "GetRemainDayOfByUser");
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserId", userId);
                param.Add("@FromDate", fromDate);
                param.Add("@ToDate", toDate);

                var result = await _dapperUnitOfWork.GetRepository().QueryAsync<RemainDayOffModel>(sqlQuery, param, null, CommandType.Text);
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        if ((leaveGroup == Constant.LeaveGroup.NGHI_THEO_GIO) &&
                                ((leaveType == Constant.LeaveType.NGHI_NUA_CA_DAU) ||
                                 (leaveType == Constant.LeaveType.NGHI_NUA_CA_SAU)) && item.WkDay == 0.5)
                        {
                            countDay += (item.WkDay * 2) ?? 0;
                        }
                        else
                        {
                            countDay += item.WkDay ?? 0;
                        }
                    }
                    if ((leaveGroup == Constant.LeaveGroup.NGHI_THEO_GIO) && ((leaveType == Constant.LeaveType.NGHI_NUA_CA_DAU) || (leaveType == Constant.LeaveType.NGHI_NUA_CA_SAU)))
                    {
                        countDay = countDay * 0.5;
                    }
                }
                return countDay;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return 0;
            }
        }

        /// <summary>
        /// Lịch sử xử lý
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<List<DocumentHistoryViewModel>>> GetHistory(Guid id)
        {
            try
            {
                var rsWF = _workflowHandler.GetDocumentHistory(id);
                if (rsWF.Status == Constant.SUCCESS)
                {
                    #region Lấy bước của trạng thái hiện tại hồ sơ

                    var lm = await FindById(id);
                    var lstUser = await _baseHandler.GetListUser();

                    var stateCurrent = new DocumentHistoryViewModel();

                    if ((lm.Data!=null) && (!lm.Data.IsFinished.HasValue))
                    {
                        stateCurrent.GhiChu = string.Empty;
                        stateCurrent.HanhDong = string.Empty;
                        stateCurrent.NguoiXuLy = lstUser.Where(x => x.ID == lm.Data.CurrentUserID).FirstOrDefault()?.FullName;
                        stateCurrent.NguoiXuLyTiepTheo = string.Empty;
                        stateCurrent.UserIdNguoiXuLy = lm.Data.CurrentUserID;
                    }
                    else
                    {
                        stateCurrent = null;
                    }

                    if (stateCurrent != null)
                    {
                        rsWF.Data.Add(stateCurrent);
                    }
                    else
                    {
                        if ((rsWF.Data.Count > 0) && (rsWF.Data[rsWF.Data.Count - 1].NguoiXuLy.Equals(rsWF.Data[rsWF.Data.Count - 1].NguoiXuLyTiepTheo)))
                        {
                            rsWF.Data[rsWF.Data.Count - 1].NguoiXuLyTiepTheo = string.Empty;
                        }
                    }
                    #endregion

                    return new Response<List<DocumentHistoryViewModel>>(Constant.ErrorCode.SUCCESS_CODE, "Success", rsWF.Data);
                }
                else
                {
                    return new Response<List<DocumentHistoryViewModel>>(Constant.ErrorCode.FAIL_CODE, rsWF.Message, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<DocumentHistoryViewModel>>(Constant.ErrorCode.FAIL_CODE, ex.Message, null);
            }
        }

        /// <summary>
        /// Bước xử lý tiếp theo
        /// </summary>
        /// <param name="dataCommand"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<List<WorkflowCommandModel>>> GetCommand(Guid documentId)
        {
            try
            {
                var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
                var inputCommand = new InputCommand()
                {
                    UserId = user.ID.ToString(),
                    DocumentId = documentId
                };
                //dataCommand.UserId = user.Id.ToString();
                var rsWF = _workflowHandler.GetCommands(inputCommand);
                if (rsWF.Status != -1)
                {
                    var result = new List<WorkflowCommandModel>();
                    foreach (var cmd in rsWF.Data)
                    {
                        var rsWC = await GetWorkflowCommandByName(cmd);
                        if (rsWC.Status == Constant.SUCCESS)
                        {
                            result.Add(rsWC.Data);
                        }
                    }
                    return new Response<List<WorkflowCommandModel>>(Constant.ErrorCode.SUCCESS_CODE, "Success", result);
                }
                else
                {
                    return new Response<List<WorkflowCommandModel>>(Constant.ErrorCode.FAIL_CODE, rsWF.Message, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<WorkflowCommandModel>>(Constant.ErrorCode.FAIL_CODE, ex.Message, null);
            }
        }

        /// <summary>
        /// Người xử lý tiếp theo
        /// </summary>
        /// <param name="dataCommand"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<List<UserInfo>>> GetNextUserProcess(Guid documentId, Guid workflowCommandId)
        {
            try
            {
                var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
                List<UserInfo> data = new List<UserInfo>();
                var workflowCommand = await GetWorkflowCommandById(workflowCommandId);
                var workflowCommandName = string.Empty;
                if (workflowCommand.Status == Constant.SUCCESS)
                {
                    workflowCommandName = workflowCommand.Data?.WorkflowCommandName;
                }
                if (string.IsNullOrEmpty(workflowCommandName))
                {
                    return new Response<List<UserInfo>>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
                }
                var inputCommand = new InputCommand()
                {
                    UserId = user.ID.ToString(),
                    DocumentId = documentId,
                    CommandName = workflowCommandName
                };
                var rsWF = _workflowHandler.GetNextUserProcess(inputCommand);
                if (rsWF.Status != -1)
                {
                    var lm = await FindById(documentId);
                    if (lm.Data != null)
                    {
                        List<string> dataUserId = rsWF.Data.Select(x => x.UserId.ToString()).ToList();
                        foreach (var item in rsWF.Data)
                        {
                            if (item.UserId == lm.Data.PreUserID)
                            {
                                data.Add(item);
                            }
                        }
                        foreach (var item in rsWF.Data)
                        {
                            if (item.UserId != lm.Data.PreUserID)
                            {
                                data.Add(item);
                            }
                        }
                    }
                    return new Response<List<UserInfo>>(Constant.ErrorCode.SUCCESS_CODE, "Success", data);
                }
                else
                {
                    return new Response<List<UserInfo>>(Constant.ErrorCode.FAIL_CODE, rsWF.Message, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<List<UserInfo>>(Constant.ErrorCode.FAIL_CODE, ex.Message, null);
            }
        }

        /// <summary>
        /// Quy trình xử lý
        /// </summary>`
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Response<bool>> ExecuteCommand(ExecuteCommandModel model)
        {
            try
            {
                var user = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);
                var workflowCommand = await GetWorkflowCommandById(model.WorkflowCommandID);
                if (workflowCommand.Status != Constant.SUCCESS)
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
                }
                var command = new InputCommand()
                {
                    DocumentId = model.LeaveManagementID,
                    UserId = user.ID.ToString(),
                    CommandName = workflowCommand.Data?.WorkflowCommandName,
                    Comment = model.Comment
                };
                if (model.NextUserID.HasValue)
                {
                    command.NextUserId = model.NextUserID.Value.ToString();
                }   
                else
                {
                    command.NextUserId = user.ID.ToString();
                }
                Guid userNextForHoSo = new Guid(command.NextUserId);
                var rsWF = _workflowHandler.ExecuteCommand(command);
                if (rsWF.Status != -1)
                {
                    #region Update HoSo
                    var lm = await FindById(command.DocumentId);

                    var lstAllowIds = new List<Guid>();
                    lstAllowIds.Add(userNextForHoSo);
                    var allowIdentityIds = JsonConvert.SerializeObject(lstAllowIds);

                    var sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/LeaveManagement.json", "LeaveManagement", "Proc_Update_Workflow");
                    var param = new DynamicParameters();
                    param.Add("@LeaveManagementID", lm.Data.LeaveManagementID);
                    param.Add("@AllowIdentityIds", allowIdentityIds);
                    param.Add("@IsDraft", false);
                    param.Add("@CurrentUserId", userNextForHoSo);
                    param.Add("@PreUserId", user.ID);
                    param.Add("@LastModifiedByUserID", user.ID);
                    param.Add("@LastModifiedOnDate", DateTime.Now);
                    var dsrs = _workflowHandler.GetStateNameOfDocuments(command.DocumentId);
                    if (dsrs.Status == 1)
                    {
                        param.Add("@IsFinished", dsrs.Data.IsFinished);
                        if (dsrs.Data.IsFinished == true)
                        {
                            param.Add("@IsInProcess", false);
                        }
                        else
                        {
                            param.Add("@IsInProcess", true);
                        }
                        var workflowState = await GetWorkflowStateByName(dsrs.Data.StateName);
                        if (workflowState != null)
                        {
                            param.Add("@WorkflowStateId", workflowState.Data.WorkflowStateID);
                        }
                    }
                    else
                    {
                        param.Add("@IsInProcess", true);
                        param.Add("@IsFinished", false);
                    }

                    var outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);

                    #endregion

                    //#region Update Notification Campain And Save Email

                    //PushNotiAndEmail(hs, dataCommand, userId);

                    //#endregion

                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, "Success", rsWF.Data);
                }
                else
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, rsWF.Message, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, ex.Message, false);
            }
        }
    }
}
