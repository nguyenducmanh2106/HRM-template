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
    public class WorkflowCommandController : ControllerBase
    {
        private readonly IWorkflowCommandHttpService _workflowCommandHttpService;

        public WorkflowCommandController(IWorkflowCommandHttpService workflowCommandHttpService)
        {
            _workflowCommandHttpService = workflowCommandHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<WorkflowCommandModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _workflowCommandHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create(WorkflowCommandCreateModel model)
        {
            return await _workflowCommandHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng WorkflowCommand
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<WorkflowCommandModel>> FindById(Guid recordID)
        {
            return await _workflowCommandHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<Guid> recordID)
        {
            return await _workflowCommandHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _workflowCommandHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật danh xưng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(Guid id, WorkflowCommandUpdateModel model)
        {
            return await _workflowCommandHttpService.Update(id, model);
        }
    }
}
