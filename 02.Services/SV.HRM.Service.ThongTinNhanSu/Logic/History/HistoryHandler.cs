using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
using static SV.HRM.Core.Utils.Constant;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class HistoryHandler : IHistoryHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly ICached _cached;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseHandler _baseHandler;

        public HistoryHandler(IMapper mapper, ICached cached, IDapperUnitOfWork dapperUnitOfWork,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IBaseHandler baseHandler
            )
        {
            _mapper = mapper;
            _cached = cached;
            _dapperUnitOfWork = dapperUnitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _baseHandler = baseHandler;
        }


        /// <summary>
        /// Tạo mới quá trình công tác
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(HistoryCreateRequestModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{Constant.TableInfo.History.TABLE_NAME}.json", Constant.TableInfo.History.TABLE_NAME, "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@Status", model.Status);
                param.Add("@DeptID", model.DeptID);
                param.Add("@HistoryNo", model.HistoryNo);
                param.Add("@DecisionDate", model.DecisionDate);
                param.Add("@FromDate", model.FromDate);
                param.Add("@Todate", model.Todate);
                param.Add("@PositionID", model.PositionID);
                param.Add("@JobTitleID", model.JobTitleID);
                param.Add("@CategoryID", model.CategoryID);
                param.Add("@WorkGroupID", model.WorkGroupID);
                param.Add("@Note", model.Note);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@CreationDate", model.CreationDate);
                param.Add("@LastUpdatedDate", model.LastUpdatedDate);
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@ExtraLogic1", model.ExtraLogic1);
                param.Add("@ExtraText7", model.ExtraText7);
                param.Add("@ExtraText8", model.ExtraText8);
                param.Add("@ExtraNumber3", model.ExtraNumber3);
                param.Add("@ExtraNumber4", model.ExtraNumber4);
                param.Add("@ExtraDate1", model.ExtraDate1);
                param.Add("@ExtraDate2", model.ExtraDate2);
                param.Add("@TrinhDoChuyenMonID", model.TrinhDoChuyenMonID);
                param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);
                int outputResult = param.Get<int>("@OutputResult");

                if (outputResult >= Constant.SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = Constant.FileType.QUA_TRINH_CONG_TAC; u.TypeId = outputResult; });
                        await _baseHandler.InsertManyFile(model.FileUpload);
                    }
                    // Write log
                    _baseHandler.CreateLog(new SystemLogModel
                    {
                        ApplicationId = userModel.ApplicationId,
                        UserId = userModel.UserId,
                        ActionByUser = LogAction.ADD,
                        Date = DateTime.Now,
                        Content = $"Thêm mới bản ghi quá trình công tác {model.StaffID} - {model.HistoryNo}"
                    });
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else if (outputResult == Constant.STATUS_NOTOK)
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE_HISTORY, Constant.ErrorCode.FAIL_MESS_HISTORY, false);
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
        /// Cập nhật quá trình công tác
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, HistoryUpdateRequestModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{Constant.TableInfo.History.TABLE_NAME}.json", Constant.TableInfo.History.TABLE_NAME, "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@v_HistoryID", id);
                param.Add("@StaffID", model.StaffID);
                param.Add("@Status", model.Status);
                param.Add("@DeptID", model.DeptID);
                param.Add("@HistoryNo", model.HistoryNo);
                param.Add("@DecisionDate", model.DecisionDate);
                param.Add("@FromDate", model.FromDate);
                param.Add("@Todate", model.Todate);
                param.Add("@PositionID", model.PositionID);
                param.Add("@JobTitleID", model.JobTitleID);
                param.Add("@CategoryID", model.CategoryID);
                param.Add("@WorkGroupID", model.WorkGroupID);
                param.Add("@Note", model.Note);
                param.Add("@ExtraText7", model.ExtraText7);
                param.Add("@ExtraText8", model.ExtraText8);
                param.Add("@ExtraNumber3", model.ExtraNumber3);
                param.Add("@ExtraNumber4", model.ExtraNumber4);
                param.Add("@ExtraDate1", model.ExtraDate1);
                param.Add("@ExtraDate2", model.ExtraDate2);
                param.Add("@TrinhDoChuyenMonID", model.TrinhDoChuyenMonID);
                param.Add("@ExtraNumber3", model.ExtraNumber3);
                param.Add("@ExtraNumber4", model.ExtraNumber4);
                param.Add("@ExtraDate1", model.ExtraDate1);
                param.Add("@ExtraDate2", model.ExtraDate2);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", model.LastUpdatedDate);
                param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);
                int outputResult = param.Get<int>("@OutputResult");

                if (outputResult == Constant.SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(Constant.FileType.QUA_TRINH_CONG_TAC, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == Constant.SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = Constant.FileType.QUA_TRINH_CONG_TAC; u.TypeId = id; });
                            var resUpdateFile = await _baseHandler.InsertManyFile(model.FileUpload);
                            if (resUpdateFile != null && resUpdateFile.Status == Constant.SUCCESS && resUpdateFile.Data)
                            {
                                // Write log
                                _baseHandler.CreateLog(new SystemLogModel
                                {
                                    ApplicationId = userModel.ApplicationId,
                                    UserId = userModel.UserId,
                                    ActionByUser = LogAction.UPDATE,
                                    Date = DateTime.Now,
                                    Content = $"Cập nhật bản ghi quá trình công tác {model.StaffID} - {model.HistoryNo}"
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
                        Content = $"Cập nhật bản ghi quá trình công tác {model.StaffID} - {model.HistoryNo}"
                    });
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else if (outputResult == Constant.DATA_IS_EXIST) // trường hợp quá trình công tác cũ có ngày kết thúc lớn hơn hoặc bằng ngày bắt đầu của quá trình công tác chuẩn bị tạo
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE_HISTORY, Constant.ErrorCode.FAIL_MESS_HISTORY, false);
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
        /// Hàm tạo quá trình công tác trước khi vào công ty
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> CreateBeforeJoiningCompany(HistoryCreateBeforeJoiningCompanyRequestModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{Constant.TableInfo.History.TABLE_NAME}.json", Constant.TableInfo.History.TABLE_NAME, "CreateBeforeJoiningCompany");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@FromDate", model.FromDate);
                param.Add("@Todate", model.Todate);
                param.Add("@ExtraText1", model.ExtraText1);
                param.Add("@ExtraText2", model.ExtraText2);
                param.Add("@ExtraText3", model.ExtraText3);
                param.Add("@ExtraText4", model.ExtraText4);
                param.Add("@ExtraText5", model.ExtraText5);
                param.Add("@ExtraText6", model.ExtraText6);
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@ExtraNumber2", model.ExtraNumber2);
                param.Add("@ExtraLogic1", model.ExtraLogic1);
                param.Add("@Note", model.Note);
                //THêm trường xác định có công tác trong ngành y tế không
                param.Add("@ExtraLogic2", model.ExtraLogic2);
                param.Add("@ExtraLogic3", model.ExtraLogic3);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@CreationDate", model.CreationDate);
                param.Add("@LastUpdatedDate", model.LastUpdatedDate);
                param.Add("@CreatedBy", model.CreatedBy);
                //param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);
                int result = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);
                //int result = param.Get<int>("@OutputResult");
                if (result > 0)
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = Constant.FileType.QUA_TRINH_CONG_TAC; u.TypeId = result; });
                        await _baseHandler.InsertManyFile(model.FileUpload);
                    }
                    // Write log
                    _baseHandler.CreateLog(new SystemLogModel
                    {
                        ApplicationId = userModel.ApplicationId,
                        UserId = userModel.UserId,
                        ActionByUser = LogAction.ADD,
                        Date = DateTime.Now,
                        Content = $"Thêm mới bản ghi quá trình công tác {model.StaffID} - {model.FromDate.Value.ToString(Constant.DateTimeFormat.DDMMYYYY)}"
                    });
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

        /// <summary>
        /// Hàm lấy về giá trị hết hạn của quá trình công tác gần nhất
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<DateTime>> GetMinFromDate(int staffID)
        {
            DateTime? result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{Constant.TableInfo.History.TABLE_NAME}.json", Constant.TableInfo.History.TABLE_NAME, "GetMinFromDate");
                var resultQuery = _dapperUnitOfWork.GetRepository().QuerySingleOrDefault<object>(sqlQuery, new { StaffID = staffID });
                if (resultQuery != null && (resultQuery as IDictionary<string, object>).ContainsKey("Todate"))
                {
                    result = (resultQuery as IDictionary<string, object>)["Todate"].AsDateTime();
                    return new Response<DateTime>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result ?? DateTime.MinValue);
                }
                else return new Response<DateTime>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, DateTime.MinValue);
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<DateTime>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, DateTime.MinValue);
            }
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<HistoryModel>> FindById(int recordID)
        {
            HistoryModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(History)}.json", nameof(History), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<HistoryModel>(sqlQuery, new { HistoryID = recordID });
                if (result != null)
                {
                    //lấy thông tin file
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.QUA_TRINH_CONG_TAC, recordID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }

                    //lấy thông tin đơn vị
                    var orgs = _baseHandler.GetOrganization();
                    if (orgs != null)
                    {
                        var organizationName = orgs.SingleOrDefault(g => g.OrganizationId == result?.DeptID);
                        if (organizationName != null)
                        {
                            result.OrganizationName = organizationName.OrganizationName;
                        }
                    }

                    return new Response<HistoryModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<HistoryModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                throw ex;
                //return new Response<T>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);
            }
        }

        /// <summary>
        /// Cập nhật quá trình công tác trước khi vào công ty
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> UpdateBeforeJoiningCompany(int id, HistoryUpdateBeforeJoiningCompanyRequestModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{Constant.TableInfo.History.TABLE_NAME}.json", Constant.TableInfo.History.TABLE_NAME, "UpdateBeforeJoiningCompany");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@FromDate", model.FromDate);
                param.Add("@Todate", model.Todate);
                param.Add("@ExtraText1", model.ExtraText1);
                param.Add("@ExtraText2", model.ExtraText2);
                param.Add("@ExtraText3", model.ExtraText3);
                param.Add("@ExtraText4", model.ExtraText4);
                param.Add("@ExtraText5", model.ExtraText5);
                param.Add("@ExtraText6", model.ExtraText6);
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@ExtraNumber2", model.ExtraNumber2);
                param.Add("@Note", model.Note);
                //THêm trường xác định có công tác trong ngành y tế không
                param.Add("@ExtraLogic2", model.ExtraLogic2);
                param.Add("@ExtraLogic3", model.ExtraLogic3);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", model.LastUpdatedDate);
                param.Add("@HistoryID", id);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult == Constant.SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(Constant.FileType.QUA_TRINH_CONG_TAC, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == Constant.SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = Constant.FileType.QUA_TRINH_CONG_TAC; u.TypeId = id; });
                            var resUpdateFile = await _baseHandler.InsertManyFile(model.FileUpload);
                            if (resUpdateFile != null && resUpdateFile.Status == Constant.SUCCESS && resUpdateFile.Data)
                            {
                                // Write log
                                _baseHandler.CreateLog(new SystemLogModel
                                {
                                    ApplicationId = userModel.ApplicationId,
                                    UserId = userModel.UserId,
                                    ActionByUser = LogAction.UPDATE,
                                    Date = DateTime.Now,
                                    Content = $"Thêm mới bản ghi quá trình công tác {model.StaffID} - {model.FromDate.Value.ToString(Constant.DateTimeFormat.DDMMYYYY)}"
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
                        Content = $"Thêm mới bản ghi quá trình công tác {model.StaffID} - {model.FromDate.Value.ToString(Constant.DateTimeFormat.DDMMYYYY)}"
                    });
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
        /// Tìm quá trình công tác có trạng thái là điều động tăng cường và có ngày trùng ngày quá trình công tác hiện tại
        /// phục vụ nghiệp vụ BV Đức Giang lấy
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<HistoryModel>> FindByBelongHistoryNow(int recordID, DateTime? FromDate, DateTime? Todate)
        {
            HistoryModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(History)}.json", nameof(History), "FindByBelongHistoryNow");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<HistoryModel>(sqlQuery, new { HistoryID = recordID, FromDate = FromDate, Todate = Todate });
                if (result != null)
                {
                    //lấy thông tin file
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.QUA_TRINH_CONG_TAC, recordID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }

                    //lấy thông tin đơn vị
                    var orgs = _baseHandler.GetOrganization();
                    if (orgs != null)
                    {
                        var organizationName = orgs.SingleOrDefault(g => g.OrganizationId == result?.DeptID);
                        if (organizationName != null)
                        {
                            result.OrganizationName = organizationName.OrganizationName;
                        }
                    }

                    return new Response<HistoryModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<HistoryModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                throw ex;
                //return new Response<T>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);
            }
        }

        public async Task<HistoryModel> FindByStaffAndDate(int staffId, int status, DateTime? fromDate, DateTime? toDate)
        {
            HistoryModel result;
            try
            {
                string sqlQuery = string.Empty;
                if (status == Constant.TrangThaiQuaTrinhCongTac.NUM_DIEU_DONG_TANG_CUONG)
                {
                    sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(History)}.json", nameof(History), "FindByStaffAndDate2");
                }
                else
                {
                    sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(History)}.json", nameof(History), "FindByStaffAndDate1");
                }
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<HistoryModel>(sqlQuery, new { StaffID = staffId, FromDate = fromDate, ToDate = toDate });
                if (result != null)
                {
                    //lấy thông tin file
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.QUA_TRINH_CONG_TAC, result.HistoryID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Lấy quá trình công tác mới nhất có trạng thái khác tăng cường
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<HistoryModel>> GetHistoryLatest(int recordID)
        {
            HistoryModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(History)}.json", nameof(History), "GetHistoryLatest");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<HistoryModel>(sqlQuery, new { StaffID = recordID });
                if (result != null)
                {
                    //lấy thông tin đơn vị
                    var orgs = _baseHandler.GetOrganization();
                    if (orgs != null)
                    {
                        var organizationName = orgs.SingleOrDefault(g => g.OrganizationId == result?.DeptID);
                        if (organizationName != null)
                        {
                            result.OrganizationName = organizationName.OrganizationName;
                        }
                    }

                    return new Response<HistoryModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<HistoryModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);

            }
            catch (Exception ex)
            {
                throw ex;
                //return new Response<T>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);
            }
        }
    }
}
