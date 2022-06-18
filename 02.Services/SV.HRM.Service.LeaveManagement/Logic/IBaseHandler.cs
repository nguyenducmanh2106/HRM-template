using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.LeaveManagement
{
    public interface IBaseHandler
    {
        /// <summary>
        /// Get list of user
        /// </summary>
        /// <returns></returns>
        Task<List<BaseUserModel>> GetListUser();

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany<T>(List<object> recordID);

        /// <summary>
        /// Xóa bản ghi theo bảng được chọn check bản ghi có được sử dụng không
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id cần xóa</param>
        /// <returns></returns>
        Task<Response<bool>> DeleteManyCheckUseRecord<T>(List<object> recordID);
    }
}
