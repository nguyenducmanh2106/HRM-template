using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public interface IHolidayHandler
    {
        /// <summary>
        /// Hàm thêm mới Holiday
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(HolidayCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<HolidayModel>> FindById(int recordID);
        /// <summary>
        /// check bản ghi có đang được bảng khác sử dụng hay không
        /// </summary>
        /// <param name="recordID"> object chưa id bản ghi và list table liên quan</param>
        /// <returns></returns>
        Task<Response<bool>> FindIdInUse(List<object> recordID);

        /// <summary>
        /// Hàm cập nhật Holiday
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, HolidayUpdateModel model);
        /// <summary>
        /// Check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckRecordInUse(List<object> obj);
    }
}
