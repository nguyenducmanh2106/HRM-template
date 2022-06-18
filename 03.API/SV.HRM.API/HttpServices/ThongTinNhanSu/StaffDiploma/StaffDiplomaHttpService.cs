using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class StaffDiplomaHttpService : IStaffDiplomaHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public StaffDiplomaHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<StaffDiplomaModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<StaffDiplomaModel>>>($"{StaffQueryFilter.LayoutCode}/GetFilter", StaffQueryFilter);
        }

        /// <summary>
        /// Hàm tạo bằng cấp/chứng chỉ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffDiplomaCreateRequestModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffDiploma/Create", model);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"StaffDiploma/DeleteMany", recordID);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng History
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffDiplomaModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<StaffDiplomaModel>>($"StaffDiploma/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm cập nhật bằng cấp chứng chỉ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id,StaffDiplomaUpdateRequestModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffDiploma/Update?id={id}", model);
        }
    }
}
