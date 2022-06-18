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
    public class StaffSalaryHttpService : IStaffSalaryHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public StaffSalaryHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<StaffSalaryModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<StaffSalaryModel>>>($"{StaffQueryFilter.LayoutCode}/GetFilter", StaffQueryFilter);
        }

        /// <summary>
        /// Hàm tạo quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(StaffSalaryCreateRequestModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffSalary/Create", model);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"StaffSalary/DeleteMany", recordID);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng StaffSalary
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffSalaryModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<StaffSalaryModel>>($"StaffSalary/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm cập nhật quá trình lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, StaffSalaryUpdateRequestModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"StaffSalary/Update?id={id}", model);
        }

        public async Task<Response<object>> GetHeSoThamNien(int? staffID, int? bacLuongID, int? staffSalaryID)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<object>>($"StaffSalary/GetHeSoThamNien?staffID={staffID}&bacLuongID={bacLuongID}&staffSalaryID={staffSalaryID}");
        }

        /// <summary>
        /// Tìm bản ghi quá trình lương liền kề
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<StaffSalaryModel>> GetStaffSalary_AdjacentBefore(int staffID, int? recordID)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<StaffSalaryModel>>($"StaffSalary/GetStaffSalary_AdjacentBefore?staffID={staffID}&recordID={recordID}");
        }
        /// <summary>
        /// check từ ngày đến ngày có trong quá trình công tác không
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<Response<bool>> CheckStaffSalaryinHistory(int staffId, DateTime fromDate, DateTime toDate)
        {
            return await _httpHelper.GetAsync<Response<bool>>($"StaffSalary/CheckStaffSalaryinHistory?staffId={staffId}&fromDate={fromDate}&toDate={toDate}");
        }
    }
}
