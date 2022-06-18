using AutoMapper;
using AutoMapper.Configuration;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SV.HRM.Caching.Interface;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.DataProcess.Infrastructure.Interfaces;
using SV.HRM.Models;
using System;
using System.Data;
using System.Threading.Tasks;
using static SV.HRM.Core.Utils.Constant;
using System.Linq;
using System.Collections.Generic;
using NLog;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public class StaffFamilyHandler : IStaffFamilyHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IDapperUnitOfWork _dapperUnitOfWork;
        private readonly IBaseHandler _baseHandler;

        public StaffFamilyHandler(IDapperUnitOfWork dapperUnitOfWork,
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
        public async Task<Response<bool>> Create(StaffFamilyCreateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffFamily)}.json", nameof(StaffFamily), "Proc_Create");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffID", model.StaffID);
                param.Add("@Relationship", model.Relationship);
                param.Add("@FullName", model.FullName);
                param.Add("@Birthday", model.Birthday);
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@ExtraLogic2", model.ExtraLogic2);
                param.Add("@ExtraText3", model.ExtraText3);
                param.Add("@IDCardNo", model.IDCardNo);
                param.Add("@IDCardIssuePlaceID", model.IDCardIssuePlaceID);
                param.Add("@IDCardIssueDate", model.IDCardIssueDate);
                param.Add("@IDCardExpireDate", model.IDCardExpireDate);
                param.Add("@Telephone", model.Telephone);
                param.Add("@ExtraText2", model.ExtraText2);
                param.Add("@PITCode", model.PITCode);
                param.Add("@ExtraText5", model.ExtraText5);
                param.Add("@ExtraText1", model.ExtraText1);
                param.Add("@Addr", model.Addr);
                param.Add("@HoKhauThuongTru", model.HoKhauThuongTru);
                param.Add("@TypeHouse", model.TypeHouse);
                param.Add("@Job", model.Job);
                param.Add("@Income", model.Income);
                param.Add("@CerID", model.CerID);
                param.Add("@CerNumber", model.CerNumber);
                param.Add("@ExtraNumber2", model.ExtraNumber2);
                param.Add("@CerTerritoryID", model.CerTerritoryID);
                param.Add("@CerLocationID", model.CerLocationID);
                param.Add("@CerDistrictID", model.CerDistrictID);
                param.Add("@CerWardID", model.CerWardID);
                param.Add("@CreatedBy", model.CreatedBy);
                param.Add("@CreationDate", DateTime.Now);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteScalarAsync<int>(sqlQuery, param, null, CommandType.StoredProcedure);

                if (outputResult >= SUCCESS) //lưu thành công
                {
                    if (model.FileUpload != null && model.FileUpload.Count > 0)
                    {
                        model.FileUpload?.ForEach(u => { u.Type = FileType.GIA_DINH; u.TypeId = outputResult; });
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
        public async Task<Response<StaffFamilyModel>> FindById(int recordID)
        {
            StaffFamilyModel result;
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffFamily)}.json", nameof(StaffFamily), "FindById");
                result = await _dapperUnitOfWork.GetRepository().QuerySingleOrDefaultAsync<StaffFamilyModel>(sqlQuery, new { StaffFamilyID = recordID });
                if (result != null)
                {
                    //lấy thông tin nơi cấp
                    result.IDCardIssuePlaceName = _baseHandler.GetLocations().SingleOrDefault(g => result.IDCardIssuePlaceID.HasValue && g.LocationID == result.IDCardIssuePlaceID)?.LocationName ?? "";

                    //lấy thông tin dân tộc
                    result.EthnicName = _baseHandler.GetEthnics().SingleOrDefault(g => result.ExtraNumber2.HasValue && g.EthnicId == result.ExtraNumber2)?.EthnicName ?? "";

                    //lấy thông tin quốc tịch
                    result.CountryName = _baseHandler.GetCountries().SingleOrDefault(g => result.CerTerritoryID.HasValue && g.CountryId == result.CerTerritoryID)?.CountryName ?? "";

                    //lấy thông tin tỉnh/thành
                    result.LocationName = _baseHandler.GetLocations().SingleOrDefault(g => result.CerLocationID.HasValue && g.LocationID == result.CerLocationID)?.LocationName ?? "";
                    
                    //lấy thông tin quận/huyện 
                    result.DistrictName = _baseHandler.GetDistricts().SingleOrDefault(g => result.CerDistrictID.HasValue && g.DistrictId == result.CerDistrictID)?.DistrictName ?? "";

                    //lấy thông tin xã/phường 
                    result.WardName = _baseHandler.GetWards().SingleOrDefault(g => result.CerWardID.HasValue && g.WardId == result.CerWardID)?.WardName ?? "";

                    //Lấy thông tin file đính kèm
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.GIA_DINH, recordID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        result.FileUpload = resFile.Data;
                    }
                    return new Response<StaffFamilyModel>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, result);
                }
                return new Response<StaffFamilyModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, result);

            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return new Response<StaffFamilyModel>(Constant.ErrorCode.FAIL_CODE, Constant.ErrorCode.FAIL_MESS, null);
            }
        }


        /// <summary>
        /// Hàm cập nhật thông tin gia đình của nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffFamilyUpdateModel model)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffFamily)}.json", nameof(StaffFamily), "Proc_Update");
                DynamicParameters param = new DynamicParameters();
                param.Add("@StaffFamilyID", id);
                param.Add("@Relationship", model.Relationship);
                param.Add("@FullName", model.FullName);
                param.Add("@Birthday", model.Birthday);
                param.Add("@ExtraNumber1", model.ExtraNumber1);
                param.Add("@ExtraLogic2", model.ExtraLogic2);
                param.Add("@ExtraText3", model.ExtraText3);
                param.Add("@IDCardNo", model.IDCardNo);
                param.Add("@IDCardIssuePlaceID", model.IDCardIssuePlaceID);
                param.Add("@IDCardIssueDate", model.IDCardIssueDate);
                param.Add("@IDCardExpireDate", model.IDCardExpireDate);
                param.Add("@Telephone", model.Telephone);
                param.Add("@ExtraText2", model.ExtraText2);
                param.Add("@PITCode", model.PITCode);
                param.Add("@ExtraText5", model.ExtraText5);
                param.Add("@ExtraText1", model.ExtraText1);
                param.Add("@Addr", model.Addr);
                param.Add("@HoKhauThuongTru", model.HoKhauThuongTru);
                param.Add("@TypeHouse", model.TypeHouse);
                param.Add("@Job", model.Job);
                param.Add("@Income", model.Income);
                param.Add("@CerID", model.CerID);
                param.Add("@CerNumber", model.CerNumber);
                param.Add("@ExtraNumber2", model.ExtraNumber2);
                param.Add("@CerTerritoryID", model.CerTerritoryID);
                param.Add("@CerLocationID", model.CerLocationID);
                param.Add("@CerDistrictID", model.CerDistrictID);
                param.Add("@CerWardID", model.CerWardID);
                param.Add("@LastUpdatedBy", model.LastUpdatedBy);
                param.Add("@LastUpdatedDate", DateTime.Now);

                int outputResult = await _dapperUnitOfWork.GetRepository().ExecuteAsync(sqlQuery, param, null, CommandType.StoredProcedure);


                if (outputResult == SUCCESS) //lưu thành công
                {
                    //Thực hiện xóa file của bản ghi liên quan trong bảng HRM_Attachment trước khi cập nhật lại danh sách file mới
                    var resDeleteFileBeforeUpdate = await _baseHandler.DeleteManyFile(FileType.GIA_DINH, new List<int>() { id });
                    if (resDeleteFileBeforeUpdate != null && resDeleteFileBeforeUpdate.Status == SUCCESS && resDeleteFileBeforeUpdate.Data)
                    {
                        //Thực hiện cập nhật lại danh sách file mới
                        if (model.FileUpload != null && model.FileUpload.Count > 0)
                        {
                            model.FileUpload.ForEach(u => { u.Type = FileType.GIA_DINH; u.TypeId = id; });
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
        /// Tìm quan hệ gia đình
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="relationship"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<StaffFamilyModel> FindByStaffAndRelation(int staffId, int relationship, string name)
        {
            try
            {
                string sqlQuery = JSONObject.GetQueryFromJSON($"SqlCommand/{nameof(StaffFamily)}.json", nameof(StaffFamily), "FindByStaffAndRelation");
                string renderQuery = String.Format(sqlQuery, staffId, relationship, name);
                var staffFamily = _dapperUnitOfWork.GetRepository().QuerySingleOrDefault<StaffFamilyModel>(renderQuery, null, null, CommandType.Text);
                if (staffFamily != null)
                {
                    var resFile = await _baseHandler.GetAttachmentByTypeAndTypeId(FileType.GIA_DINH, staffFamily.StaffFamilyID);
                    if (resFile != null && resFile.Status == SUCCESS && resFile.Data.Count > 0)
                    {
                        staffFamily.FileUpload = resFile.Data;
                    }
                }
                return staffFamily;
            }
            catch (Exception ex)
            {
                logger.Error($"[ERROR]: {ex}");
                return null;
            }
        }
    }
}
