using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IUniformHttpService
    {
        /// <summary>
        /// Lọc dữ liệu đồng phục
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<Response<List<UniformModel>>> GetFilter(EntityGeneric queryFilter);

        /// <summary>
        /// Tạo or sửa đồng phục
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<int>> CreateOrUpdate(UniformCreateRequestModel model);

        /// <summary>
        /// Lấy chi tiết đồng phục
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<UniformDetailModel>> GetById(int id);

        /// <summary>
        /// Lấy tên nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<string>> GetStaffFullNameById(int id);

        /// <summary>
        /// Xóa danh sách đồng phục
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany(List<int> ids);
    }
}
