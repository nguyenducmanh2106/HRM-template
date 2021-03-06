using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public interface ILeaveTypeHandler
    {
        /// <summary>
        /// Hàm thêm mới nhóm kiểu nghỉ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(LeaveTypeCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<LeaveTypeModel>> FindById(int recordID);


        /// <summary>
        /// Hàm cập nhật nhóm kiểu nghỉ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, LeaveTypeUpdateModel model);
        /// <summary>
        /// Check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckRecordInUse(List<object> obj);
    }
}
