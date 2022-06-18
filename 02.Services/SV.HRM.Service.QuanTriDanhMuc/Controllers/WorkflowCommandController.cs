using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WorkflowCommandController : ControllerBase
    {
        private readonly IWorkflowCommandHandler _workflowCommandHandler;
        private readonly IBaseHandler _baseHandler;

        public WorkflowCommandController(IBaseHandler baseHandler,
             IWorkflowCommandHandler workflowCommandHandler
            )
        {
            _baseHandler = baseHandler;
            _workflowCommandHandler = workflowCommandHandler;
        }
        [HttpPost]
        public async Task<Response<List<object>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<object>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Hàm tạo mới bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] WorkflowCommandCreateModel entity)
        {
            return await _workflowCommandHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<WorkflowCommandModel>> FindById(Guid recordID)
        {
            return await _workflowCommandHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<WorkflowCommand>(entity);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _workflowCommandHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(Guid id, [FromBody] WorkflowCommandUpdateModel entity)
        {
            return await _workflowCommandHandler.Update(id, entity);
        }
    }
}
