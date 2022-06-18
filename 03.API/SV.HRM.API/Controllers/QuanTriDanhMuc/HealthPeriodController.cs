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
    public class HealthPeriodController : ControllerBase
    {
        private readonly IHealthPeriodHttpService _healthPeriodHttpService;

        public HealthPeriodController(IHealthPeriodHttpService healthPeriodHttpService)
        {
            _healthPeriodHttpService = healthPeriodHttpService;
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<List<HealthPeriodModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _healthPeriodHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới mối quan hệ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(HealthPeriodCreateModel model)
        {
            return await _healthPeriodHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng HealthPeriod
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<HealthPeriodModel>> FindById(int recordID)
        {
            return await _healthPeriodHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _healthPeriodHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _healthPeriodHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật mối quan hệ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, HealthPeriodUpdateModel model)
        {
            return await _healthPeriodHttpService.Update(id, model);
        }
        /// <summary>
        /// check bản ghi có đang được sử dụng không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _healthPeriodHttpService.FindIdInUse(recordID);
        }

        /// <summary>
        /// check date trong kỳ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> CheckDateInPeriod(HealthPeriod model)
        {
            return await _healthPeriodHttpService.CheckDateInPeriod(model);
        }
    }
}
