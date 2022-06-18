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
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class StaffDiplomaHandler : IStaffDiplomaHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly ICached _cached;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseHandler _baseHandler;
        public StaffDiplomaHandler(IMapper mapper, ICached cached, IDapperUnitOfWork dapperUnitOfWork,
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
        /// Tạo mới bằng cấp chứng chỉ của nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffDiplomaCreateRequestModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffDiploma)}.json", nameof(StaffDiploma), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID ?? 0);
                param.Add("@DiplomaID", model.DiplomaID);
                param.Add("@DiplomaNo", model.DiplomaNo?.Trim());
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@ExtraDate3", model.ExtraDate3);
                param.Add("@ExtraText1", model.ExtraText1);
                param.Add("@ExtraDate1", model.ExtraDate1);
                param.Add("@ExtraDate2", model.ExtraDate2);
                param.Add("@ExtraText10", model.ExtraText10);
                param.Add("@ExtraLogic1", model.ExtraLogic1);
                param.Add("@ExtraLogic2", model.ExtraLogic2);
                param.Add("@TrinhDoDaoTaoID", model.TrinhDoDaoTaoID);
                param.Add("@TrinhDoChuyenMonID", model.TrinhDoChuyenMonID);
                //param.Add("@MainSubject", model.MainSubject?.Trim());
                param.Add("@SpecialityID", model.SpecialityID);
                param.Add("@SchoolID", model.SchoolID);
                param.Add("@Note", model.Note);
                param.Add("@Note", model.Note);
                param.Add("@ChuyenKhoaID", model.ChuyenKhoaID);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@CreationDate", DateTime.Now);
                param.Add("@LastUpdatedDate", DateTime.Now);
                param.Add("@CreatedBy", model.CreatedBy);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult >= Constant.SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = Constant.FileType.BANG_CAP_CHUNG_CHI; u.TypeId = outputResult; });
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
        /// Hàm cập nhật bằng cấp chứng chỉ của nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffDiplomaUpdateRequestModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffDiploma)}.json", nameof(StaffDiploma), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffDiplomaID", id);
                param.Add("@StaffID", model.StaffID);
                param.Add("@DiplomaID", model.DiplomaID);
                param.Add("@DiplomaNo", model.DiplomaNo?.Trim());
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@ExtraDate3", model.ExtraDate3);
                param.Add("@ExtraText1", model.ExtraText1);
                param.Add("@ExtraDate1", model.ExtraDate1);
                param.Add("@ExtraDate2", model.ExtraDate2);
                param.Add("@ExtraText10", model.ExtraText10);
                param.Add("@ExtraLogic1", model.ExtraLogic1);
                param.Add("@ExtraLogic2", model.ExtraLogic2);
                param.Add("@TrinhDoDaoTaoID", model.TrinhDoDaoTaoID);
                param.Add("@TrinhDoChuyenMonID", model.TrinhDoChuyenMonID);
                //param.Add("@MainSubject", model.MainSubject?.Trim());
                param.Add("@SpecialityID", model.SpecialityID);
                param.Add("@SchoolID", model.SchoolID);
                param.Add("@Note", model.Note);
                param.Add("@ChuyenKhoaID", model.ChuyenKhoaID);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);
               

                if (outputResult >= Constant.SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(Constant.FileType.BANG_CAP_CHUNG_CHI, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == Constant.SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = Constant.FileType.BANG_CAP_CHUNG_CHI; u.TypeId = id; });
                            var resUpdateFile = await _baseHandler.InsertManyFile(model.FileUpload);
                            if (resUpdateFile != null && resUpdateFile.Status == Constant.SUCCESS && resUpdateFile.Data)
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

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffDiplomaModel>> FindById(int recordID)
        {
            StaffDiplomaModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffDiploma)}.json", nameof(StaffDiploma), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffDiplomaModel>(sqlQuery, new { StaffDiplomaID = recordID });
                if (result != null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.BANG_CAP_CHUNG_CHI, recordID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<StaffDiplomaModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<StaffDiplomaModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffDiplomaModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<StaffDiplomaModel> FindByStaffAndDiplomaNo(int staffId, string diplomaNo)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffDiploma)}.json", nameof(StaffDiploma), "FindByStaffAndDiplomaNo");
                string renderQuery = String.Format(sqlQuery, staffId, diplomaNo);
                var staffDiploma = _dapperUnitOfWork.GetRepository().QuerySingleOrDefault<StaffDiplomaModel>(renderQuery, null, null, CommandType.Text);
                if (staffDiploma!=null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.BANG_CAP_CHUNG_CHI, staffDiploma.StaffDiplomaID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        staffDiploma.FileUpload = resFile.Data;
                    }
                }
                return staffDiploma;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }
    }
}
