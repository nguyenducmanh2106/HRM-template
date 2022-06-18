using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers.Staff
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StaffMilitaryController:ControllerBase
    {
        private readonly IStaffMilitaryHttpService _staffMilitaryHttpService;

        public StaffMilitaryController(IStaffMilitaryHttpService staffMilitaryHttpService)
        {
            _staffMilitaryHttpService = staffMilitaryHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffMilitaryModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _staffMilitaryHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm tạo thông tin quân ngữ nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(StaffMilitaryCreateModel model)
        {
            return await _staffMilitaryHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng StaffMilitary
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffMilitaryModel>> FindById(int recordID)
        {
            return await _staffMilitaryHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _staffMilitaryHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm cập nhật thông tin quân ngữ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, StaffMilitaryUpdateModel model)
        {
            return await _staffMilitaryHttpService.Update(id, model);
        }
    }
}
