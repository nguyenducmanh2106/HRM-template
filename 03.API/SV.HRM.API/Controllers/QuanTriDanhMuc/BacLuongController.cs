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
    public class BacLuongController : ControllerBase
    {
        private readonly IBacLuongHttpService _bacLuongHttpService;

        public BacLuongController(IBacLuongHttpService bacLuongHttpService)
        {
            _bacLuongHttpService = bacLuongHttpService;
        }
        /// <summary>
        /// Grid view BacLuong
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<BacLuongModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _bacLuongHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới bậc lương
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(BacLuongCreateModel model)
        {
            return await _bacLuongHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng BacLuong
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<BacLuongModel>> FindById(int recordID)
        {
            return await _bacLuongHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _bacLuongHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _bacLuongHttpService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật BacLuong
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, BacLuongUpdateModel model)
        {
            return await _bacLuongHttpService.Update(id, model);
        }
    }
}
