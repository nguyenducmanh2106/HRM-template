using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class UniformHttpService : IUniformHttpService
    {
        private readonly IHttpHelper _httpHelper;
        public UniformHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<int>> CreateOrUpdate(UniformCreateRequestModel model)
        {
            return await _httpHelper.PostAsync<Response<int>>("Uniform/CreateOrUpdate", model);
        }

        public async Task<Response<bool>> DeleteMany(List<int> ids)
        {
            return await _httpHelper.PostAsync<Response<bool>>("Uniform/DeleteMany", ids);
        }

        public async Task<Response<UniformDetailModel>> GetById(int id)
        {
            return await _httpHelper.GetAsync<Response<UniformDetailModel>>($"Uniform/GetById?id={id}");
        }

        public async Task<Response<List<UniformModel>>> GetFilter(EntityGeneric filter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<UniformModel>>>("Uniform/GetFilter", filter);
        }

        public async Task<Response<string>> GetStaffFullNameById(int id)
        {
            return await _httpHelper.GetAsync<Response<string>>($"Uniform/GetStaffFullNameById?id={id}");
        }
    }
}
