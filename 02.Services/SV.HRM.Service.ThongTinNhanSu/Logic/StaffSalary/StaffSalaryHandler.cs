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
using static SV.HRM.Core.Utils.Constant;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class StaffSalaryHandler : IStaffSalaryHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly ICached _cached;
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseHandler _baseHandler;

        public StaffSalaryHandler(IMapper mapper, ICached cached, IDapperUnitOfWork dapperUnitOfWork,
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
        /// Tạo mới quá trình lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffSalaryCreateRequestModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{Constant.TableInfo.StaffSalary.TABLE_NAME}.json", Constant.TableInfo.StaffSalary.TABLE_NAME, "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@NhomNgachLuongID", model.NhomNgachLuongID);
                param.Add("@NgachLuongID", model.NgachLuongID);
                param.Add("@BacLuongID", model.BacLuongID);
                param.Add("@SoQuyetDinh", model.SoQuyetDinh?.Trim());
                param.Add("@NgayQuyetDinh", model.NgayQuyetDinh);
                param.Add("@TuNgay", model.TuNgay);
                param.Add("@DenNgay", model.DenNgay);
                param.Add("@LuongCoSo", model.LuongCoSo);
                param.Add("@HeSoLuong", model.HeSoLuong);
                param.Add("@CurrencyID", model.CurrencyID);
                param.Add("@GhiChu", model.GhiChu);
                param.Add("@PhuCap1", model.PhuCap1);
                param.Add("@PhuCap2", model.PhuCap2);
                param.Add("@PhuCap3", model.PhuCap3);
                param.Add("@PhuCap4", model.PhuCap4);
                param.Add("@PhuCap5", model.PhuCap5);
                param.Add("@LoaiNangLuong", model.LoaiNangLuong);
                param.Add("@HeSoBaoLuu", model.HeSoBaoLuu);
                param.Add("@HeSoThamNienVuotKhung", model.HeSoThamNienVuotKhung);
                param.Add("@SoThangNangLuongTruocHan", model.SoThangNangLuongTruocHan);
                param.Add("@SoThangKeoDaiNangLuong", model.SoThangKeoDaiNangLuong);

                param.Add("@CreatedByUserId", model.CreatedByUserId);
                param.Add("@CreatedOnDate", DateTime.Now);
                param.Add("@LastModifiedByUserId", model.LastModifiedByUserId);
                param.Add("@LastModifiedOnDate", model.LastModifiedOnDate);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = FileType.QUA_TRINH_LUONG; u.TypeId = outputResult; });
                        await _baseHandler.InsertManyFile(model.FileUpload);
                    }
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                else //trường hợp còn lại cụ thể là store proc trả ra = 0 là trường hợp exception lỗi của SQL
                {
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.DUPLICATE_STAFF_SALARY, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        /// <summary>
        /// Xóa quá trình lương
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffSalary)}.json", nameof(StaffSalary), "DeleteMany");

                int result = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, new { @recordIDs = recordID });
                if (result > 0)
                {
                    return new Response<bool>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, true);
                }
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);

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
        public async Task<Response<StaffSalaryModel>> FindById(int recordID)
        {
            StaffSalaryModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffSalary)}.json", nameof(StaffSalary), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffSalaryModel>(sqlQuery, new { StaffSalaryID = recordID });
                if (result != null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.QUA_TRINH_LUONG, recordID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<StaffSalaryModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<StaffSalaryModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffSalaryModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Hàm lấy hệ số thâm niên
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<object>> GetHeSoThamNien(int? staffID, int? bacLuongID,int? staffSalaryID)
        {
            object result = null;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffSalary)}.json", nameof(StaffSalary), "GetHeSoThamNien");
                var resultQuery = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffSalaryModel>(sqlQuery, new { StaffID = staffID, BacLuongID = bacLuongID, StaffSalaryID= staffSalaryID },null,CommandType.StoredProcedure);

                if (resultQuery != null)
                {
                    result = (object)resultQuery;
                }
                return new Response<object>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<object>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        /// <summary>
        /// Cập nhật quá trình lương
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffSalaryUpdateRequestModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{Constant.TableInfo.StaffSalary.TABLE_NAME}.json", Constant.TableInfo.StaffSalary.TABLE_NAME, "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffSalaryID", id);
                param.Add("@StaffID", model.StaffID);
                param.Add("@NhomNgachLuongID", model.NhomNgachLuongID);
                param.Add("@NgachLuongID", model.NgachLuongID);
                param.Add("@BacLuongID", model.BacLuongID);
                param.Add("@SoQuyetDinh", model.SoQuyetDinh?.Trim());
                param.Add("@NgayQuyetDinh", model.NgayQuyetDinh);
                param.Add("@TuNgay", model.TuNgay);
                param.Add("@DenNgay", model.DenNgay);
                param.Add("@LuongCoSo", model.LuongCoSo);
                param.Add("@HeSoLuong", model.HeSoLuong);
                param.Add("@CurrencyID", model.CurrencyID);
                param.Add("@GhiChu", model.GhiChu);
                param.Add("@PhuCap1", model.PhuCap1);
                param.Add("@PhuCap2", model.PhuCap2);
                param.Add("@PhuCap3", model.PhuCap3);
                param.Add("@PhuCap4", model.PhuCap4);
                param.Add("@PhuCap5", model.PhuCap5);
                param.Add("@LoaiNangLuong", model.LoaiNangLuong);
                param.Add("@HeSoBaoLuu", model.HeSoBaoLuu);
                param.Add("@HeSoThamNienVuotKhung", model.HeSoThamNienVuotKhung);
                param.Add("@SoThangNangLuongTruocHan", model.SoThangNangLuongTruocHan);
                param.Add("@SoThangKeoDaiNangLuong", model.SoThangKeoDaiNangLuong);
                param.Add("@LastModifiedByUserId", model.LastModifiedByUserId);
                param.Add("@LastModifiedOnDate", DateTime.Now);
                param.Add("@OutputResult", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);
                int outputResult = param.Get<int>("@OutputResult");
                if (outputResult == SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(FileType.QUA_TRINH_LUONG, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = FileType.QUA_TRINH_LUONG; u.TypeId = id; });
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
                    return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.DUPLICATE_STAFF_SALARY, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<bool>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, false);
            }
        }

        public async Task<StaffSalaryModel> FindByStaffAndDecisionNo(int staffId, string decisionNo, DateTime? decisionDate)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffSalary)}.json", nameof(StaffSalary), "FindByStaffAndDecisionNo");
                var staffSalary = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffSalaryModel>(sqlQuery, new { StaffID = staffId, DecisionNo = decisionNo, DecisionDate = decisionDate }, null, CommandType.StoredProcedure);
                if (staffSalary != null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(Constant.FileType.QUA_TRINH_LUONG, staffSalary.StaffSalaryID);
                    if (resFile != null && resFile.Status == Constant.SUCCESS && resFile.Data.Count > 0)
                    {
                        staffSalary.FileUpload = resFile.Data;
                    }
                }
                return staffSalary;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }

        /// <summary>
        /// lấy quá trình lương liền kề trước đó
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public async Task<Response<StaffSalaryModel>> GetStaffSalary_AdjacentBefore(int staffId, int? recordId)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffSalary)}.json", nameof(StaffSalary), "GetStaffSalary_AdjacentBefore");
                var staffSalary = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffSalaryModel>(sqlQuery, new { StaffID = staffId, StaffSalaryID = recordId }, null, CommandType.StoredProcedure);

                return new Response<StaffSalaryModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, staffSalary);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// check từ ngày đến ngày có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<Response<bool>> CheckStaffSalaryinHistory(int staffId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                bool resultCheck = true;
                if (fromDate != null)
                {
                    string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffSalary)}.json", nameof(StaffSalary), "Check_Staff_Salary_In_History");
                    if (toDate == null)
                    {
                        toDate = DateTime.Now;
                    }
                    string queryRender = String.Format(sqlQuery, staffId, fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"));
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
