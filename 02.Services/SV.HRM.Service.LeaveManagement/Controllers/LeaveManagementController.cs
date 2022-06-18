using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.LeaveManagement
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LeaveManagementController : ControllerBase
    {
        private readonly ILeaveManagementHandler _leaveManagementHandler;
        private readonly IBaseHandler _baseHandler;

        public LeaveManagementController(ILeaveManagementHandler leaveManagementHandler, IBaseHandler baseHandler)
        {
            _leaveManagementHandler = leaveManagementHandler;
            _baseHandler = baseHandler;
        }

        [HttpPost]
        public async Task<Response<List<LeaveManagementModel>>> GetFilter(LeaveManagementQueryFilter queryFilter)
        {
            return await _leaveManagementHandler.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm tạo đăng ký nghỉ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] LeaveManagementCreateModel entity)
        {
            return await _leaveManagementHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi đăng ký nghỉ
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<LeaveManagementModel>> FindById(Guid recordID)
        {
            return await _leaveManagementHandler.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sách bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<object> entity)
        {
            return await _baseHandler.DeleteMany<Models.LeaveManagement>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sách bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _leaveManagementHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật đăng ký nghỉ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(Guid id, [FromBody] LeaveManagementUpdateModel entity)
        {
            return await _leaveManagementHandler.Update(id, entity);
        }

        /// <summary>
        /// Lấy danh sách lịch sử xử lý
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<DocumentHistoryViewModel>>> GetHistory(Guid id)
        {
            return await _leaveManagementHandler.GetHistory(id);
        }

        /// <summary>
        /// Lấy danh sách bước xử lý tiếp theo
        /// </summary>
        /// <param name="dataCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<WorkflowCommandModel>>> GetCommand(Guid documentId)
        {
            return await _leaveManagementHandler.GetCommand(documentId);
        }

        /// <summary>
        /// Lấy danh sách người xử lý tiếp theo
        /// </summary>
        /// <param name="dataCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<UserInfo>>> GetNextUserProcess(Guid documentId, Guid workflowCommandId)
        {
            return await _leaveManagementHandler.GetNextUserProcess(documentId, workflowCommandId);
        }

        /// <summary>
        /// Chạy quy trình xử lý
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> ExecuteCommand([FromBody] ExecuteCommandModel model)
        {
            return await _leaveManagementHandler.ExecuteCommand(model);
        }
    }
}
