using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class ProfileDocumentHttpService : IProfileDocumentHttpService
    {
        private IHttpHelper _httpHelper;
        public ProfileDocumentHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<ProfileDocumentModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<ProfileDocumentModel>>>($"{StaffQueryFilter.LayoutCode}/GetFilter", StaffQueryFilter);
        }
        /// <summary>
        /// Hàm thêm mới quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(ProfileDocumentCreate model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ProfileDocument/Create", model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng ProfileDocument
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<ProfileDocumentModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<ProfileDocumentModel>>($"ProfileDocument/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"ProfileDocument/DeleteMany", recordID);
        }

        /// <summary>
        /// Hàm cập nhật hồ sơ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, ProfileDocumentUpdate model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"ProfileDocument/Update?id={id}", model);
        }
    }
}
