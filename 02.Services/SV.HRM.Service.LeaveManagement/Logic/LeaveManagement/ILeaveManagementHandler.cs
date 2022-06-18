
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.Service.LeaveManagement
{
    public interface ILeaveManagementHandler
    {
        Task<Response<List<LeaveManagementModel>>> GetFilter(LeaveManagementQueryFilter queryFilter);

        /// <summary>
        /// Hàm thêm mới đăng ký nghỉ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(LeaveManagementCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <typeparam name="T">Tên bảng</typeparam>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<LeaveManagementModel>> FindById(Guid recordID);

        /// <summary>
        /// Hàm cập nhật đăng ký nghỉ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(Guid id, LeaveManagementUpdateModel model);

        /// <summary>
        /// Check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> CheckRecordInUse(List<object> obj);

        /// <summary>
        /// Lấy bản ghi config theo user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<UserConfigModel>> GetUserConfigByUserId(int userId);

        /// <summary>
        /// Get workflow state by name
        /// </summary>
        /// <param name="workflowStateName"></param>
        /// <returns></returns>
        Task<Response<WorkflowStateModel>> GetWorkflowStateByName(string workflowStateName);

        /// <summary>
        /// Get workflow command by name
        /// </summary>
        /// <param name="workflowCommandName"></param>
        /// <returns></returns>
        Task<Response<WorkflowCommandModel>> GetWorkflowCommandByName(string workflowCommandName);

        /// <summary>
        /// Get remain day off by user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="leaveGroup"></param>
        /// <param name="leaveType"></param>
        /// <returns></returns>
        Task<double> GetRemainDayOfByUser(int userId, DateTime fromDate, DateTime toDate, int leaveGroup, int leaveType);

        /// <summary>
        /// Lấy danh sách lịch sử xử lý
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<List<DocumentHistoryViewModel>>> GetHistory(Guid id);

        /// <summary>
        /// Lấy danh sách bước xử lý tiếp theo
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Task<Response<List<WorkflowCommandModel>>> GetCommand(Guid documentId);

        /// <summary>
        /// Lấy danh sách người xử lý tiếp theo
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="workflowCommandId"></param>
        /// <returns></returns>
        Task<Response<List<UserInfo>>> GetNextUserProcess(Guid documentId, Guid workflowCommandId);

        /// <summary>
        /// Chạy quy trình xử lý
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<bool>> ExecuteCommand(ExecuteCommandModel model);
    }
}
