using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IQuanLySucKhoeHandler
    {
        /// <summary>
        /// Hàm thêm mới quản lý sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(QuanLySucKhoeCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<QuanLySucKhoeModel>> FindById(int recordID);

        /// <summary>
        /// Hàm cập nhật quản lý sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, QuanLySucKhoeUpdateModel model);

        /// <summary>
        /// Tìm thông tin QLSK
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="healthPeriod"></param>
        /// <returns></returns>
        Task<QuanLySucKhoeModel> FindByStaffAndPeriod(int staffId, int healthPeriod);

        /// <summary>
        /// check date có trong kỳ khám sức
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="healthPeriod"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckDateBetween(int staffId, int healthPeriod,DateTime date);

        /// <summary>
        /// check date có trong kỳ khám sức
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="healthPeriod"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckHealthPeriodAndHistory(int staffId, int healthPeriod);
    }
}
