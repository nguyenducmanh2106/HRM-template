using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface IContractTypeHttpService
    {
        Task<Response<List<ContractTypeModel>>> GetFilter(EntityGeneric queryFilter);

        /// <summary>
        /// Hàm thêm mới loại hợp đồng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(ContractTypeCreateModel model);


        /// <summary>
        /// Tìm bản ghi trong bảng ContractType
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<ContractTypeModel>> FindById(int recordID);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<int> recordID);
        /// <summary>
        /// Hàm xóa bản ghi check có sử dụng hay không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteManyUseRecord(List<object> recordID);

        /// <summary>
        /// Hàm cập nhật loại hợp đồng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(int id, ContractTypeUpdateModel model);
    }
}
