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
    public class DecisionHttpService : IDecisionHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public DecisionHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<DecisionModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<DecisionModel>>>($"{StaffQueryFilter.LayoutCode}/GetFilter", StaffQueryFilter);
        }

        public async Task<Response<List<DecisionModel>>> GetByStaff(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<DecisionModel>>>("Decision/GetByStaff", StaffQueryFilter);
        }

        /// <summary>
        /// Hàm thêm mới quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(DecisionCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Decision/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng Decision
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<DecisionModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<DecisionModel>>($"Decision/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Decision/DeleteMany", recordID);
        }

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, DecisionUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"Decision/Update?id={id}", model);
        }

        public async Task<Response<bool>> CheckStaffDecisionInHistory(int staffId, DateTime date)
        {
            return await _httpHelper.GetAsync<Response<bool>>($"Decision/CheckStaffDecisionInHistory?staffId={staffId}&date={date}");
        }
    }
}
