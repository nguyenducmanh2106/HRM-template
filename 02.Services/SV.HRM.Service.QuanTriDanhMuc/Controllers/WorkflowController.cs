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
    public class WorkflowController : ControllerBase
    {
        private readonly IWorkflowHandler _workflowHandler;
        private readonly IBaseHandler _baseHandler;

        public WorkflowController(IBaseHandler baseHandler,
             IWorkflowHandler workflowHandler
            )
        {
            _baseHandler = baseHandler;
            _workflowHandler = workflowHandler;
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
        public async Task<Response<bool>> Create([FromBody] WorkflowCreateModel entity)
        {
            return await _workflowHandler.Create(entity);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<WorkflowModel>> FindById(Guid recordID)
        {
            return await _workflowHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<Workflow>(entity);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _workflowHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(Guid id, [FromBody] WorkflowUpdateModel entity)
        {
            return await _workflowHandler.Update(id, entity);
        }
    }
}
