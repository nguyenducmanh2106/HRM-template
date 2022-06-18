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
    public class HistoryHttpService : IHistoryHttpService
    {
        private readonly IHttpHelper _httpHelper;

        public HistoryHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<HistoryModel>>> GetFilter(EntityGeneric StaffQueryFilter)
        {
            return await _httpHelper.PostAsync<Response<List<HistoryModel>>>("History/GetFilter", StaffQueryFilter);
        }

        /// <summary>
        /// Hàm tạo quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(HistoryCreateRequestModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"History/Create", model);
        }


        /// <summary>
        /// Hàm tạo quá trình công tác trước khi vào công ty
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> CreateBeforeJoiningCompany(HistoryCreateBeforeJoiningCompanyRequestModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"History/CreateBeforeJoiningCompany", model);
        }

        /// <summary>
        /// Hàm lấy về giá trị hết hạn của quá trình công tác gần nhất
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<DateTime>> GetMinFromDate(int staffID)
        {
            return await _httpHelper.GetAsync<Response<DateTime>>($"History/GetMinFromDate?staffID={staffID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<int> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"History/DeleteMany", recordID);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng History
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<HistoryModel>> FindById(int recordID)
        {
            return await _httpHelper.GetAsync<Response<HistoryModel>>($"History/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm cập nhật quá trình công tác
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(int id, HistoryUpdateRequestModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"History/Update?id={id}", model);
        }

        /// <summary>
        /// Cập nhật quá trình công tác trước khi vào công ty
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<bool>> UpdateBeforeJoiningCompany(int id, HistoryUpdateBeforeJoiningCompanyRequestModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"History/UpdateBeforeJoiningCompany?id={id}", model);
        }

        /// <summary>
        /// Lấy quá trình công tác mới nhất có trạng thái khác tăng cường
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<HistoryModel>> GetHistoryLatest(int recordID)
        {
            return await _httpHelper.GetAsync<Response<HistoryModel>>($"History/GetHistoryLatest?recordID={recordID}");
        }

    }
}
