using Dapper;
using Microsoft.Extensions.Logging;
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
    public class StaffDocumentHandler : IStaffDocumentHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;

        public StaffDocumentHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler
            )
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
        }

        public async Task<Response<bool>> Create(StaffLicenseCreate model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/StaffDocument.json", "StaffDocument", "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@Type", model.Type);
                param.Add("@LicenseNumber", model.LicenseNumber);
                param.Add("@IssuePlaceID", model.IssuePlaceID);
                param.Add("@IssueDate", model.IssueDate);
                param.Add("@ExpiredIssueDate", model.ExpiredIssueDate);
                param.Add("@PracticingScope", model.PracticingScope);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = FileType.VALID_DOCUMENTS; u.TypeId = outputResult; });
                        await _baseHandler.InsertManyFile(model.FileUpload);
                    }
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

        public async Task<Response<StaffLicenseModel>> FindById(int recordID)
        {
            StaffLicenseModel result;
            try
            {
                var location = _baseHandler.GetLocations();
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/StaffDocument.json", "StaffDocument", "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffLicenseModel>(sqlQuery, new { StaffLicenseID = recordID });
                if (result != null)
                {

                    switch (result.Type)
                    {
                        case Constant.Licenses.CMND:
                            result.TypeName = "Chứng minh nhân dân/CCCD";
                            break;
                        case Constant.Licenses.VISA:
                            result.TypeName = "Visa";
                            break;
                        case Constant.Licenses.HOCHIEU:
                            result.TypeName = "Hộ chiếu";
                            break;
                        case Constant.Licenses.GPLD:
                            result.TypeName = "Giấy phép lao động";
                            break;
                        case Constant.Licenses.CCHN:
                            result.TypeName = "Chứng chỉ hành nghề";
                            break;
                        default:
                            break;
                    }
                    result.IssuePlaceName = location?.SingleOrDefault(g => g.LocationID == result.IssuePlaceID)?.LocationName ?? "";
                    //Lấy thông tin file đính kèm
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.VALID_DOCUMENTS, recordID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<StaffLicenseModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<StaffLicenseModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffLicenseModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<Response<List<StaffLicenseModel>>> GetFilter(EntityGeneric queryFilter)
        {
            var location = _baseHandler.GetLocations();
            var response = await _baseHandler.GetFilter<StaffLicenseModel>(queryFilter);
            if (response != null && response.Status == Constant.STATUS_OK)
            {
                var data = response.Data;
                foreach (var item in data)
                {
                    switch (item.Type)
                    {
                        case Constant.Licenses.CMND:
                            item.TypeName = "Chứng minh nhân dân/CCCD";
                            break;
                        case Constant.Licenses.VISA:
                            item.TypeName = "Visa";
                            break;
                        case Constant.Licenses.HOCHIEU:
                            item.TypeName = "Hộ chiếu";
                            break;
                        case Constant.Licenses.GPLD:
                            item.TypeName = "Giấy phép lao động";
                            break;
                        case Constant.Licenses.CCHN:
                            item.TypeName = "Chứng chỉ hành nghề";
                            break;
                        default:
                            break;
                    }
                    item.IssuePlaceName = location?.SingleOrDefault(g => g.LocationID == item.IssuePlaceID)?.LocationName ?? "";
                    //Lấy thông tin file đính kèm
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.VALID_DOCUMENTS, item.StaffLicenseID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        item.FileUpload = resFile.Data;
                    }
                };

            }
            return response;
        }

        public async Task<Response<bool>> Update(int id, StaffLicenseUpdate model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/StaffDocument.json", "StaffDocument", "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffLicenseID", id);
                param.Add("@StaffID", model.StaffID);
                param.Add("@Type", model.Type);
                param.Add("@LicenseNumber", model.LicenseNumber);
                param.Add("@IssuePlaceID", model.IssuePlaceID);
                param.Add("@IssueDate", model.IssueDate);
                param.Add("@ExpiredIssueDate", model.ExpiredIssueDate);
                param.Add("@PracticingScope", model.PracticingScope);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult == SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(FileType.VALID_DOCUMENTS, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = FileType.VALID_DOCUMENTS; u.TypeId = id; });
                            var resUpdateFile = await _baseHandler.InsertManyFile(model.FileUpload);
                            if (resUpdateFile != null && resUpdateFile.Status == SUCCESS && resUpdateFile.Data)
                            {
                                return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                            }
                        }
                    }
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
