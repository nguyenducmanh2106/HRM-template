﻿using Microsoft.AspNetCore.Http;
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
    public class ChucVuKiemNhiemController : ControllerBase
    {
        private readonly IChucVuKiemNhiemHttpService _chucVuKiemNhiemHttpService;

        public ChucVuKiemNhiemController(IChucVuKiemNhiemHttpService chucVuKiemNhiemHttpService)
        {
            _chucVuKiemNhiemHttpService = chucVuKiemNhiemHttpService;
        }
        /// <summary>
        /// Grid chức vụ kiêm nhiệm
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<ChucVuKiemNhiemModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _chucVuKiemNhiemHttpService.GetFilter(queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới chức vụ kiêm nhiệm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(ChucVuKiemNhiemCreateModel model)
        {
            return await _chucVuKiemNhiemHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng ChucVuKiemNhiem
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ChucVuKiemNhiemModel>> FindById(int recordID)
        {
            return await _chucVuKiemNhiemHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _chucVuKiemNhiemHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _chucVuKiemNhiemHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật ChucVuKiemNhiem
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, ChucVuKiemNhiemUpdateModel model)
        {
            return await _chucVuKiemNhiemHttpService.Update(id, model);
        }
    }
}
