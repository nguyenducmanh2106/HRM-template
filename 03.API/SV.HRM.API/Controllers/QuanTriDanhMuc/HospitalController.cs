using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SV.HRM.API.HttpServices;
using SV.HRM.Core;
using SV.HRM.Core.Utils;
using SV.HRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace SV.HRM.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalHttpService _hospitalHttpService;

        public HospitalController(IHospitalHttpService hospitalHttpService)
        {
            _hospitalHttpService = hospitalHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<HospitalModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _hospitalHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới bệnh viện
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(HospitalCreateModel model)
        {
            return await _hospitalHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Hospital
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<HospitalModel>> FindById(int recordID)
        {
            return await _hospitalHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _hospitalHttpService.Delete(recordID);
        }


        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {           
            return await _hospitalHttpService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật bệnh viện
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, HospitalUpdateModel model)
        {
            return await _hospitalHttpService.Update(id, model);
        }
    }
}
