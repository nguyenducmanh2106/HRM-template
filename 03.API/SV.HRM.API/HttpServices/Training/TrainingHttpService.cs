using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class TrainingHttpService : ITrainingHttpService
    {
        private IHttpHelper _httpHelper;
        public TrainingHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<bool>> CreateObject(string layout, object createObject)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Training/CreateObject?layout={layout}", createObject);
        }

        public async Task<Response<bool>> UpdateObject(string layout, int id, object createObject)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Training/UpdateObject?layout={layout}&id={id}", createObject);
        }

        public async Task<Response<List<Dictionary<string, object>>>> ReportTraining(EntityGeneric TrainingQueryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<Dictionary<string, object>>>>("Training/ReportTraining", TrainingQueryFilter);
        }

        /// <summary>
        /// Tìm bản ghi
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<QuanLyDaoTaoModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<QuanLyDaoTaoModel>>($"Training/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Training/DeleteMany", recordID);
        }
    }
}
