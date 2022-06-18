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
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class StaffMilitaryHandler : IStaffMilitaryHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;

        public StaffMilitaryHandler(IDapperUnitOfWork dapperUnitOfWork, IBaseHandler baseHandler)
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
        }
        /// <summary>
        /// Tạo mới thông tin quân ngữ của nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffMilitaryCreateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffMilitary)}.json", nameof(StaffMilitary), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@EnlistmentDate", model.EnlistmentDate);
                param.Add("@DischargeDate", model.DischargeDate);
                param.Add("@MilitaryUnit", model.MilitaryUnit);
                param.Add("@Rank", model.Rank);
                param.Add("@Type", model.Type);
                param.Add("@Kind", model.Kind);
                param.Add("@Is2212", model.Is2212);
                param.Add("@Is2707", model.Is2707);
                param.Add("@Note", model.Note);

                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreationDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = FileType.THONG_TIN_QUAN_NGU; u.TypeId = outputResult; });
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


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffMilitaryModel>> FindById(int recordID)
        {
            StaffMilitaryModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffMilitary)}.json", nameof(StaffMilitary), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffMilitaryModel>(sqlQuery, new { StaffMilitaryID = recordID });
                if (result != null)
                {
                    //Lấy thông tin file đính kèm
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.THONG_TIN_QUAN_NGU, recordID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<StaffMilitaryModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<StaffMilitaryModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffMilitaryModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }


        /// <summary>
        /// Hàm cập nhật thông tin quân ngữ của nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffMilitaryUpdateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffMilitary)}.json", nameof(StaffMilitary), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffMilitaryID", id);
                param.Add("@EnlistmentDate", model.EnlistmentDate);
                param.Add("@DischargeDate", model.DischargeDate);
                param.Add("@MilitaryUnit", model.MilitaryUnit);
                param.Add("@Rank", model.Rank);
                param.Add("@Type", model.Type);
                param.Add("@Kind", model.Kind);
                param.Add("@Is2212", model.Is2212);
                param.Add("@Is2707", model.Is2707);
                param.Add("@Note", model.Note);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult == SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(FileType.THONG_TIN_QUAN_NGU, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = FileType.THONG_TIN_QUAN_NGU; u.TypeId = id; });
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
