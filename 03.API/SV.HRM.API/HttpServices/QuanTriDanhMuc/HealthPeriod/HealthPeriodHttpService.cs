using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class HealthPeriodHttpService : IHealthPeriodHttpService
    {
        private IHttpHelper _httpHelper;

        public HealthPeriodHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client,  httpContextAccessor);
        }

        public async Task<Response<List<HealthPeriodModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<HealthPeriodModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới mối quan hệ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(HealthPeriodCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"HealthPeriod/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng HealthPeriod
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<HealthPeriodModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<HealthPeriodModel>>($"HealthPeriod/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"HealthPeriod/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"HealthPeriod/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật mối quan hệ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, HealthPeriodUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"HealthPeriod/Update?id={id}", model);
        }
        /// <summary>
        /// check bản ghi có sử dụng không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"HealthPeriod/FindIdInUse", recordID);
        }

        public async Task<Response<bool>> CheckDateInPeriod(HealthPeriod model)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"HealthPeriod/CheckDateInPeriod",model);
        }
    }
}
