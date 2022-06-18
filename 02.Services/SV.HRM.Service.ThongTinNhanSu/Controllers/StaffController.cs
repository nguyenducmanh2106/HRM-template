using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SV.HRM.Core.Utils;
using static SV.HRM.Core.Utils.Constant;
using Newtonsoft.Json;
using static SV.HRM.Service.ThongTinNhanSu.CustomAuthorize;

namespace SV.HRM.Service.ThongTinNhanSu.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffHandler _StaffHandler;
        private readonly IBaseHandler _baseHandler;
        private readonly IHistoryHandler _historyHandler;
        public StaffController(IStaffHandler dbStaffHandler, IBaseHandler baseHandler, IHistoryHandler historyHandler)
        {
            _StaffHandler = dbStaffHandler;
            _baseHandler = baseHandler;
            _historyHandler = historyHandler;
        }

        [CustomAuthorize(new string[] { Role.HSNV_MANAGER }, Right.VIEW)]
        [HttpPost]
        public async Task<Response<List<StaffModel>>> GetFilter(EntityGeneric queryFilter)
        {
            try
            {
                var lstApplication = _StaffHandler.GetListApplication();
                var responseStaff = await _baseHandler.GetFilter<StaffModel>(queryFilter);
                List<StaffModel> staffs = new List<StaffModel>();
                if (responseStaff != null && responseStaff.Status == Constant.SUCCESS && lstApplication != null)
                {
                    staffs = responseStaff.Data;
                    var staff = (from _staff in staffs
                                 join _app in lstApplication on Convert.ToInt32(_staff.DeptID) equals _app.OrganizationId into tableDefault
                                 from _result in tableDefault.DefaultIfEmpty()
                                 select new StaffModel()
                                 {
                                     LabourSubjectName = _staff.ExtraNumber2 == 0 ? "Viên chức" : _staff.ExtraNumber2 == 1 ? "Hợp đồng 68" : _staff.ExtraNumber2 == 2 ? "Hợp đồng không xác định thời hạn" : _staff.ExtraNumber2 == 3 ? "Hợp đồng ngắn hạn" : _staff.ExtraNumber2 == 4 ? "Hợp đồng thử việc" : _staff.ExtraNumber2 == 5 ? "Hợp đồng chuyên gia" : _staff.ExtraNumber2 == 6 ? "Hợp đồng thời vụ" : _staff.ExtraNumber2 == 7 ? "Hợp đồng khác" : null,
                                     OccupationName = _staff.OccupationName,
                                     PositionName = _staff.PositionName,
                                     StaffID = _staff.StaffID,
                                     StaffCode = _staff.StaffCode,
                                     FullName = _staff.FullName,
                                     OrganizationName = _result?.OrganizationName,
                                     JobTitleName = _staff.JobTitleName,
                                     JoiningDate = _staff.JoiningDate,
                                     Mobiphone = _staff.Mobiphone,
                                     FromDateEnhancedManeuvering = _staff.FromDateEnhancedManeuvering,
                                     TodateEnhancedManeuvering = _staff.TodateEnhancedManeuvering,
                                     DeptIDEnhancedManeuvering = _staff.DeptIDEnhancedManeuvering,
                                     DeptNameEnhancedManeuvering = lstApplication.SingleOrDefault(g => g.OrganizationId == _staff.DeptIDEnhancedManeuvering)?.OrganizationName ?? "",
                                     TrinhDoChuyenMonName = _staff.TrinhDoChuyenMonName
                                 }
                             ).ToList();
                    if (staff != null)
                    {
                        return new Response<List<StaffModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, staff, responseStaff.DataCount, responseStaff.TotalCount, responseStaff.TotalPage, responseStaff.PageNumber, responseStaff.PageSize);
                    }
                    else return new Response<List<StaffModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
                }
                return new Response<List<StaffModel>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tạo thông tin chung nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.HSNV_MANAGER }, Right.CREATE)]
        [HttpPost]
        public async Task<Response<int>> CreateStaffGeneralInfo([FromBody] StaffCreateRequestModel model)
        {
            return await _StaffHandler.CreateStaffGeneralInfo(model);
        }

        /// <summary>
        /// Cập nhập thông tin chung nhân viên 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.HSNV_MANAGER }, Right.UPDATE)]
        [HttpPost]
        public async Task<Response<bool>> UpdateStaffGeneralInfo([FromBody] StaffUpdateRequestModel model)
        {
            return await _StaffHandler.UpdateStaffGeneralInfo(model);
        }

        /// <summary>
        /// Tạo hoặc sửa thông tin khác của nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.HSNV_MANAGER }, Right.UPDATE_TTK)]
        [HttpPost]
        public async Task<Response<int>> CreateOrUpdateStaffOrtherInfo([FromBody] StaffCreateRequestModel model)
        {
            return await _StaffHandler.CreateOrUpdateStaffOrtherInfo(model);
        }

        /// <summary>
        /// Chi tiết thông tin chung nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffDetailModel>> GetStaffGeneralInfoById(int id)
        {
            return await _StaffHandler.GetStaffGeneralInfoById(id);
        }

        /// <summary>
        /// Chi tiết thông tin khác nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffDetailModel>> GetStaffOrtherInfoById(int id)
        {
            return await _StaffHandler.GetStaffOrtherInfoById(id);
        }

        [HttpGet]
        public async Task<Response<int>> GetStaffIDByAccountID(int userID)
        {
            return await _StaffHandler.GetStaffIDByAccountID(userID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// Đã có trigger để xóa các dữ liệu liên quan đến nhân viên bị xóa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CustomAuthorize(new string[] { Role.HSNV_MANAGER }, Right.DELETE)]
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _StaffHandler.DeleteAll(entity);
        }

        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> ReportStaff(EntityGeneric queryFilter)
        {
            try
            {
                var lstApplication = _StaffHandler.GetListApplication();
                var ethnics = _baseHandler.GetEthnics();
                var territorys = _baseHandler.GetCountries();
                var cities = _baseHandler.GetLocations();
                var responseStaff = await _baseHandler.GetFilter<object>(queryFilter);
                List<object> staffs = new List<object>();
                if (responseStaff != null && responseStaff.Status == Constant.SUCCESS && lstApplication != null)
                {
                    staffs = responseStaff.Data;
                    var json = JsonConvert.SerializeObject(staffs);
                    var dictionarys = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                    //return new Response<List<object>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, staffs, responseStaff.DataCount, responseStaff.TotalCount, responseStaff.TotalPage, responseStaff.PageNumber, responseStaff.PageSize);
                    foreach (Dictionary<string, object> dictionary in dictionarys)
                    {
                        if (dictionary.ContainsKey("DeptID"))
                        {
                            var organization = lstApplication.SingleOrDefault(g => dictionary["DeptID"] != null && (g.OrganizationId == (Int64)dictionary["DeptID"]));
                            dictionary["OrganizationName"] = organization?.OrganizationName ?? null;

                            var organizationParentName = lstApplication.SingleOrDefault(g => organization != null && organization.ParentOrganizationId != null && (g.OrganizationId == organization.ParentOrganizationId))?.OrganizationName ?? null;
                            dictionary["Khoi"] = organizationParentName;
                        }
                        if (dictionary.ContainsKey("EthnicID"))
                        {
                            var ethnicName = ethnics.SingleOrDefault(g => dictionary["EthnicID"] != null && (g.EthnicId == (Int64)dictionary["EthnicID"]))?.EthnicName ?? null;
                            dictionary["EthnicName"] = ethnicName;
                        }
                        if (dictionary.ContainsKey("TerritoryID"))
                        {
                            var territoryName = territorys.SingleOrDefault(g => dictionary["TerritoryID"] != null && (g.CountryId == (Int64)dictionary["TerritoryID"]))?.CountryName ?? null;
                            dictionary["TerritoryName"] = territoryName;
                        }
                        if (dictionary.ContainsKey("TerritoryID"))
                        {
                            var territoryName = territorys.SingleOrDefault(g => dictionary["TerritoryID"] != null && (g.CountryId == (Int64)dictionary["TerritoryID"]))?.CountryName ?? null;
                            dictionary["TerritoryName"] = territoryName;
                        }
                        if (dictionary.ContainsKey("IssuePlaceID"))
                        {
                            var issuePlaceName = cities.SingleOrDefault(g => dictionary["IssuePlaceID"] != null && (g.LocationID == (Int64)dictionary["IssuePlaceID"]))?.LocationName ?? null;
                            dictionary["IssuePlaceName"] = issuePlaceName;
                        }
                        if (dictionary.ContainsKey("DeptIDEnhancedManeuvering"))
                        {
                            var khoaPhongTangCuong = lstApplication.SingleOrDefault(g => dictionary["DeptIDEnhancedManeuvering"] != null && (g.OrganizationId == (Int64)dictionary["DeptIDEnhancedManeuvering"]))?.OrganizationName ?? null;
                            dictionary["KhoaPhongTangCuong"] = khoaPhongTangCuong;
                        }
                        if (dictionary.ContainsKey("DeptIDCurrent"))
                        {
                            var organizationCurrent = lstApplication.SingleOrDefault(g => dictionary["DeptIDCurrent"] != null && (g.OrganizationId == (Int64)dictionary["DeptIDCurrent"]))?.OrganizationName ?? null;
                            dictionary["OrganizationCurrent"] = organizationCurrent;
                        }
                        if (dictionary.ContainsKey("ChungChiQuanLyBenhVien"))
                        {
                            dictionary["ChungChiQuanLyBenhVien"] = dictionary["ChungChiQuanLyBenhVien"] != null ? true : false;
                        }
                        if (dictionary.ContainsKey("ChungChiQuanLyDieuDuong"))
                        {
                            dictionary["ChungChiQuanLyDieuDuong"] = dictionary["ChungChiQuanLyDieuDuong"] != null ? true : false;
                        }
                    }
                    if (dictionarys != null)
                    {
                        return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, dictionarys, responseStaff.DataCount, responseStaff.TotalCount, responseStaff.TotalPage, responseStaff.PageNumber, responseStaff.PageSize);
                    }
                    else return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
                }

                return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tạo tài khoản người dùng từ thông tin hồ sơ nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<int>> GenerateAccount()
        {
            return await _StaffHandler.GenerateAccount();
        }
    }
}
