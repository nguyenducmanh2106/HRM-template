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
    public class TrinhDoChuyenMonController : ControllerBase
    {
        private readonly ITrinhDoChuyenMonHttpService _trinhDoChuyenMonHttpService;

        public TrinhDoChuyenMonController(ITrinhDoChuyenMonHttpService trinhDoChuyenMonHttpService)
        {
            _trinhDoChuyenMonHttpService = trinhDoChuyenMonHttpService;
        }
        [HttpPost]
        public async Task<Response<List<TrinhDoChuyenMonModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _trinhDoChuyenMonHttpService.GetFilter(queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(TrinhDoChuyenMonCreateModel model)
        {
            return await _trinhDoChuyenMonHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng TrinhDoChuyenMon
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<TrinhDoChuyenMonModel>> FindById(int recordID)
        {
            return await _trinhDoChuyenMonHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _trinhDoChuyenMonHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _trinhDoChuyenMonHttpService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, TrinhDoChuyenMonUpdateModel model)
        {
            return await _trinhDoChuyenMonHttpService.Update(id, model);
        }
        /// <summary>
        /// check bản ghi có đang được sử dụng không
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> FindIdInUse(List<object> recordID)
        {
            return await _trinhDoChuyenMonHttpService.FindIdInUse(recordID);
        }
    }
}
