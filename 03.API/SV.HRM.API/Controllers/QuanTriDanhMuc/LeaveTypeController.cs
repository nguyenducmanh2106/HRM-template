using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers.QuanTriDanhMuc
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LeaveTypeController : ControllerBase
    {
        private readonly ILeaveTypeHttpService _leaveTypeHttpService;

        public LeaveTypeController(ILeaveTypeHttpService leaveTypeHttpService)
        {
            _leaveTypeHttpService = leaveTypeHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<LeaveTypeModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _leaveTypeHttpService.GetFilter(queryFilter);
        }


        /// <summary>
        /// Hàm thêm mới nhóm kiểu nghỉ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(LeaveTypeCreateModel model)
        {
            return await _leaveTypeHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng LeaveType
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<LeaveTypeModel>> FindById(int recordID)
        {
            return await _leaveTypeHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _leaveTypeHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _leaveTypeHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật nhóm kiểu nghỉ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, LeaveTypeUpdateModel model)
        {
            return await _leaveTypeHttpService.Update(id, model);
        }
    }
}
