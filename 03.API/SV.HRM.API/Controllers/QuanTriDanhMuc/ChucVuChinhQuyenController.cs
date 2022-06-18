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
    public class ChucVuChinhQuyenController : ControllerBase
    {
        private readonly IChucVuChinhQuyenHttpService _chucVuChinhQuyenHttpService;

        public ChucVuChinhQuyenController(IChucVuChinhQuyenHttpService chucVuChinhQuyenHttpService)
        {
            _chucVuChinhQuyenHttpService = chucVuChinhQuyenHttpService;
        }
        /// <summary>
        /// Grid chức vụ chính quyền
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<ChucVuChinhQuyenModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _chucVuChinhQuyenHttpService.GetFilter(queryFilter);
        }


        /// <summary>
        /// Hàm thêm mới chức vụ chính quyền
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(ChucVuChinhQuyenCreateModel model)
        {
            return await _chucVuChinhQuyenHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng ChucVuChinhQuyen
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ChucVuChinhQuyenModel>> FindById(int recordID)
        {
            return await _chucVuChinhQuyenHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _chucVuChinhQuyenHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _chucVuChinhQuyenHttpService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật ChucVuChinhQuyen
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, ChucVuChinhQuyenUpdateModel model)
        {
            return await _chucVuChinhQuyenHttpService.Update(id, model);
        }
    }
}
