using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.Training
{
    public interface IDonViDaoTaoHandler
    {
        /// <summary>
        /// Hàm thêm trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(DonViDaoTaoCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<DonViDaoTaoModel>> FindById(int recordID);

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
        Task<Response<bool>> Update(int id, DonViDaoTaoUpdateModel model);

        /// <summary>
        /// Check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckRecordInUse(List<object> obj);
    }
}
