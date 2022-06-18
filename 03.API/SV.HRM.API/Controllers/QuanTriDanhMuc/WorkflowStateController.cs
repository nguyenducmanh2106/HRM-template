using Microsoft.AspNetCore.Mvc;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.Controllers.QuanTriDanhMuc
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WorkflowStateController : ControllerBase
    {
        private readonly IWorkflowStateHttpService _workflowStateHttpService;

        public WorkflowStateController(IWorkflowStateHttpService workflowStateHttpService)
        {
            _workflowStateHttpService = workflowStateHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<WorkflowStateModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _workflowStateHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create(WorkflowStateCreateModel model)
        {
            return await _workflowStateHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng WorkflowState
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<WorkflowStateModel>> FindById(Guid recordID)
        {
            return await _workflowStateHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<Guid> recordID)
        {
            return await _workflowStateHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _workflowStateHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(Guid id, WorkflowStateUpdateModel model)
        {
            return await _workflowStateHttpService.Update(id, model);
        }
    }
}
