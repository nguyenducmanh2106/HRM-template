using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class AttendanceHttpService : IAttendanceHttpService
    {
        private readonly IHttpHelper _httpHelper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AttendanceHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpHelper = new HttpHelper(client, httpContextAccessor);
            _configuration = configuration;
        }
        /// <summary>
        /// trả về danh sách nhân viên chấm công
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<Response<List<Dictionary<string, object>>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<Dictionary<string, object>>>>("Attendance/GetFilter", queryFilter);
        }
        /// <summary>
        /// đếm tổng số nhân viên,.....
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<Response<List<object>>> CountStaffAttendance(string deptID, DateTime? date)
        {
            return await _httpHelper.GetAsync<Response<List<object>>>($"Attendance/CountStaffAttendance?deptID={deptID}&date={date}");
        }

        public async Task<Response<List<StaffComboboxModel>>> GetComboboxStaff(string deptID, string searchText, DateTime? date, string actions)
        {
            return await _httpHelper.GetAsync<Response<List<StaffComboboxModel>>>($"Attendance/GetComboboxStaff?deptID={deptID}&searchText={searchText}&date={date}&actions={actions}");
        }

        public async Task<Response<bool>> Create(object model)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Attendance/Create", model);
        }

        public async Task<Response<bool>> Update(int id, AttendanceUpdateModel model)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Attendance/Update?id={id}", model);
        }

        public async Task<Response<AttendanceModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<AttendanceModel>>($"Attendance/FindById?recordID={recordID}");
        }
        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Attendance/DeleteMany", recordID);
        }
        /// <summary>
        /// duyệt chấm công
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> ConfirmAttendance(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Attendance/ConfirmAttendance", recordID);
        }
        public async Task<Response<int>> GetDeptIDByUserID(int userID)
        {
            return await _httpHelper.GetAsync<Response<int>>($"Attendance/GetDeptIDByUserID?userID={userID}");
        }

        public async Task<Response<List<object>>> GetNoteLabour()
        {
            return await _httpHelper.GetAsync<Response<List<object>>>($"Attendance/GetNoteLabour");
        }

        public async Task<Response<int>> CheckPostAttendance(int recordID)
        {
            return await _httpHelper.GetAsync<Response<int>>($"Attendance/CheckPostAttendance?recordID={recordID}");
        }
    }
}
