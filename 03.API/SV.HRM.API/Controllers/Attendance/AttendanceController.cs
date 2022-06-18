using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.API.Controllers.Attendance
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AttendanceController : ControllerBase
    {
        private readonly ILogger<AttendanceController> _logger;
        private readonly IAttendanceHttpService _attendanceHttpService;
        private readonly IBaseHttpService _baseService;
        public AttendanceController(ILogger<AttendanceController> logger, IAttendanceHttpService attendanceHttpService, IBaseHttpService baseService)
        {
            _logger = logger;
            _attendanceHttpService = attendanceHttpService;
            _baseService = baseService;
        }

        [HttpPost]
        public async Task<Response<List<Dictionary<string, object>>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _attendanceHttpService.GetFilter(queryFilter);
        }
        [HttpGet]
        public async Task<Response<List<object>>> CountStaffAttendance(string deptID, DateTime? date)
        {
            return await _attendanceHttpService.CountStaffAttendance(deptID,date);
        }

        [HttpGet]
        public async Task<Response<List<StaffComboboxModel>>> GetComboboxStaff(string deptID, string searchText, DateTime? date, string actions)
        {
            return await _attendanceHttpService.GetComboboxStaff(deptID, searchText, date, actions);
        }

        [HttpPost]
        public async Task<Response<bool>> Create(object model)
        {
            return await _attendanceHttpService.Create(model);
        }

        [HttpPost]
        public async Task<Response<bool>> Update(int id, AttendanceUpdateModel model)
        {
            return await _attendanceHttpService.Update(id,model);
        }
        /// <summary>
        /// Tìm bản ghi trong bảng AcademicRank
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<AttendanceModel>> FindById(int recordID)
        {
            return await _attendanceHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _attendanceHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm duyệt chấm công
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> ConfirmAttendance(List<int> recordID)
        {
            return await _attendanceHttpService.ConfirmAttendance(recordID);
        }
        /// <summary>
        /// trả về phòng ban theo userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<int>> GetDeptIDByUserID(int userID)
        {
            return await _attendanceHttpService.GetDeptIDByUserID(userID);
        }
        /// <summary>
        /// trả về danh sách chú thích
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<object>>> GetNoteLabour()
        {
            return await _attendanceHttpService.GetNoteLabour();
        }
        /// <summary>
        /// kiểm tra xem bản ghi đã được duyệt hay chưa
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<int>> CheckPostAttendance(int recordID)
        {
            return await _attendanceHttpService.CheckPostAttendance(recordID);
        }
    }
}
