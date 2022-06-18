using SV.HRM.Core;
using SV.HRM.Models;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IStaffFamilyHandler
    {
        /// <summary>
        /// Hàm tạo thông tin gia đình nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffFamilyCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffFamilyModel>> FindById(int recordID);

        /// <summary>
        /// Hàm cập nhật thông tin gia đình của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, StaffFamilyUpdateModel model);

        /// <summary>
        /// Tìm  theo mối quan hệ và tên
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="relationship"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<StaffFamilyModel> FindByStaffAndRelation(int staffId, int relationship, string name);
    }
}
