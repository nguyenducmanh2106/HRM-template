using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;

namespace SV.HRM.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public partial class LeaveManagementController : ControllerBase
    {
        private readonly ILeaveManagementHttpService _leaveManagementService;
        private readonly IBaseHttpService _baseService;

        public LeaveManagementController(ILeaveManagementHttpService leaveManagementService, IBaseHttpService baseService)
        {
            _leaveManagementService = leaveManagementService;
            _baseService = baseService;
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<LeaveManagementModel>>> GetFilter(LeaveManagementQueryFilter filter)
        {
            return await _leaveManagementService.GetFilter(filter);
        }

        /// <summary>
        /// Hàm thêm mới ngân hàng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(LeaveManagementCreateModel model)
        {
            return await _leaveManagementService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Bank
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<LeaveManagementModel>> FindById(Guid recordID)
        {
            return await _leaveManagementService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<object> recordID)
        {
            return await _leaveManagementService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _leaveManagementService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật ngân hàng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(Guid id, LeaveManagementUpdateModel model)
        {
            return await _leaveManagementService.Update(id, model);
        }

        /// <summary>
        /// Lấy danh sách lịch sử xử lý
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<List<DocumentHistoryViewModel>>> GetHistory(Guid id)
        {
            return await _leaveManagementService.GetHistory(id);
        }

        /// <summary>
        /// Lấy danh sách bước xử lý tiếp theo
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<WorkflowCommandModel>>> GetCommand(Guid documentId)
        {
            return await _leaveManagementService.GetCommand(documentId);
        }

        /// <summary>
        /// Lấy danh sách người xử lý tiếp theo
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="workflowCommandId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<UserInfo>>> GetNextUserProcess(Guid documentId, Guid workflowCommandId)
        {
            return await _leaveManagementService.GetNextUserProcess(documentId, workflowCommandId);
        }

        /// <summary>
        /// Chạy quy trình xử lý
        /// </summary>
        /// <param name="dataCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> ExecuteCommand([FromBody] ExecuteCommandModel model)
        {
            return await _leaveManagementService.ExecuteCommand(model);
        }
    }
}
