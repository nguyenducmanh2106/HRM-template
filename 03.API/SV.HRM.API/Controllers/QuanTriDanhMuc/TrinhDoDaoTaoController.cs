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
    public class TrinhDoDaoTaoController : ControllerBase
    {
        private readonly ITrinhDoDaoTaoHttpService _trinhDoDaoTaoHttpService;

        public TrinhDoDaoTaoController(ITrinhDoDaoTaoHttpService trinhDoDaoTaoHttpService)
        {
            _trinhDoDaoTaoHttpService = trinhDoDaoTaoHttpService;
        }
        [HttpPost]
        public async Task<Response<List<TrinhDoDaoTaoModel>>> GetFilter(EntityGeneric queryFilter)
        {
            return await _trinhDoDaoTaoHttpService.GetFilter(queryFilter);
        }
        /// <summary>
        /// Hàm thêm mới trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Create(TrinhDoDaoTaoCreateModel model)
        {
            return await _trinhDoDaoTaoHttpService.Create(model);
        }

        /// <summary>
        /// Tìm bản ghi trong bảng TrinhDoDaoTao
        /// </summary>
        /// <param name="recordID">Id bản ghi cần lấy</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<TrinhDoDaoTaoModel>> FindById(int recordID)
        {
            return await _trinhDoDaoTaoHttpService.FindById(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteMany(List<int> recordID)
        {
            return await _trinhDoDaoTaoHttpService.Delete(recordID);
        }

        /// <summary>
        /// Hàm xóa bản ghi check bản ghi có được sử dụng không
        /// </summary>
        /// <param name="staffID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<bool>> DeleteManyUseRecord(List<object> objectDelete)
        {
            return await _trinhDoDaoTaoHttpService.DeleteManyUseRecord(objectDelete);
        }

        /// <summary>
        /// Hàm cập nhật trình độ chuyên môn
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Response<bool>> Update(int id, TrinhDoDaoTaoUpdateModel model)
        {
            return await _trinhDoDaoTaoHttpService.Update(id, model);
        }
    }
}
