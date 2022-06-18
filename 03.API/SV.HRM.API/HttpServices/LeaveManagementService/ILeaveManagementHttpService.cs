using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SV.HRM.API.HttpServices
{
    public interface ILeaveManagementHttpService
    {
        Task<Response<List<LeaveManagementModel>>> GetFilter(LeaveManagementQueryFilter filter);

        /// <summary>
        /// Hàm thêm mới quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Create(LeaveManagementCreateModel model);

        /// <summary>
        /// Tìm bản ghi trong bảng LeaveManagement
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        Task<Response<LeaveManagementModel>> FindById(Guid recordID);

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> Delete(List<object> recordID);
        /// <summary>
        /// Hàm xóa bản ghi check có sử dụng hay không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        Task<Response<bool>> DeleteManyUseRecord(List<object> recordID);

        /// <summary>
        /// Hàm cập nhật quyết định
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response<bool>> Update(Guid id, LeaveManagementUpdateModel model);

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
