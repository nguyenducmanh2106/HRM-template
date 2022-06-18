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
    public class QuanLySucKhoeController:ControllerBase
    {
        private readonly IQuanLySucKhoeHttpService _quanLySucKhoeHttpService;

        public QuanLySucKhoeController(IQuanLySucKhoeHttpService quanLySucKhoeHttpService)
        {
            _quanLySucKhoeHttpService = quanLySucKhoeHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="StaffQueryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<QuanLySucKhoeModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _quanLySucKhoeHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới quản lý sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(QuanLySucKhoeCreateModel model)
        {
            return await _quanLySucKhoeHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng QuanLySucKhoe
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<QuanLySucKhoeModel>> FindById(int recordID)
        {
            return await _quanLySucKhoeHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _quanLySucKhoeHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm cập nhật quản lý sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, QuanLySucKhoeUpdateModel model)
        {
            return await _quanLySucKhoeHttpService.Update(id, model);
        }
        /// <summary>
        /// check ngày có trong kỳ khám
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="healthPeriod"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<bool>> CheckDateBetween(int staffId, int healthPeriod, DateTime date)
        {
            return await _quanLySucKhoeHttpService.CheckDateBetween(staffId, healthPeriod, date);
        }
        /// <summary>
        /// check ngày có trong kỳ khám
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="healthPeriod"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<bool>> CheckHealthPeriodAndHistory(int staffId, int healthPeriod)
        {
            return await _quanLySucKhoeHttpService.CheckHealthPeriodAndHistory(staffId, healthPeriod);
        }
    }
}
