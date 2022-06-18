using Dapper;
using Microsoft.Extensions.Logging;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using System.Collections.Generic;
using System;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using System.Data;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class StaffPartyHandler : IStaffPartyHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;

        public StaffPartyHandler(IDapperUnitOfWork dapperUnitOfWork,
             IBaseHandler baseHandler
            )
        {
            _dapperUnitOfWork = dapperUnitOfWork;
            _baseHandler = baseHandler;
        }

        /// <summary>
        /// Tạo mới thông tin gia đình của nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffPartyCreateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffParty)}.json", nameof(StaffParty), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@ChiBoID", model.ChiBoID);
                param.Add("@ChucVuDangID", model.ChucVuDangID);
                param.Add("@TuNgay", model.TuNgay);
                param.Add("@DenNgay", model.DenNgay);
                param.Add("@LyLuanChinhTri", model.LyLuanChinhTri);
                param.Add("@NgheNghiepTruocKhiVaoDang", model.NgheNghiepTruocKhiVaoDang);
                param.Add("@NgayVaoDang", model.NgayVaoDang);
                param.Add("@NgayVaoDangChinhThuc", model.NgayVaoDangChinhThuc);
                param.Add("@NoiKetNapDang", model.NoiKetNapDang);
                param.Add("@NoiCongNhan", model.NoiCongNhan);
                param.Add("@SoLiLich", model.SoLiLich);
                param.Add("@SoTheDangVien", model.SoTheDangVien);
                param.Add("@GhiChu", model.GhiChu);
                param.Add("@NgayChuyenDen", model.NgayChuyenDen);
                param.Add("@NoiChuyenDen", model.NoiChuyenDen);
                param.Add("@NgayChuyenDi", model.NgayChuyenDi);
                param.Add("@NoiChuyenDi", model.NoiChuyenDi);
                param.Add("@NgayBiChet", model.NgayBiChet);
                param.Add("@LyDoChet", model.LyDoChet);
                param.Add("@NgayRaKhoiDang", model.NgayRaKhoiDang);
                param.Add("@HinhThucRaKhoiDang", model.HinhThucRaKhoiDang);

                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreationDate", DateTime.Now);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = FileType.THONG_TIN_DANG; u.TypeId = outputResult; });
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
        public async Task<Response<StaffPartyModel>> FindById(int recordID)
        {
            StaffPartyModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffParty)}.json", nameof(StaffParty), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffPartyModel>(sqlQuery, new { StaffPartyID = recordID });
                if (result != null)
                {
                    //Lấy thông tin file đính kèm
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.THONG_TIN_DANG, recordID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<StaffPartyModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<StaffPartyModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffPartyModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }


        /// <summary>
        /// Hàm cập nhật thông tin gia đình của nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffPartyUpdateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffParty)}.json", nameof(StaffParty), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffPartyID", id);
                param.Add("@ChiBoID", model.ChiBoID);
                param.Add("@ChucVuDangID", model.ChucVuDangID);
                param.Add("@TuNgay", model.TuNgay);
                param.Add("@DenNgay", model.DenNgay);
                param.Add("@LyLuanChinhTri", model.LyLuanChinhTri);
                param.Add("@NgheNghiepTruocKhiVaoDang", model.NgheNghiepTruocKhiVaoDang);
                param.Add("@NgayVaoDang", model.NgayVaoDang);
                param.Add("@NgayVaoDangChinhThuc", model.NgayVaoDangChinhThuc);
                param.Add("@NoiKetNapDang", model.NoiKetNapDang);
                param.Add("@NoiCongNhan", model.NoiCongNhan);
                param.Add("@SoLiLich", model.SoLiLich);
                param.Add("@SoTheDangVien", model.SoTheDangVien);
                param.Add("@GhiChu", model.GhiChu);
                param.Add("@NgayChuyenDen", model.NgayChuyenDen);
                param.Add("@NoiChuyenDen", model.NoiChuyenDen);
                param.Add("@NgayChuyenDi", model.NgayChuyenDi);
                param.Add("@NoiChuyenDi", model.NoiChuyenDi);
                param.Add("@NgayBiChet", model.NgayBiChet);
                param.Add("@LyDoChet", model.LyDoChet);
                param.Add("@NgayRaKhoiDang", model.NgayRaKhoiDang);
                param.Add("@HinhThucRaKhoiDang", model.HinhThucRaKhoiDang);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult == SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(FileType.THONG_TIN_DANG, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = FileType.THONG_TIN_DANG; u.TypeId = id; });
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

        /// <summary>
        /// Tìm và kiểm tra bản ghi đầu tiên để fill giá trị cho các bản ghi tạo sau đó theo nghiệp vụ
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffPartyModel>> FirstOrDefaultByStaffID(int recordID)
        {
            StaffPartyModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffParty)}.json", nameof(StaffParty), "FirstOrDefaultByStaffID");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffPartyModel>(sqlQuery, new { StaffID = recordID });
                return new Response<StaffPartyModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffPartyModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }

        public async Task<StaffPartyModel> FindByStaffAndTime(int staffId, DateTime? fromDate, DateTime? toDate)
        {
            StaffPartyModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffParty)}.json", nameof(StaffParty), "FindByStaffAndTime");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffPartyModel>(sqlQuery, new { StaffID = staffId, FromDate = fromDate, ToDate = toDate });
                return result;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }
    }
}
