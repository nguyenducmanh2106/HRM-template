using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class LabourContractHttpService : ILabourContractHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public LabourContractHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        /// <summary>
        /// Tạo mới hợp đồng lao động
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(LabourContractCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"LabourContract/Create", model);
        }

        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"LabourContract/DeleteMany", recordID);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng StaffSalary
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<LabourContractModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<LabourContractModel>>($"LabourContract/FindById?recordID={recordID}");
        }

        public async Task<Response<List<LabourContractModel>>> GetComboboxParentLabourContractID(string layoutCode, string keySearch, int q)
        {
            return await _httpHelper.GetAsync<Response<List<LabourContractModel>>>($"LabourContract/GetComboboxParentLabourContractID?layoutCode={layoutCode}&keySearch={keySearch}&q={q}");
        }

        public async Task<Response<List<LabourContractModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<LabourContractModel>>>($"{StaffQueryFilter.LayoutCode}/GetFilter", StaffQueryFilter);
        }

        public async Task<Response<bool>> Update(int id, LabourContractUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"LabourContract/Update?id={id}", model);
        }
    }
}
