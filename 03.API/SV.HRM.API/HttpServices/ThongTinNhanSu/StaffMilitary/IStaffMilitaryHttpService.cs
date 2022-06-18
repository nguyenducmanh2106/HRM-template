using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IStaffMilitaryHttpService
    {
        Task<Response<List<StaffMilitaryModel>>> GetFilter(EntityGeneric StaffQueryFilter);
        /// <summary>
        /// Hàm tạo thông tin quân ngũ nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(StaffMilitaryCreateModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng StaffMilitary
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<StaffMilitaryModel>> FindById(int recordID);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);

        /// <summary>
        /// Hàm cập nhật thông tin quân ngữ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, StaffMilitaryUpdateModel model);
    }
}
