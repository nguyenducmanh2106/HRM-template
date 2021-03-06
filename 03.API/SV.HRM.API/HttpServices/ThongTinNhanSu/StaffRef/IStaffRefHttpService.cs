using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IStaffRefHttpService
    {
        /// <summary>
        /// Lọc dữ liệu tham chiếu
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<Response<List<StaffRefModel>>> GetFilter(EntityGeneric queryFilter);

        /// <summary>
        /// Tạo or sửa tham chiếu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<int>> CreateOrUpdate(StaffRefCreateRequestModel model);

        /// <summary>
        /// Chi tiết tham chiếu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<StaffRefDetailModel>> GetById(int id);

        /// <summary>
        /// Xóa tham chiếu
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany(List<int> ids);
    }
}
