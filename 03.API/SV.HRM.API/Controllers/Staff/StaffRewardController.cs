using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers.Staff
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StaffRewardController:ControllerBase
    {
        private readonly IStaffRewardHttpService _staffRewardHttpService;

        public StaffRewardController(IStaffRewardHttpService staffRewardHttpService)
        {
            _staffRewardHttpService = staffRewardHttpService;
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffRewardModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _staffRewardHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffRewardModel>>> GetByStaff(EntityGeneric queryFilter)
        {
            return await _staffRewardHttpService.GetByStaff(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(StaffRewardCreateModel model)
        {
            return await _staffRewardHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng StaffReward
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffRewardModel>> FindById(int recordID)
        {
            return await _staffRewardHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _staffRewardHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm cập nhật khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, StaffRewardUpdateModel model)
        {
            return await _staffRewardHttpService.Update(id, model);
        }
        /// <summary>
        /// check xem ngày cấp có trong quá trình công tác không
        /// </summary>
        /// <param name="staffID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<bool>> CheckStaffRewardInHistory(int staffID, DateTime date)
        {
            return await _staffRewardHttpService.CheckStaffRewardInHistory(staffID,date);
        }
    }
}
