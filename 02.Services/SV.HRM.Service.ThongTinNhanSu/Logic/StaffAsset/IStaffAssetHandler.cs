using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.ThongTinNhanSu
{
    public interface IStaffAssetHandler
    {
        /// <summary>
        /// Tìm kiếm danh sách trang bị bảo hộ
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<Response<List<StaffAssetModel>>> GetFilter(EntityGeneric filter);

        /// <summary>
        /// Chi tiết bảo hộ lao động
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<StaffAssetDetailModel>> GetById(int id);

        /// <summary>
        /// Lấy tên nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<string>> GetStaffFullNameById(int id);

        /// <summary>
        /// Tạo or sửa bảo hộ lao động
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<int>> CreateOrUpdate(StaffAssetCreateRequestModel model);

        /// <summary>
        /// <summary>
        /// Xóa bảo hộ lao động
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteMany(List<int> ids);
    }
}
