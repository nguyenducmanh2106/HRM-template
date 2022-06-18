using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class StaffAssetHttpService : IStaffAssetHttpService
    {
        private readonly IHttpHelper _httpHelper;
        public StaffAssetHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<StaffAssetDetailModel>> GetById(int id)
        {
            return await _httpHelper.GetAsync<Response<StaffAssetDetailModel>>($"StaffAsset/GetById?id={id}");
        }

        public async Task<Response<string>> GetStaffFullNameById(int id)
        {
            return await _httpHelper.GetAsync<Response<string>>($"StaffAsset/GetStaffFullNameById?id={id}");
        }

        public async Task<Response<List<StaffAssetModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<StaffAssetModel>>>("StaffAsset/GetFilter", queryFilter);
        }

        public async Task<Response<int>> CreateOrUpdate(StaffAssetCreateRequestModel model)
        {
            return await _httpHelper.PostAsync<Response<int>>("StaffAsset/CreateOrUpdate", model);
        }

        public async Task<Response<bool>> DeleteMany(List<int> ids)
        {
            return await _httpHelper.PostAsync<Response<bool>>("StaffAsset/DeleteMany", ids);
        }
    }
}
