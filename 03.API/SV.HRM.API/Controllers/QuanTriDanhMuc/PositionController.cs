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
    public class PositionController : ControllerBase
    {
        private readonly IPositionHttpService _positionHttpService;

        public PositionController(IPositionHttpService positionHttpService)
        {
            _positionHttpService = positionHttpService;
        }

        [HttpPost]
        public async Task<Response<List<PositionModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _positionHttpService.GetFilter(queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới chức danh
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(PositionCreateModel model)
        {
            return await _positionHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Position
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<PositionModel>> FindById(int recordID)
        {
            return await _positionHttpService.FindById(recordID);
        }

        /// <summary>
        /// check bản ghi có đang được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _positionHttpService.FindIdInUse(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _positionHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _positionHttpService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật chức danh
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, PositionUpdateModel model)
        {
            return await _positionHttpService.Update(id, model);
        }
    }
}