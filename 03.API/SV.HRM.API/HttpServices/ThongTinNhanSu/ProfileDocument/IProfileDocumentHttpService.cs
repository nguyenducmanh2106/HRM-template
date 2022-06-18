using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IProfileDocumentHttpService
    {
        Task<Response<List<ProfileDocumentModel>>> GetFilter(EntityGeneric StaffQueryFilter);
        
        /// <summary>
        /// Hàm thêm mới hồ sơ đính kèm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(ProfileDocumentCreate model);

        /// <summary>
        /// Tìm bản ghi trong bảng ProfileDocument
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<ProfileDocumentModel>> FindById(int recordID);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);

        /// <summary>
        /// Hàm cập nhật hồ sơ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, ProfileDocumentUpdate model);
    }
}
