using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SV.HRM.Core;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.QuanTriDanhMuc.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HealthPeriodController : ControllerBase
    {
        private readonly IHealthPeriodHandler _healthPeriodHandler;
        private readonly IBaseHandler _baseHandler;
        public HealthPeriodController(IBaseHandler baseHandler,
             IHealthPeriodHandler healthPeriodHandler
            )
        {
            _baseHandler = baseHandler;
            _healthPeriodHandler = healthPeriodHandler;
        }
        [HttpPost]
        public async Task<Response<List<HealthPeriodModel>>> GetFilter([FromBody] EntityGeneric queryFilter)
        {
            try
            {
                var response = await _baseHandler.GetFilter<HealthPeriodModel>(queryFilter);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hàm tạo kỳ khám sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Create([FromBody] HealthPeriodCreateModel entity)
        {
            return await _healthPeriodHandler.Create(entity);
        }


        /// <summary>
        /// Tìm bản ghi trong bảng chỉ định
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<HealthPeriodModel>> FindById(int recordID)
        {
            return await _healthPeriodHandler.FindById(recordID);
        }
        /// <summary>
        /// Tìm bản ghi trong bản có được sử dụng không
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _healthPeriodHandler.FindIdInUse(recordID);
        }

        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> entity)
        {
            return await _baseHandler.DeleteMany<HealthPeriod>(entity);
        }
        /// <summary>
        /// Hàm xóa danh sach bản ghi được chọn check có được sử dụng hay không
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> entity)
        {
            return await _healthPeriodHandler.CheckRecordInUse(entity);
        }

        /// <summary>
        /// Hàm cập nhật kỳ khám sức khỏe
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> Update(int id, [FromBody] HealthPeriodUpdateModel entity)
        {
            return await _healthPeriodHandler.Update(id, entity);
        }

        /// <summary>
        /// Hàm check date cho kỳ khám sức khỏe
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> CheckDateInPeriod(HealthPeriod model)
        {
            return await _healthPeriodHandler.CheckDateInPeriod(model);
        }
    }
}
