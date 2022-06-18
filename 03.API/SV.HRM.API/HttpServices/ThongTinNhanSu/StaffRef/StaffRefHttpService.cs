using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class StaffRefHttpService : IStaffRefHttpService
    {
        private readonly IHttpHelper _httpHelper;
        public StaffRefHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<StaffRefDetailModel>> GetById(int id)
        {
            return await _httpHelper.GetAsync<Response<StaffRefDetailModel>>($"StaffRef/GetById?id={id}");
        }

        public async Task<Response<List<StaffRefModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<StaffRefModel>>>("StaffRef/GetFilter", StaffQueryFilter);
        }

        public async Task<Response<int>> CreateOrUpdate(StaffRefCreateRequestModel model)
        {
            return await _httpHelper.PostAsync<Response<int>>("StaffRef/CreateOrUpdate", model);
        }

        public async Task<Response<bool>> DeleteMany(List<int> ids)
        {
            return await _httpHelper.PostAsync<Response<bool>>("StaffRef/DeleteMany", ids);
        }
    }
}
