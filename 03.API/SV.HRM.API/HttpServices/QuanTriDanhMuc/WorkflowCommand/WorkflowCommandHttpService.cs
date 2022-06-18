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
    public class WorkflowCommandHttpService : IWorkflowCommandHttpService
    {
        private IHttpHelper _httpHelper;
        public WorkflowCommandHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<WorkflowCommandModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<WorkflowCommandModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(WorkflowCommandCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"WorkflowCommand/Create", model);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng WorkflowCommand
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<WorkflowCommandModel>> FindById(Guid recordID)
        {
            return await _httpHelper.GetAsync<Response<WorkflowCommandModel>>($"WorkflowCommand/FindById?recordID={recordID.ToString()}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<Guid> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"WorkflowCommand/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"WorkflowCommand/DeleteManyUseRecord", recordID);
        }

        /// <summary>
        /// Hàm cập nhật danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(Guid id, WorkflowCommandUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"WorkflowCommand/Update?id={id}", model);
        }

    }
}
