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
    public partial class StaffDiplomaController : ControllerBase
    {
        private readonly IStaffDiplomaHttpService _staffDiplomaService;

        public StaffDiplomaController(IStaffDiplomaHttpService staffDiplomaService)
        {
            _staffDiplomaService = staffDiplomaService;
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<StaffDiplomaModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _staffDiplomaService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm tạo bằng cấp/chứng chỉ cho nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(StaffDiplomaCreateRequestModel model)
        {
            return await _staffDiplomaService.Create(model);
        }


        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _staffDiplomaService.Delete(recordID);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng staffDiploma
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<StaffDiplomaModel>> FindById(int recordID)
        {
            return await _staffDiplomaService.FindById(recordID);
        }

        /// <summary>
        /// Hàm cập nhật bằng cấp chứng chỉ của nhân viên
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, StaffDiplomaUpdateRequestModel model)
        {
            return await _staffDiplomaService.Update(id, model);
        }

    }
}
