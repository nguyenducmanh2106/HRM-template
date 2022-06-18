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
    public class QuanLySucKhoeHandler:IQuanLySucKhoeHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly ICached _cached;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseHandler _baseHandler;

        public QuanLySucKhoeHandler(IDapperUnitOfWork dapperUnitOfWork,
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
        public async Task<Response<bool>> Create(QuanLySucKhoeCreateModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(QuanLySucKhoe)}.json", nameof(QuanLySucKhoe), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@HealthPeriodID", model.HealthPeriodID);
                param.Add("@NgayKham", model.NgayKham);
                param.Add("@PhamViKham", model.PhamViKham);
                param.Add("@XepLoaiSucKhoe", model.XepLoaiSucKhoe);
                param.Add("@BenhLy", model.BenhLy);
                param.Add("@CanhBaoBenhTat", model.CanhBaoBenhTat);
                param.Add("@TuVanHuongDan", model.TuVanHuongDan);
                param.Add("@GhiChu", model.GhiChu);
                param.Add("@ChieuCao", model.ChieuCao);
                param.Add("@CanNang", model.CanNang);
                param.Add("@NhomMau", model.NhomMau);

                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreationDate", DateTime.Now);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = FileType.QUAN_LY_SUC_KHOE; u.TypeId = outputResult; });
                        await _baseHandler.InsertManyFile(model.FileUpload);
                    }
                    // Write log
                    _baseHandler.CreateLog(new SystemLogModel
                    {
                        ApplicationId = userModel.ApplicationId,
                        UserId = userModel.UserId,
                        ActionByUser = LogAction.ADD,
                        Date = DateTime.Now,
                        Content = $"Thêm mới bản ghi quản lý sức khỏe {model.StaffID} - {model.HealthPeriodID}"
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
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<QuanLySucKhoeModel>> FindById(int recordID)
        {
            QuanLySucKhoeModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(QuanLySucKhoe)}.json", nameof(QuanLySucKhoe), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<QuanLySucKhoeModel>(sqlQuery, new { QuanLySucKhoeID = recordID });
                if (result != null)
                {
                    //Lấy thông tin file đính kèm
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.QUAN_LY_SUC_KHOE, recordID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<QuanLySucKhoeModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<QuanLySucKhoeModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<QuanLySucKhoeModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Hàm cập nhật quản lý sức khỏe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, QuanLySucKhoeUpdateModel model)
        {
            try
            {
                var userModel = CurrentUser.GetCurrentUserInfo(_httpContextAccessor, _cached);

                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(QuanLySucKhoe)}.json", nameof(QuanLySucKhoe), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@QuanLySucKhoeID", id);
                param.Add("@StaffID", model.StaffID);
                param.Add("@HealthPeriodID", model.HealthPeriodID);
                param.Add("@NgayKham", model.NgayKham);
                param.Add("@PhamViKham", model.PhamViKham);
                param.Add("@XepLoaiSucKhoe", model.XepLoaiSucKhoe);
                param.Add("@BenhLy", model.BenhLy);
                param.Add("@CanhBaoBenhTat", model.CanhBaoBenhTat);
                param.Add("@TuVanHuongDan", model.TuVanHuongDan);
                param.Add("@ChieuCao", model.ChieuCao);
                param.Add("@CanNang", model.CanNang);
                param.Add("@NhomMau", model.NhomMau);
                param.Add("@GhiChu", model.GhiChu);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult == SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(FileType.QUAN_LY_SUC_KHOE, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = FileType.QUAN_LY_SUC_KHOE; u.TypeId = id; });
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
                                    Content = $"Cập nhật bản ghi quản lý sức khỏe {model.StaffID} - {model.HealthPeriodID}"
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
                        Content = $"Cập nhật bản ghi quản lý sức khỏe {model.StaffID} - {model.HealthPeriodID}"
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
        /// Tìm thông tin quản lý sức khỏe
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="healthPeriod"></param>
        /// <returns></returns>
        public async Task<QuanLySucKhoeModel> FindByStaffAndPeriod(int staffId, int healthPeriod)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(QuanLySucKhoe)}.json", nameof(QuanLySucKhoe), "FindByStaffAndPeriod");
                string renderQuery = String.Format(sqlQuery, staffId, healthPeriod);
                var quanlysuckhoe = _dapperUnitOfWork.GetRepository().QuerySingleOrDefault<QuanLySucKhoeModel>(renderQuery, null, null, CommandType.Text);
                if (quanlysuckhoe != null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.QUAN_LY_SUC_KHOE, quanlysuckhoe.QuanLySucKhoeID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        quanlysuckhoe.FileUpload = resFile.Data;
                    }
                }
                return quanlysuckhoe;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }

        public async Task<Response<bool>> CheckDateBetween(int staffId, int healthPeriod, DateTime date)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(QuanLySucKhoe)}.json", nameof(QuanLySucKhoe), "Check_Date_HealthPeriod");
                string renderQuery = String.Format(sqlQuery, staffId, healthPeriod, date.ToString("yyyy-MM-dd"));
                var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<HealthPeriod>(sql: renderQuery, null, null, CommandType.Text);
                if(result != null)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else
                {
                    return new Response<bool>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, true);
                }
            }
            catch (Exception)
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }
        public async Task<Response<bool>> CheckHealthPeriodAndHistory(int staffId, int healthPeriod)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(QuanLySucKhoe)}.json", nameof(QuanLySucKhoe), "Check_HealthPeriod_History");
                string renderQuery = String.Format(sqlQuery, staffId, healthPeriod);
                var result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<HealthPeriod>(sql: renderQuery, null, null, CommandType.Text);
                if (result != null)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else
                {
                    return new Response<bool>(Constant.ErrorCode.NOTFOUND_CODE, Constant.ErrorCode.NOTFOUND_MESS, true);
                }
            }
            catch (Exception)
            {
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }
    }
}
