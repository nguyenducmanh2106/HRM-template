using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class JobTitleHttpService : IJobTitleHttpService
    {
        private IHttpHelper _httpHelper;
        public JobTitleHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<JobTitleModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<JobTitleModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới vị trí việc làm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(JobTitleCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"JobTitle/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng JobTitle
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<JobTitleModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<JobTitleModel>>($"JobTitle/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"JobTitle/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"JobTitle/DeleteManyUseRecord", recordID);
        }
        /// <summary>
        /// Hàm cập nhật vị trí việc làm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, JobTitleUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"JobTitle/Update?id={id}", model);
        }
    }
}
