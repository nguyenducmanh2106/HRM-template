using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.Attendance.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IBaseHandler _baseHandler;
        private readonly IAttendanceHandler _attendanceHandler;
        public AttendanceController(IBaseHandler baseHandler, IAttendanceHandler attendanceHandler)
        {
            _baseHandler = baseHandler;
            _attendanceHandler = attendanceHandler;
        }

        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> GetFilter(EntityGeneric queryFilter)
        {
            try
            {
                var listOrganization = _attendanceHandler.GetListOrganization();
                var responseAttendance = await _baseHandler.GetFilter<object>(queryFilter);
                List<object> attendances = new List<object>();
                if (responseAttendance != null && responseAttendance.Status == Constant.SUCCESS && listOrganization != null)
                {
                    attendances = responseAttendance.Data;
                    var json = JsonConvert.SerializeObject(attendances);
                    var dictionarys = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
                    foreach (Dictionary<string, object> dictionary in dictionarys)
                    {
                        if (dictionary.ContainsKey("DeptID"))
                        {
                            var organization = listOrganization.SingleOrDefault(g => dictionary["DeptID"] != null && (g.OrganizationId == (Int64)dictionary["DeptID"]));
                            dictionary["OrganizationName"] = organization?.OrganizationName ?? null;

                            var organizationParentName = listOrganization.SingleOrDefault(g => organization != null && organization.ParentOrganizationId != null && (g.OrganizationId == organization.ParentOrganizationId))?.OrganizationName ?? null;
                            dictionary["Khoi"] = organizationParentName;
                        }
                        if (dictionary.ContainsKey("LichSuXuLy"))
                        {
                            if (dictionary["LichSuXuLy"] != null && dictionary["LichSuXuLy"].ToString().Trim().Length > 0)
                            {
                                var textLSXL = "";
                                var lichSuXuLy = dictionary["LichSuXuLy"].ToString().Split(",");
                                if (lichSuXuLy.Length > 0)
                                {
                                    for (int i = lichSuXuLy.Length; i > 0; i--)
                                    {
                                        textLSXL += (lichSuXuLy[i - 1].ToString() + System.Environment.NewLine);
                                    }
                                }
                                dictionary["LichSuXuLy"] = textLSXL;
                            }
                        }
                    }
                    if (dictionarys != null)
                    {
                        return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, dictionarys, responseAttendance.DataCount, responseAttendance.TotalCount, responseAttendance.TotalPage, responseAttendance.PageNumber, responseAttendance.PageSize);
                    }
                    else
                    {
                        return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
                    }
                }
                return new Response<List<Dictionary<string, object>>>(Constant.ErrorCode.SUCCESS_CODE, Constant.ErrorCode.SUCCESS_MESS, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public async Task<Response<List<object>>> CountStaffAttendance(string deptID, DateTime? date)
        {
            return await _attendanceHandler.CountStaffAttendance(deptID, date);
        }

        [HttpGet]
        public async Task<Response<List<StaffComboboxModel>>> GetComboboxStaff(string deptID, string searchText, DateTime? date, string actions)
        {
            return await _attendanceHandler.GetComboboxStaff(deptID, searchText, date, actions);
        }
        [HttpPost]
        public async Task<Response<bool>> Create(object model)
        {
            return await _attendanceHandler.Create(model);
        }
        [HttpPost]
        public async Task<Response<bool>> Update(int id, AttendanceUpdateModel model)
        {
            return await _attendanceHandler.Update(id, model);
        }
        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<AttendanceModel>> FindById(int recordID)
        {
            return await _attendanceHandler.FindById(recordID);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<SV.HRM.Models.Attendance>(entity);
        }
        /// <summary>
        /// Hàm duyệt chấm công
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> ConfirmAttendance(List<int> recordID)
        {
            return await _attendanceHandler.ConfirmAttendance(recordID);
        }
        /// <summary>
        /// lấy id phòng ban theo userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<int>> GetDeptIDByUserID(int userID)
        {
            return await _attendanceHandler.GetDeptIDByUserID(userID);
        }

        [HttpGet]
        public async Task<Response<List<object>>> GetNoteLabour()
        {
            return await _attendanceHandler.GetNoteLabour();
        }
        /// <summary>
        /// kiểm tra xem bản ghi đã được duyệt hay chưa
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<int>> CheckPostAttendance(int recordID)
        {
            return await _attendanceHandler.CheckPostAttendance(recordID);
        }
    }
}
