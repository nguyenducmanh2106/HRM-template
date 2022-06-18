using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IUniformHandler
    {
        /// <summary>
        /// Lọc danh sách đồng phục
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<Response<List<UniformModel>>> GetFilter(EntityGeneric filter);

        /// <summary>
        /// Chi tiết đồng phục
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
        /// Tạo or sửa đồng phục
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<int>> CreateOrUpdate(UniformCreateRequestModel model);

        /// <summary>
        /// Xóa danh sách đồng phục
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany(List<int> ids);
    }
}
