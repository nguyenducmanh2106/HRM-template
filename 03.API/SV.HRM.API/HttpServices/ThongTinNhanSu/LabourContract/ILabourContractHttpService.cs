using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface ILabourContractHttpService
    {
        Task<Response<List<LabourContractModel>>> GetFilter(EntityGeneric StaffQueryFilter);
        Task<Response<List<LabourContractModel>>> GetComboboxParentLabourContractID(string layoutCode, string keySearch, int q);

        /// <summary>
        /// Hàm tạo hợp đồng lao động
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(LabourContractCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng LabourContract
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<LabourContractModel>> FindById(int recordID);

        /// <summary>
        /// Hàm cập nhật hợp đồng lao động của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, LabourContractUpdateModel model);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);
    }
}
