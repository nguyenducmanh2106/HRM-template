using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class StaffDocumentHttpService : IStaffDocumentHttpService
    {
        private IHttpHelper _httpHelper;
        public StaffDocumentHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<StaffLicenseModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<StaffLicenseModel>>>($"{StaffQueryFilter.LayoutCode}/GetFilter", StaffQueryFilter);
        }
        /// <summary>
        /// Hàm thêm mới giấy tờ có thời hạn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffLicenseCreate model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffDocument/Create", model);
        }

        /// <summary>
        /// Tìm bản ghi
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffLicenseModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<StaffLicenseModel>>($"StaffDocument/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"StaffDocument/DeleteMany", recordID);
        }

        /// <summary>
        /// Hàm cập nhật giấy tờ có thời hạn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffLicenseUpdate model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffDocument/Update?id={id}", model);
        }
    }
}
