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
    public class StaffDocumentController
    {
        private readonly IStaffDocumentHttpService _staffDocumentHttpService;

        public StaffDocumentController(IStaffDocumentHttpService staffDocumentHttpService)
        {
            _staffDocumentHttpService = staffDocumentHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffLicenseModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _staffDocumentHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới giấy tờ có thời hạn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(StaffLicenseCreate model)
        {
            return await _staffDocumentHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffLicenseModel>> FindById(int recordID)
        {
            return await _staffDocumentHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _staffDocumentHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm cập nhật hồ sơ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, StaffLicenseUpdate model)
        {
            return await _staffDocumentHttpService.Update(id, model);
        }
    }
}
