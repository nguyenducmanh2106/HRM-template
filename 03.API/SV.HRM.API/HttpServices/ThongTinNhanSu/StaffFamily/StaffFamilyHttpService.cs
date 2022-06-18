using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class StaffFamilyHttpService : IStaffFamilyHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public StaffFamilyHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<StaffFamilyModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<StaffFamilyModel>>>($"{StaffQueryFilter.LayoutCode}/GetFilter", StaffQueryFilter);
        }

        /// <summary>
        /// Hàm tạo thông tin gia đình nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffFamilyCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffFamily/Create", model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng StaffFamily
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffFamilyModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<StaffFamilyModel>>($"StaffFamily/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"StaffFamily/DeleteMany", recordID);
        }

        /// <summary>
        /// Hàm cập nhật thông tin gia đình của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffFamilyUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffFamily/Update?id={id}", model);
        }
    }
}
