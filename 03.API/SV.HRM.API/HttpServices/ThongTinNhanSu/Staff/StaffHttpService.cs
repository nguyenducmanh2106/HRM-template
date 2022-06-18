using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using RestSharp;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class StaffHttpService : IStaffHttpService
    {
        private readonly IHttpHelper _httpHelper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StaffHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpHelper = new HttpHelper(client, httpContextAccessor);
            _configuration = configuration;
        }

        public async Task<Response<List<StaffModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<StaffModel>>>("Staff/GetFilter", StaffQueryFilter);
        }

        public async Task<Response<List<StaffModel>>> GetAll(string q, int page)
        {
            return await _httpHelper.GetAsync<Response<List<StaffModel>>>($"Staff/GetAll?q={q}&page={page}");
        }

        public async Task<Response<StaffModel>> GetById(int StaffId)
        {
            return await _httpHelper.GetAsync<Response<StaffModel>>($"Staff/GetById?StaffId={StaffId}");
        }

        public async Task<Response<StaffDetailModel>> GetStaffGeneralInfoById(int id)
        {
            return await _httpHelper.GetAsync<Response<StaffDetailModel>>($"Staff/GetStaffGeneralInfoById?id={id}");
        }

        public async Task<Response<StaffDetailModel>> GetStaffOrtherInfoById(int id)
        {
            return await _httpHelper.GetAsync<Response<StaffDetailModel>>($"Staff/GetStaffOrtherInfoById?id={id}");
        }

        public async Task<Response<int>> GetStaffIDByAccountID(int userID)
        {
            return await _httpHelper.GetAsync<Response<int>>($"Staff/GetStaffIDByAccountID?userID={userID}");
        }

        public async Task<Response<int>> CreateStaffGeneralInfo(StaffCreateRequestModel model)
        {
            return await _httpHelper.PostAsync<Response<int>>("Staff/CreateStaffGeneralInfo", model);
        }

        public async Task<Response<bool>> UpdateStaffGeneralInfo(StaffUpdateRequestModel model)
        {
            return await _httpHelper.PostAsync<Response<bool>>("Staff/UpdateStaffGeneralInfo", model);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"Staff/DeleteMany", recordID);
        }

        public async Task<Response<bool>> DeleteList(List<int> id)
        {
            return await _httpHelper.PostAsync<Response<bool>>("Staff/Delete", id);
        }

        public async Task<Response<int>> CreateOrUpdateStaffOrtherInfo(StaffCreateRequestModel model)
        {
            return await _httpHelper.PostAsync<Response<int>>("Staff/CreateOrUpdateStaffOrtherInfo", model);
        }

        public async Task<Response<List<Dictionary<string, object>>>> ReportStaff(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<Dictionary<string, object>>>>("Staff/ReportStaff", StaffQueryFilter);
        }

        public async Task<Response<int>> GenerateAccount()
        {
            return await _httpHelper.PostAsync<Response<int>>("Staff/GenerateAccount", null);
        }
    }
}
