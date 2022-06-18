using Dapper;
using Microsoft.Extensions.Logging;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using System.Linq;
using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SV.HRM.Caching.Interface;
using Microsoft.AspNetCore.Http;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class ProfileDocumentHandler : IProfileDocumentHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly ICached _cached;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseHandler _baseHandler;

        public ProfileDocumentHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler, ICached cached, IHttpContextAccessor httpContextAccessor
            )
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _cached = cached;
            _httpContextAccessor = httpContextAccessor;
            _baseHandler = baseHandler;
        }

        public async Task<Response<bool>> Create(ProfileDocumentCreate model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(ProfileDocument)}.json", nameof(ProfileDocument), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@ProfileDocumentName", model.ProfileDocumentName);
                param.Add("@ProfileDocumentNo", model.ProfileDocumentNo);
                param.Add("@IssueDate", model.IssueDate);
                param.Add("@IssuePlace", model.IssuePlace);
                param.Add("@StaffID", model.StaffID);
                param.Add("@Note", model.Note);
                param.Add("@CreatedDate", DateTime.Now);
                param.Add("@CreatedBy", 0);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = FileType.HO_SO; u.TypeId = outputResult; });
                        await _baseHandler.InsertManyFile(model.FileUpload);
                    }
                    // Write log
                    _baseHandler.CreateLog(new SystemLogModel
                    {
                        ApplicationId = userModel.ApplicationId,
                        UserId = userModel.UserId,
                        ActionByUser = LogAction.ADD,
                        Date = DateTime.Now,
                        Content = $"Tạo mới bản ghi tài liệu {model.StaffID} - {model.ProfileDocumentNo}"
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

        public async Task<Response<ProfileDocumentModel>> FindById(int recordID)
        {
            ProfileDocumentModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(ProfileDocument)}.json", nameof(ProfileDocument), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<ProfileDocumentModel>(sqlQuery, new { ProfileDocumentID = recordID });
                if (result != null)
                {
                    //Lấy thông tin file đính kèm
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.HO_SO, recordID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<ProfileDocumentModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<ProfileDocumentModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<ProfileDocumentModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<List<ProfileDocumentModel>>> GetFilter(EntityGeneric queryFilter)
        {
            var location = _baseHandler.GetLocations();
            var response = await _baseHandler.GetFilter<ProfileDocumentModel>(queryFilter);
            if (response != null && response.Status == Constant.STATUS_OK)
            {
                var data = response.Data;
                foreach (var item in data)
                {
                    switch (item.Type)
                    {
                        case 13:
                            item.IssuePlace = location.SingleOrDefault(g => g.LocationID == Convert.ToInt32(item.IssuePlace))?.LocationName ?? null;
                            break;
                        default:
                            break;
                    }

                    //Lấy thông tin file đính kèm
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(item.Type, item.ProfileDocumentID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        item.FileUpload = resFile.Data;
                        item.FileRecord = item.FileUpload.Count();
                    }
                    if (item.Type != 12)
                    {
                        item.ProfileDocumentID = 0;
                    }
                };

            }
            return response;
        }

        public async Task<Response<bool>> Update(int id, ProfileDocumentUpdate model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(ProfileDocument)}.json", nameof(ProfileDocument), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@ProfileDocumentID", id);
                param.Add("@ProfileDocumentName", model.ProfileDocumentName);
                param.Add("@ProfileDocumentNo", model.ProfileDocumentNo);
                param.Add("@IssueDate", model.IssueDate);
                param.Add("@IssuePlace", model.IssuePlace);
                param.Add("@Note", model.Note);
                param.Add("@UpdatedBy", 0);
                param.Add("@UpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult == SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(FileType.HO_SO, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = FileType.HO_SO; u.TypeId = id; });
                            var resUpdateFile = await _baseHandler.InsertManyFile(model.FileUpload);
                            if (resUpdateFile != null && resUpdateFile.Status == SUCCESS && resUpdateFile.Data)
                            {
                                // Write log
                                _baseHandler.CreateLog(new SystemLogModel
                                {
                                    ApplicationId = userModel.ApplicationId,
                                    UserId = userModel.UserId,
                                    ActionByUser = LogAction.ADD,
                                    Date = DateTime.Now,
                                    Content = $"Cập nhật bản ghi tài liệu {model.StaffID} - {model.ProfileDocumentNo}"
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
                        ActionByUser = LogAction.ADD,
                        Date = DateTime.Now,
                        Content = $"Cập nhật bản ghi tài liệu {model.StaffID} - {model.ProfileDocumentNo}"
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
    }
}
