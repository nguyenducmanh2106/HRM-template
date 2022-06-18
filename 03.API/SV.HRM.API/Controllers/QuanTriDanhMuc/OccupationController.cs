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
    public class OccupationController : ControllerBase
    {
        private readonly IOccupationHttpService _occupationHttpService;

        public OccupationController(IOccupationHttpService occupationHttpService)
        {
            _occupationHttpService = occupationHttpService;
        }

        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<OccupationModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _occupationHttpService.GetFilter(queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới nghề
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(OccupationCreateModel model)
        {
            return await _occupationHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Occupation
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<OccupationModel>> FindById(int recordID)
        {
            return await _occupationHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _occupationHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _occupationHttpService.DeleteManyUseRecord(objectDelete);
        }


        /// <summary>
        /// Hàm cập nhật Occupation
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, OccupationUpdateModel model)
        {
            return await _occupationHttpService.Update(id, model);
        }
    }
}
