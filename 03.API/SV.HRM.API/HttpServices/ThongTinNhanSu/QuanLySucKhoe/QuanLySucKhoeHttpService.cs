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
    public class QuanLySucKhoeHttpService : IQuanLySucKhoeHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public QuanLySucKhoeHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<QuanLySucKhoeModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<QuanLySucKhoeModel>>>($"{StaffQueryFilter.LayoutCode}/GetFilter", StaffQueryFilter);
        }

        /// <summary>
        /// Hàm thêm mới quản lý sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(QuanLySucKhoeCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"QuanLySucKhoe/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng QuanLySucKhoe
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<QuanLySucKhoeModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<QuanLySucKhoeModel>>($"QuanLySucKhoe/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"QuanLySucKhoe/DeleteMany", recordID);
        }

        /// <summary>
        /// Hàm cập nhật quản lý sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, QuanLySucKhoeUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"QuanLySucKhoe/Update?id={id}", model);
        }
        /// <summary>
        /// check date có trong kỳ khám
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> CheckDateBetween(int staffId, int healthPeriod, DateTime date)
        {
            return await _httpHelper.GetAsync<Response<bool>>($"QuanLySucKhoe/CheckDateBetween?staffId={staffId}&healthPeriod={healthPeriod}&date={date}");
        }
        /// <summary>
        /// check 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> CheckHealthPeriodAndHistory(int staffId, int healthPeriod)
        {
            return await _httpHelper.GetAsync<Response<bool>>($"QuanLySucKhoe/CheckHealthPeriodAndHistory?staffId={staffId}&healthPeriod={healthPeriod}");
        }
    }
}
