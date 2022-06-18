using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.Attendance
{
    public interface IAttendanceHandler
    {
        // danh sách phòng ban
        List<OrganizationModel> GetListOrganization();
        // đếm tổng số nhân viên,...
        Task<Response<List<object>>> CountStaffAttendance(string deptID, DateTime? date);
        // lấy combobox staff theo phòng ban
        Task<Response<List<StaffComboboxModel>>> GetComboboxStaff(string deptID, string searchText, DateTime? date,string actions);
        /// <summary>
        /// thêm mới chấm công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(object model);
        /// <summary>
        /// cập nhật chấm công
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id,AttendanceUpdateModel model);
        /// <summary>
        /// hàm xác nhận duyệt
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> ConfirmAttendance(List<int> recordID);
        /// <summary>
        /// tìm kiếm bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<AttendanceModel>> FindById(int recordID);
        /// <summary>
        /// lấy id phòng ban theo userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<Response<int>> GetDeptIDByUserID(int userID);

        // lấy thông tin chú thích các công
        Task<Response<List<object>>> GetNoteLabour();
        /// <summary>
        /// check xem bản ghi đã được duyệt hay chưa
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<int>> CheckPostAttendance(int recordID);
    }
}
