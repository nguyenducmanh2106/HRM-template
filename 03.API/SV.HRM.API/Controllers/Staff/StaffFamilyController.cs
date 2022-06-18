using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StaffFamilyController:ControllerBase
    {
        private readonly IStaffFamilyHttpService _staffFamilyHttpService;

        public StaffFamilyController(IStaffFamilyHttpService staffFamilyHttpService)
        {
            _staffFamilyHttpService = staffFamilyHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffFamilyModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _staffFamilyHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm tạo thông tin gia đình nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(StaffFamilyCreateModel model)
        {
            return await _staffFamilyHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng StaffFamily
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffFamilyModel>> FindById(int recordID)
        {
            return await _staffFamilyHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _staffFamilyHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm cập nhật thông tin gia đình của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, StaffFamilyUpdateModel model)
        {
            return await _staffFamilyHttpService.Update(id, model);
        }
    }
}
