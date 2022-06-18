using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IAttendanceHttpService
    {
        // trả về danh sách chấm công
        Task<Response<List<Dictionary<string, object>>>> GetFilter(EntityGeneric queryFilter);

        // count các thống kê
        Task<Response<List<object>>> CountStaffAttendance(string deptID, DateTime? date);
        // lấy về combobox staff
        Task<Response<List<StaffComboboxModel>>> GetComboboxStaff(string deptID, string searchText, DateTime? date, string actions);
        /// <summary>
        /// thêm mới chấm công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(object model);
        /// <summary>
        /// cập nhật chấm công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, AttendanceUpdateModel model);
        /// <summary>
        /// Tìm bản ghi trong bảng AcademicRank
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<AttendanceModel>> FindById(int recordID);
        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);
        /// <summary>
        /// duyệt chấm công
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> ConfirmAttendance(List<int> recordID);
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
