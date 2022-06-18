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
    public class StaffPartyController: ControllerBase
    {
        private readonly IStaffPartyHttpService _staffPartyHttpService;

        public StaffPartyController(IStaffPartyHttpService staffPartyHttpService)
        {
            _staffPartyHttpService = staffPartyHttpService;
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffPartyModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _staffPartyHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm tạo thông tin đảng nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(StaffPartyCreateModel model)
        {
            return await _staffPartyHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng StaffParty
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffPartyModel>> FindById(int recordID)
        {
            return await _staffPartyHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _staffPartyHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm cập nhật thông tin đảng của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, StaffPartyUpdateModel model)
        {
            return await _staffPartyHttpService.Update(id, model);
        }

        /// <summary>
        /// Tìm và kiểm tra bản ghi đầu tiên để fill giá trị cho các bản ghi tạo sau đó theo nghiệp vụ
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffPartyModel>> FirstOrDefaultByStaffID(int recordID)
        {
            return await _staffPartyHttpService.FirstOrDefaultByStaffID(recordID);
        }
    }
}
