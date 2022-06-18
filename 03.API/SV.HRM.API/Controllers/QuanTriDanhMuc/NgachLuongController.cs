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
    public class NgachLuongController : ControllerBase
    {
        private readonly INgachLuongHttpService _NgachLuongHttpService;

        public NgachLuongController(INgachLuongHttpService NgachLuongHttpService)
        {
            _NgachLuongHttpService = NgachLuongHttpService;
        }
        /// <summary>
        /// Grid
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<NgachLuongModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _NgachLuongHttpService.GetFilter(queryFilter);
        }


        /// <summary>
        /// Hàm thêm mới NgachLuong
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(NgachLuongCreateModel model)
        {
            return await _NgachLuongHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng NgachLuong
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<NgachLuongModel>> FindById(int recordID)
        {
            return await _NgachLuongHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _NgachLuongHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _NgachLuongHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật vị trí việc làm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, NgachLuongUpdateModel model)
        {
            return await _NgachLuongHttpService.Update(id, model);
        }
    }
}
