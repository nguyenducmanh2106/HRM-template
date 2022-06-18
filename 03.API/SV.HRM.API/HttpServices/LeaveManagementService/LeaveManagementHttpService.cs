using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SV.HRM.Core;
using SV.HRM.Core.Helper;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public class LeaveManagementHttpService : ILeaveManagementHttpService
    {
        private readonly IHttpHelper _httpHelper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LeaveManagementHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpHelper = new HttpHelper(client, httpContextAccessor);
            _configuration = configuration;
        }

        public async Task<Response<List<LeaveManagementModel>>> GetFilter(LeaveManagementQueryFilter filter)
        {
            return await _httpHelper.PostAsync<Response<List<LeaveManagementModel>>>("LeaveManagement/GetFilter", filter);
        }

        /// <summary>
        /// Hàm thêm mới quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(LeaveManagementCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"LeaveManagement/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng LeaveManagement
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<LeaveManagementModel>> FindById(Guid recordID)
        {
            return await _httpHelper.GetAsync<Response<LeaveManagementModel>>($"LeaveManagement/FindById?recordID={recordID}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"LeaveManagement/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"LeaveManagement/DeleteManyUseRecord", recordID);
        }

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(Guid id, LeaveManagementUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"LeaveManagement/Update?id={id}", model);
        }

        public async Task<Response<List<DocumentHistoryViewModel>>> GetHistory(Guid id)
        {
            return await _httpHelper.GetAsync<Response<List<DocumentHistoryViewModel>>>($"LeaveManagement/GetHistory?id={id}");
        }

        public async Task<Response<List<WorkflowCommandModel>>> GetCommand(Guid documentId)
        {
            return await _httpHelper.PostAsync<Response<List<WorkflowCommandModel>>>($"LeaveManagement/GetCommand?documentId={documentId}", null);
        }

        public async Task<Response<List<UserInfo>>> GetNextUserProcess(Guid documentId, Guid workflowCommandId)
        {
            return await _httpHelper.PostAsync<Response<List<UserInfo>>>($"LeaveManagement/GetNextUserProcess?documentId={documentId}&workflowCommandId={workflowCommandId}", null);
        }

        public async Task<Response<bool>> ExecuteCommand(ExecuteCommandModel model)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"LeaveManagement/ExecuteCommand", model);
        }
    }
}
