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
    public class ChuyenNganhController : ControllerBase
    {
        private readonly IChuyenNganhHttpService _chuyenNganhHttpService;

        public ChuyenNganhController(IChuyenNganhHttpService chuyenNganhHttpService)
        {
            _chuyenNganhHttpService = chuyenNganhHttpService;
        }
        /// <summary>
        /// Grid chuyên nganh
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<ChuyenNganhModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _chuyenNganhHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới chuyên nganh
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(ChuyenNganhCreateModel model)
        {
            return await _chuyenNganhHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng ChuyenNganh
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ChuyenNganhModel>> FindById(int recordID)
        {
            return await _chuyenNganhHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _chuyenNganhHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _chuyenNganhHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật ChuyenNganh
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, ChuyenNganhUpdateModel model)
        {
            return await _chuyenNganhHttpService.Update(id, model);
        }
    }
}
