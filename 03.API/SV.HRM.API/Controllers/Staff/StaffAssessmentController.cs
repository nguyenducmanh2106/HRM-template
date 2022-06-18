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
    public class StaffAssessmentController:ControllerBase
    {
        private readonly IStaffAssessmentHttpService _staffAssessmentHttpService;

        public StaffAssessmentController(IStaffAssessmentHttpService staffAssessmentHttpService)
        {
            _staffAssessmentHttpService = staffAssessmentHttpService;
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffAssessmentModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _staffAssessmentHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffAssessmentModel>>> GetByStaff(EntityGeneric queryFilter)
        {
            return await _staffAssessmentHttpService.GetByStaff(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(StaffAssessmentCreateModel model)
        {
            return await _staffAssessmentHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng StaffAssessment
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffAssessmentModel>> FindById(int recordID)
        {
            return await _staffAssessmentHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _staffAssessmentHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm cập nhật khen thưởng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, StaffAssessmentUpdateModel model)
        {
            return await _staffAssessmentHttpService.Update(id, model);
        }
        /// <summary>
        /// check xem năm có trong quá trình công tác không
        /// </summary>
        /// <param name="staffID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<bool>> CheckStaffAssessmentInHistory(int staffID, int year)
        {
            return await _staffAssessmentHttpService.CheckStaffAssessmentInHistory(staffID, year);
        }
    }
}
