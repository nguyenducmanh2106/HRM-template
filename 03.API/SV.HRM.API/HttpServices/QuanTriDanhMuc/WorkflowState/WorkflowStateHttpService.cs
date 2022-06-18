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
    public class WorkflowStateHttpService : IWorkflowStateHttpService
    {
        private IHttpHelper _httpHelper;
        public WorkflowStateHttpService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _httpHelper = new HttpHelper(client, httpContextAccessor);
        }

        public async Task<Response<List<WorkflowStateModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<List<WorkflowStateModel>>>($"{queryFilter.LayoutCode}/GetFilter", queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Create(WorkflowStateCreateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"WorkflowState/Create", model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng WorkflowState
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        public async Task<Response<WorkflowStateModel>> FindById(Guid recordID)
        {
            return await _httpHelper.GetAsync<Response<WorkflowStateModel>>($"WorkflowState/FindById?recordID={recordID.ToString()}");
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Delete(List<Guid> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"WorkflowState/DeleteMany", recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check có đang được sử dụng hay không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> recordID)
        {
            return await _httpHelper.PostAsync<Response<bool>>($"WorkflowState/DeleteManyUseRecord", recordID);
        }

        /// <summary>
        /// Hàm cập nhật danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response<bool>> Update(Guid id, WorkflowStateUpdateModel model)
        {
            return await _httpHelper.PostAsyncCustomGrid<Response<bool>>($"WorkflowState/Update?id={id}", model);
        }
    }
}
