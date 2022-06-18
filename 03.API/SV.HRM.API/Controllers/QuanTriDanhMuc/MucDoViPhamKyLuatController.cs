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
    public class MucDoViPhamKyLuatController : ControllerBase
    {
        private readonly IMucDoViPhamKyLuatHttpService _mucDoViPhamKyLuatHttpService;

        public MucDoViPhamKyLuatController(IMucDoViPhamKyLuatHttpService mucDoViPhamKyLuatHttpService)
        {
            _mucDoViPhamKyLuatHttpService = mucDoViPhamKyLuatHttpService;
        }
        /// <summary>
        /// Grid mức độ vi phạm kỷ luật
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<MucDoViPhamKyLuatModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _mucDoViPhamKyLuatHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới mức đọ vi phạm kỷ luật
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(MucDoViPhamKyLuatCreateModel model)
        {
            return await _mucDoViPhamKyLuatHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng MucDoViPhamKyLuat
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<MucDoViPhamKyLuatModel>> FindById(int recordID)
        {
            return await _mucDoViPhamKyLuatHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _mucDoViPhamKyLuatHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _mucDoViPhamKyLuatHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật MucDoViPhamKyLuat
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, MucDoViPhamKyLuatUpdateModel model)
        {
            return await _mucDoViPhamKyLuatHttpService.Update(id, model);
        }
    }
}
