using SV.HRM.Core;
using SV.HRM.Models;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IStaffMilitaryHandler
    {
        /// <summary>
        /// Hàm tạo thông tin quân ngũ nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffMilitaryCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffMilitaryModel>> FindById(int recordID);


        /// <summary>
        /// Hàm cập nhật thông tin quân ngũ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, StaffMilitaryUpdateModel model);
    }
}
