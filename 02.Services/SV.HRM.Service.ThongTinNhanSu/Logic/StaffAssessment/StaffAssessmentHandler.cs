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
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class StaffAssessmentHandler : IStaffAssessmentHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;

        public StaffAssessmentHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler
            )
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
        }
        /// <summary>
        /// Tạo mới nhận thưởng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffAssessmentCreateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffAssessment)}.json", nameof(StaffAssessment), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@AssessmentType", model.AssessmentType);
                param.Add("@Year", model.Year);
                param.Add("@Note", model.Note);
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreationDate", DateTime.Now);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

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


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffAssessmentModel>> FindById(int recordID)
        {
            StaffAssessmentModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffAssessment)}.json", nameof(StaffAssessment), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffAssessmentModel>(sqlQuery, new { StaffAssessmentID = recordID });
                if (result != null)
                {
                    return new Response<StaffAssessmentModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<StaffAssessmentModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffAssessmentModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Hàm cập nhật khen thưởng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffAssessmentUpdateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffAssessment)}.json", nameof(StaffAssessment), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffAssessmentID", id);
                param.Add("@StaffID", model.StaffID);
                param.Add("@AssessmentType", model.AssessmentType);
                param.Add("@Year", model.Year);
                param.Add("@Note", model.Note);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult == Constant.SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(Constant.FileType.KHEN_THUONG, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == Constant.SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
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

        public async Task<StaffAssessmentModel> FindByDecisionNo(int AssessmentType, int objectId, string decisionNo)
        {
            try
            {
                var sqlQuery = string.Empty;
                if (AssessmentType.Equals(Constant.LoaiKhenThuong.NUM_KHEN_THUONG_CA_NHAN))
                {
                    sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffAssessment)}.json", nameof(StaffAssessment), "FindByStaffAndDecisionNo");
                }
                if (AssessmentType.Equals(Constant.LoaiKhenThuong.NUM_KHEN_THUONG_TAP_THE))
                {
                    sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffAssessment)}.json", nameof(StaffAssessment), "FindByDeptAndDecisionNo");
                }

                string renderQuery = String.Format(sqlQuery, objectId, decisionNo);
                var Assessment = _dapperUnitOfWork.GetRepository().QuerySingleOrDefault<StaffAssessmentModel>(renderQuery, null, null, CommandType.Text);
                if (Assessment != null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.KHEN_THUONG, Assessment.StaffAssessmentID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        Assessment.FileUpload = resFile.Data;
                    }
                }
                return Assessment;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }

        public async Task<Response<bool>> CheckStaffAssessmentInHistory(int staffID, int year)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffAssessment)}.json", nameof(StaffAssessment), "Check_Staff_Assessment_In_History");
                string renderQuery = String.Format(sqlQuery, staffID, year);
                var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<History>(renderQuery,null,null,CommandType.Text);
                if (result != null)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                return new Response<bool>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, false);
            }
            catch (Exception)
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }
    }
}
