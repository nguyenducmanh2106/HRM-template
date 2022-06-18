using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class StaffMilitaryHttpService : IStaffMilitaryHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public StaffMilitaryHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<StaffMilitaryModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<StaffMilitaryModel>>>($"{StaffQueryFilter.LayoutCode}/GetFilter", StaffQueryFilter);
        }

        /// <summary>
        /// Hàm tạo thông tin quân ngữ nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffMilitaryCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffMilitary/Create", model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng StaffMilitary
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffMilitaryModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<StaffMilitaryModel>>($"StaffMilitary/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"StaffMilitary/DeleteMany", recordID);
        }

        /// <summary>
        /// Hàm cập nhật thông tin quân ngữ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffMilitaryUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffMilitary/Update?id={id}", model);
        }
    }
}
