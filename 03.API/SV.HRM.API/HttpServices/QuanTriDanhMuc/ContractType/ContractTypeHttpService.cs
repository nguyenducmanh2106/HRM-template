using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class ContractTypeHttpService : IContractTypeHttpService
    {
        private IHttpHelper _httpHelper;
        public ContractTypeHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<ContractTypeModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<ContractTypeModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới loại hợp đồng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(ContractTypeCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ContractType/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng ContractType
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<ContractTypeModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<ContractTypeModel>>($"ContractType/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"ContractType/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"ContractType/DeleteManyUseRecord", recordID);
        }

        /// <summary>
        /// Hàm cập nhật loại hợp đồng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, ContractTypeUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ContractType/Update?id={id}", model);
        }
    }
}
