using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Models;
using NLog;

namespace SV.HRM.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public partial class StaffSalaryController : ControllerBase
    {
        private readonly IStaffSalaryHttpService _staffSalaryService;

        public StaffSalaryController(IStaffSalaryHttpService staffSalaryService)
        {
            _staffSalaryService = staffSalaryService;
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffSalaryModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _staffSalaryService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm tạo quá trình lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(StaffSalaryCreateRequestModel model)
        {
            return await _staffSalaryService.Create(model);
        }



        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _staffSalaryService.Delete(recordID);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng staffDiploma
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffSalaryModel>> FindById(int recordID)
        {
            return await _staffSalaryService.FindById(recordID);
        }

        /// <summary>
        /// Hàm cập nhật quá trình lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] StaffSalaryUpdateRequestModel model)
        {
            return await _staffSalaryService.Update(id, model);
        }


        /// <summary>
        /// Hàm lấy hệ số thâm niên
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<object>> GetHeSoThamNien(int? staffID, int? bacLuongID, int? staffSalaryID)
        {
            return await _staffSalaryService.GetHeSoThamNien(staffID, bacLuongID, staffSalaryID);
        }


        /// <summary>
        /// Tìm bản ghi quá trình lương liền kề
        /// </summary>
        /// <param name="staffID">Id bản ghi cần lấy</param>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<StaffSalaryModel>> GetStaffSalary_AdjacentBefore(int staffID, int? recordID)
        {
            return await _staffSalaryService.GetStaffSalary_AdjacentBefore(staffID, recordID);
        }
        [HttpGet]
        public async Task<Response<bool>> CheckStaffSalaryinHistory(int staffId, DateTime fromDate, DateTime toDate)
        {
            return await _staffSalaryService.CheckStaffSalaryinHistory(staffId,fromDate, toDate);
        }
    }
}
