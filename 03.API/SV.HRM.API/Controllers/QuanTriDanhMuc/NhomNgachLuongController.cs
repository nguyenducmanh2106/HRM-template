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
    public class NhomNgachLuongController : ControllerBase
    {
        private readonly INhomNgachLuongHttpService _nhomNgachLuongHttpService;

        public NhomNgachLuongController(INhomNgachLuongHttpService nhomNgachLuongHttpService)
        {
            _nhomNgachLuongHttpService = nhomNgachLuongHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<NhomNgachLuongModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _nhomNgachLuongHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới Nhóm ngạch lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(NhomNgachLuongCreateModel model)
        {
            return await _nhomNgachLuongHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng NhomNgachLuong
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<NhomNgachLuongModel>> FindById(int recordID)
        {
            return await _nhomNgachLuongHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _nhomNgachLuongHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _nhomNgachLuongHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật nhóm ngạch lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, NhomNgachLuongUpdateModel model)
        {
            return await _nhomNgachLuongHttpService.Update(id, model);
        }
    }
}
