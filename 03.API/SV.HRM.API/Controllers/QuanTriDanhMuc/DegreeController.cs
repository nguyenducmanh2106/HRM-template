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
    [Route("[controller]/[action]")]
    [ApiController]
    public class DegreeController : ControllerBase
    {
        private readonly IDegreeHttpService _DegreeHttpService;

        public DegreeController(
            IDegreeHttpService DegreeHttpService)
        {
            _DegreeHttpService = DegreeHttpService;
        }
        [HttpPost]
        public async Task<Response<List<DegreeModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _DegreeHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới Degree
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(DegreeCreateModel model)
        {
            return await _DegreeHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng Degree
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<DegreeModel>> FindById(int recordID)
        {
            return await _DegreeHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _DegreeHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _DegreeHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật Degree
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, DegreeUpdateModel model)
        {
            return await _DegreeHttpService.Update(id, model);
        }
    }
}
