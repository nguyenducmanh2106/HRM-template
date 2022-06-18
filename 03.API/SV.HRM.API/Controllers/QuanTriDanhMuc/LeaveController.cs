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
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveHttpService _leaveHttpService;

        public LeaveController(ILeaveHttpService leaveHttpService)
        {
            _leaveHttpService = leaveHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<LeaveModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _leaveHttpService.GetFilter(queryFilter);
        }


        /// <summary>
        /// Hàm thêm mới Leave
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(LeaveCreateModel model)
        {
            return await _leaveHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Leave
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<LeaveModel>> FindById(int recordID)
        {
            return await _leaveHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _leaveHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _leaveHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật Leave
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, LeaveUpdateModel model)
        {
            return await _leaveHttpService.Update(id, model);
        }
    }
}
