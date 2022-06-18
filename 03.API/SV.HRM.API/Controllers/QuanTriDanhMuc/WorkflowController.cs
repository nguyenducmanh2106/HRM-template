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
    public class WorkflowController : ControllerBase
    {
        private readonly IWorkflowHttpService _workflowHttpService;

        public WorkflowController(IWorkflowHttpService workflowHttpService)
        {
            _workflowHttpService = workflowHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<WorkflowModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _workflowHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create(WorkflowCreateModel model)
        {
            return await _workflowHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Workflow
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<WorkflowModel>> FindById(Guid recordID)
        {
            return await _workflowHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<Guid> recordID)
        {
            return await _workflowHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _workflowHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(Guid id, WorkflowUpdateModel model)
        {
            return await _workflowHttpService.Update(id, model);
        }
    }
}
