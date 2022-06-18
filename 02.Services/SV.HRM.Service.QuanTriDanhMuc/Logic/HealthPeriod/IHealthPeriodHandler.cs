using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public interface IHealthPeriodHandler
    {
        /// <summary>
        /// Hàm thêm mới kỳ khám sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(HealthPeriodCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<HealthPeriodModel>> FindById(int recordID);
        /// <summary>
        /// check bản ghi có đang được bảng khác sử dụng hay không
        /// </summary>
        /// <param name="recordID"> object chưa id bản ghi và list table liên quan</param>
        /// <returns></returns>
        Task<Response<bool>> FindIdInUse(List<object> recordID);

        /// <summary>
        /// Hàm cập nhật kỳ khám sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, HealthPeriodUpdateModel model);

        /// <summary>
        /// Hàm kiểm tra fromdate và todate có trong nhiệm kỳ nào không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckDateInPeriod(HealthPeriod model);
        /// <summary>
        /// Check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckRecordInUse(List<object> obj);
    }
}
