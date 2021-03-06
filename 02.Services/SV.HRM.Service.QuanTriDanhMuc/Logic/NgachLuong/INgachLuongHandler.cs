using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public interface INgachLuongHandler
    {
        /// <summary>
        /// Hàm thêm mới Ngạch lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(NgachLuongCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<NgachLuongModel>> FindById(int recordID);


        /// <summary>
        /// Hàm cập nhật Ngạch lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, NgachLuongUpdateModel model);
        /// <summary>
        /// Check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckRecordInUse(List<object> obj);
    }
}
