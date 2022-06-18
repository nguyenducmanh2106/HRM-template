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
    public class ReligionController : ControllerBase
    {
        private readonly IReligionHttpService _ReligionHttpService;

        public ReligionController(IReligionHttpService ReligionHttpService)
        {
            _ReligionHttpService = ReligionHttpService;
        }
        /// <summary>
        /// Grid chuyên nganh
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<ReligionModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _ReligionHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới chuyên nganh
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(ReligionCreateModel model)
        {
            return await _ReligionHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Religion
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ReligionModel>> FindById(int recordID)
        {
            return await _ReligionHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _ReligionHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _ReligionHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật Religion
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, ReligionUpdateModel model)
        {
            return await _ReligionHttpService.Update(id, model);
        }
    }
}
