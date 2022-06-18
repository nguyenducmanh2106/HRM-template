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
    public class StaffRewardHttpService : IStaffRewardHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public StaffRewardHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<StaffRewardModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<StaffRewardModel>>>($"StaffReward/GetFilter", StaffQueryFilter);
        }
        public async Task<Response<List<StaffRewardModel>>> GetByStaff(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<StaffRewardModel>>>($"{StaffQueryFilter.LayoutCode}/GetByStaff", StaffQueryFilter);
        }

        /// <summary>
        /// Hàm thêm mới khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffRewardCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffReward/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng StaffReward
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffRewardModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<StaffRewardModel>>($"StaffReward/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"StaffReward/DeleteMany", recordID);
        }

        /// <summary>
        /// Hàm cập nhật khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffRewardUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffReward/Update?id={id}", model);
        }

        public async Task<Response<bool>> CheckStaffRewardInHistory(int staffID, DateTime date)
        {
            return await _httpHelper.GetAsync<Response<bool>>($"StaffReward/CheckStaffRewardInHistory?staffID={staffID}&date={date}");
        }
    }
}
