using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class WKTimeHttpService : IWKTimeHttpService
    {
        private IHttpHelper _httpHelper;
        public WKTimeHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<WKTimeModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<WKTimeModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(WKTimeRequestCreate model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"WKTime/Create", model);
        }

        public async Task<Response<bool>> DeleteMany(string textSearch, List<int> recordIDs)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"WKTime/DeleteMany?textSearch={textSearch}", recordIDs);
        }
    }
}
