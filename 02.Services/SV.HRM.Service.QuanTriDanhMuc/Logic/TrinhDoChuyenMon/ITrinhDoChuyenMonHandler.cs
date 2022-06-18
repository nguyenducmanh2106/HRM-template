using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public interface ITrinhDoChuyenMonHandler
    {
        /// <summary>
        /// Hàm thêm trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(TrinhDoChuyenMonCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<TrinhDoChuyenMonModel>> FindById(int recordID);
        /// <summary>
        /// check bản ghi có đang được bảng khác sử dụng hay không
        /// </summary>
        /// <param name="recordID"> object chưa id bản ghi và list table liên quan</param>
        /// <returns></returns>
        Task<Response<bool>> FindIdInUse(List<object> recordID);

        /// <summary>
        /// Hàm cập nhật trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, TrinhDoChuyenMonUpdateModel model);
        /// <summary>
        /// Check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckRecordInUse(List<object> obj);
    }
}
