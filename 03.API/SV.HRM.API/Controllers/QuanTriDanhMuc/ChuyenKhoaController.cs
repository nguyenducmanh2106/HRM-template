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
    public class ChuyenKhoaController : ControllerBase
    {
        private readonly IChuyenKhoaHttpService _chuyenKhoaHttpService;

        public ChuyenKhoaController(IChuyenKhoaHttpService chuyenKhoaHttpService)
        {
            _chuyenKhoaHttpService = chuyenKhoaHttpService;
        }
        /// <summary>
        /// Grid chuyên khoa
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<List<ChuyenKhoaModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _chuyenKhoaHttpService.GetFilter(queryFilter);
        }

        /// <summary>
        /// Hàm thêm mới chuyên khoa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(ChuyenKhoaCreateModel model)
        {
            return await _chuyenKhoaHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng ChuyenKhoa
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ChuyenKhoaModel>> FindById(int recordID)
        {
            return await _chuyenKhoaHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _chuyenKhoaHttpService.Delete(recordID);
        }
        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _chuyenKhoaHttpService.DeleteManyUseRecord(objectDelete);
        }
        /// <summary>
        /// Hàm cập nhật ChuyenKhoa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, ChuyenKhoaUpdateModel model)
        {
            return await _chuyenKhoaHttpService.Update(id, model);
        }
    }
}
