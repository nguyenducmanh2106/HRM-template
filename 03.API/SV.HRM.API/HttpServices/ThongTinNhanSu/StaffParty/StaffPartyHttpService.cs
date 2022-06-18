using Microsoft.AspNetCore.Http;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class StaffPartyHttpService : IStaffPartyHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public StaffPartyHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<StaffPartyModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<StaffPartyModel>>>($"{StaffQueryFilter.LayoutCode}/GetFilter", StaffQueryFilter);
        }

        /// <summary>
        /// Hàm tạo thông tin đảng nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffPartyCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffParty/Create", model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng StaffParty
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffPartyModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<StaffPartyModel>>($"StaffParty/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"StaffParty/DeleteMany", recordID);
        }

        /// <summary>
        /// Hàm cập nhật thông tin đảng của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffPartyUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffParty/Update?id={id}", model);
        }

        /// <summary>
        /// Tìm và kiểm tra bản ghi đầu tiên để fill giá trị cho các bản ghi tạo sau đó theo nghiệp vụ
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffPartyModel>> FirstOrDefaultByStaffID(int recordID)
        {
            return await _httpHelper.GetAsync<Response<StaffPartyModel>>($"StaffParty/FirstOrDefaultByStaffID?recordID={recordID}");
        }
    }
}
